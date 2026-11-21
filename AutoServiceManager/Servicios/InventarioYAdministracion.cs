using System.Globalization;
using AutoServiceManager.Modelos;
using AutoServiceManager.Persistencia;
using Microsoft.Data.Sqlite;

namespace AutoServiceManager.Servicios;

public sealed partial class ServicioTaller
{
    public IReadOnlyList<Repuesto> Repuestos() => datos.Leer(sql =>
        sql.Consultar("SELECT id,nombre,stock,precio FROM repuestos ORDER BY nombre,id", LeerRepuesto));

    public Repuesto RegistrarRepuesto(string nombre, int stock, decimal precio)
    {
        nombre = Requerido(nombre, "Nombre del repuesto");
        if (stock < 0)
            throw new ReglaNegocioException("El stock inicial debe ser cero o mayor.");
        long valor = ImporteValido(precio, "Precio de venta");
        return datos.Escribir(sql =>
        {
            string id = sql.SiguienteCodigo("REP");
            sql.Ejecutar("INSERT INTO repuestos VALUES ($0,$1,$2,$3)", id, nombre, stock, valor);
            return BuscarRepuesto(sql, id);
        });
    }

    public IReadOnlyList<Proveedor> Proveedores() => datos.Leer(sql =>
        sql.Consultar("SELECT id,nombre,telefono,correo FROM proveedores ORDER BY nombre,id",
            fila => new Proveedor(fila.GetString(0), fila.GetString(1), fila.GetString(2), fila.GetString(3))));

    public Proveedor RegistrarProveedor(string nombre, string telefono, string correo)
    {
        nombre = Requerido(nombre, "Nombre del proveedor");
        telefono = Requerido(telefono, "Teléfono");
        correo = Requerido(correo, "Correo");
        return datos.Escribir(sql =>
        {
            var proveedor = new Proveedor(sql.SiguienteCodigo("PROV"), nombre, telefono, correo);
            sql.Ejecutar("INSERT INTO proveedores VALUES ($0,$1,$2,$3)", proveedor.Id, nombre, telefono, correo);
            return proveedor;
        });
    }

    public IReadOnlyList<Compra> Compras() => datos.Leer(sql => sql.Consultar("""
        SELECT c.id,c.fecha,p.nombre,r.nombre,d.cantidad,d.costo
        FROM compras c JOIN proveedores p ON p.id=c.proveedor_id
        JOIN detalles_compra d ON d.compra_id=c.id JOIN repuestos r ON r.id=d.repuesto_id
        ORDER BY c.fecha DESC, c.rowid DESC, d.id
        """, fila => new Compra(fila.GetString(0), Fecha(fila, 1), fila.GetString(2),
            fila.GetString(3), fila.GetInt32(4), Dinero(fila, 5))));

    public string RegistrarCompra(string proveedorId, IReadOnlyList<DetalleCompra> detalles)
    {
        if (detalles.Count == 0)
            throw new ReglaNegocioException("Agrega al menos un repuesto a la compra.");
        return datos.Escribir(sql =>
        {
            DebeExistir(sql, "SELECT EXISTS(SELECT 1 FROM proveedores WHERE id = $0)", proveedorId,
                "Selecciona un proveedor registrado.");
            string id = sql.SiguienteCodigo("COM");
            sql.Ejecutar("INSERT INTO compras VALUES ($0,$1,$2)", id, DateTimeOffset.Now, proveedorId);
            foreach (var detalle in detalles)
            {
                if (detalle.Cantidad <= 0)
                    throw new ReglaNegocioException("La cantidad debe ser mayor que cero.");
                long costo = ImporteValido(detalle.Costo, "Costo unitario", admiteCero: false);
                var repuesto = BuscarRepuesto(sql, detalle.RepuestoId);
                if ((long)repuesto.Stock + detalle.Cantidad > int.MaxValue)
                    throw new ReglaNegocioException("La cantidad supera el stock máximo permitido.");
                sql.Ejecutar("UPDATE repuestos SET stock = stock + $1 WHERE id = $0", repuesto.Id, detalle.Cantidad);
                sql.Ejecutar("""
                    INSERT INTO detalles_compra (compra_id,repuesto_id,cantidad,costo) VALUES ($0,$1,$2,$3)
                    """, id, repuesto.Id, detalle.Cantidad, costo);
            }
            return id;
        });
    }

    public IReadOnlyList<Empleado> Empleados() => datos.Leer(sql => sql.Consultar("""
        SELECT cedula,nombres,telefono,direccion,rol,sueldo FROM empleados ORDER BY nombres,cedula
        """, fila => new Empleado(fila.GetString(0), fila.GetString(1), fila.GetString(2), fila.GetString(3),
            Enum.Parse<RolEmpleado>(fila.GetString(4)), Dinero(fila, 5))));

