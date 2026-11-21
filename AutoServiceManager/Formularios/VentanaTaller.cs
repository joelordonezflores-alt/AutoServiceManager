using System.Diagnostics;
using AutoServiceManager.Modelos;
using AutoServiceManager.Servicios;
using Microsoft.Data.Sqlite;

namespace AutoServiceManager.Formularios;

internal class VentanaTaller : Form
{
    private readonly List<Func<bool>> borradores = [];
    protected ServicioTaller Taller { get; }

    protected VentanaTaller(ServicioTaller taller, string titulo, Size tamano)
    {
        Taller = taller;
        Text = titulo;
        ClientSize = tamano;
        MinimumSize = new Size(520, 330);
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleDimensions = new SizeF(96, 96);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = new Font("Segoe UI", 10);
        BackColor = Estilo.Fondo;
        Padding = new Padding(14);
        ShowInTaskbar = false;
        MinimizeBox = false;
        Shown += (_, _) =>
        {
            Rectangle area = Screen.FromControl(this).WorkingArea;
            if (Width > area.Width || Height > area.Height)
                Bounds = new Rectangle(area.Location, new Size(Math.Min(Width, area.Width), Math.Min(Height, area.Height)));
        };
    }

    protected bool Ejecutar(Action accion)
    {
        try
        {
            accion();
            return true;
        }
        catch (ReglaNegocioException error)
        {
            MessageBox.Show(this, error.Message, "Revisa los datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception error) when (error is SqliteException or IOException or UnauthorizedAccessException
            or InvalidOperationException or System.ComponentModel.Win32Exception or ArgumentException
            or OverflowException or FormatException or System.Text.Json.JsonException)
        {
            MessageBox.Show(this, error.Message, "No se pudo completar la operación",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        return false;
    }

    protected void Informar(string texto) => MessageBox.Show(this, texto, Text,
        MessageBoxButtons.OK, MessageBoxIcon.Information);

    protected void VigilarBorrador(Func<bool> hayCambios) => borradores.Add(hayCambios);

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing && DialogResult != DialogResult.OK && borradores.Any(consultar => consultar()))
        {
            e.Cancel = MessageBox.Show(this, "¿Descartar los cambios de este formulario?",
                Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes;
        }
        base.OnFormClosing(e);
    }

    protected override bool ProcessDialogKey(Keys keyData)
    {
        if (keyData != Keys.Escape)
            return base.ProcessDialogKey(keyData);
        Close();
        return true;
    }

    protected static void AbrirArchivo(string ruta) =>
        Process.Start(new ProcessStartInfo(Path.GetFullPath(ruta)) { UseShellExecute = true });

    protected static T Fila<T>(DataGridView tabla, string mensaje) where T : class =>
        tabla.CurrentRow?.Tag as T ?? throw new ReglaNegocioException(mensaje);
}
