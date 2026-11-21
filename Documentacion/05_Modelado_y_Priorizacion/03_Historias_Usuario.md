# Historias de Usuario — AutoService Manager

## HU-01: Crear orden de trabajo
* **Requerimiento asociado:** RF-01 — Gestión de órdenes de trabajo
* **Historia:** Como **recepcionista**, quiero **crear una orden de trabajo, con o sin cita previa del cliente**, para **registrar el ingreso del vehículo al taller**.
* **Criterios de aceptación:**
  * **Given** que el cliente seleccionado tiene una cita pendiente (no cancelada ni atendida), **When** la recepcionista abre **Crear orden** y selecciona al cliente, **Then** el sistema carga la placa, marca, modelo, año y motivo desde la cita.
  * **Given** que los datos del vehículo están completos y el año es numérico, **When** la recepcionista confirma con **Crear orden**, **Then** el sistema crea la orden con código `ORD-XXX` en estado `Ingresado`, registra el vehículo si es nuevo y, si había cita, la marca como atendida.
  * **Given** que falta algún dato del vehículo o el año no es numérico, **When** la recepcionista intenta crear la orden, **Then** el sistema muestra un error de validación y no crea la orden.
  * **Given** que la placa ya está registrada a nombre de otro cliente, **When** la recepcionista intenta crear la orden, **Then** el sistema muestra un error y no crea la orden.

## HU-02: Registrar diagnóstico y generar cotización
* **Requerimiento asociado:** RF-02 — Diagnóstico y cotización
* **Historia:** Como **mecánico**, quiero **registrar el diagnóstico, los repuestos y la mano de obra, y generar la cotización**, para **informar al cliente la falla encontrada y el costo estimado de la reparación**.
* **Criterios de aceptación:**
  * **Given** que el mecánico selecciona una orden y un repuesto con stock disponible, **When** lo añade a la orden, **Then** el sistema lo agrega a la orden y descuenta una unidad del inventario.
  * **Given** que el repuesto seleccionado no tiene stock, **When** el mecánico intenta añadirlo, **Then** el sistema muestra una advertencia de stock insuficiente y no lo agrega.
  * **Given** que el mecánico ingresó el diagnóstico y un costo de mano de obra numérico, **When** genera la cotización, **Then** el sistema guarda el diagnóstico y la cotización en la orden, con un total igual a la mano de obra más los repuestos, y cambia el estado de la orden a `Diagnosticado - Esperando Aprobación`.
  * **Given** que el campo de diagnóstico o el de mano de obra no tiene texto, o la mano de obra no es numérica, **When** el mecánico intenta generar la cotización, **Then** el sistema muestra un error y no genera la cotización.

## HU-03: Aprobar o rechazar cotización
* **Requerimiento asociado:** RF-03 — Aprobación de cotización
* **Historia:** Como **cliente**, quiero **aprobar o rechazar la cotización que se me presenta**, para **decidir si autorizo la reparación por el monto estimado**.
* **Criterios de aceptación:**
  * **Given** que se generó la cotización y el sistema consulta si el cliente la aprueba, **When** la respuesta es **Sí**, **Then** la cotización pasa a estado `Aprobada` y la orden pasa a `Cotización aprobada - En reparación`.
  * **Given** que se generó la cotización y el sistema consulta si el cliente la aprueba, **When** la respuesta es cualquiera distinta de **Sí** (por ejemplo, **No** o cerrar el cuadro), **Then** la cotización pasa a estado `Rechazada` y la orden pasa a `Cotización rechazada`.

## HU-04: Registrar avance de reparación
* **Requerimiento asociado:** RF-04 — Seguimiento del avance operativo
* **Historia:** Como **operario del taller (mecánico, pintor o lavador)**, quiero **registrar el avance del trabajo sobre una orden**, para **mantener actualizado el estado de la orden y el costo del trabajo realizado**.
* **Criterios de aceptación:**
  * **Given** que existe al menos una orden, **When** el operario selecciona su rol, la orden y un nuevo estado (por ejemplo `Mecánica terminada` o `Listo para Entrega`) y guarda, **Then** el sistema actualiza el estado de la orden y muestra la confirmación.
  * **Given** que el operario ingresa un costo del trabajo mayor que cero, **When** guarda el avance, **Then** el sistema agrega ese trabajo como servicio de la orden, que se suma al subtotal de la factura si esta aún no ha sido generada.
  * **Given** que el costo ingresado no es numérico, **When** el operario intenta guardar, **Then** el sistema muestra un error y no actualiza la orden.
  * **Given** que no hay órdenes registradas, **When** el operario abre **Registrar avance**, **Then** el sistema muestra "No hay órdenes activas" y no permite seleccionar ninguna.

## HU-05: Generar factura y registrar pago
* **Requerimiento asociado:** RF-05 — Facturación y pagos
* **Historia:** Como **recepcionista**, quiero **generar la factura, registrar el pago y obtener el comprobante en PDF**, para **formalizar el cobro de los servicios y repuestos de la orden**.
* **Criterios de aceptación:**
  * **Given** que la orden tiene servicios o repuestos y la recepcionista ingresa un monto válido menor que el saldo pendiente y un método de pago, **When** registra el pago, **Then** el sistema crea la factura `FAC-XXX` con subtotal, IVA del 15% y total, registra el pago, actualiza el saldo y deja la orden en `Facturada - Saldo pendiente`.
  * **Given** que la recepcionista ingresa un monto igual al saldo pendiente, **When** registra el pago, **Then** la factura queda pagada, el saldo es cero y la orden pasa a `Pagada - Lista para entrega`.
  * **Given** que el monto es mayor que el saldo pendiente, no es numérico o es menor o igual a cero, **When** la recepcionista intenta registrar el pago, **Then** el sistema muestra un error y no registra el pago.
  * **Given** que la orden no tiene servicios ni repuestos, **When** la recepcionista intenta facturar, **Then** el sistema muestra un error y no genera la factura.
  * **Given** que la orden tiene servicios o repuestos, **When** la recepcionista solicita el PDF, **Then** el sistema genera `FAC-XXX.pdf` en la carpeta de facturas y lo abre, sin exigir un pago previo.
