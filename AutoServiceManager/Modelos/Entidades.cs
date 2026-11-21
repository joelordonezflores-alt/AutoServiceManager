namespace AutoServiceManager.Modelos;

public enum EstadoCita { Programada, Confirmada, Atendida, Cancelada }
public enum EstadoCotizacion { Pendiente, Aprobada, Rechazada }
public enum MetodoPago { Efectivo, TarjetaCredito, Transferencia }
public enum RolEmpleado { Administrador, Mecánico, Pintor, Lavador, Recepcionista }
public enum TipoTrabajo { Mecánica, Pintura, Lavado }

public sealed record Cliente(string Cedula, string Nombres, string Telefono, string Direccion, string Correo);
public sealed record Vehiculo(string Placa, string Marca, string Modelo, int Anio, string Cedula)
{
    public string Etiqueta => $"{Placa} · {Marca} {Modelo}";
}

public sealed record Cita(string Id, DateTimeOffset Fecha, string Cedula, string Cliente,
    string Placa, string Motivo, EstadoCita Estado);
public sealed record Orden(string Id, DateTimeOffset FechaIngreso, DateTimeOffset? FechaEntrega,
    string Placa, string Cliente, string Estado, string Observaciones, string Tipo, string? CitaId)
{
    public string Etiqueta => $"{Id} · {Placa}";
}

public sealed record Partida(string Tipo, string Codigo, string Nombre, string Categoria, int Cantidad, decimal Precio)
{
    public decimal Importe => Cantidad * Precio;
}

public sealed record Cotizacion(string Id, DateTimeOffset Fecha, string Diagnostico, decimal ManoObra,
    decimal Estimado, EstadoCotizacion Estado);
public sealed record Avance(DateTimeOffset Fecha, string Rol, string Estado, decimal Costo);
public sealed record DetalleOrden(Orden Orden, IReadOnlyList<Partida> Partidas, Cotizacion? Cotizacion,
    IReadOnlyList<Avance> Avances);
public sealed record Repuesto(string Id, string Nombre, int Stock, decimal Precio)
{
    public string Etiqueta => $"{Nombre} · Stock: {Stock}";
}

public sealed record Proveedor(string Id, string Nombre, string Telefono, string Correo);
public sealed record DetalleCompra(string RepuestoId, int Cantidad, decimal Costo);
public sealed record Compra(string Id, DateTimeOffset Fecha, string Proveedor, string Repuesto,
    int Cantidad, decimal Costo)
{
    public decimal Total => Cantidad * Costo;
}

public sealed record Empleado(string Cedula, string Nombres, string Telefono, string Direccion,
    RolEmpleado Rol, decimal Sueldo);
public sealed record Servicio(string Id, string Nombre, string Categoria, decimal Precio);

public sealed record Pago(string Id, DateTimeOffset Fecha, MetodoPago Metodo, decimal Monto);
public sealed record Factura(string Id, string OrdenId, DateTimeOffset Fecha, Cliente Cliente, Vehiculo Vehiculo,
    decimal TasaIva, decimal Subtotal, decimal Iva, decimal Total, IReadOnlyList<Partida> Partidas,
    IReadOnlyList<Pago> Pagos)
{
    public decimal Pagado => Pagos.Sum(pago => pago.Monto);
    public decimal Saldo => Math.Max(0, Total - Pagado);
}

public sealed record ResumenFactura(decimal TasaIva, decimal Subtotal, decimal Iva, decimal Total,
    decimal Saldo, string? FacturaId);

public sealed class ReglaNegocioException(string mensaje) : Exception(mensaje);
