using AutoServiceManager.Modelos;
using AutoServiceManager.Servicios;

namespace AutoServiceManager.Formularios;

internal sealed class CrearOrden : VentanaTaller
{
    private readonly EditorVehiculo editor;
    private readonly ComboBox tipo = Estilo.Selector();
    private readonly TextBox observaciones = Estilo.TextoLargo(85);
    private readonly Label citaPrevia = Estilo.Etiqueta("Sin cita pendiente");
    private Cita? cita;
    private bool seleccionando;

    public CrearOrden(ServicioTaller taller) : base(taller, "Crear nueva orden de trabajo", new Size(660, 590))
    {
        editor = new EditorVehiculo(taller);
        Estilo.Cargar(tipo, Enum.GetValues<TipoTrabajo>(), elemento => elemento.ToString());
        editor.ClienteCambiado += PrecargarCita;
        editor.VehiculoCambiado += CitaDelVehiculo;
        var crear = Estilo.Boton("Crear orden", () => Ejecutar(() =>
        {
            var orden = Taller.CrearOrden(editor.Obtener(), Estilo.Obtener<TipoTrabajo>(tipo, "Selecciona el tipo de trabajo."),
                observaciones.Text, cita?.Id);
            Informar($"Orden creada: {orden.Id}.");
            DialogResult = DialogResult.OK;
        }), principal: true);
        Controls.Add(Estilo.Secciones(Estilo.Titulo("Recepción del vehículo"),
            Estilo.Desplazable(editor.Campos(("Cita previa", citaPrevia), ("Tipo de trabajo", tipo),
                ("Problema reportado", observaciones))),
            Estilo.Acciones(crear, Estilo.Boton("Cancelar", Close))));
        VigilarBorrador(() => editor.TieneDatosNuevos || observaciones.Text != (cita?.Motivo ?? ""));
        Shown += (_, _) => Ejecutar(() => editor.CargarClientes());
    }

    private void PrecargarCita() => Ejecutar(() =>
    {
        var cliente = editor.Cliente;
        var pendiente = cliente == null ? null : Taller.CitaPendiente(cliente.Cedula);
        if (pendiente != null)
        {
            seleccionando = true;
            try { editor.ElegirVehiculo(pendiente.Placa); }
            finally { seleccionando = false; }
        }
        MostrarCita(pendiente);
    });

    private void CitaDelVehiculo()
    {
        if (!seleccionando)
            Ejecutar(() =>
            {
                var vehiculo = editor.Existente;
                MostrarCita(vehiculo == null ? null : Taller.CitaPendiente(vehiculo.Cedula, vehiculo.Placa));
            });
    }

    private void MostrarCita(Cita? pendiente)
    {
        if (pendiente?.Id == cita?.Id)
            return;
        if (pendiente == null)
        {
            if (cita != null && observaciones.Text == cita.Motivo)
                observaciones.Clear();
            citaPrevia.Text = "Sin cita pendiente para el vehículo.";
        }
        else
        {
            observaciones.Text = pendiente.Motivo;
            citaPrevia.Text = $"{pendiente.Id} · {Estilo.Fecha(pendiente.Fecha)}";
            TipoTrabajo trabajo = pendiente.Motivo.ToLowerInvariant() switch
            {
                string texto when texto.Contains("pint") => TipoTrabajo.Pintura,
                string texto when texto.Contains("lav") => TipoTrabajo.Lavado,
                _ => TipoTrabajo.Mecánica
            };
            Estilo.Seleccionar<TipoTrabajo>(tipo, trabajo.ToString());
        }
        cita = pendiente;
    }
}

internal sealed class ListaOrdenes : VentanaTaller
{
    private readonly DataGridView tabla = Estilo.Tabla(("id", "Orden", 60), ("placa", "Placa", 65),
        ("dueno", "Propietario", 100), ("fecha", "Ingreso", 90), ("estado", "Estado", 130),
        ("tipo", "Trabajo", 75), ("detalle", "Observaciones", 145));
    private readonly string? placa;

