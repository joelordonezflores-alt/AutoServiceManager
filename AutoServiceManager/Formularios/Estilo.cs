using System.Globalization;

namespace AutoServiceManager.Formularios;

internal static class Estilo
{
    public static readonly Color AzulOscuro = Color.FromArgb(23, 50, 77);
    public static readonly Color Azul = Color.FromArgb(47, 128, 237);
    public static readonly Color AzulClaro = Color.FromArgb(232, 241, 252);
    public static readonly Color Fondo = Color.FromArgb(244, 247, 250);
    public static readonly Color Texto = Color.FromArgb(38, 50, 56);
    public static readonly Color Secundario = Color.FromArgb(77, 99, 113);
    public static readonly Color Borde = Color.FromArgb(218, 226, 234);
    public static readonly Color Rojo = Color.FromArgb(196, 69, 54);
    public static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("es-EC");

    public static Button Boton(string texto, Action clic, bool principal = false, bool peligro = false)
    {
        Color fondo = peligro ? Rojo : principal ? Azul : AzulClaro;
        Color textoActivo = peligro || principal ? Color.White : AzulOscuro;
        var boton = new Button
        {
            Text = texto,
            AutoSize = true,
            MinimumSize = new Size(92, 36),
            Padding = new Padding(10, 3, 10, 3),
            Margin = new Padding(4),
            BackColor = fondo,
            ForeColor = textoActivo,
            FlatStyle = FlatStyle.Flat,
            UseVisualStyleBackColor = false,
            Cursor = Cursors.Hand
        };
        boton.FlatAppearance.BorderColor = principal || peligro ? fondo : Borde;
        boton.EnabledChanged += (_, _) =>
        {
            boton.BackColor = boton.Enabled ? fondo : Fondo;
            boton.ForeColor = boton.Enabled ? textoActivo : Secundario;
            boton.FlatAppearance.BorderColor = boton.Enabled && (principal || peligro) ? fondo : Borde;
        };
        boton.Click += (_, _) => clic();
        return boton;
    }

    public static Label Etiqueta(string texto) => new()
    {
        Text = texto,
        AutoSize = true,
        Anchor = AnchorStyles.Left,
        Margin = new Padding(4, 7, 10, 7),
        ForeColor = AzulOscuro
    };

    public static Label Titulo(string texto) => new()
    {
        Text = texto,
        AutoSize = true,
        ForeColor = AzulOscuro,
        Font = new Font("Segoe UI", 13, FontStyle.Bold),
        Margin = new Padding(4, 6, 4, 8)
    };

