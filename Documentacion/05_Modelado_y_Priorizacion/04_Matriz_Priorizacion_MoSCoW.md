# Matriz de Priorización MoSCoW — AutoService Manager

## Tabla de priorización

| # | Caso de uso | Prioridad | Justificación |
|---|-------------|-----------|---------------|
| 1 | Registrar cliente | **Must** | Todo ingreso de vehículo parte de un cliente registrado |
| 2 | Registrar vehículo | **Must** | La orden de trabajo no se puede crear sin un vehículo |
| 3 | Crear orden de trabajo | **Must** | Es el punto de partida de toda atención en el taller |
| 4 | Registrar diagnóstico | **Must** | Es el paso previo a la cotización |
| 5 | Generar cotización | **Must** | Es el paso previo a la aprobación del cliente |
| 6 | Aprobar o rechazar cotización | **Must** | Define si la reparación se autoriza |
| 7 | Agregar servicios y repuestos | **Must** | Sin servicios ni repuestos, la orden no se puede facturar |
| 8 | Generar factura | **Must** | Formaliza el cobro de la orden |
| 9 | Registrar pago | **Must** | Cierra el ciclo de cobro y deja la orden lista para entrega |
| 10 | Guardar y recuperar datos | **Must** | Los datos están en memoria hasta que se guardan, y sin esto se pierden al cerrar |
| 11 | Agendar y gestionar cita | **Should** | La cita previa es opcional, y se puede crear una orden sin ella |
| 12 | Registrar avance | **Should** | No bloquea la cotización ni la factura |
| 13 | Generar factura PDF | **Should** | La factura se puede pagar sin comprobante PDF |
| 14 | Consultar estado del vehículo | **Should** | Mejora el seguimiento, pero no interviene en el cobro |
| 15 | Consultar clientes | **Should** | Facilita la búsqueda, pero el cliente también se puede elegir al crear la orden |
| 16 | Consultar inventario | **Should** | Permite ver el stock de repuestos que usa el mecánico |
| 17 | Registrar compra | **Should** | Aumenta el stock, y el inventario se puede iniciar con datos de demostración |
| 18 | Registrar proveedor | **Could** | Solo apoya el registro de compras |
| 19 | Gestionar empleados | **Could** | Es soporte administrativo que no bloquea la atención de una orden |
| 20 | Gestionar catálogo de servicios | **Could** | Es soporte administrativo que no bloquea la atención de una orden |
| 21 | Generar reporte general | **Could** | Es una función de gestión, no operativa |

## Acuerdo de negociación

**Fecha:** [completar]
**Participantes:** [completar]

### Puntos negociados

| # | Punto | Postura A | Postura B | Decisión |
|---|-------|-----------|-----------|----------|
| 1 | Agendar y gestionar cita | **Must**, aparece en el flujo del diagrama de secuencia | **Should**, la cita es opcional en el diagrama de casos de uso | **Should** |
| 2 | Registrar avance | **Must**, actualiza el estado de la orden | **Should**, no bloquea la facturación | **Should** |
| 3 | Generar factura PDF | **Must**, entrega el comprobante al cliente | **Should**, la factura se puede pagar sin PDF | **Should** |
| 4 | Registrar compra | **Must**, repone el stock | **Should**, el inventario inicial ya existe | **Should** |

### Acuerdos

1. Los casos de uso **Must have** se implementan completos y se validan primero.
2. Los **Should have** se implementan después de los Must, si el tiempo lo permite.
3. Los **Could have** se atienden solo si queda tiempo, sin comprometer los anteriores.
4. Cualquier cambio de prioridad se documenta en este archivo con fecha y motivo.
