using AutoServiceManager.Modelos;
using AutoServiceManager.Servicios;

namespace AutoServiceManager.Formularios;

internal sealed class RegistroCliente : VentanaTaller
{
    public RegistroCliente(ServicioTaller taller) : base(taller, "Registrar nuevo cliente", new Size(560, 355))
    {
        var cedula = new TextBox();
        var nombres = new TextBox();
        var telefono = new TextBox();
        var direccion = new TextBox();
        var correo = new TextBox();
        var guardar = Estilo.Boton("Guardar cliente", () => Ejecutar(() =>
        {
            var cliente = taller.RegistrarCliente(cedula.Text, nombres.Text, telefono.Text, direccion.Text, correo.Text);
            Informar($"Cliente registrado: {cliente.Nombres}.");
            DialogResult = DialogResult.OK;
        }), principal: true);
        Controls.Add(Estilo.Secciones(Estilo.Titulo("Datos del propietario"),
            Estilo.Desplazable(Estilo.Campos(("Cédula", cedula), ("Nombres", nombres), ("Teléfono", telefono),
                ("Dirección", direccion), ("Correo", correo))),
            Estilo.Acciones(guardar, Estilo.Boton("Cancelar", Close))));
        AcceptButton = guardar;
        VigilarBorrador(() => new[] { cedula, nombres, telefono, direccion, correo }.Any(campo => campo.TextLength > 0));
    }
}

internal sealed class ListaClientes : VentanaTaller
{
    private readonly DataGridView clientes = Estilo.Tabla(("cedula", "Cédula", 70), ("nombres", "Nombres", 120),
        ("telefono", "Teléfono", 75), ("direccion", "Dirección", 120), ("correo", "Correo", 125));
    private readonly DataGridView vehiculos = Estilo.Tabla(("placa", "Placa", 80), ("marca", "Marca", 90),
        ("modelo", "Modelo", 100), ("anio", "Año", 55));

    public ListaClientes(ServicioTaller taller) : base(taller, "Clientes y vehículos registrados", new Size(900, 575))
    {
        var paginas = new TabControl();
        paginas.TabPages.Add(Estilo.Pestana("Clientes", Estilo.Secciones(null, clientes, Estilo.Acciones(
            Estilo.Boton("Registrar cliente", () =>
            {
                using var registro = new RegistroCliente(taller);
                if (registro.ShowDialog(this) == DialogResult.OK)
                    Actualizar();
            }, principal: true), Estilo.Boton("Refrescar", Actualizar)))));
        paginas.TabPages.Add(Estilo.Pestana("Vehículos", Estilo.Secciones(null, vehiculos, Estilo.Acciones(
            Estilo.Boton("Registrar vehículo", () =>
            {
                using var registro = new RegistroVehiculo(taller, clientes.CurrentRow?.Tag as Cliente);
                if (registro.ShowDialog(this) == DialogResult.OK)
                    Actualizar();
            }, principal: true),
            Estilo.Boton("Ver historial de órdenes", () => Ejecutar(() =>
            {
                var vehiculo = Fila<Vehiculo>(vehiculos, "Selecciona un vehículo.");
                using var historial = new ListaOrdenes(taller, vehiculo.Placa);
                historial.ShowDialog(this);
            })), Estilo.Boton("Refrescar", Actualizar)))));
        vehiculos.Columns.Add(new DataGridViewTextBoxColumn { Name = "propietario", HeaderText = "Propietario",
            FillWeight = 125, MinimumWidth = 100 });
        Controls.Add(Estilo.Secciones(Estilo.Titulo("Clientes y vehículos"), paginas,
            Estilo.Acciones(Estilo.Boton("Cerrar", Close))));
        Shown += (_, _) => Actualizar();
    }

    private void Actualizar() => Ejecutar(() =>
    {
        var lista = Taller.Clientes();
        var nombres = lista.ToDictionary(item => item.Cedula, item => item.Nombres, StringComparer.OrdinalIgnoreCase);
        Estilo.Llenar(clientes, lista, item => [item.Cedula, item.Nombres, item.Telefono, item.Direccion, item.Correo]);
        Estilo.Llenar(vehiculos, Taller.Vehiculos(), item => [item.Placa, item.Marca, item.Modelo, item.Anio,
            nombres.GetValueOrDefault(item.Cedula, item.Cedula)]);
    });
}

