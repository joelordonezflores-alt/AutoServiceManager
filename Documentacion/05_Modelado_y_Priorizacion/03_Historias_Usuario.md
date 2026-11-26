# 03. Historias de Usuario

**Proyecto:** AutoServiceManager · **Actividad:** PPE07 · **Versión documental:** 1.0 — Entrega académica final.

Los criterios describen los resultados de aceptación de la versión 1.0. La evidencia y las brechas se registran en la matriz de trazabilidad.

### HU-01 · Crear orden de trabajo

**Requerimientos:** RF-04, RD-01; RF-02 y RD-03 para el alta previa; RNF-05 para validación.\
**Casos de uso:** CU-07, CU-20.\
**Historia:** Como recepcionista, quiero crear una orden con o sin cita, para registrar la recepción del vehículo y el servicio solicitado.

- **Dado que** el cliente y el vehículo están registrados y se selecciona un servicio, **cuando** confirmo la creación sin cita, **entonces** la nueva orden queda asociada a la placa y figura como Recibido.
- **Dado que** hay una cita pendiente del mismo cliente y vehículo, **cuando** la vinculo y confirmo una orden válida, **entonces** la orden queda vinculada y la cita se marca Atendida.
- **Dado que** falta un servicio o la placa corresponde a otro cliente, **cuando** intento guardar la orden, **entonces** se identifica la condición y el sistema no guarda la orden.

### HU-02 · Registrar diagnóstico y generar cotización

**Requerimientos:** RF-12, RF-10 y RF-13.\
**Casos de uso:** CU-01, CU-02, CU-03 y CU-14.\
**Historia:** Como mecánico, quiero registrar el diagnóstico y preparar una cotización, para explicar al cliente el trabajo y su costo previsto.

- **Dado que** hay una orden, un diagnóstico con fallas, observaciones y trabajos recomendados, y un servicio del catálogo, **cuando** genero la cotización, **entonces** se guarda el diagnóstico y el total suma los servicios y repuestos estimados, con stock intacto.
- **Dado que** el diagnóstico está vacío, **cuando** intento generar la cotización, **entonces** se indica qué dato debe completar el mecánico y la cotización no se guarda.
- **Dado que** el trabajo usa dos unidades de un repuesto con stock cinco, **cuando** registro por separado el consumo efectivo, **entonces** se añade el repuesto a la orden y quedan tres unidades.

### HU-03 · Registrar aprobación o rechazo de la cotización

**Requerimientos:** RF-12 · Diagnóstico y gestión de cotizaciones.\
**Casos de uso:** CU-12.\
**Historia:** Como cliente, quiero que el mecánico registre la decisión que le comunico sobre la cotización, para dejar constancia de si autorizo la reparación.

- **Dado que** existe una cotización pendiente y el cliente comunica que la aprueba, **cuando** el mecánico confirma el registro de aprobación, **entonces** se conserva la decisión Aprobada.
- **Dado que** existe una cotización pendiente y el cliente comunica que la rechaza, **cuando** el mecánico confirma el registro de rechazo, **entonces** se conserva la decisión Rechazada.
- **Dado que** el mecánico abrió la confirmación y la cierra o pulsa No a la confirmación de guardar, **cuando** vuelve al detalle de la orden, **entonces** la cotización conserva su estado.

### HU-04 · Registrar el avance de reparación

**Requerimientos:** RF-06 y RNF-05.\
**Casos de uso:** CU-19.\
**Historia:** Como operario de taller, quiero guardar una observación y el estado de una orden activa, para mantener visible el avance del trabajo.

- **Dado que** hay una orden activa y dispongo de una observación, **cuando** guardo el avance con el estado En reparación, **entonces** el historial muestra la observación y la orden muestra el estado actualizado.
- **Dado que** la orden ya está Entregada, **cuando** intento guardar otro avance, **entonces** la aplicación impide el cambio y conserva el historial previo.

### HU-05 · Generar comprobante y registrar pago

**Requerimientos:** RF-08, RF-14 y RD-02.\
**Casos de uso:** CU-09, CU-10, CU-11.\
**Historia:** Como integrante de facturación, quiero registrar pagos y exportar el comprobante de una orden, para informar al cliente los importes, los pagos y el saldo.

- **Dado que** la orden tiene un saldo de 100, **cuando** registro un pago de 30 y luego uno de 70, **entonces** el saldo pasa primero a 70 y luego a cero; ambas operaciones se permiten aunque todavía no exista un PDF.
- **Dado que** el importe supera el saldo o carece de un valor válido, **cuando** intento registrar el pago, **entonces** el sistema informa el problema y conserva el saldo.
- **Dado que** la orden tiene partidas por un subtotal mayor que cero, **cuando** exporto el comprobante, **entonces** el archivo muestra número, fecha, cliente, vehículo, partidas, subtotal, IVA configurado, total y la leyenda de RD-02.