    public Empleado RegistrarEmpleado(Empleado empleado)
    {
        var nuevo = empleado with
        {
            Cedula = Clave(empleado.Cedula, "Cédula"),
            Nombres = Requerido(empleado.Nombres, "Nombres"),
            Telefono = empleado.Telefono.Trim(),
            Direccion = empleado.Direccion.Trim(),
            Rol = Opcion(empleado.Rol),
            Sueldo = ImporteValido(empleado.Sueldo, "Sueldo") / 100m
        };
        return datos.Escribir(sql =>
        {
            if (sql.Ejecutar("""
                INSERT INTO empleados VALUES ($0,$1,$2,$3,$4,$5) ON CONFLICT(cedula) DO NOTHING
                """, nuevo.Cedula, nuevo.Nombres, nuevo.Telefono, nuevo.Direccion, nuevo.Rol, Centavos(nuevo.Sueldo)) == 0)
                throw new ReglaNegocioException("Ya existe un empleado con esa cédula.");
            return nuevo;
        });
    }

    public IReadOnlyList<Servicio> Servicios() => datos.Leer(sql =>
        sql.Consultar("SELECT id,nombre,categoria,precio FROM servicios ORDER BY nombre,id", LeerServicio));

    public Servicio RegistrarServicio(Servicio servicio)
    {
        var nuevo = new Servicio(Clave(servicio.Id, "Código de servicio"),
            Requerido(servicio.Nombre, "Nombre del servicio"), Requerido(servicio.Categoria, "Categoría"),
            ImporteValido(servicio.Precio, "Precio del servicio") / 100m);
        return datos.Escribir(sql =>
        {
            if (sql.Ejecutar("""
                INSERT INTO servicios VALUES ($0,$1,$2,$3) ON CONFLICT(id) DO NOTHING
                """, nuevo.Id, nuevo.Nombre, nuevo.Categoria, Centavos(nuevo.Precio)) == 0)
                throw new ReglaNegocioException("Ya existe un servicio con ese código.");
            return nuevo;
        });
    }

    public string GenerarReporte() => datos.Leer(sql =>
    {
        var pagos = sql.Consultar("SELECT factura_id,monto FROM pagos",
            fila => (Id: fila.GetString(0), Monto: Dinero(fila, 1)));
        var pagadosPorFactura = pagos.GroupBy(p => p.Id, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.Sum(p => p.Monto), StringComparer.OrdinalIgnoreCase);
        decimal saldo = sql.Consultar("SELECT id,total FROM facturas", fila => (fila.GetString(0), Dinero(fila, 1)))
            .Sum(factura => Math.Max(0, factura.Item2 - pagadosPorFactura.GetValueOrDefault(factura.Item1)));
        decimal compras = sql.Consultar("SELECT cantidad,costo FROM detalles_compra",
            fila => fila.GetInt32(0) * Dinero(fila, 1)).Sum();
        var cultura = CultureInfo.GetCultureInfo("es-EC");
        return $"""
            REPORTE GENERAL AUTOSERVICE MANAGER
            Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}

            CLIENTES Y RECEPCIÓN
            Clientes registrados: {sql.Entero("SELECT count(*) FROM clientes")}
            Vehículos registrados: {sql.Entero("SELECT count(*) FROM vehiculos")}
            Citas totales: {sql.Entero("SELECT count(*) FROM citas")}
            Citas pendientes: {sql.Entero("SELECT count(*) FROM citas WHERE estado IN ('Programada','Confirmada')")}

            OPERACIÓN DEL TALLER
            Órdenes de trabajo: {sql.Entero("SELECT count(*) FROM ordenes")}
            Empleados: {sql.Entero("SELECT count(*) FROM empleados")}
            Servicios catalogados: {sql.Entero("SELECT count(*) FROM servicios")}

            INVENTARIO Y COMPRAS
            Repuestos: {sql.Entero("SELECT count(*) FROM repuestos")}
            Repuestos con stock bajo (hasta 5): {sql.Entero("SELECT count(*) FROM repuestos WHERE stock <= 5")}
            Compras registradas: {sql.Entero("SELECT count(*) FROM compras")}
            Valor de compras: $ {compras.ToString("N2", cultura)}

            FACTURACIÓN
            Facturas emitidas: {sql.Entero("SELECT count(*) FROM facturas")}
            Total cobrado: $ {pagos.Sum(p => p.Monto).ToString("N2", cultura)}
            Saldo pendiente: $ {saldo.ToString("N2", cultura)}
            """;
    });

    private static Repuesto BuscarRepuesto(SesionSql sql, string id) => sql.Consultar("""
        SELECT id,nombre,stock,precio FROM repuestos WHERE id = $0
        """, LeerRepuesto, id).SingleOrDefault()
        ?? throw new ReglaNegocioException("El repuesto seleccionado ya no está disponible.");

    private static Repuesto LeerRepuesto(SqliteDataReader fila) =>
        new(fila.GetString(0), fila.GetString(1), fila.GetInt32(2), Dinero(fila, 3));

    private static Servicio LeerServicio(SqliteDataReader fila) =>
        new(fila.GetString(0), fila.GetString(1), fila.GetString(2), Dinero(fila, 3));
}
