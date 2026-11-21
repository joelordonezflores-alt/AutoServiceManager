using AutoServiceManager.Modelos;
using AutoServiceManager.Persistencia;

namespace AutoServiceManager.Servicios;

public sealed partial class ServicioTaller
{
    public decimal ObtenerIva() => datos.Leer(TasaIva);

    public void GuardarIva(decimal porcentaje)
    {
        if (porcentaje is < 0 or > 100 || decimal.Round(porcentaje, 2) != porcentaje)
            throw new ReglaNegocioException("El IVA debe estar entre 0 y 100, con hasta dos decimales.");
        datos.Escribir(sql => sql.Ejecutar("UPDATE configuracion SET valor = $0 WHERE clave = 'iva'",
            Centavos(porcentaje)));
    }

    public ResumenFactura ConsultarFacturacion(string ordenId) => datos.Leer(sql =>
    {
        BuscarOrden(sql, ordenId);
        var factura = BuscarFactura(sql, ordenId);
        return factura == null ? CalcularResumen(sql, ordenId)
            : new(factura.TasaIva, factura.Subtotal, factura.Iva, factura.Total, factura.Saldo, factura.Id);
    });

    public Factura EmitirFactura(string ordenId) =>
        datos.Escribir(sql => BuscarFactura(sql, ordenId) ?? CrearFactura(sql, ordenId));

    public Factura? FacturaDeOrden(string ordenId) => datos.Leer(sql => BuscarFactura(sql, ordenId));

    public Pago RegistrarPago(string ordenId, decimal monto, MetodoPago metodo)
    {
        long centavos = ImporteValido(monto, "Monto a pagar", admiteCero: false);
        Opcion(metodo);
        return datos.Escribir(sql =>
        {
            var factura = BuscarFactura(sql, ordenId) ?? CrearFactura(sql, ordenId);
            if (centavos > Centavos(factura.Saldo))
                throw new ReglaNegocioException($"El pago supera el saldo pendiente de $ {factura.Saldo:N2}.");

            var pago = new Pago(sql.SiguienteCodigo("PAG"), DateTimeOffset.Now, metodo, centavos / 100m);
            sql.Ejecutar("INSERT INTO pagos VALUES ($0,$1,$2,$3,$4)", pago.Id, factura.Id, pago.Fecha, pago.Metodo, centavos);
            if (pago.Monto == factura.Saldo)
                ActualizarEstadoOrden(sql, factura.OrdenId, "Pagada - Lista para entrega");
            return pago;
        });
    }

    private static decimal TasaIva(SesionSql sql) =>
        sql.Entero("SELECT valor FROM configuracion WHERE clave = 'iva'") / 100m;

    private static ResumenFactura CalcularResumen(SesionSql sql, string ordenId)
    {
        decimal subtotal = PartidasOrden(sql, ordenId).Sum(partida => partida.Importe);
        decimal tasa = TasaIva(sql);
        decimal iva = Centavos(subtotal * tasa / 100) / 100m;
        return new(tasa, subtotal, iva, subtotal + iva, subtotal + iva, null);
    }

    private static Factura CrearFactura(SesionSql sql, string ordenId)
    {
        var orden = BuscarOrden(sql, ordenId);
        var resumen = CalcularResumen(sql, ordenId);
        if (resumen.Subtotal <= 0)
            throw new ReglaNegocioException("La orden no tiene servicios o repuestos con importe para facturar.");

        var vehiculo = sql.Consultar("""
            SELECT placa,marca,modelo,anio,cedula FROM vehiculos WHERE placa=$0
            """, LeerVehiculo, orden.Placa).Single();
        var cliente = sql.Consultar("""
            SELECT cedula,nombres,telefono,direccion,correo FROM clientes WHERE cedula=$0
            """, LeerCliente, vehiculo.Cedula).Single();
        string id = sql.SiguienteCodigo("FAC");
        GuardarFactura(sql, id, orden.Id, DateTimeOffset.Now, resumen.TasaIva, resumen.Subtotal,
            resumen.Iva, resumen.Total, cliente, vehiculo, PartidasOrden(sql, orden.Id));
        ActualizarEstadoOrden(sql, orden.Id, "Facturada - Saldo pendiente");
        return BuscarFactura(sql, orden.Id)!;
    }

    private static void GuardarFactura(SesionSql sql, string id, string ordenId, DateTimeOffset fecha, decimal tasa,
        decimal subtotal, decimal iva, decimal total, Cliente cliente, Vehiculo vehiculo, IEnumerable<Partida> partidas)
    {
        sql.Ejecutar("""
            INSERT INTO facturas VALUES ($0,$1,$2,$3,$4,$5,$6,$7,$8,$9,$10,$11,$12,$13,$14,$15)
            """, id, ordenId, fecha, Centavos(tasa), Centavos(subtotal), Centavos(iva), Centavos(total),
            cliente.Cedula, cliente.Nombres, cliente.Telefono, cliente.Direccion, cliente.Correo,
            vehiculo.Placa, vehiculo.Marca, vehiculo.Modelo, vehiculo.Anio);
        foreach (var partida in partidas)
            sql.Ejecutar("""
                INSERT INTO partidas_factura (factura_id,tipo,codigo,nombre,categoria,cantidad,precio)
                VALUES ($0,$1,$2,$3,$4,$5,$6)
                """, id, partida.Tipo, partida.Codigo, partida.Nombre, partida.Categoria,
                partida.Cantidad, Centavos(partida.Precio));
    }

    private static Factura? BuscarFactura(SesionSql sql, string ordenId)
    {
        var cabecera = sql.Consultar("""
            SELECT id,orden_id,fecha,tasa_iva,subtotal,iva,total,cedula,cliente,telefono,direccion,correo,placa,marca,modelo,anio
            FROM facturas WHERE orden_id = $0
            """, fila => new Factura(fila.GetString(0), fila.GetString(1), Fecha(fila, 2),
                new(fila.GetString(7), fila.GetString(8), fila.GetString(9), fila.GetString(10), fila.GetString(11)),
                new(fila.GetString(12), fila.GetString(13), fila.GetString(14), fila.GetInt32(15), fila.GetString(7)),
                Dinero(fila, 3), Dinero(fila, 4), Dinero(fila, 5), Dinero(fila, 6), [], []), ordenId).SingleOrDefault();
        if (cabecera == null)
            return null;

        return cabecera with
        {
            Partidas = sql.Consultar("""
                SELECT tipo,codigo,nombre,categoria,cantidad,precio FROM partidas_factura WHERE factura_id=$0 ORDER BY id
                """, LeerPartida, cabecera.Id),
            Pagos = sql.Consultar("""
                SELECT id,fecha,metodo,monto FROM pagos WHERE factura_id=$0 ORDER BY fecha,rowid
                """, fila => new Pago(fila.GetString(0), Fecha(fila, 1),
                    Enum.Parse<MetodoPago>(fila.GetString(2)), Dinero(fila, 3)), cabecera.Id)
        };
    }
}
