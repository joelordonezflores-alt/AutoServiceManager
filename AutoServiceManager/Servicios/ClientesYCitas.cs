using AutoServiceManager.Modelos;
using AutoServiceManager.Persistencia;
using Microsoft.Data.Sqlite;

namespace AutoServiceManager.Servicios;

public sealed partial class ServicioTaller
{
    public IReadOnlyList<Cliente> Clientes() => datos.Leer(sql => sql.Consultar("""
        SELECT cedula, nombres, telefono, direccion, correo FROM clientes ORDER BY nombres, cedula
        """, LeerCliente));

    public IReadOnlyList<Vehiculo> Vehiculos(string? cedula = null) => datos.Leer(sql => sql.Consultar("""
        SELECT placa, marca, modelo, anio, cedula FROM vehiculos
        WHERE $0 IS NULL OR cedula = $0 ORDER BY placa
        """, LeerVehiculo, cedula?.Trim()));

    public Cliente RegistrarCliente(string cedula, string nombres, string telefono, string direccion, string correo)
    {
        var cliente = new Cliente(Clave(cedula, "Cédula"), Requerido(nombres, "Nombres"),
            Requerido(telefono, "Teléfono"), Requerido(direccion, "Dirección"), Requerido(correo, "Correo"));
        return datos.Escribir(sql =>
        {
            if (sql.Ejecutar("""
                INSERT INTO clientes VALUES ($0,$1,$2,$3,$4) ON CONFLICT(cedula) DO NOTHING
                """, cliente.Cedula, cliente.Nombres, cliente.Telefono, cliente.Direccion, cliente.Correo) == 0)
                throw new ReglaNegocioException("Ya existe un cliente con esa cédula.");
            return cliente;
        });
    }

    public Vehiculo RegistrarVehiculo(Vehiculo vehiculo) =>
        datos.Escribir(sql => AsegurarVehiculo(sql, vehiculo, soloNuevo: true));

    public IReadOnlyList<Cita> Citas() => datos.Leer(sql =>
        sql.Consultar(ConsultaCitas + " ORDER BY c.fecha, c.id", LeerCita));

    public Cita? CitaPendiente(string cedula, string? placa = null) => datos.Leer(sql =>
        sql.Consultar(ConsultaCitas + """

            WHERE c.cedula = $0 AND ($1 IS NULL OR c.placa = $1)
              AND c.estado IN ('Programada', 'Confirmada')
            ORDER BY c.fecha DESC, c.rowid DESC LIMIT 1
            """, LeerCita, cedula, placa).SingleOrDefault());

    public Cita AgendarCita(Vehiculo vehiculo, DateTimeOffset fecha, string motivo)
    {
        motivo = Requerido(motivo, "Motivo");
        return datos.Escribir(sql =>
        {
            var asociado = AsegurarVehiculo(sql, vehiculo);
            string id = sql.SiguienteCodigo("CIT");
            sql.Ejecutar("INSERT INTO citas VALUES ($0,$1,$2,$3,$4,$5)",
                id, fecha, asociado.Cedula, asociado.Placa, motivo, EstadoCita.Programada);
            return BuscarCita(sql, id);
        });
    }

    public void CambiarEstadoCita(string id, EstadoCita estado) => datos.Escribir(sql =>
    {
        var cita = BuscarCita(sql, id);
        if (cita.Estado == estado)
            return;
        bool valida = (cita.Estado, estado) switch
        {
            (EstadoCita.Programada, EstadoCita.Confirmada or EstadoCita.Atendida or EstadoCita.Cancelada) => true,
            (EstadoCita.Confirmada, EstadoCita.Atendida or EstadoCita.Cancelada) => true,
            _ => false
        };
        if (!valida)
            throw new ReglaNegocioException($"Una cita {cita.Estado.ToString().ToLowerInvariant()} no puede pasar a {estado.ToString().ToLowerInvariant()}.");
        sql.Ejecutar("UPDATE citas SET estado = $1 WHERE id = $0", id, estado);
    });

    private const string ConsultaCitas = """
        SELECT c.id, c.fecha, c.cedula, cl.nombres, c.placa, c.motivo, c.estado
        FROM citas c JOIN clientes cl ON cl.cedula = c.cedula
        """;

    private static Cita BuscarCita(SesionSql sql, string id) =>
        sql.Consultar(ConsultaCitas + " WHERE c.id = $0", LeerCita, id).SingleOrDefault()
        ?? throw new ReglaNegocioException("La cita seleccionada ya no está disponible.");

    private static Cita LeerCita(SqliteDataReader fila) =>
        new(fila.GetString(0), Fecha(fila, 1), fila.GetString(2), fila.GetString(3),
            fila.GetString(4), fila.GetString(5), Enum.Parse<EstadoCita>(fila.GetString(6)));

    private static Cliente LeerCliente(SqliteDataReader fila) =>
        new(fila.GetString(0), fila.GetString(1), fila.GetString(2), fila.GetString(3), fila.GetString(4));

    private static Vehiculo LeerVehiculo(SqliteDataReader fila) =>
        new(fila.GetString(0), fila.GetString(1), fila.GetString(2), fila.GetInt32(3), fila.GetString(4));

    private static Vehiculo AsegurarVehiculo(SesionSql sql, Vehiculo vehiculo, bool soloNuevo = false)
    {
        string placa = Clave(vehiculo.Placa, "Placa");
        string cedula = Clave(vehiculo.Cedula, "Cliente");
        DebeExistir(sql, "SELECT EXISTS(SELECT 1 FROM clientes WHERE cedula = $0)", cedula, "Selecciona un cliente registrado.");
        var existente = sql.Consultar("""
            SELECT placa, marca, modelo, anio, cedula FROM vehiculos WHERE placa = $0
            """, LeerVehiculo, placa).SingleOrDefault();
        if (existente != null)
        {
            if (!string.Equals(existente.Cedula, cedula, StringComparison.OrdinalIgnoreCase))
                throw new ReglaNegocioException("La placa ya está registrada para otro cliente.");
            if (soloNuevo)
                throw new ReglaNegocioException("El vehículo ya está registrado.");
            return existente;
        }

        if (vehiculo.Anio is < 1900 or > 2100)
            throw new ReglaNegocioException("El año debe estar entre 1900 y 2100.");
        var nuevo = new Vehiculo(placa, Requerido(vehiculo.Marca, "Marca"),
            Requerido(vehiculo.Modelo, "Modelo"), vehiculo.Anio, cedula);
        sql.Ejecutar("INSERT INTO vehiculos VALUES ($0,$1,$2,$3,$4)",
            nuevo.Placa, nuevo.Marca, nuevo.Modelo, nuevo.Anio, nuevo.Cedula);
        return nuevo;
    }
}
