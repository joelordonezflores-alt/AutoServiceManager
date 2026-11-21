using System.Globalization;
using System.Text.Json.Nodes;
using AutoServiceManager.Modelos;
using AutoServiceManager.Servicios;
using PdfSharp.Pdf.IO;

internal static class Program
{
    private static void Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("es-EC");
        if (args.Length is < 1 or > 2)
            throw new ArgumentException("Indica una carpeta nueva para las pruebas y, opcionalmente, el JSON de Java.");

        string ruta = Path.GetFullPath(args[0]);
        if (Directory.Exists(ruta))
            throw new IOException("Selecciona una carpeta nueva para evitar reusar datos de otra prueba.");
        Directory.CreateDirectory(ruta);
        VerificarServicios(ruta);
        if (args.Length == 2)
            VerificarImportacion(ruta, args[1]);
    }

    private static void VerificarServicios(string ruta)
    {
        string archivo = Path.Combine(ruta, "pruebas.sqlite");
        var taller = new ServicioTaller(archivo);
        Igual(15m, taller.ObtenerIva(), "IVA inicial");
        Igual(4, taller.Repuestos().Count, "inventario de demostración");
        Caso("Inicialización de SQLite e inventario de referencia");

        var cliente = taller.RegistrarCliente(" 001-a ", "Inés de Prueba", "022", "Dirección de prueba", "ines@example.test");
        var otro = taller.RegistrarCliente("002", "Segundo Cliente", "033", "Dirección de prueba", "otro@example.test");
        Igual("001-A", cliente.Cedula, "normalización de cédula");
        Rechaza(() => taller.RegistrarCliente("001-a", "Duplicado", "022", "X", "x@example.test"));
        Rechaza(() => taller.RegistrarCliente("   ", "Vacío", "022", "X", "x@example.test"));
        var vehiculo = new Vehiculo(" abc-123 ", "Marca", "Modelo", 2022, cliente.Cedula);
        vehiculo = taller.RegistrarVehiculo(vehiculo);
        Igual("ABC-123", vehiculo.Placa, "normalización de placa");
        Rechaza(() => taller.RegistrarVehiculo(vehiculo with { Cedula = otro.Cedula }));
        Caso("Clientes, cédulas duplicadas y propiedad de vehículos");

        var agendada = taller.AgendarCita(vehiculo, DateTimeOffset.Now.AddDays(1), "Pintura de prueba");
        taller.CambiarEstadoCita(agendada.Id, EstadoCita.Confirmada);
        Igual(agendada.Id, taller.CitaPendiente(cliente.Cedula)?.Id, "cita pendiente");
        var segundoVehiculo = vehiculo with { Placa = "ABC-124" };
        Rechaza(() => taller.CrearOrden(segundoVehiculo, TipoTrabajo.Pintura, "X", agendada.Id));
        Igual(1, taller.Vehiculos().Count, "reversión de alta de vehículo");
        Igual(EstadoCita.Confirmada, taller.Citas().Single().Estado, "estado tras error");
        var orden = taller.CrearOrden(vehiculo, TipoTrabajo.Pintura, agendada.Motivo, agendada.Id);
        Igual("ORD-001", orden.Id, "código de orden");
        Igual(EstadoCita.Atendida, taller.Citas().Single().Estado, "atención desde orden");
        Rechaza(() => taller.CambiarEstadoCita(agendada.Id, EstadoCita.Cancelada));
        var cancelada = taller.AgendarCita(segundoVehiculo, DateTimeOffset.Now.AddDays(2), "Lavado");
        taller.CambiarEstadoCita(cancelada.Id, EstadoCita.Cancelada);
        Rechaza(() => taller.CrearOrden(segundoVehiculo, TipoTrabajo.Lavado, "X", cancelada.Id));
        Caso("Agenda, transiciones y alta de orden con reversión transaccional");

        var filtro = taller.Repuestos().Single(item => item.Id == "REP-001");
        var proveedor = taller.RegistrarProveedor("Proveedor de prueba", "011", "proveedor@example.test");
        var extra = taller.RegistrarRepuesto("Arandela de prueba", 0, 3m);
        Rechaza(() => taller.RegistrarCompra(proveedor.Id, [new(filtro.Id, 1, 8), new(extra.Id, 0, 1)]));
        Igual(filtro.Stock, Stock(filtro.Id), "reversión de primera línea de compra");
        Igual(0, taller.Compras().Count, "compra inválida");
        Igual("COM-001", taller.RegistrarCompra(proveedor.Id, [new(filtro.Id, 4, 8m), new(extra.Id, 3, 1.25m)]),
            "numeración tras reversión");
        Igual(14, Stock(filtro.Id), "stock tras compra");
        Igual(35.75m, taller.Compras().Sum(item => item.Total), "total de compra");
        Rechaza(() => taller.AgregarRepuestoAOrden(orden.Id, filtro.Id, 15));
        taller.AgregarRepuestoAOrden(orden.Id, filtro.Id, 2);
        Igual(12, Stock(filtro.Id), "stock tras consumo");
        Caso("Compras, proveedores, cantidades, consumo y rechazo por falta de stock");

        var empleado = taller.RegistrarEmpleado(new("E-1", "Alicia", "", "", RolEmpleado.Mecánico, 650));
        Rechaza(() => taller.RegistrarEmpleado(empleado with { Cedula = "e-1" }));
        var servicio = taller.RegistrarServicio(new("SRV-01", "Alineación", "Mecánica", 18.75m));
        Rechaza(() => taller.RegistrarServicio(servicio with { Id = "srv-01" }));
        taller.AgregarServicioAOrden(orden.Id, servicio.Id);
        Igual(71m, taller.GenerarDiagnostico(orden.Id, "Diagnóstico preliminar", 40).Estimado, "cotización inicial");
        var cotizacion = taller.GenerarDiagnostico(orden.Id, "Diagnóstico revisado", 50);
        Igual(81m, cotizacion.Estimado, "cotización revisada");
        Igual(1, taller.DetalleOrden(orden.Id).Partidas.Count(p => p.Codigo == "SRV-MEC"), "mano de obra sin duplicar");
        taller.ResolverCotizacion(orden.Id, EstadoCotizacion.Aprobada);
        Igual(EstadoCotizacion.Aprobada, taller.DetalleOrden(orden.Id).Cotizacion?.Estado, "aprobación guardada");
        taller.RegistrarAvance(orden.Id, RolEmpleado.Pintor, "Listo para Entrega", 10);
        Igual(1, taller.DetalleOrden(orden.Id).Avances.Count, "historial de avance");
        var segundaOrden = taller.CrearOrden(segundoVehiculo, TipoTrabajo.Lavado, "Orden independiente");
        taller.GenerarDiagnostico(segundaOrden.Id, "Cotización rechazada", 20);
        taller.ResolverCotizacion(segundaOrden.Id, EstadoCotizacion.Rechazada);
        Igual("Cotización rechazada", taller.DetalleOrden(segundaOrden.Id).Orden.Estado, "rechazo guardado");
        Caso("Personal, catálogo, diagnóstico, cotización y avance de taller");

        Rechaza(() => taller.RegistrarPago(orden.Id, 9999m, MetodoPago.Efectivo));
        Igual(null, taller.FacturaDeOrden(orden.Id), "reversión de emisión cuando se rechaza el pago");
        var factura = taller.EmitirFactura(orden.Id);
        Igual("FAC-001", factura.Id, "código de factura");
        Igual(109.75m, factura.Subtotal, "subtotal");
        Igual(16.46m, factura.Iva, "redondeo de IVA");
        Igual(126.21m, factura.Total, "total");
        Igual(factura.Id, taller.EmitirFactura(orden.Id).Id, "emisión idempotente");
        Rechaza(() => taller.GuardarIva(100.01m));
        taller.GuardarIva(12.50m);
        taller.AgregarServicioAOrden(orden.Id, servicio.Id);
        Igual(126.21m, taller.ConsultarFacturacion(orden.Id).Total, "partidas emitidas inmutables");
        Igual(15m, taller.FacturaDeOrden(orden.Id)?.TasaIva, "IVA histórico");
        Igual(12.50m, taller.EmitirFactura(segundaOrden.Id).TasaIva, "IVA nuevo");
        var pago = taller.RegistrarPago(orden.Id, 25m, MetodoPago.Transferencia);
        Igual("PAG-001", pago.Id, "código de pago");
        Igual(101.21m, taller.ConsultarFacturacion(orden.Id).Saldo, "saldo tras pago parcial");
        Rechaza(() => taller.RegistrarPago(orden.Id, 101.22m, MetodoPago.Efectivo));
        taller.RegistrarPago(orden.Id, 101.21m, MetodoPago.TarjetaCredito);
        Igual(0m, taller.ConsultarFacturacion(orden.Id).Saldo, "liquidación");
        Igual("Pagada - Lista para entrega", taller.DetalleOrden(orden.Id).Orden.Estado, "estado tras liquidación");
        Rechaza(() => taller.RegistrarPago(orden.Id, 0.01m, MetodoPago.Efectivo));
        Caso("Una factura por orden, IVA configurable, redondeo, pagos y sobrepago");

        var tercerOrden = taller.CrearOrden(vehiculo, TipoTrabajo.Mecánica, "Prueba de pagos simultáneos");
        taller.GenerarDiagnostico(tercerOrden.Id, "Prueba", 10m);
        decimal saldo = taller.EmitirFactura(tercerOrden.Id).Saldo;
        int exitos = 0;
        Parallel.For(0, 2, _ =>
        {
            try
            {
                taller.RegistrarPago(tercerOrden.Id, saldo, MetodoPago.Efectivo);
                Interlocked.Increment(ref exitos);
            }
            catch (ReglaNegocioException) { }
        });
        Igual(1, exitos, "pago concurrente único");
        Igual(0m, taller.ConsultarFacturacion(tercerOrden.Id).Saldo, "liquidación concurrente");
        Caso("Dos intentos simultáneos de cobro dejan un solo pago");

        var reabierto = new ServicioTaller(archivo);
        Igual(12.50m, reabierto.ObtenerIva(), "IVA tras reinicio");
        Igual(2, reabierto.FacturaDeOrden(orden.Id)?.Pagos.Count, "pagos tras reinicio");
        Igual(12, reabierto.Repuestos().Single(p => p.Id == filtro.Id).Stock, "stock tras reinicio");
        Igual(RolEmpleado.Mecánico, reabierto.Empleados().Single().Rol, "rol tras reinicio");
        string reporte = reabierto.GenerarReporte();
        if (!reporte.Contains("Compras registradas: 1") || !reporte.Contains("Facturas emitidas: 3"))
            throw new InvalidOperationException("El reporte no coincide con las operaciones guardadas.");
        string copia = Path.Combine(ruta, "copia.sqlite");
        reabierto.CrearCopia(copia);
        Igual(3, new ServicioTaller(copia).Ordenes().Count, "copia de seguridad restaurable");
        Caso("Reapertura de SQLite, reporte y copia de seguridad");

        string comprobante = Path.Combine(ruta, factura.Id + ".pdf");
        factura = reabierto.FacturaDeOrden(orden.Id)!;
        GeneradorPdf.GenerarFactura(factura, comprobante);
        ComprobarPdf(comprobante, $"Comprobante de servicio {factura.Id}");
        string informe = Path.Combine(ruta, "Reporte.pdf");
        GeneradorPdf.GenerarReporte(reporte, informe);
        ComprobarPdf(informe, "Reporte general del taller");
        string multipagina = Path.Combine(ruta, "Comprobante-multipagina.pdf");
        GeneradorPdf.GenerarFactura(factura with { Partidas = Enumerable.Repeat(factura.Partidas[0], 100).ToList() },
            multipagina);
        using var pdf = PdfReader.Open(multipagina, PdfDocumentOpenMode.Import);
        if (pdf.PageCount < 2)
            throw new InvalidOperationException("Se esperaba paginación en el comprobante extenso.");
        Caso("PDF de factura, reporte y comprobante de varias páginas");

        int Stock(string id) => taller.Repuestos().Single(item => item.Id == id).Stock;
    }

    private static void VerificarImportacion(string ruta, string json)
    {
        string destino = Path.Combine(ruta, "archivo-java.sqlite");
        ServicioTaller.ConvertirDatosJava(json, destino);
        var nuevo = new ServicioTaller(destino);
        Igual("José «Prueba»", nuevo.Clientes().Single().Nombres, "Unicode en el historial");
        var vehiculo = nuevo.Vehiculos().Single();
        Igual("LEG-015", vehiculo.Placa, "vehículo recuperado a partir de la orden");
        Igual("ORD-015", nuevo.Ordenes().Single().Id, "código de orden importado");
        Igual(EstadoCotizacion.Aprobada, nuevo.DetalleOrden("ORD-015").Cotizacion?.Estado, "cotización importada");
        Igual(EstadoCita.Atendida, nuevo.Citas().Single().Estado, "cita importada");
        Igual(15.50m, nuevo.Compras().Single().Total, "compra importada");
        Igual(RolEmpleado.Mecánico, nuevo.Empleados().Single().Rol, "rol importado");
        Igual("FAC-012", nuevo.FacturaDeOrden("ORD-015")?.Id, "código de factura importado");
        Igual(36m, nuevo.FacturaDeOrden("ORD-015")?.Saldo, "saldo importado");
        Igual("ORD-018", nuevo.CrearOrden(vehiculo, TipoTrabajo.Mecánica, "").Id, "continuidad de secuencia importada");
        Rechaza(() => ServicioTaller.ConvertirDatosJava(json, destino));
        var documento = JsonNode.Parse(File.ReadAllText(json))!;
        documento["datos"]!["facturas"]![0]!["subtotal"] = 999;
        string divergente = Path.Combine(ruta, "muestra-inconsistente.json");
        File.WriteAllText(divergente, documento.ToJsonString());
        string sinPublicar = Path.Combine(ruta, "importacion-inconsistente.sqlite");
        Rechaza(() => ServicioTaller.ConvertirDatosJava(divergente, sinPublicar));
        Igual(false, File.Exists(sinPublicar), "sin base parcial si el archivo histórico es inconsistente");
        Caso("Conversión de archivo Java con relaciones, saldos y secuencias históricas");
    }

    private static void ComprobarPdf(string ruta, string titulo)
    {
        using var pdf = PdfReader.Open(ruta, PdfDocumentOpenMode.Import);
        Igual(titulo, pdf.Info.Title, "título del PDF");
        if (pdf.PageCount == 0)
            throw new InvalidOperationException("El PDF no contiene páginas.");
    }

    private static void Rechaza(Action accion)
    {
        try { accion(); }
        catch (ReglaNegocioException) { return; }
        throw new InvalidOperationException("Se esperaba un rechazo por una regla de negocio.");
    }

    private static void Igual<T>(T esperado, T actual, string caso)
    {
        if (!EqualityComparer<T>.Default.Equals(esperado, actual))
            throw new InvalidOperationException($"{caso}: se esperaba «{esperado}» y se obtuvo «{actual}».");
    }

    private static void Caso(string nombre) => Console.WriteLine($"OK · {nombre}");
}
