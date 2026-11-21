using AutoServiceManager.Modelos;
using AutoServiceManager.Servicios;

namespace AutoServiceManager.Formularios;

internal sealed class GestionInventario : VentanaTaller
{
    private readonly DataGridView inventario = Estilo.Tabla(("id", "Código", 70), ("nombre", "Repuesto", 155),
        ("stock", "Stock", 55), ("precio", "Precio de venta", 100));
    private readonly DataGridView proveedores = Estilo.Tabla(("id", "Código", 70), ("nombre", "Proveedor", 125),
        ("telefono", "Teléfono", 80), ("correo", "Correo", 130));
    private readonly DataGridView compras = Estilo.Tabla(("id", "Compra", 65), ("fecha", "Fecha", 90),
        ("prov", "Proveedor", 95), ("repuesto", "Repuesto", 115), ("cantidad", "Cant.", 52),
        ("costo", "Costo unitario", 75), ("total", "Importe", 75));
    private readonly ComboBox proveedorCompra = Estilo.Selector();
    private readonly ComboBox repuestoCompra = Estilo.Selector();

    public GestionInventario(ServicioTaller taller) : base(taller, "Inventario, proveedores y compras", new Size(945, 645))
    {
        var pestanas = new TabControl();
        pestanas.TabPages.Add(Estilo.Pestana("Inventario", Estilo.Secciones(null, inventario, Estilo.Acciones(
            Estilo.Boton("Registrar repuesto", RegistrarRepuesto, principal: true),
            Estilo.Boton("Refrescar inventario", Actualizar)))));
        pestanas.TabPages.Add(Estilo.Pestana("Proveedores", CrearProveedores()));
        pestanas.TabPages.Add(Estilo.Pestana("Registrar compra", CrearCompras()));
        Controls.Add(Estilo.Secciones(Estilo.Titulo("Inventario y compras"), pestanas,
            Estilo.Acciones(Estilo.Boton("Cerrar", Close))));
        Shown += (_, _) => Actualizar();
    }

    private Control CrearProveedores()
    {
        var nombre = new TextBox();
        var telefono = new TextBox();
        var correo = new TextBox();
        VigilarBorrador(() => nombre.TextLength + telefono.TextLength + correo.TextLength > 0);
        var campos = Estilo.Campos(("Nombre", nombre), ("Teléfono", telefono), ("Correo", correo));
        return Estilo.Secciones(campos, proveedores, Estilo.Acciones(Estilo.Boton("Registrar proveedor", () => Ejecutar(() =>
        {
            var proveedor = Taller.RegistrarProveedor(nombre.Text, telefono.Text, correo.Text);
            nombre.Clear();
            telefono.Clear();
            correo.Clear();
            Actualizar();
            Informar($"Proveedor creado: {proveedor.Id}.");
        }), principal: true)));
    }