    public ListaOrdenes(ServicioTaller taller, string? placa = null)
        : base(taller, "Estado de vehículos en el taller", new Size(1040, 550))
    {
        this.placa = placa;
        var titulo = placa == null ? "Estado actual de vehículos en el taller" : $"Historial del vehículo {placa}";
        Controls.Add(Estilo.Secciones(Estilo.Titulo(titulo), tabla, Estilo.Acciones(
            Estilo.Boton("Ver detalle", VerDetalle, principal: true), Estilo.Boton("Refrescar lista", Actualizar),
            Estilo.Boton("Cerrar", Close))));
        tabla.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) VerDetalle(); };
        Shown += (_, _) => Actualizar();
    }

    private void Actualizar() => Ejecutar(() => Estilo.Llenar(tabla, Taller.Ordenes(placa), orden =>
        [orden.Id, orden.Placa, orden.Cliente, Estilo.Fecha(orden.FechaIngreso), orden.Estado, orden.Tipo, orden.Observaciones]));

    private void VerDetalle() => Ejecutar(() =>
    {
        var orden = Fila<Orden>(tabla, "Selecciona una orden en la tabla.");
        using var detalle = new ConsultaOrden(Taller, orden.Id);
        detalle.ShowDialog(this);
    });
}

internal sealed class ConsultaOrden : VentanaTaller
{
    public ConsultaOrden(ServicioTaller taller, string id) : base(taller, $"Detalle de {id}", new Size(850, 640))
    {
        var partes = Estilo.Tabla(("tipo", "Tipo", 70), ("nombre", "Detalle", 180), ("cantidad", "Cantidad", 60),
            ("precio", "Unitario", 85), ("importe", "Importe", 85));
        var avances = Estilo.Tabla(("fecha", "Fecha", 105), ("rol", "Rol", 80),
            ("estado", "Estado", 135), ("costo", "Cargo", 80));
        var resumen = Estilo.TextoLargo(130, lectura: true);
        resumen.Dock = DockStyle.Fill;
        var pestanas = new TabControl();
        pestanas.TabPages.Add(Estilo.Pestana("Resumen y cotización", resumen));
        pestanas.TabPages.Add(Estilo.Pestana("Servicios y repuestos", partes));
        pestanas.TabPages.Add(Estilo.Pestana("Avances", avances));
        Controls.Add(Estilo.Secciones(Estilo.Titulo(id), pestanas, Estilo.Acciones(Estilo.Boton("Cerrar", Close))));
        Shown += (_, _) => Ejecutar(() =>
        {
            var detalle = taller.DetalleOrden(id);
            var orden = detalle.Orden;
            resumen.Text = $"""
                Placa: {orden.Placa}
                Cliente: {orden.Cliente}
                Ingreso: {Estilo.Fecha(orden.FechaIngreso)}
                Entrega: {(orden.FechaEntrega is { } entrega ? Estilo.Fecha(entrega) : "Sin fecha registrada")}
                Estado: {orden.Estado}
                Tipo de trabajo: {orden.Tipo}
                Cita: {orden.CitaId ?? "Sin cita"}

                Observaciones:
                {orden.Observaciones}
                """.ReplaceLineEndings();
            if (detalle.Cotizacion is { } cotizacion)
                resumen.AppendText($"{Environment.NewLine}{Environment.NewLine}Cotización {cotizacion.Id} · {cotizacion.Estado}" +
                    $"{Environment.NewLine}Estimado: {Estilo.Dinero(cotizacion.Estimado)}" +
                    $"{Environment.NewLine}Diagnóstico: {cotizacion.Diagnostico}");
            Estilo.Llenar(partes, detalle.Partidas, partida => [partida.Tipo, partida.Nombre, partida.Cantidad,
                Estilo.Dinero(partida.Precio), Estilo.Dinero(partida.Importe)]);
            Estilo.Llenar(avances, detalle.Avances, avance => [Estilo.Fecha(avance.Fecha), avance.Rol, avance.Estado,
                Estilo.Dinero(avance.Costo)]);
        });
    }
}
