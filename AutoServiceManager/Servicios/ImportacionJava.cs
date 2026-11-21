using System.Globalization;
using System.Text.Json;
using AutoServiceManager.Modelos;
using AutoServiceManager.Persistencia;

namespace AutoServiceManager.Servicios;

public sealed partial class ServicioTaller
{
    public static void ConvertirDatosJava(string origenJson, string destinoSqlite)
    {
        string destino = Path.GetFullPath(destinoSqlite);
        if (File.Exists(destino))
            throw new ReglaNegocioException("Selecciona una ruta nueva para la base convertida.");

        using var entrada = File.OpenRead(origenJson);
        using var documento = JsonDocument.Parse(entrada);
        var raiz = new RegistroJava(documento.RootElement);
        if (raiz.Entero("version") != 1 || raiz.Objeto("datos").Texto("clase") != "DatosSistema")
            throw new ReglaNegocioException("La exportación Java es incompatible.");
        var exportacion = raiz.Objeto("datos");
        string temporal = Path.Combine(Path.GetDirectoryName(destino)!, $".{Guid.NewGuid():N}.sqlite");
        try
        {
            var baseNueva = new BaseDatos(temporal, inventarioInicial: false);
            baseNueva.Escribir(sql => ImportarRegistros(sql, exportacion));
            File.Move(temporal, destino);
        }
        finally
        {
            File.Delete(temporal);
        }
    }

    private static void ImportarRegistros(SesionSql sql, RegistroJava datosJava)
    {
        foreach (var item in datosJava.Lista("clientes"))
        {
            var cliente = ClienteJava(item);
            sql.Ejecutar("INSERT INTO clientes VALUES ($0,$1,$2,$3,$4)", cliente.Cedula, cliente.Nombres,
                cliente.Telefono, cliente.Direccion, cliente.Correo);
        }
        foreach (var item in datosJava.Lista("vehiculos"))
        {
            var vehiculo = VehiculoJava(item);
            sql.Ejecutar("INSERT INTO vehiculos VALUES ($0,$1,$2,$3,$4)", vehiculo.Placa, vehiculo.Marca,
                vehiculo.Modelo, vehiculo.Anio, vehiculo.Cedula);
        }
        foreach (var item in datosJava.Lista("citas"))
            sql.Ejecutar("INSERT INTO citas VALUES ($0,$1,$2,$3,$4,$5)", item.Texto("idCita"), item.Fecha("fecha"),
                item.Objeto("cliente").Texto("cedula"), item.Objeto("vehiculo").Texto("placa"), item.Texto("motivo"),
                EstadoJava<EstadoCita>(item.Texto("estado")));
        foreach (var item in datosJava.Lista("ordenes"))
            ImportarOrden(sql, item);
        foreach (var item in datosJava.Lista("repuestos"))
            sql.Ejecutar("INSERT INTO repuestos VALUES ($0,$1,$2,$3)", item.Texto("idRepuesto"),
                item.Texto("nombre"), item.Entero("stock"), Centavos(item.Importe("precioUnitario")));
        foreach (var item in datosJava.Lista("proveedores"))
            sql.Ejecutar("INSERT INTO proveedores VALUES ($0,$1,$2,$3)",
                item.Texto("idProveedor"), item.Texto("nombre"), item.Texto("telefono"), item.Texto("correo"));
        foreach (var item in datosJava.Lista("compras"))
        {
            string id = item.Texto("idCompra");
            sql.Ejecutar("INSERT INTO compras VALUES ($0,$1,$2)", id, item.Fecha("fecha"),
                item.Objeto("proveedor").Texto("idProveedor"));
            foreach (var detalle in item.Lista("detalles"))
                sql.Ejecutar("""
                    INSERT INTO detalles_compra (compra_id,repuesto_id,cantidad,costo) VALUES ($0,$1,$2,$3)
                    """, id, detalle.Objeto("repuesto").Texto("idRepuesto"), detalle.Entero("cantidad"),
                    Centavos(detalle.Importe("precioUnitario")));
        }
        foreach (var item in datosJava.Lista("empleados"))
        {
            RolEmpleado rol = item.Texto("clase") == "Mecanico"
                ? RolEmpleado.Mecánico : EstadoJava<RolEmpleado>(item.Texto("clase"));
            sql.Ejecutar("INSERT INTO empleados VALUES ($0,$1,$2,$3,$4,$5)", item.Texto("cedula"),
                item.Texto("nombres"), item.Texto("telefono"), item.Texto("direccion"), rol,
                Centavos(item.Importe("sueldo")));
        }
        foreach (var item in datosJava.Lista("servicios"))
        {
            var servicio = ServicioJava(item);
            sql.Ejecutar("INSERT INTO servicios VALUES ($0,$1,$2,$3)",
                servicio.Id, servicio.Nombre, servicio.Categoria, Centavos(servicio.Precio));
        }
        foreach (var item in datosJava.Lista("facturas"))
            ImportarFactura(sql, item);

        SembrarContador(sql, "CIT", "SELECT id FROM citas", datosJava.Entero("contadorCitas"));
        SembrarContador(sql, "ORD", "SELECT id FROM ordenes", datosJava.Entero("contadorOrdenes"));
        SembrarContador(sql, "PROV", "SELECT id FROM proveedores", datosJava.Entero("contadorProveedores"));
        SembrarContador(sql, "COM", "SELECT id FROM compras", datosJava.Entero("contadorCompras"));
        SembrarContador(sql, "FAC", "SELECT id FROM facturas", datosJava.Entero("contadorFacturas"));
        SembrarContador(sql, "PAG", "SELECT id FROM pagos", datosJava.Entero("contadorPagos"));
        SembrarContador(sql, "REP", "SELECT id FROM repuestos", 1);
    }