internal sealed class RegistroVehiculo : VentanaTaller
{
    public RegistroVehiculo(ServicioTaller taller, Cliente? cliente) : base(taller, "Registrar vehículo", new Size(560, 365))
    {
        var editor = new EditorVehiculo(taller, permiteElegirExistente: false);
        var guardar = Estilo.Boton("Registrar vehículo", () => Ejecutar(() =>
        {
            var vehiculo = taller.RegistrarVehiculo(editor.Obtener());
            Informar($"Vehículo registrado: {vehiculo.Placa}.");
            DialogResult = DialogResult.OK;
        }), principal: true);
        Controls.Add(Estilo.Secciones(Estilo.Titulo("Datos del vehículo"), Estilo.Desplazable(editor.Campos()),
            Estilo.Acciones(guardar, Estilo.Boton("Cancelar", Close))));
        AcceptButton = guardar;
        VigilarBorrador(() => editor.TieneDatosNuevos);
        Shown += (_, _) => Ejecutar(() => editor.CargarClientes(cliente?.Cedula));
    }
}

internal sealed class EditorVehiculo(ServicioTaller taller, bool permiteElegirExistente = true)
{
    private readonly ComboBox cliente = Estilo.Selector();
    private readonly ComboBox vehiculo = Estilo.Selector();
    private readonly TextBox placa = new() { CharacterCasing = CharacterCasing.Upper };
    private readonly TextBox marca = new();
    private readonly TextBox modelo = new();
    private readonly NumericUpDown anio = Estilo.Numero(1900, 2100, 0, DateTime.Today.Year);
    public event Action? ClienteCambiado;
    public event Action? VehiculoCambiado;
    public Cliente? Cliente => Estilo.Seleccion<Cliente>(cliente);
    public Vehiculo? Existente => Estilo.Seleccion<Vehiculo>(vehiculo) is { Placa.Length: > 0 } item ? item : null;
    public bool TieneDatosNuevos => Existente == null && (placa.TextLength + marca.TextLength + modelo.TextLength > 0);

    public TableLayoutPanel Campos(params (string, Control)[] adicionales)
    {
        var campos = new List<(string, Control)> { ("Cliente", cliente) };
        if (permiteElegirExistente)
            campos.Add(("Vehículo registrado", vehiculo));
        campos.AddRange([("Placa", placa), ("Marca", marca), ("Modelo", modelo), ("Año", anio)]);
        campos.AddRange(adicionales);
        cliente.SelectedIndexChanged += (_, _) =>
        {
            if (Cliente is not { } seleccionado)
                return;
            if (permiteElegirExistente)
                CargarVehiculos(seleccionado.Cedula);
            ClienteCambiado?.Invoke();
        };
        vehiculo.SelectedIndexChanged += (_, _) =>
        {
            AplicarVehiculo();
            VehiculoCambiado?.Invoke();
        };
        return Estilo.Campos(campos.ToArray());
    }

    public void CargarClientes(string? cedula = null)
    {
        Estilo.Cargar(cliente, taller.Clientes(), item => $"{item.Cedula} · {item.Nombres}", item => item.Cedula);
        if (cedula != null)
            Estilo.Seleccionar<Cliente>(cliente, cedula);
    }

    public void ElegirVehiculo(string clave) => Estilo.Seleccionar<Vehiculo>(vehiculo, clave);

    public void RefrescarVehiculos(string? clave = null)
    {
        if (Cliente is { } seleccionado)
            CargarVehiculos(seleccionado.Cedula);
        if (clave != null)
            ElegirVehiculo(clave);
    }

    public Vehiculo Obtener()
    {
        var seleccionado = Estilo.Obtener<Cliente>(cliente, "Primero registra y selecciona un cliente.");
        return Existente ?? new Vehiculo(placa.Text, marca.Text, modelo.Text, (int)anio.Value, seleccionado.Cedula);
    }

    private void CargarVehiculos(string cedula)
    {
        var opciones = new List<Vehiculo> { new("", "", "", 0, cedula) };
        opciones.AddRange(taller.Vehiculos(cedula));
        Estilo.Cargar(vehiculo, opciones, item => item.Placa.Length == 0 ? "Registrar otro vehículo" : item.Etiqueta,
            item => item.Placa);
        AplicarVehiculo();
    }

    private void AplicarVehiculo()
    {
        Vehiculo? seleccionado = Existente;
        bool registrado = !string.IsNullOrEmpty(seleccionado?.Placa);
        placa.ReadOnly = marca.ReadOnly = modelo.ReadOnly = registrado;
        anio.Enabled = !registrado;
        placa.Text = registrado ? seleccionado!.Placa : "";
        marca.Text = registrado ? seleccionado!.Marca : "";
        modelo.Text = registrado ? seleccionado!.Modelo : "";
        anio.Value = registrado ? Math.Clamp(seleccionado!.Anio, 1900, 2100) : DateTime.Today.Year;
    }
}