    public static TableLayoutPanel Campos(params (string, Control)[] campos)
    {
        var panel = new TableLayoutPanel { AutoSize = true, ColumnCount = 2, Dock = DockStyle.Top };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 168));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        foreach (var (nombre, control) in campos)
        {
            int fila = panel.RowCount++;
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.Controls.Add(Etiqueta(nombre), 0, fila);
            control.AccessibleName ??= nombre;
            control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            control.Margin = new Padding(4, 5, 4, 5);
            panel.Controls.Add(control, 1, fila);
        }
        return panel;
    }

    public static FlowLayoutPanel Acciones(params Control[] botones)
    {
        var panel = new FlowLayoutPanel
        {
            AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Dock = DockStyle.Fill,
            Padding = new Padding(0, 5, 0, 0)
        };
        panel.Controls.AddRange(botones);
        return panel;
    }

    public static TableLayoutPanel Secciones(Control? superior, Control centro, Control? inferior = null)
    {
        var panel = new TableLayoutPanel { ColumnCount = 1, Dock = DockStyle.Fill, Margin = Padding.Empty };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        if (superior != null)
            Agregar(superior, SizeType.AutoSize);
        Agregar(centro, SizeType.Percent);
        if (inferior != null)
            Agregar(inferior, SizeType.AutoSize);
        return panel;

        void Agregar(Control control, SizeType tipo)
        {
            panel.RowStyles.Add(new RowStyle(tipo, 100));
            control.Dock = DockStyle.Fill;
            panel.Controls.Add(control, 0, panel.RowCount++);
        }
    }

    public static Panel Desplazable(Control contenido, bool expandirAltura = false)
    {
        var panel = new Panel { AutoScroll = true, Dock = DockStyle.Fill };
        contenido.Dock = DockStyle.Top;
        panel.Controls.Add(contenido);
        if (expandirAltura)
            panel.SizeChanged += (_, _) => contenido.Height = Math.Max(contenido.MinimumSize.Height, panel.ClientSize.Height);
        return panel;
    }

    public static TabPage Pestana(string nombre, Control contenido)
    {
        var pagina = new TabPage(nombre) { BackColor = Fondo, Padding = new Padding(12) };
        contenido.Dock = DockStyle.Fill;
        pagina.Controls.Add(contenido);
        return pagina;
    }

    public static DataGridView Tabla(params (string clave, string titulo, int ancho)[] columnas)
    {
        var tabla = new DataGridView
        {
            AllowUserToAddRows = false, AllowUserToDeleteRows = false, AllowUserToResizeRows = false,
            ReadOnly = true, MultiSelect = false, RowHeadersVisible = false, BackgroundColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle, GridColor = Borde, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, EnableHeadersVisualStyles = false,
            Dock = DockStyle.Fill
        };
        tabla.ColumnHeadersDefaultCellStyle.BackColor = AzulOscuro;
        tabla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        tabla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        tabla.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        tabla.DefaultCellStyle.SelectionBackColor = AzulClaro;
        tabla.DefaultCellStyle.SelectionForeColor = AzulOscuro;
        tabla.AlternatingRowsDefaultCellStyle.BackColor = Fondo;
        tabla.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
        foreach (var (clave, titulo, ancho) in columnas)
            tabla.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = clave, HeaderText = titulo, FillWeight = ancho, MinimumWidth = 64, SortMode = DataGridViewColumnSortMode.NotSortable
            });
        return tabla;
    }

    public static void Llenar<T>(DataGridView tabla, IEnumerable<T> datos, Func<T, object[]> valores)
        where T : notnull
    {
        object? anterior = tabla.CurrentRow?.Cells[0].Value;
        tabla.Rows.Clear();
        DataGridViewRow? seleccion = null;
        foreach (T dato in datos)
        {
            var fila = tabla.Rows[tabla.Rows.Add(valores(dato))];
            fila.Tag = dato;
            if (Equals(fila.Cells[0].Value, anterior))
                seleccion = fila;
        }
        if (seleccion != null)
            tabla.CurrentCell = seleccion.Cells[0];
    }

    public static ComboBox Selector() => new() { DropDownStyle = ComboBoxStyle.DropDownList };
    public static TextBox TextoLargo(int alto = 90, bool lectura = false) => new()
    {
        Multiline = true, ScrollBars = ScrollBars.Vertical, Height = alto, ReadOnly = lectura
    };

    public static NumericUpDown Numero(decimal min = 0, decimal max = 999_999_999.99m, int decimales = 2,
        decimal valor = 0) => new()
    {
        Minimum = min, Maximum = max, DecimalPlaces = decimales, Value = valor,
        ThousandsSeparator = decimales > 0, Increment = decimales > 0 ? 0.01m : 1
    };

    public static string Dinero(decimal monto) => "$ " + monto.ToString("N2", Cultura);
    public static string Fecha(DateTimeOffset fecha) => fecha.ToLocalTime().ToString("dd/MM/yyyy HH:mm", Cultura);

    public static void Cargar<T>(ComboBox combo, IEnumerable<T> elementos, Func<T, string> etiqueta,
        Func<T, string>? clave = null) where T : notnull
    {
        string? anterior = (combo.SelectedItem as Opcion<T>)?.Clave;
        var opciones = elementos.Select(item => new Opcion<T>(item, etiqueta(item), clave?.Invoke(item) ?? etiqueta(item))).ToArray();
        combo.BeginUpdate();
        combo.Items.Clear();
        combo.Items.AddRange(opciones);
        combo.Enabled = opciones.Length > 0;
        int indice = Array.FindIndex(opciones, item => item.Clave == anterior);
        combo.SelectedIndex = indice >= 0 ? indice : opciones.Length > 0 ? 0 : -1;
        combo.EndUpdate();
    }

    public static T? Seleccion<T>(ComboBox combo) where T : class =>
        (combo.SelectedItem as Opcion<T>)?.Valor;

    public static T Obtener<T>(ComboBox combo, string mensaje) where T : notnull =>
        combo.SelectedItem is Opcion<T> opcion ? opcion.Valor : throw new Modelos.ReglaNegocioException(mensaje);

    public static void Seleccionar<T>(ComboBox combo, string clave) where T : notnull
    {
        for (int indice = 0; indice < combo.Items.Count; indice++)
        {
            if (combo.Items[indice] is Opcion<T> opcion && opcion.Clave == clave)
            {
                combo.SelectedIndex = indice;
                return;
            }
        }
    }

    private sealed record Opcion<T>(T Valor, string Etiqueta, string Clave) where T : notnull
    {
        public override string ToString() => Etiqueta;
    }
}