    private static void ImportarOrden(SesionSql sql, RegistroJava item)
    {
        string id = item.Texto("idOrden");
        string observaciones = item.Texto("observaciones");
        string tipo = "";
        int finTipo = observaciones.IndexOf("] - ", StringComparison.Ordinal);
        if (observaciones.StartsWith('[') && finTipo > 0)
        {
            tipo = observaciones[1..finTipo];
            if (tipo.Equals("Mecanica", StringComparison.OrdinalIgnoreCase))
                tipo = "Mecánica";
            observaciones = observaciones[(finTipo + 4)..];
        }
        sql.Ejecutar("""
            INSERT INTO ordenes (id,fecha_ingreso,fecha_entrega,placa,estado,observaciones,tipo) VALUES ($0,$1,$2,$3,$4,$5,$6)
            """, id, item.Fecha("fechaIngreso"), item.FechaOpcional("fechaEntrega"), item.Objeto("vehiculo").Texto("placa"),
            item.Texto("estado"), observaciones, tipo);

        var servicios = item.Lista("servicios").Select(ServicioJava).ToList();
        int ultimoDiagnostico = servicios.FindLastIndex(s => s.Id == "SRV-MEC" && s.Nombre == "Mano de Obra Mecánica");
        var cotizacion = item.Objeto("cotizacion");
        for (int i = 0; i < servicios.Count; i++)
        {
            var servicio = servicios[i];
            AgregarPartida(sql, id, new("Servicio", servicio.Id, servicio.Nombre, servicio.Categoria, 1, servicio.Precio),
                i == ultimoDiagnostico && cotizacion.Existe ? "Diagnostico" : "");
        }
        foreach (var repuesto in item.Lista("repuestos"))
            AgregarPartida(sql, id, new("Repuesto", repuesto.Texto("idRepuesto"), repuesto.Texto("nombre"),
                "Repuesto", 1, repuesto.Importe("precioUnitario")));

        if (cotizacion.Existe)
        {
            var diagnostico = cotizacion.Objeto("diagnostico");
            sql.Ejecutar("INSERT INTO cotizaciones VALUES ($0,$1,$2,$3,$4,$5,$6)", id,
                cotizacion.Texto("idCotizacion"), cotizacion.Fecha("fecha"), diagnostico.Texto("observaciones"),
                ultimoDiagnostico < 0 ? 0 : Centavos(servicios[ultimoDiagnostico].Precio),
                Centavos(cotizacion.Importe("costoEstimado")), EstadoJava<EstadoCotizacion>(cotizacion.Texto("estado")));
        }
    }

