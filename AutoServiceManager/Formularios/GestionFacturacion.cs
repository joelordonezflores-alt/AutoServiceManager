using AutoServiceManager.Modelos;
using AutoServiceManager.Servicios;

namespace AutoServiceManager.Formularios;

internal sealed class GestionFacturacion : VentanaTaller
{
    private readonly ComboBox ordenes = Estilo.Selector();
    private readonly ComboBox metodos = Estilo.Selector();
    private readonly Label idFactura = Estilo.Etiqueta("Sin factura emitida");
    private readonly TextBox subtotal = new() { ReadOnly = true };
    private readonly TextBox iva = new() { ReadOnly = true };
    private readonly TextBox total = new() { ReadOnly = true };
    private readonly TextBox saldo = new() { ReadOnly = true };
    private readonly NumericUpDown monto = Estilo.Numero();
    private readonly Button emitir;
    private readonly Button registrar;
    private readonly Button pdf;
    private readonly DataGridView pagos = Estilo.Tabla(("id", "Pago", 70), ("fecha", "Fecha", 115),
        ("metodo", "Método", 110), ("monto", "Monto", 80));

    public GestionFacturacion(ServicioTaller taller) : base(taller, "Facturación y pagos", new Size(820, 660))
    {
        emitir = Estilo.Boton("Emitir factura", Emitir);
        registrar = Estilo.Boton("Registrar pago", Pagar, principal: true);
        pdf = Estilo.Boton("Generar y abrir PDF", Exportar);
        emitir.Enabled = registrar.Enabled = pdf.Enabled = false;
        var formularios = Estilo.Campos(("Orden", ordenes), ("Factura", idFactura), ("Subtotal", subtotal),
            ("IVA", iva), ("Total de la factura", total), ("Saldo pendiente", saldo), ("Monto a pagar ($)", monto),
            ("Método de pago", metodos));
        var cuerpo = Estilo.Secciones(formularios, pagos);
        cuerpo.MinimumSize = new Size(0, 475);
        Controls.Add(Estilo.Secciones(Estilo.Titulo("Facturación y pagos"), Estilo.Desplazable(cuerpo, expandirAltura: true),
            Estilo.Acciones(emitir, registrar, pdf, Estilo.Boton("Cerrar", Close))));
        ordenes.SelectedIndexChanged += (_, _) => Actualizar();
        Estilo.Cargar(metodos, Enum.GetValues<MetodoPago>(), GeneradorPdf.EtiquetaPago);
        VigilarBorrador(() => monto.Value > 0);
        Shown += (_, _) => Ejecutar(() => Estilo.Cargar(ordenes, taller.Ordenes(), item => item.Etiqueta, item => item.Id));
    }

    private void Emitir() => Ejecutar(() =>
    {
        var factura = Taller.EmitirFactura(OrdenActual().Id);
        Actualizar();
        Informar($"Factura {factura.Id} emitida.\nTotal: {Estilo.Dinero(factura.Total)}.");
    });

    private void Pagar() => Ejecutar(() =>
    {
        var orden = OrdenActual();
        var pago = Taller.RegistrarPago(orden.Id, monto.Value,
            Estilo.Obtener<MetodoPago>(metodos, "Selecciona el método de pago."));
        monto.Value = 0;
        Actualizar();
        Informar($"Pago {pago.Id} registrado por {Estilo.Dinero(pago.Monto)}.");
    });

    private void Exportar() => Ejecutar(() =>
    {
        var orden = OrdenActual();
        string id = Taller.ConsultarFacturacion(orden.Id).FacturaId ?? $"Comprobante-{orden.Id}";
        using var selector = new SaveFileDialog
        {
            Filter = "Documento PDF (*.pdf)|*.pdf", DefaultExt = "pdf", FileName = id + ".pdf",
            InitialDirectory = Directory.Exists(GeneradorPdf.CarpetaFacturas) ? GeneradorPdf.CarpetaFacturas
                : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };
        if (selector.ShowDialog(this) != DialogResult.OK)
            return;
        var factura = Taller.EmitirFactura(orden.Id);
        Actualizar();
        GeneradorPdf.GenerarFactura(factura, selector.FileName);
        Informar($"Comprobante {factura.Id} guardado en:\n{selector.FileName}");
        AbrirArchivo(selector.FileName);
    });

    private Orden OrdenActual() => Estilo.Obtener<Orden>(ordenes, "Selecciona una orden de trabajo.");

    private void Actualizar()
    {
        if (Estilo.Seleccion<Orden>(ordenes) is not { } orden)
            return;
        Ejecutar(() =>
        {
            var resumen = Taller.ConsultarFacturacion(orden.Id);
            idFactura.Text = resumen.FacturaId ?? "Sin factura emitida";
            subtotal.Text = Estilo.Dinero(resumen.Subtotal);
            iva.Text = $"{resumen.TasaIva:0.##} %  ·  {Estilo.Dinero(resumen.Iva)}";
            total.Text = Estilo.Dinero(resumen.Total);
            saldo.Text = Estilo.Dinero(resumen.Saldo);
            emitir.Enabled = resumen.FacturaId == null && resumen.Total > 0;
            registrar.Enabled = resumen.Saldo > 0;
            pdf.Enabled = resumen.FacturaId != null || resumen.Total > 0;
            var factura = Taller.FacturaDeOrden(orden.Id);
            Estilo.Llenar(pagos, factura?.Pagos ?? [], pago => [pago.Id, Estilo.Fecha(pago.Fecha),
                GeneradorPdf.EtiquetaPago(pago.Metodo), Estilo.Dinero(pago.Monto)]);
        });
    }
}
