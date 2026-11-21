using System.Globalization;
using AutoServiceManager.Modelos;
using AutoServiceManager.Persistencia;
using Microsoft.Data.Sqlite;

namespace AutoServiceManager.Servicios;

public sealed partial class ServicioTaller
{
    private readonly BaseDatos datos;

    public ServicioTaller(string ruta, bool inventarioInicial = true) =>
        datos = new BaseDatos(ruta, inventarioInicial);

    public static string RutaPredeterminada => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "AutoServiceManager", "autoservice.sqlite");

    public string RutaDatos => datos.Ruta;
    public void CrearCopia(string destino) => datos.CrearCopia(destino);

    private static string Requerido(string? texto, string campo)
    {
        if (string.IsNullOrWhiteSpace(texto))
            throw new ReglaNegocioException($"Completa el campo «{campo}».");
        return texto.Trim();
    }

    private static string Clave(string? texto, string campo) => Requerido(texto, campo).ToUpperInvariant();

    private static long Centavos(decimal importe) =>
        decimal.ToInt64(decimal.Round(importe, 2, MidpointRounding.AwayFromZero) * 100);

    private static long ImporteValido(decimal importe, string campo, bool admiteCero = true)
    {
        if (importe < 0 || importe > 999_999_999.99m)
            throw new ReglaNegocioException($"«{campo}» debe estar entre 0 y 999 999 999,99.");
        long valor = Centavos(importe);
        if (!admiteCero && valor <= 0)
            throw new ReglaNegocioException($"«{campo}» debe ser mayor que cero.");
        return valor;
    }

    private static decimal Dinero(SqliteDataReader fila, int columna) => fila.GetInt64(columna) / 100m;
    private static DateTimeOffset Fecha(SqliteDataReader fila, int columna) =>
        DateTimeOffset.ParseExact(fila.GetString(columna), "O", CultureInfo.InvariantCulture);

    private static T Opcion<T>(T valor) where T : struct, Enum =>
        Enum.IsDefined(valor) ? valor : throw new ReglaNegocioException("Selecciona una opción válida.");

    private static void DebeExistir(SesionSql sql, string consulta, string id, string mensaje)
    {
        if (sql.Entero(consulta, id) == 0)
            throw new ReglaNegocioException(mensaje);
    }
}