    private Control CrearCompras()
    {
        var cantidad = Estilo.Numero(1, int.MaxValue, 0, 1);
        var costo = Estilo.Numero();
        var detalle = new List<DetalleCompra>();
        var lista = new ListBox { Height = 75 };
        VigilarBorrador(() => detalle.Count > 0 || costo.Value > 0);
        var total = Estilo.Etiqueta(Estilo.Dinero(0));
        var campos = Estilo.Campos(("Proveedor", proveedorCompra), ("Repuesto", repuestoCompra), ("Cantidad", cantidad),
            ("Costo unitario ($)", costo), ("Detalle de compra", lista), ("Total de la compra", total));
        var espacio = new SplitContainer
        {
            Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, Size = new Size(850, 520),
            SplitterDistance = 278, FixedPanel = FixedPanel.Panel1, Panel1MinSize = 130, Panel2MinSize = 90
        };
        espacio.Panel1.Controls.Add(Estilo.Desplazable(campos));
        espacio.Panel2.Controls.Add(compras);
        return Estilo.Secciones(null, espacio, Estilo.Acciones(
            Estilo.Boton("Añadir al detalle", () => Ejecutar(() =>
            {
                var repuesto = Estilo.Obtener<Repuesto>(repuestoCompra, "Selecciona un repuesto.");
                if (costo.Value <= 0)
                    throw new ReglaNegocioException("El costo unitario debe ser mayor que cero.");
                detalle.Add(new(repuesto.Id, (int)cantidad.Value, costo.Value));
                lista.Items.Add($"{repuesto.Nombre} · {cantidad.Value:0} × {Estilo.Dinero(costo.Value)}");
                total.Text = Estilo.Dinero(detalle.Sum(item => item.Cantidad * item.Costo));
            })),
            Estilo.Boton("Quitar del detalle", () =>
            {
                int indice = lista.SelectedIndex;
                if (indice < 0)
                    return;
                detalle.RemoveAt(indice);
                lista.Items.RemoveAt(indice);
                total.Text = Estilo.Dinero(detalle.Sum(item => item.Cantidad * item.Costo));
            }),
            Estilo.Boton("Registrar compra y aumentar stock", () => Ejecutar(() =>
            {
                var proveedor = Estilo.Obtener<Proveedor>(proveedorCompra, "Primero registra y selecciona un proveedor.");
                IReadOnlyList<DetalleCompra> lineas = detalle;
                if (detalle.Count == 0)
                {
                    var repuesto = Estilo.Obtener<Repuesto>(repuestoCompra, "Selecciona un repuesto.");
                    lineas = [new DetalleCompra(repuesto.Id, (int)cantidad.Value, costo.Value)];
                }
                string id = Taller.RegistrarCompra(proveedor.Id, lineas);
                detalle.Clear();
                lista.Items.Clear();
                costo.Value = 0;
                cantidad.Value = 1;
                total.Text = Estilo.Dinero(0);
                Actualizar();
                Informar($"Compra {id} registrada. El inventario se actualizó.");
            }), principal: true)));
    }

    private void RegistrarRepuesto()
    {
        using var registro = new RegistroRepuesto(Taller);
        if (registro.ShowDialog(this) == DialogResult.OK)
            Actualizar();
    }

    private void Actualizar() => Ejecutar(() =>
    {
        var repuestos = Taller.Repuestos();
        var listaProveedores = Taller.Proveedores();
        Estilo.Llenar(inventario, repuestos, item => [item.Id, item.Nombre, item.Stock, Estilo.Dinero(item.Precio)]);
        Estilo.Llenar(proveedores, listaProveedores, item => [item.Id, item.Nombre, item.Telefono, item.Correo]);
        Estilo.Llenar(compras, Taller.Compras(), item => [item.Id, Estilo.Fecha(item.Fecha), item.Proveedor,
            item.Repuesto, item.Cantidad, Estilo.Dinero(item.Costo), Estilo.Dinero(item.Total)]);
        Estilo.Cargar(proveedorCompra, listaProveedores, item => $"{item.Id} · {item.Nombre}", item => item.Id);
        Estilo.Cargar(repuestoCompra, repuestos, item => $"{item.Id} · {item.Etiqueta}", item => item.Id);
    });
}

internal sealed class RegistroRepuesto : VentanaTaller
{
    public RegistroRepuesto(ServicioTaller taller) : base(taller, "Registrar repuesto", new Size(550, 340))
    {
        var nombre = new TextBox();
        var stock = Estilo.Numero(0, int.MaxValue, 0);
        var precio = Estilo.Numero();
        var guardar = Estilo.Boton("Guardar repuesto", () => Ejecutar(() =>
        {
            var repuesto = taller.RegistrarRepuesto(nombre.Text, (int)stock.Value, precio.Value);
            Informar($"Repuesto registrado: {repuesto.Id}.");
            DialogResult = DialogResult.OK;
        }), principal: true);
        Controls.Add(Estilo.Secciones(Estilo.Titulo("Nuevo repuesto"),
            Estilo.Desplazable(Estilo.Campos(("Nombre", nombre), ("Stock inicial", stock), ("Precio de venta ($)", precio))),
            Estilo.Acciones(guardar, Estilo.Boton("Cancelar", Close))));
        VigilarBorrador(() => nombre.TextLength > 0 || stock.Value != 0 || precio.Value != 0);
    }
}
