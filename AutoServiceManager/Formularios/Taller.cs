using AutoServiceManager.Modelos;
using AutoServiceManager.Servicios;

namespace AutoServiceManager.Formularios;

internal sealed class ModuloMecanico : VentanaTaller
{
    private readonly ComboBox ordenes = Estilo.Selector();
    private readonly ComboBox repuestos = Estilo.Selector();
    private readonly ComboBox servicios = Estilo.Selector();
    private readonly TextBox diagnostico = Estilo.TextoLargo(70);
    private readonly NumericUpDown manoObra = Estilo.Numero();
    private readonly NumericUpDown cantidad = Estilo.Numero(1, int.MaxValue, 0, 1);
    private readonly Label cotizacion = Estilo.Etiqueta("Sin cotización");
    private readonly DataGridView partidas = Estilo.Tabla(("tipo", "Tipo", 70), ("detalle", "Detalle", 180),
        ("cantidad", "Cantidad", 60), ("importe", "Importe", 80));
    private readonly Button aprobar;
    private readonly Button rechazar;
    private string diagnosticoInicial = "";
    private decimal manoObraInicial;

    public ModuloMecanico(ServicioTaller taller) : base(taller, "AutoService — Módulo mecánico", new Size(850, 690))
    {
        aprobar = Estilo.Boton("Aprobar cotización", () => Resolver(EstadoCotizacion.Aprobada));
        rechazar = Estilo.Boton("Rechazar cotización", () => Resolver(EstadoCotizacion.Rechazada), peligro: true);
        aprobar.Enabled = rechazar.Enabled = false;
        var controles = Estilo.Campos(("Orden a revisar", ordenes), ("Diagnóstico técnico", diagnostico),
            ("Mano de obra ($)", manoObra), ("Cotización", cotizacion), ("Repuesto", repuestos),
            ("Cantidad de repuestos", cantidad), ("Servicio de catálogo", servicios));
        var superior = new TableLayoutPanel { AutoSize = true, ColumnCount = 1, Dock = DockStyle.Top };
        superior.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        superior.Controls.Add(controles);
        superior.Controls.Add(Estilo.Acciones(
            Estilo.Boton("Añadir repuesto", AgregarRepuesto),
            Estilo.Boton("Añadir servicio", AgregarServicio)));
        var cuerpo = Estilo.Secciones(superior, partidas);
        cuerpo.MinimumSize = new Size(0, 490);
        Controls.Add(Estilo.Secciones(Estilo.Titulo("Diagnósticos y cotizaciones"), Estilo.Desplazable(cuerpo, expandirAltura: true),
            Estilo.Acciones(Estilo.Boton("Generar diagnóstico y cotización", Generar, principal: true),
                aprobar, rechazar, Estilo.Boton("Cerrar", Close))));
        ordenes.SelectedIndexChanged += (_, _) => ActualizarOrden(cargarDiagnostico: true);
        VigilarBorrador(() => diagnostico.Text != diagnosticoInicial || manoObra.Value != manoObraInicial);
        Shown += (_, _) => Ejecutar(() =>
        {
            Estilo.Cargar(ordenes, taller.Ordenes(), orden => orden.Etiqueta, orden => orden.Id);
            CargarRepuestos();
            Estilo.Cargar(servicios, taller.Servicios(), item => $"{item.Nombre} · {Estilo.Dinero(item.Precio)}", item => item.Id);
        });
    }

    private Orden OrdenActual() => Estilo.Obtener<Orden>(ordenes, "Selecciona una orden de trabajo.");

    private void CargarRepuestos() => Estilo.Cargar(repuestos, Taller.Repuestos(), repuesto =>
        $"{repuesto.Etiqueta} · {Estilo.Dinero(repuesto.Precio)}", repuesto => repuesto.Id);

    private void AgregarRepuesto() => Ejecutar(() =>
    {
        var repuesto = Estilo.Obtener<Repuesto>(repuestos, "Selecciona un repuesto.");
        Taller.AgregarRepuestoAOrden(OrdenActual().Id, repuesto.Id, (int)cantidad.Value);
        CargarRepuestos();
        ActualizarOrden();
    });

