## 1. Actores del Sistema
* **Mecánico:** Empleado del taller que registra el diagnóstico, genera la cotización y agrega los servicios y repuestos de la orden.
* **Recepcionista:** Empleado que atiende al cliente. Registra clientes, vehículos y citas, crea las órdenes de trabajo, genera facturas y registra los pagos.
* **Cliente:** Dueño del vehículo que solicita el servicio, agenda la cita, aprueba o rechaza la cotización y realiza el pago.
* **Administrador:** Empleado que gestiona los empleados, el catálogo de servicios, los proveedores, las compras, el inventario y los reportes.
* **Operario:** Empleado que registra el avance del trabajo sobre el vehículo.
  
## 2. Listado de Casos de Uso

### Mecánico
* CU-01: Registrar diagnóstico
* CU-02: Generar cotización
* CU-03: Agregar servicios y repuestos

### Recepcionista
* CU-04: Registrar cliente
* CU-05: Consultar clientes
* CU-06: Agendar y gestionar cita
* CU-07: Crear orden de trabajo
* CU-08: Consultar estado del vehículo
* CU-09: Generar factura
* CU-10: Generar factura PDF
* CU-11: Registrar pago

### Cliente
* CU-06: Agendar y gestionar cita
* CU-12: Aprobar o rechazar cotización
* CU-08: Consultar estado del vehículo
* CU-11: Registrar pago

### Administrador
* CU-13: Gestionar empleados
* CU-14: Gestionar catálogo de servicios
* CU-15: Generar reporte general
* CU-16: Registrar proveedor
* CU-17: Registrar compra
* CU-18: Consultar inventario

### Operario
* CU-19: Registrar avance

### Casos de Uso de Soporte
* CU-20: Registrar vehículo
* CU-21: Guardar y recuperar datos

## 3. Escenario Detallado: CU-09: Generar factura

## Caso de uso: Generar factura

* **Actor principal:** Recepcionista
* **Precondiciones:**
  * Existe una orden de trabajo (el sistema recibe `orden` para generar la factura).
* **Flujo principal:**
  1. La recepcionista selecciona **Generar factura**.
  2. La interfaz llama a `GestorFacturacion` (`obtenerOGenerarFactura(orden)`).
  3. `GestorFacturacion` guarda la factura en `DatosSistema`.
  4. `GestorFacturacion` retorna a la interfaz subtotal, IVA, total y saldo.
  5. El cliente realiza el pago y la recepcionista registra monto y método (**Registrar pago**, incluido en este caso de uso).
  6. La interfaz llama a `registrarPago(factura, monto, método)` y `GestorFacturacion` agrega el pago a `DatosSistema`.
  7. Si la factura queda pagada, el estado pasa a `Lista para entrega`.
* **Flujos alternativos:**
  * **FA-1 (Pago parcial):** En el paso 7, si el pago no cubre el total, el sistema mantiene el saldo pendiente.
  * **FA-2 (Generar PDF):** Es opcional («extiende»). La recepcionista solicita el PDF, `GeneradorFacturaPdf` ejecuta `generar(factura)`, construye el documento con iText y retorna `FAC-XXX.pdf`. La interfaz abre el comprobante.
* **Postcondiciones:**
  * Factura guardada en `DatosSistema`, con el pago registrado.
  * Pago total: estado `Lista para entrega`.
  * Pago parcial: saldo pendiente.
  * Los datos se escriben en archivo en **Guardar y salir** (`ArchivoDatos.guardar`).











