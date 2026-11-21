using AutoServiceManager.Modelos;
using AutoServiceManager.Persistencia;
using Microsoft.Data.Sqlite;

namespace AutoServiceManager.Servicios;

public sealed partial class ServicioTaller
{
    public static IReadOnlyList<string> EstadosAvance { get; } = Array.AsReadOnly(new[]
    {
        "En revisión mecánica", "Mecánica terminada", "En área de pintura",
        "Pintura terminada", "En área de lavado", "Listo para Entrega"
    });

    public static IReadOnlyList<RolEmpleado> RolesTaller { get; } =
        Array.AsReadOnly(new[] { RolEmpleado.Mecánico, RolEmpleado.Pintor, RolEmpleado.Lavador });

    public IReadOnlyList<Orden> Ordenes(string? placa = null) => datos.Leer(sql =>
        sql.Consultar(ConsultaOrdenes + " WHERE $0 IS NULL OR o.placa = $0 ORDER BY o.fecha_ingreso DESC, o.rowid DESC",
            LeerOrden, placa?.Trim()));

    public Orden CrearOrden(Vehiculo vehiculo, TipoTrabajo tipo, string observaciones, string? citaId = null)
    {
        Opcion(tipo);
        return datos.Escribir(sql =>
        {
            var asociado = AsegurarVehiculo(sql, vehiculo);
            Cita? cita = citaId == null ? null : BuscarCita(sql, citaId);
            if (cita != null && (cita.Estado is not (EstadoCita.Programada or EstadoCita.Confirmada)
                || !string.Equals(cita.Cedula, asociado.Cedula, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(cita.Placa, asociado.Placa, StringComparison.OrdinalIgnoreCase)))
                throw new ReglaNegocioException("La cita debe estar pendiente y corresponder al cliente y al vehículo seleccionados.");

            string id = sql.SiguienteCodigo("ORD");
            sql.Ejecutar("""
                INSERT INTO ordenes (id, fecha_ingreso, placa, estado, observaciones, tipo, cita_id)
                VALUES ($0,$1,$2,'Ingresado',$3,$4,$5)
                """, id, DateTimeOffset.Now, asociado.Placa, observaciones.Trim(), tipo, cita?.Id);
            if (cita != null)
                sql.Ejecutar("UPDATE citas SET estado = 'Atendida' WHERE id = $0", cita.Id);
            return BuscarOrden(sql, id);
        });
    }

    public DetalleOrden DetalleOrden(string id) => datos.Leer(sql =>
        new DetalleOrden(BuscarOrden(sql, id), PartidasOrden(sql, id), BuscarCotizacion(sql, id),
            sql.Consultar("""
                SELECT fecha, rol, estado, costo FROM avances WHERE orden_id = $0 ORDER BY id
                """, fila => new Avance(Fecha(fila, 0), fila.GetString(1), fila.GetString(2), Dinero(fila, 3)), id)));

    public void AgregarRepuestoAOrden(string ordenId, string repuestoId, int cantidad = 1)
    {
        if (cantidad <= 0)
            throw new ReglaNegocioException("La cantidad debe ser mayor que cero.");
        datos.Escribir(sql =>
        {
            BuscarOrden(sql, ordenId);
            var repuesto = BuscarRepuesto(sql, repuestoId);
            if (sql.Ejecutar("UPDATE repuestos SET stock = stock - $1 WHERE id = $0 AND stock >= $1",
                repuesto.Id, cantidad) != 1)
                throw new ReglaNegocioException("No hay stock suficiente de este repuesto.");
            AgregarPartida(sql, ordenId, new("Repuesto", repuesto.Id, repuesto.Nombre, "Repuesto", cantidad, repuesto.Precio));
        });
    }

    public void AgregarServicioAOrden(string ordenId, string servicioId) => datos.Escribir(sql =>
    {
        BuscarOrden(sql, ordenId);
        var servicio = sql.Consultar("""
            SELECT id, nombre, categoria, precio FROM servicios WHERE id = $0
            """, LeerServicio, servicioId).SingleOrDefault()
            ?? throw new ReglaNegocioException("El servicio seleccionado ya no está disponible.");
        AgregarPartida(sql, ordenId, new("Servicio", servicio.Id, servicio.Nombre, servicio.Categoria, 1, servicio.Precio));
    });

    public Cotizacion GenerarDiagnostico(string ordenId, string diagnostico, decimal manoObra)
    {
        diagnostico = Requerido(diagnostico, "Diagnóstico");
        long costo = ImporteValido(manoObra, "Mano de obra");
        return datos.Escribir(sql =>
        {
            var orden = BuscarOrden(sql, ordenId);
            decimal repuestos = PartidasOrden(sql, orden.Id).Where(p => p.Tipo == "Repuesto").Sum(p => p.Importe);
            string id = $"COT-DIAG-{orden.Id}";
            sql.Ejecutar("DELETE FROM partidas_orden WHERE orden_id = $0 AND origen = 'Diagnostico'", orden.Id);
            AgregarPartida(sql, orden.Id,
                new("Servicio", "SRV-MEC", "Mano de Obra Mecánica", "Mecánica", 1, costo / 100m), "Diagnostico");
            sql.Ejecutar("""
                INSERT INTO cotizaciones VALUES ($0,$1,$2,$3,$4,$5,'Pendiente')
                ON CONFLICT(orden_id) DO UPDATE SET fecha=excluded.fecha, diagnostico=excluded.diagnostico,
                    mano_obra=excluded.mano_obra, estimado=excluded.estimado, estado=excluded.estado
                """, orden.Id, id, DateTimeOffset.Now, diagnostico, costo, Centavos(repuestos) + costo);
            ActualizarEstadoOrden(sql, orden.Id, "Diagnosticado - Esperando Aprobación");
            return BuscarCotizacion(sql, orden.Id)!;
        });
    }

    public void ResolverCotizacion(string ordenId, EstadoCotizacion estado)
    {
        if (estado is not (EstadoCotizacion.Aprobada or EstadoCotizacion.Rechazada))
            throw new ReglaNegocioException("Selecciona si el cliente aprueba o rechaza la cotización.");
        datos.Escribir(sql =>
        {
            if (sql.Ejecutar("""
                UPDATE cotizaciones SET estado = $1 WHERE orden_id = $0 AND estado = 'Pendiente'
                """, ordenId, estado) != 1)
                throw new ReglaNegocioException("La orden no tiene una cotización pendiente.");
            ActualizarEstadoOrden(sql, ordenId, estado == EstadoCotizacion.Aprobada
                ? "Cotización aprobada - En reparación" : "Cotización rechazada");
        });
    }

    public void RegistrarAvance(string ordenId, RolEmpleado rol, string estado, decimal costo)
    {
        if (!RolesTaller.Contains(rol) || !EstadosAvance.Contains(estado))
            throw new ReglaNegocioException("Selecciona el rol y el estado del avance.");
        long valor = ImporteValido(costo, "Costo del trabajo");
        datos.Escribir(sql =>
        {
            BuscarOrden(sql, ordenId);
            sql.Ejecutar("""
                INSERT INTO avances (orden_id, fecha, rol, estado, costo) VALUES ($0,$1,$2,$3,$4)
                """, ordenId, DateTimeOffset.Now, rol, estado, valor);
            if (valor > 0)
                AgregarPartida(sql, ordenId, new("Servicio", $"SRV-{rol.ToString().ToUpperInvariant()}",
                    $"Servicio de {rol}", rol.ToString(), 1, valor / 100m));
            ActualizarEstadoOrden(sql, ordenId, estado);
        });
    }

    private const string ConsultaOrdenes = """
        SELECT o.id, o.fecha_ingreso, o.fecha_entrega, o.placa, c.nombres, o.estado, o.observaciones, o.tipo, o.cita_id
        FROM ordenes o JOIN vehiculos v ON v.placa = o.placa JOIN clientes c ON c.cedula = v.cedula
        """;

    private static Orden BuscarOrden(SesionSql sql, string id) =>
        sql.Consultar(ConsultaOrdenes + " WHERE o.id = $0", LeerOrden, id).SingleOrDefault()
        ?? throw new ReglaNegocioException("La orden seleccionada ya no está disponible.");

    private static Orden LeerOrden(SqliteDataReader fila) =>
        new(fila.GetString(0), Fecha(fila, 1), fila.IsDBNull(2) ? null : Fecha(fila, 2), fila.GetString(3),
            fila.GetString(4), fila.GetString(5), fila.GetString(6), fila.GetString(7), fila.IsDBNull(8) ? null : fila.GetString(8));

    private static void ActualizarEstadoOrden(SesionSql sql, string ordenId, string estado) =>
        sql.Ejecutar("UPDATE ordenes SET estado = $1 WHERE id = $0", ordenId, estado);

    private static Cotizacion? BuscarCotizacion(SesionSql sql, string id) => sql.Consultar("""
        SELECT id, fecha, diagnostico, mano_obra, estimado, estado FROM cotizaciones WHERE orden_id = $0
        """, fila => new Cotizacion(fila.GetString(0), Fecha(fila, 1), fila.GetString(2), Dinero(fila, 3),
            Dinero(fila, 4), Enum.Parse<EstadoCotizacion>(fila.GetString(5))), id).SingleOrDefault();

    private static List<Partida> PartidasOrden(SesionSql sql, string id) => sql.Consultar("""
        SELECT tipo, codigo, nombre, categoria, cantidad, precio FROM partidas_orden WHERE orden_id = $0 ORDER BY id
        """, LeerPartida, id);

    private static Partida LeerPartida(SqliteDataReader fila) => new(fila.GetString(0), fila.GetString(1),
        fila.GetString(2), fila.GetString(3), fila.GetInt32(4), Dinero(fila, 5));

    private static void AgregarPartida(SesionSql sql, string ordenId, Partida partida, string origen = "") =>
        sql.Ejecutar("""
            INSERT INTO partidas_orden (orden_id,tipo,codigo,nombre,categoria,cantidad,precio,origen)
            VALUES ($0,$1,$2,$3,$4,$5,$6,$7)
            """, ordenId, partida.Tipo, partida.Codigo, partida.Nombre, partida.Categoria,
            partida.Cantidad, Centavos(partida.Precio), origen);
}
