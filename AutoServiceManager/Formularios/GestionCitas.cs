using AutoServiceManager.Modelos;
using AutoServiceManager.Servicios;

namespace AutoServiceManager.Formularios;

internal sealed class GestionCitas : VentanaTaller
{
    private readonly DataGridView tabla = Estilo.Tabla(("id", "Cita", 60), ("fecha", "Fecha y hora", 95),
        ("cliente", "Cliente", 110), ("placa", "Placa", 65), ("motivo", "Motivo", 145), ("estado", "Estado", 80));

    public GestionCitas(ServicioTaller taller) : base(taller, "Gestión de citas y vehículos", new Size(855, 545))
    {
        var editor = new EditorVehiculo(taller);
        var fecha = new DateTimePicker
        {
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "dd/MM/yyyy HH:mm",
            Value = DateTime.Now
        };
        var motivo = new TextBox();
        var paginas = new TabControl();
        var agendar = Estilo.Boton("Agendar cita", () => Ejecutar(() =>
        {
            var cita = taller.AgendarCita(editor.Obtener(), new DateTimeOffset(fecha.Value), motivo.Text);
            editor.RefrescarVehiculos(cita.Placa);
            motivo.Clear();
            Actualizar();
            paginas.SelectedIndex = 1;
            Informar($"Cita creada: {cita.Id}.");
        }), principal: true);
        paginas.TabPages.Add(Estilo.Pestana("Agendar cita", Estilo.Secciones(null,
            Estilo.Desplazable(editor.Campos(("Fecha y hora", fecha), ("Motivo", motivo))), Estilo.Acciones(agendar))));

        paginas.TabPages.Add(Estilo.Pestana("Agenda", Estilo.Secciones(null, tabla, Estilo.Acciones(
            Estilo.Boton("Confirmar", () => Cambiar(EstadoCita.Confirmada), principal: true),
            Estilo.Boton("Marcar atendida", () => Cambiar(EstadoCita.Atendida)),
            Estilo.Boton("Cancelar cita", () => Cambiar(EstadoCita.Cancelada), peligro: true),
            Estilo.Boton("Refrescar", Actualizar)))));
        Controls.Add(Estilo.Secciones(Estilo.Titulo("Gestión de citas y vehículos"), paginas,
            Estilo.Acciones(Estilo.Boton("Cerrar", Close))));
        VigilarBorrador(() => editor.TieneDatosNuevos || motivo.TextLength > 0);
        Shown += (_, _) => Ejecutar(() =>
        {
            editor.CargarClientes();
            Actualizar();
        });
    }

    private void Cambiar(EstadoCita estado) => Ejecutar(() =>
    {
        var cita = Fila<Cita>(tabla, "Selecciona una cita en la agenda.");
        if (estado == EstadoCita.Cancelada && MessageBox.Show(this, $"¿Cancelar la cita {cita.Id}?",
            Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        Taller.CambiarEstadoCita(cita.Id, estado);
        Actualizar();
    });

    private void Actualizar() => Ejecutar(() => Estilo.Llenar(tabla, Taller.Citas(), cita =>
        [cita.Id, Estilo.Fecha(cita.Fecha), cita.Cliente, cita.Placa, cita.Motivo, cita.Estado]));
}