    private void AgregarServicio() => Ejecutar(() =>
    {
        var servicio = Estilo.Obtener<Servicio>(servicios, "Selecciona un servicio del catálogo.");
        Taller.AgregarServicioAOrden(OrdenActual().Id, servicio.Id);
        ActualizarOrden();
    });

    private void Generar() => Ejecutar(() =>
    {
        var orden = OrdenActual();
        var creada = Taller.GenerarDiagnostico(orden.Id, diagnostico.Text, manoObra.Value);
        ActualizarOrden(cargarDiagnostico: true);
        Informar($"Cotización {creada.Id} guardada.\nEstimado: {Estilo.Dinero(creada.Estimado)}.");
    });

    private void Resolver(EstadoCotizacion estado) => Ejecutar(() =>
    {
        var orden = OrdenActual();
        string verbo = estado == EstadoCotizacion.Aprobada ? "aprobar" : "rechazar";
        if (MessageBox.Show(this, $"¿Registrar que el cliente decidió {verbo} la cotización?",
            Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        Taller.ResolverCotizacion(orden.Id, estado);
        ActualizarOrden();
    });

    private void ActualizarOrden(bool cargarDiagnostico = false)
    {
        if (Estilo.Seleccion<Orden>(ordenes) is not { } orden)
            return;
        Ejecutar(() =>
        {
            var detalle = Taller.DetalleOrden(orden.Id);
            var actual = detalle.Cotizacion;
            Estilo.Llenar(partidas, detalle.Partidas, partida =>
                [partida.Tipo, partida.Nombre, partida.Cantidad, Estilo.Dinero(partida.Importe)]);
            cotizacion.Text = actual == null ? "Sin cotización" : $"{actual.Id} · {actual.Estado} · {Estilo.Dinero(actual.Estimado)}";
            aprobar.Enabled = rechazar.Enabled = actual?.Estado == EstadoCotizacion.Pendiente;
            if (cargarDiagnostico)
            {
                diagnostico.Text = diagnosticoInicial = actual?.Diagnostico ?? "";
                manoObra.Value = manoObraInicial = Math.Clamp(actual?.ManoObra ?? 0, manoObra.Minimum, manoObra.Maximum);
            }
        });
    }
}

internal sealed class RegistroAvance : VentanaTaller
{
    public RegistroAvance(ServicioTaller taller) : base(taller, "Registrar avance de taller", new Size(640, 340))
    {
        var ordenes = Estilo.Selector();
        var roles = Estilo.Selector();
        var estados = Estilo.Selector();
        var costo = Estilo.Numero();
        var guardar = Estilo.Boton("Actualizar avance", () => Ejecutar(() =>
        {
            var orden = Estilo.Obtener<Orden>(ordenes, "Selecciona una orden de trabajo.");
            var rol = Estilo.Obtener<RolEmpleado>(roles, "Selecciona el rol de taller.");
            var estado = Estilo.Obtener<string>(estados, "Selecciona el estado.");
            taller.RegistrarAvance(orden.Id, rol, estado, costo.Value);
            Informar($"Avance guardado para {orden.Id}.\nEstado: {estado}.");
            DialogResult = DialogResult.OK;
        }), principal: true);
        Controls.Add(Estilo.Secciones(Estilo.Titulo("Avance y mano de obra"),
            Estilo.Desplazable(Estilo.Campos(("Orden", ordenes), ("Rol", roles), ("Nuevo estado", estados),
                ("Costo del trabajo ($)", costo))),
            Estilo.Acciones(guardar, Estilo.Boton("Cancelar", Close))));
        VigilarBorrador(() => costo.Value != 0);
        Shown += (_, _) => Ejecutar(() =>
        {
            Estilo.Cargar(ordenes, taller.Ordenes(), item => item.Etiqueta, item => item.Id);
            Estilo.Cargar(roles, ServicioTaller.RolesTaller, item => item.ToString());
            Estilo.Cargar(estados, ServicioTaller.EstadosAvance, item => item);
        });
    }
}