    private static void ImportarFactura(SesionSql sql, RegistroJava item)
    {
        string id = item.Texto("idFactura");
        var orden = item.Objeto("orden");
        string ordenId = orden.Texto("idOrden");
        var vehiculo = orden.Objeto("vehiculo");
        var partidas = PartidasOrden(sql, ordenId);
        decimal subtotal = item.Importe("subtotal");
        if (Centavos(partidas.Sum(p => p.Importe)) != Centavos(subtotal))
            throw new ReglaNegocioException($"La factura {id} tiene un subtotal histórico distinto a sus partidas. Revisa la factura de origen antes de importar.");
        GuardarFactura(sql, id, ordenId, item.Fecha("fechaEmision"), 15, subtotal, item.Importe("iva"),
            item.Importe("total"), ClienteJava(vehiculo.Objeto("dueno")), VehiculoJava(vehiculo), partidas);
        foreach (var pago in item.Lista("pagos"))
            sql.Ejecutar("INSERT INTO pagos VALUES ($0,$1,$2,$3,$4)", pago.Texto("idPago"), id, pago.Fecha("fechaPago"),
                EstadoJava<MetodoPago>(pago.Texto("metodo").Replace("_", "")), Centavos(pago.Importe("monto")));
    }

    private static Cliente ClienteJava(RegistroJava item) => new(item.Texto("cedula"), item.Texto("nombres"),
        item.Texto("telefono"), item.Texto("direccion"), item.Texto("correo"));
    private static Vehiculo VehiculoJava(RegistroJava item) => new(item.Texto("placa"), item.Texto("marca"),
        item.Texto("modelo"), item.Entero("anio"), item.Objeto("dueno").Texto("cedula"));
    private static Servicio ServicioJava(RegistroJava item) => new(item.Texto("idServicio"), item.Texto("nombre"),
        item.Texto("tipo"), item.Importe("precioManoObra"));

    private static T EstadoJava<T>(string nombre) where T : struct, Enum =>
        Enum.TryParse<T>(nombre, ignoreCase: true, out var valor) && Enum.IsDefined(valor)
            ? valor : throw new ReglaNegocioException($"Valor histórico desconocido: «{nombre}».");

    private static void SembrarContador(SesionSql sql, string prefijo, string consulta, int contador)
    {
        long siguiente = Math.Max(1, contador);
        string inicio = prefijo + "-";
        foreach (string codigo in sql.Consultar(consulta, fila => fila.GetString(0)))
        {
            if (codigo.StartsWith(inicio, StringComparison.OrdinalIgnoreCase) && int.TryParse(codigo.AsSpan(inicio.Length),
                NumberStyles.None, CultureInfo.InvariantCulture, out int numero))
                siguiente = Math.Max(siguiente, (long)numero + 1);
        }
        sql.Ejecutar("INSERT INTO secuencias VALUES ($0,$1)", prefijo, siguiente);
    }

    private readonly record struct RegistroJava(JsonElement Elemento)
    {
        public bool Existe => Elemento.ValueKind == JsonValueKind.Object;
        private JsonElement Campo(string nombre) => Existe && Elemento.TryGetProperty(nombre, out var valor) ? valor : default;
        public RegistroJava Objeto(string nombre) => new(Campo(nombre));
        public string Texto(string nombre) => Campo(nombre) is { ValueKind: JsonValueKind.String } valor
            ? valor.GetString()! : "";
        public int Entero(string nombre) => Campo(nombre) is { ValueKind: JsonValueKind.Number } valor ? valor.GetInt32() : 0;
        public decimal Importe(string nombre) => Campo(nombre).GetDecimal();
        public DateTimeOffset Fecha(string nombre) => FechaOpcional(nombre)
            ?? throw new ReglaNegocioException($"El registro histórico carece de fecha: {nombre}.");
        public DateTimeOffset? FechaOpcional(string nombre) => Campo(nombre) is { ValueKind: JsonValueKind.Number } valor
            ? DateTimeOffset.FromUnixTimeMilliseconds(valor.GetInt64()) : null;
        public List<RegistroJava> Lista(string nombre) => Campo(nombre) is { ValueKind: JsonValueKind.Array } valor
            ? valor.EnumerateArray().Select(item => new RegistroJava(item)).ToList() : [];
    }
}
