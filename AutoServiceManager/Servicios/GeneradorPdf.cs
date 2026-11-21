using System.Globalization;
using AutoServiceManager.Modelos;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using Color = MigraDoc.DocumentObjectModel.Color;

namespace AutoServiceManager.Servicios;

public static class GeneradorPdf
{
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("es-EC");
    private static readonly Color Azul = Color.FromRgb(23, 50, 77);

    public static void GenerarFactura(Factura factura, string ruta)
    {
        var documento = CrearDocumento("Comprobante de servicio " + factura.Id);
        var seccion = CrearSeccion(documento);
        Encabezado(seccion, "COMPROBANTE DE SERVICIO " + factura.Id);
        seccion.AddParagraph($"Orden {factura.OrdenId}    ·    Emitido {Fecha(factura.Fecha)}");
        seccion.AddParagraph($"Cliente: {factura.Cliente.Nombres}    ·    C.I. {factura.Cliente.Cedula}");
        seccion.AddParagraph($"Contacto: {factura.Cliente.Telefono}    ·    {factura.Cliente.Correo}");
        seccion.AddParagraph($"Vehículo: {factura.Vehiculo.Marca} {factura.Vehiculo.Modelo} ({factura.Vehiculo.Anio})    ·    {factura.Vehiculo.Placa}");

        var detalle = CrearTabla(seccion, [2.25, 7.7, 1.45, 2.6, 3.0],
            "Tipo", "Descripción", "Cant.", "Unitario", "Importe");
        foreach (var partida in factura.Partidas)
        {
            string descripcion = partida.Categoria == partida.Tipo
                ? partida.Nombre : $"{partida.Nombre} ({partida.Categoria})";
            Fila(detalle, partida.Tipo, descripcion, partida.Cantidad.ToString(Cultura),
                Moneda(partida.Precio), Moneda(partida.Importe));
        }

        var totales = CrearTabla(seccion, [6, 4], "Concepto", "Valor");
        Fila(totales, "Subtotal", Moneda(factura.Subtotal));
        Fila(totales, $"IVA ({factura.TasaIva.ToString("0.##", Cultura)} %)", Moneda(factura.Iva));
        Fila(totales, "Total a cancelar", Moneda(factura.Total));
        Fila(totales, "Total pagado", Moneda(factura.Pagado));
        Fila(totales, "Saldo pendiente", Moneda(factura.Saldo));
        if (factura.Pagos.Count > 0)
        {
            var pagos = CrearTabla(seccion, [3.3, 4.5, 5.4, 3.8], "Comprobante", "Fecha", "Método", "Monto");
            foreach (var pago in factura.Pagos)
                Fila(pagos, pago.Id, Fecha(pago.Fecha), EtiquetaPago(pago.Metodo), Moneda(pago.Monto));
        }
        Guardar(documento, ruta);
    }

    public static void GenerarReporte(string reporte, string ruta)
    {
        var documento = CrearDocumento("Reporte general del taller");
        var seccion = CrearSeccion(documento);
        Encabezado(seccion, "REPORTE GENERAL DEL TALLER");
        foreach (string linea in reporte.ReplaceLineEndings("\n").Split('\n').Skip(1))
        {
            var parrafo = seccion.AddParagraph(linea);
            if (linea.Length > 0 && linea == linea.ToUpperInvariant())
            {
                parrafo.Format.Font.Bold = true;
                parrafo.Format.Font.Color = Azul;
                parrafo.Format.KeepWithNext = true;
            }
        }
        Guardar(documento, ruta);
    }

    public static string EtiquetaPago(MetodoPago metodo) => metodo == MetodoPago.TarjetaCredito
        ? "Tarjeta de crédito" : metodo.ToString();

    public static string CarpetaFacturas => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AutoServiceManager", "Facturas");

    private static Document CrearDocumento(string titulo)
    {
        var documento = new Document();
        documento.Info.Title = titulo;
        documento.Info.Author = "AutoService Manager";
        var normal = documento.Styles["Normal"]!;
        normal.Font.Name = "Arial";
        normal.Font.Size = 10;
        normal.ParagraphFormat.SpaceAfter = 4;
        return documento;
    }

    private static Section CrearSeccion(Document documento)
    {
        var seccion = documento.AddSection();
        seccion.PageSetup.PageFormat = PageFormat.A4;
        seccion.PageSetup.LeftMargin = Unit.FromCentimeter(2);
        seccion.PageSetup.RightMargin = Unit.FromCentimeter(2);
        return seccion;
    }

    private static void Encabezado(Section seccion, string texto)
    {
        var titulo = seccion.AddParagraph("AUTOSERVICE MANAGER");
        titulo.Format.Font.Size = 20;
        titulo.Format.Font.Bold = true;
        titulo.Format.Font.Color = Azul;
        titulo.Format.Alignment = ParagraphAlignment.Center;
        var subtitulo = seccion.AddParagraph(texto);
        subtitulo.Format.Alignment = ParagraphAlignment.Center;
        subtitulo.Format.SpaceAfter = 16;
    }

    private static Table CrearTabla(Section seccion, double[] anchos, params string[] columnas)
    {
        seccion.AddParagraph().Format.SpaceAfter = 3;
        var tabla = seccion.AddTable();
        tabla.TopPadding = 4;
        tabla.BottomPadding = 4;
        tabla.Borders.Width = 0.4;
        tabla.Borders.Color = Colors.LightGray;
        foreach (double ancho in anchos)
            tabla.AddColumn(Unit.FromCentimeter(ancho));
        var cabecera = Fila(tabla, columnas);
        cabecera.HeadingFormat = true;
        cabecera.Shading.Color = Azul;
        cabecera.Format.Font.Bold = true;
        cabecera.Format.Font.Color = Colors.White;
        return tabla;
    }

    private static Row Fila(Table tabla, params string[] campos)
    {
        var fila = tabla.AddRow();
        for (int columna = 0; columna < campos.Length; columna++)
            fila.Cells[columna].AddParagraph(campos[columna]);
        return fila;
    }

    private static void Guardar(Document documento, string ruta)
    {
        string destino = Path.GetFullPath(ruta);
        string carpeta = Path.GetDirectoryName(destino)!;
        Directory.CreateDirectory(carpeta);
        string temporal = Path.Combine(carpeta, $".{Guid.NewGuid():N}.tmp");
        try
        {
            var renderizador = new PdfDocumentRenderer { Document = documento };
            renderizador.RenderDocument();
            renderizador.PdfDocument.Save(temporal);
            File.Move(temporal, destino, overwrite: true);
        }
        finally
        {
            File.Delete(temporal);
        }
    }

    private static string Moneda(decimal monto) => "$ " + monto.ToString("N2", Cultura);
    private static string Fecha(DateTimeOffset fecha) => fecha.ToLocalTime().ToString("dd/MM/yyyy HH:mm", Cultura);
}
