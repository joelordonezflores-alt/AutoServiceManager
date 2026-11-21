using AutoServiceManager.Modelos;
using AutoServiceManager.Servicios;

namespace AutoServiceManager.Formularios;

internal sealed class Administracion : VentanaTaller
{
    private readonly DataGridView personal = Estilo.Tabla(("cedula", "Cédula", 72), ("nombres", "Nombres", 140),
        ("rol", "Rol", 85), ("telefono", "Teléfono", 75), ("direccion", "Dirección", 120), ("sueldo", "Sueldo", 75));
    private readonly DataGridView servicios = Estilo.Tabla(("id", "Código", 70), ("nombre", "Servicio", 135),
        ("categoria", "Categoría", 100), ("precio", "Precio", 75));
    private readonly TextBox reporte = Estilo.TextoLargo(200, lectura: true);
    private readonly NumericUpDown iva = Estilo.Numero(0, 100);
    private decimal ivaGuardado;

    public Administracion(ServicioTaller taller) : base(taller, "AutoService — Administración", new Size(930, 650))
    {
        var paginas = new TabControl();
        paginas.TabPages.Add(Estilo.Pestana("Gestión de personal", CrearPersonal()));
        paginas.TabPages.Add(Estilo.Pestana("Catálogo de servicios", CrearServicios()));
        paginas.TabPages.Add(Estilo.Pestana("Reportes del taller", CrearReportes()));
        paginas.TabPages.Add(Estilo.Pestana("Configuración", CrearConfiguracion()));
        Controls.Add(Estilo.Secciones(Estilo.Titulo("Panel de administración"), paginas,
            Estilo.Acciones(Estilo.Boton("Cerrar", Close))));
        Shown += (_, _) => Ejecutar(() =>
        {
            CargarPersonal();
            CargarServicios();
            iva.Value = ivaGuardado = Taller.ObtenerIva();
        });
    }

    private Control CrearPersonal()
    {
        var cedula = new TextBox();
        var nombres = new TextBox();
        var telefono = new TextBox();
        var direccion = new TextBox();
        var rol = Estilo.Selector();
        var sueldo = Estilo.Numero();
        VigilarBorrador(() => cedula.TextLength + nombres.TextLength + telefono.TextLength + direccion.TextLength > 0 || sueldo.Value > 0);
        Estilo.Cargar(rol, Enum.GetValues<RolEmpleado>(), opcion => opcion.ToString());
        var campos = Estilo.Campos(("Cédula", cedula), ("Nombres", nombres), ("Teléfono", telefono),
            ("Dirección", direccion), ("Rol", rol), ("Sueldo base ($)", sueldo));
        return Estilo.Secciones(campos, personal, Estilo.Acciones(Estilo.Boton("Registrar empleado", () => Ejecutar(() =>
        {
            var empleado = Taller.RegistrarEmpleado(new(cedula.Text, nombres.Text, telefono.Text, direccion.Text,
                Estilo.Obtener<RolEmpleado>(rol, "Selecciona un rol."), sueldo.Value));
            cedula.Clear();
            nombres.Clear();
            telefono.Clear();
            direccion.Clear();
            sueldo.Value = 0;
            CargarPersonal();
            Informar($"Empleado registrado: {empleado.Nombres}.");
        }), principal: true)));
    }

    private Control CrearServicios()
    {
        var codigo = new TextBox();
        var nombre = new TextBox();
        var categoria = new TextBox { Text = "General" };
        var precio = Estilo.Numero();
        VigilarBorrador(() => codigo.TextLength + nombre.TextLength > 0 || categoria.Text != "General" || precio.Value > 0);
        return Estilo.Secciones(Estilo.Campos(("Código", codigo), ("Nombre", nombre), ("Categoría", categoria),
            ("Precio de mano de obra ($)", precio)), servicios, Estilo.Acciones(
            Estilo.Boton("Agregar servicio", () => Ejecutar(() =>
            {
                var nuevo = Taller.RegistrarServicio(new(codigo.Text, nombre.Text, categoria.Text, precio.Value));
                codigo.Clear();
                nombre.Clear();
                categoria.Text = "General";
                precio.Value = 0;
                CargarServicios();
                Informar($"Servicio registrado: {nuevo.Id}.");
            }), principal: true)));
    }

    private Control CrearReportes()
    {
        reporte.Font = new Font("Consolas", 10);
        return Estilo.Secciones(null, reporte, Estilo.Acciones(
            Estilo.Boton("Generar reporte actual", () => Ejecutar(() =>
                reporte.Text = Taller.GenerarReporte().ReplaceLineEndings()), principal: true),
            Estilo.Boton("Exportar reporte a PDF", () => Ejecutar(() =>
            {
                using var selector = new SaveFileDialog
                {
                    Filter = "Documento PDF (*.pdf)|*.pdf", DefaultExt = "pdf",
                    FileName = $"Reporte-taller-{DateTime.Today:yyyy-MM-dd}.pdf"
                };
                if (selector.ShowDialog(this) != DialogResult.OK)
                    return;
                string actual = Taller.GenerarReporte();
                reporte.Text = actual.ReplaceLineEndings();
                GeneradorPdf.GenerarReporte(actual, selector.FileName);
                Informar($"Reporte guardado en:\n{selector.FileName}");
            }))));
    }

    private Control CrearConfiguracion()
    {
        var descripcion = Estilo.Etiqueta("La tasa se aplica a las facturas nuevas. Valor inicial heredado del programa Java: 15 %.");
        VigilarBorrador(() => iva.Value != ivaGuardado);
        descripcion.MaximumSize = new Size(670, 0);
        return Estilo.Secciones(descripcion, Estilo.Desplazable(Estilo.Campos(("IVA (%)", iva))), Estilo.Acciones(
            Estilo.Boton("Guardar IVA", () => Ejecutar(() =>
            {
                Taller.GuardarIva(iva.Value);
                ivaGuardado = iva.Value;
                Informar($"Tasa guardada: {iva.Value:0.##} %.");
            }), principal: true), Estilo.Boton("Convertir datos de Java", ConvertirDatos)));
    }

    private void CargarPersonal() => Estilo.Llenar(personal, Taller.Empleados(), elemento => [elemento.Cedula,
        elemento.Nombres, elemento.Rol, elemento.Telefono, elemento.Direccion, Estilo.Dinero(elemento.Sueldo)]);

    private void CargarServicios() => Estilo.Llenar(servicios, Taller.Servicios(), elemento =>
        [elemento.Id, elemento.Nombre, elemento.Categoria, Estilo.Dinero(elemento.Precio)]);

    private void ConvertirDatos() => Ejecutar(() =>
    {
        using var origen = new OpenFileDialog { Filter = "Exportación de Java (*.json)|*.json" };
        if (origen.ShowDialog(this) != DialogResult.OK)
            return;
        using var destino = new SaveFileDialog
        {
            Filter = "Base de datos SQLite (*.sqlite)|*.sqlite",
            FileName = "AutoServiceManager-Java.sqlite",
            OverwritePrompt = false
        };
        if (destino.ShowDialog(this) != DialogResult.OK)
            return;
        ServicioTaller.ConvertirDatosJava(origen.FileName, destino.FileName);
        Informar($"Base convertida en:\n{destino.FileName}\n\nPara abrirla, inicia la aplicación con --datos y esta ruta.");
    });
}
