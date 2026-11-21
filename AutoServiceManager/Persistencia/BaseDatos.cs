using System.Globalization;
using Microsoft.Data.Sqlite;

namespace AutoServiceManager.Persistencia;

internal sealed class BaseDatos
{
    private const int VersionEsquema = 1;
    private readonly string cadena;
    public string Ruta { get; }

    public BaseDatos(string ruta, bool inventarioInicial = true)
    {
        Ruta = Path.GetFullPath(ruta);
        Directory.CreateDirectory(Path.GetDirectoryName(Ruta)!);
        cadena = new SqliteConnectionStringBuilder
        {
            DataSource = Ruta,
            ForeignKeys = true,
            Pooling = false,
            DefaultTimeout = 5
        }.ToString();

        Escribir(sql =>
        {
            int version = (int)sql.Entero("PRAGMA user_version");
            if (version == VersionEsquema)
                return;
            if (version != 0)
                throw new InvalidOperationException($"La base de datos tiene una versión incompatible ({version}).");

            using Stream recurso = typeof(BaseDatos).Assembly
                .GetManifestResourceStream("AutoServiceManager.Esquema.sql")!;
            using var lector = new StreamReader(recurso);
            sql.Ejecutar(lector.ReadToEnd());
            if (inventarioInicial)
            {
                sql.Ejecutar("""
                    INSERT INTO repuestos VALUES
                        ('REP-001', 'Filtro de Aceite', 10, 1550),
                        ('REP-002', 'Pastillas de Freno', 5, 4500),
                        ('REP-003', 'Bujías (Set 4)', 20, 2500),
                        ('REP-004', 'Aceite de Motor 5W-30', 15, 3500)
                    """);
                sql.Ejecutar("INSERT INTO secuencias VALUES ('REP', 5)");
            }
            sql.Ejecutar($"PRAGMA user_version = {VersionEsquema}");
        });
    }

    public T Leer<T>(Func<SesionSql, T> consultar)
    {
        using var conexion = Abrir();
        using var transaccion = conexion.BeginTransaction(deferred: true);
        var resultado = consultar(new SesionSql(conexion, transaccion));
        transaccion.Commit();
        return resultado;
    }

    public T Escribir<T>(Func<SesionSql, T> modificar)
    {
        using var conexion = Abrir();
        using var transaccion = conexion.BeginTransaction();
        var resultado = modificar(new SesionSql(conexion, transaccion));
        transaccion.Commit();
        return resultado;
    }

    public void Escribir(Action<SesionSql> modificar) =>
        Escribir(sql =>
        {
            modificar(sql);
            return true;
        });

    public void CrearCopia(string ruta)
    {
        string destino = Path.GetFullPath(ruta);
        using (new FileStream(destino, FileMode.CreateNew)) { }
        try
        {
            using var origen = Abrir();
            using var copia = new SqliteConnection(new SqliteConnectionStringBuilder
            {
                DataSource = destino,
                Pooling = false
            }.ToString());
            copia.Open();
            origen.BackupDatabase(copia);
        }
        catch
        {
            File.Delete(destino);
            throw;
        }
    }

    private SqliteConnection Abrir()
    {
        var conexion = new SqliteConnection(cadena);
        try
        {
            conexion.Open();
            return conexion;
        }
        catch
        {
            conexion.Dispose();
            throw;
        }
    }
}

internal sealed class SesionSql(SqliteConnection conexion, SqliteTransaction transaccion)
{
    public int Ejecutar(string sql, params object?[] valores)
    {
        using var comando = Preparar(sql, valores);
        return comando.ExecuteNonQuery();
    }

    public long Entero(string sql, params object?[] valores)
    {
        using var comando = Preparar(sql, valores);
        return (long)(comando.ExecuteScalar() ?? 0L);
    }

    public List<T> Consultar<T>(string sql, Func<SqliteDataReader, T> mapear, params object?[] valores)
    {
        using var comando = Preparar(sql, valores);
        using var lector = comando.ExecuteReader();
        var resultado = new List<T>();
        while (lector.Read())
            resultado.Add(mapear(lector));
        return resultado;
    }

    public string SiguienteCodigo(string prefijo)
    {
        Ejecutar("INSERT OR IGNORE INTO secuencias VALUES ($0, 1)", prefijo);
        long numero = Entero("SELECT siguiente FROM secuencias WHERE prefijo = $0", prefijo);
        Ejecutar("UPDATE secuencias SET siguiente = siguiente + 1 WHERE prefijo = $0", prefijo);
        return $"{prefijo}-{numero.ToString("D3", CultureInfo.InvariantCulture)}";
    }

    private SqliteCommand Preparar(string sql, object?[] valores)
    {
        var comando = conexion.CreateCommand();
        comando.Transaction = transaccion;
        comando.CommandText = sql;
        for (int i = 0; i < valores.Length; i++)
        {
            object valor = valores[i] switch
            {
                null => DBNull.Value,
                DateTimeOffset fecha => fecha.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture),
                Enum opcion => opcion.ToString(),
                object otro => otro
            };
            comando.Parameters.AddWithValue($"${i}", valor);
        }
        return comando;
    }
}
