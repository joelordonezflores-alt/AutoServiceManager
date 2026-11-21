import ec.edu.epn.autoservice.modelo.*;
import ec.edu.epn.autoservice.persistencia.DatosSistema;
import java.io.ObjectOutputStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.StandardOpenOption;
import java.util.Date;

public final class CrearMuestraJava {
    public static void main(String[] args) throws Exception {
        var datos = new DatosSistema();
        var fecha = new Date(1_735_689_600_000L);
        var cliente = new Cliente("H-15", "José «Prueba»", "000", "Dirección", "prueba@example.test");
        var vehiculo = new Vehiculo("LEG-015", "Marca", "Modelo", 2020, cliente);
        var cita = new Cita("CIT-007", fecha, "Pintura", cliente, vehiculo);
        cita.marcarAtendida();
        var mecanismo = new Mecanico("M-90", "Mecánico de prueba", "000", "Dirección", 650);
        var repuesto = new Repuesto("REP-001", "Pieza de prueba", 4, 15.50);
        var mecanismoServicio = new Servicio("SRV-MEC", "Mano de Obra Mecánica", "Mecánica", 24.50);
        var orden = new OrdenTrabajo("ORD-015", fecha, "Facturada - Saldo pendiente", vehiculo, "[Mecanica] - Ruido");
        orden.agregarRepuesto(repuesto);
        orden.agregarServicio(mecanismoServicio);
        var diagnostico = new Diagnostico("DIAG-ORD-015", "Prueba histórica", fecha);
        orden.setDiagnostico(diagnostico);
        var cotizacion = diagnostico.generarCotizacion(40);
        cotizacion.aprobarCotizacion();
        orden.setCotizacion(cotizacion);
        var factura = new Factura("FAC-012", fecha, orden);
        factura.registrarPago(new Pago("PAG-022", 10, MetodoPago.TARJETA_CREDITO, fecha));
        var proveedor = new Proveedor("PROV-004", "Ejemplo", "000", "proveedor@example.test");
        var compra = new Compra("COM-006", fecha, proveedor);
        compra.agregarDetalle(new DetalleCompra(repuesto, 2, 7.75));
        datos.getClientes().add(cliente);
        datos.getEmpleados().add(mecanismo);
        datos.getCitas().add(cita);
        datos.getRepuestos().add(repuesto);
        datos.getProveedores().add(proveedor);
        datos.getCompras().add(compra);
        datos.getOrdenes().add(orden);
        datos.getFacturas().add(factura);
        datos.getServicios().add(mecanismoServicio);
        datos.setContadorOrdenes(18);
        try (var salida = new ObjectOutputStream(Files.newOutputStream(Path.of(args[0]), StandardOpenOption.CREATE_NEW))) {
            salida.writeObject(datos);
        }
    }
}
