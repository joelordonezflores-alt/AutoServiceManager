using AutoServiceManager.Formularios;
using AutoServiceManager.Servicios;

namespace AutoServiceManager
{
    public partial class Form1 : Form
    {
        private readonly ServicioTaller? taller;

        public Form1()
        {
            InitializeComponent();
            ConstruirMenu();
        }

        internal Form1(ServicioTaller taller) : this() => this.taller = taller;

        private void ConstruirMenu()
        {
            Text = "AutoService Manager";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1050, 730);
            MinimumSize = new Size(740, 530);
            Font = new Font("Segoe UI", 10);
            BackColor = Estilo.Fondo;

            var tarjetas = new TableLayoutPanel
            {
                ColumnCount = 3, RowCount = 2, Dock = DockStyle.Top, AutoSize = true,
                Padding = new Padding(20, 12, 20, 12)
            };
            for (int columna = 0; columna < 3; columna++)
                tarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 3));
            for (int fila = 0; fila < 2; fila++)
                tarjetas.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            tarjetas.Controls.Add(Tarjeta("Clientes y vehículos", "Registra propietarios y consulta sus vehículos.",
                Modulo("Registrar cliente", servicio => new RegistroCliente(servicio), principal: true),
                Modulo("Ver lista de clientes", servicio => new ListaClientes(servicio)),
                Modulo("Gestionar citas", servicio => new GestionCitas(servicio))), 0, 0);
            tarjetas.Controls.Add(Tarjeta("Órdenes de trabajo", "Crea órdenes y revisa el estado del trabajo.",
                Modulo("Crear orden de trabajo", servicio => new CrearOrden(servicio), principal: true),
                Modulo("Ver estado de vehículos", servicio => new ListaOrdenes(servicio))), 1, 0);
            tarjetas.Controls.Add(Tarjeta("Taller y avances", "Gestiona diagnósticos, cotizaciones y avances.",
                Modulo("Módulo mecánico", servicio => new ModuloMecanico(servicio), principal: true),
                Modulo("Registrar avance", servicio => new RegistroAvance(servicio))), 2, 0);
            tarjetas.Controls.Add(Tarjeta("Inventario", "Consulta existencias, proveedores y compras.",
                Modulo("Abrir inventario", servicio => new GestionInventario(servicio), principal: true)), 0, 1);
            tarjetas.Controls.Add(Tarjeta("Facturación", "Emite comprobantes y registra cobros.",
                Modulo("Generar factura", servicio => new GestionFacturacion(servicio), principal: true)), 1, 1);
            tarjetas.Controls.Add(Tarjeta("Administración", "Gestiona personal, servicios e informes del taller.",
                Modulo("Panel de administrador", servicio => new Administracion(servicio), principal: true)), 2, 1);

            var contenido = Estilo.Secciones(Encabezado(), Estilo.Desplazable(tarjetas), Pie());
            Controls.Add(contenido);

            Shown += (_, _) =>
            {
                Rectangle area = Screen.FromControl(this).WorkingArea;
                if (Width > area.Width || Height > area.Height)
                    Bounds = new Rectangle(area.Location, new Size(Math.Min(Width, area.Width), Math.Min(Height, area.Height)));
            };
        }

        private Control Pie()
        {
            var pie = new TableLayoutPanel
            {
                AutoSize = true, ColumnCount = 2, BackColor = Color.White, Padding = new Padding(20, 7, 20, 7)
            };
            pie.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            pie.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320));
            pie.Controls.Add(Estilo.Etiqueta("Almacenamiento local · Guardado automático"), 0, 0);
            var acciones = Estilo.Acciones(
                Estilo.Boton("Crear copia de seguridad", GuardarCopia),
                Estilo.Boton("Salir", Close, peligro: true));
            acciones.WrapContents = false;
            pie.Controls.Add(acciones, 1, 0);
            return pie;
        }

        private static Control Encabezado()
        {
            var panel = new TableLayoutPanel
            {
                BackColor = Estilo.AzulOscuro, AutoSize = true, ColumnCount = 2, RowCount = 1,
                Padding = new Padding(24, 16, 24, 16)
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 84));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            var logo = new PictureBox
            {
                Size = new Size(68, 68), SizeMode = PictureBoxSizeMode.Zoom, Margin = new Padding(0, 0, 16, 0),
                AccessibleName = "Logo de AutoService Manager", TabStop = false
            };
            using (var fuente = typeof(Form1).Assembly.GetManifestResourceStream("AutoServiceManager.logo.png")!)
            using (var original = Image.FromStream(fuente))
            {
                logo.Image = new Bitmap(original);
            }
            logo.Disposed += (_, _) => logo.Image?.Dispose();
            panel.Controls.Add(logo, 0, 0);

            var texto = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, ColumnCount = 1, RowCount = 2 };
            var titulo = Estilo.Titulo("AutoService Manager");
            titulo.Font = new Font("Segoe UI", 23, FontStyle.Bold);
            titulo.ForeColor = Color.White;
            var subtitulo = Estilo.Etiqueta("Panel de control para la gestión integral del taller automotriz");
            subtitulo.ForeColor = Color.FromArgb(205, 219, 232);
            texto.Controls.Add(titulo, 0, 0);
            texto.Controls.Add(subtitulo, 0, 1);
            panel.Controls.Add(texto, 1, 0);
            return panel;
        }

        private static Control Tarjeta(string titulo, string descripcion, params Button[] botones)
        {
            var tarjeta = new TableLayoutPanel
            {
                ColumnCount = 1, AutoSize = true, Dock = DockStyle.Fill, BackColor = Color.White,
                Padding = new Padding(12), Margin = new Padding(7), MinimumSize = new Size(0, 162)
            };
            tarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tarjeta.Paint += (_, e) => ControlPaint.DrawBorder(e.Graphics, tarjeta.ClientRectangle,
                Estilo.Borde, ButtonBorderStyle.Solid);
            var detalle = Estilo.Etiqueta(descripcion);
            detalle.ForeColor = Estilo.Secundario;
            tarjeta.SizeChanged += (_, _) => detalle.MaximumSize = new Size(Math.Max(120, tarjeta.ClientSize.Width - tarjeta.Padding.Horizontal - 12), 0);
            tarjeta.Controls.Add(Estilo.Titulo(titulo));
            tarjeta.Controls.Add(detalle);
            foreach (var boton in botones)
            {
                boton.Dock = DockStyle.Top;
                tarjeta.Controls.Add(boton);
            }
            return tarjeta;
        }

        private Button Modulo(string nombre, Func<ServicioTaller, Form> crear, bool principal = false) =>
            Estilo.Boton(nombre, () =>
            {
                if (taller == null)
                    return;
                using var formulario = crear(taller);
                formulario.ShowDialog(this);
            }, principal);

        private void GuardarCopia()
        {
            if (taller == null)
                return;
            using var selector = new SaveFileDialog
            {
                Filter = "Base de datos SQLite (*.sqlite)|*.sqlite",
                FileName = $"AutoServiceManager-{DateTime.Now:yyyyMMdd-HHmmss}.sqlite",
                OverwritePrompt = false
            };
            if (selector.ShowDialog(this) != DialogResult.OK)
                return;
            try
            {
                taller.CrearCopia(selector.FileName);
                MessageBox.Show(this, "Copia de seguridad guardada.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception error) when (error is IOException or UnauthorizedAccessException or Microsoft.Data.Sqlite.SqliteException)
            {
                MessageBox.Show(this, error.Message, "No se pudo guardar la copia", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
