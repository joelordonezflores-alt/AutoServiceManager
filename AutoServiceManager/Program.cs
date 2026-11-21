using System.Globalization;
using AutoServiceManager.Servicios;
using Microsoft.Data.Sqlite;

namespace AutoServiceManager
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("es-EC");
            ApplicationConfiguration.Initialize();
            try
            {
                string ruta = args switch
                {
                    [] => ServicioTaller.RutaPredeterminada,
                    ["--datos", string archivo] when File.Exists(archivo) => archivo,
                    ["--datos", _] => throw new FileNotFoundException("La base de datos indicada no existe."),
                    _ => throw new ArgumentException("Uso: AutoServiceManager.exe [--datos ruta.sqlite]")
                };
                var taller = new ServicioTaller(ruta);
                Application.Run(new Form1(taller));
            }
            catch (Exception error) when (error is SqliteException or IOException or UnauthorizedAccessException
                or InvalidOperationException or ArgumentException)
            {
                MessageBox.Show($"No se pudo iniciar AutoService Manager.\n\n{error.Message}",
                    "Error de inicio", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
