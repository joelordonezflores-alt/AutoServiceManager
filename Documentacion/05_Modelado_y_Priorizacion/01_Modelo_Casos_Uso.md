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

## 3. Escenario Detallado: CU-07 Crear orden de trabajo

### Caso de Uso: Crear orden de trabajo

* **Actor principal:** Recepcionista 
* **Precondiciones:**
  * El usuario ha iniciado sesión.
  * El cliente y el vehículo están registrados.

* **Flujo principal:**
  1. El usuario selecciona la opción *"Crear orden de trabajo"*.
  2. El sistema solicita buscar al cliente.
  3. El usuario selecciona el cliente.
  4. El sistema muestra los vehículos asociados.
  5. El usuario selecciona el vehículo.
  6. El usuario ingresa el detalle del problema o servicio solicitado.
  7. El sistema registra la orden de trabajo.
  8. El sistema muestra la confirmación de creación.

* **Flujos alternativos:**
  * **A1:** Si el cliente no existe, el sistema permite registrarlo.
  * **A2:** Si el vehículo no está registrado, el sistema permite agregarlo antes de crear la orden.

* **Postcondición:**
  * La orden de trabajo queda almacenada y lista para seguimiento.











