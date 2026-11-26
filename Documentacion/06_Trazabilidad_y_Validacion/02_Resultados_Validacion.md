# 02. Resultados de Validación

**Proyecto:** AutoServiceManager · **Actividad:** PPE08 · **Fecha:** 25/11/2026 (Ecuador) · **Versión 1.0 — Entrega académica final**

La validación documental coteja requisitos, diseño y matriz de trazabilidad con la evidencia de adquisición simulada y el código fuente. La comprobación técnica usa los resultados de las pruebas de servicios de la base `cec25d1`. Joel Ordoñez coordina el alcance documental, Ronald Veloz la especificación y Pablo Cuasapaz la trazabilidad y las pruebas, como responsabilidades del proyecto.

## 1. Checklist de validación documental

El checklist verifica que cada requisito tenga una necesidad identificada, un enunciado consistente, un criterio comprobable, fuente y prioridad, y que exista una solución técnicamente viable para el entorno del proyecto. La necesidad puede tener origen documental, simulado o técnico. Las observaciones identifican las brechas del producto y las pruebas por completar. Validación con interesados registra la ratificación formal.

| ID | Necesidad documentada | Claro | Consistente | Completo | Factible | Verificable | Fuente identificada | Prioridad | Validación con interesados | Observación |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| RF-01 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-01 · El alta existente exige dirección y correo. |
| RF-02 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | T02 · Falta verificar el alta desde la interfaz. |
| RF-03 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-02 · Faltan reprogramación y control de coincidencias. |
| RF-04 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-03 · La creación usa un estado distinto y admite el alta del vehículo en la misma operación. |
| RF-05 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-03 · Falta informar cuando no hay historial. |
| RF-06 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-03 · Faltan observación, secuencia y restricciones de avance. |
| RF-07 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | T01/T04 · Existe alta y actualización de stock por movimientos. |
| RF-08 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-05 · Está implementada la exportación. Falta la leyenda. |
| RF-09 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-06 · Faltan edición y validación de teléfono. |
| RF-10 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-07 · Faltan edición y validación de precio mayor que cero. |
| RF-11 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-07 · Faltan desgloses de estados y existencias. |
| RF-12 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-04 · Existe registro de la decisión. El cálculo actual difiere del especificado. |
| RF-13 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | T04 · Consumo y rechazo por falta de stock probados. |
| RF-14 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | T06/T07 · Pago, liquidación y sobrepago probados. |
| RF-15 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | T04 · Alta probada como parte del flujo de compra. |
| RF-16 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-06 · Necesidad técnica derivada de RNF-02. Inicio de sesión pendiente. |
| RNF-01 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-08 · Perfil de medición definido. Medición pendiente. |
| RNF-02 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-06 · Tabla de permisos definida. Implementación pendiente. |
| RNF-03 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | T08 · Reapertura regular probada. Desconexión y cierre inesperado pendientes. |
| RNF-04 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | T08 · Copia recuperable probada. Restricción de acceso pendiente. |
| RNF-05 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-08 · Queda pendiente completar la verificación por formulario. |
| RNF-06 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-08 · Compilación verificada. Recorrido interactivo pendiente. |
| RD-01 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | T02/T03 · Asociación y propiedad comprobadas a nivel de servicio. |
| RD-02 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | D-05 · Importes probados. Falta la leyenda en el PDF. |
| RD-03 | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Pendiente | T02/T03 · Normalización, unicidad y asociación con el propietario comprobadas. |

## 2. Observaciones y correcciones

| Hallazgo | Corrección en la especificación | Estado del producto o de la evidencia |
| --- | --- | --- |
| La agenda estaba reducida al alta y el modelo mostraba al cliente operándola. | RF-03 define consulta, reprogramación, cancelación y un turno activo por fecha y hora. El actor es Recepción. | Agenda básica comprobada por T03. Reprogramación y control de coincidencias pendientes. |
| Los estados de la orden, de la cotización y del cobro se mezclaban. | RF-04 y RF-06 fijan el inicio y la secuencia de orden. RF-12 y RF-14 registran datos de cotización o pago sin añadir estados de orden. | T03, T05 y T06 cubren la operación existente. El código usa otro vocabulario y registra estados de orden adicionales. |
| Las historias citaban IDs con otra función. CU-12 carecía de vínculo directo. | Se vincularon las cinco historias a sus requisitos. RF-12 incorpora la decisión de cotización documentada en HU-03 y CU-12. | T05 acredita el registro de aceptación y rechazo. El cálculo de cotización todavía difiere de RF-12. |
| La priorización listaba casos y la matriz incluía 14 requisitos. | MoSCoW y la matriz cubren los 25 IDs. Cada CP tiene un recorrido con resultado esperado en su criterio de aceptación. | La matriz distingue pruebas ejecutadas, cobertura parcial y pruebas pendientes. |
| El pago dependía del PDF y la matriz usaba un comprobante tributario. | El registro de pago se separa de la exportación. RD-02 fija los datos y la leyenda del comprobante de servicio. | T06 y T07 verifican cobros. T09 verifica emisión y paginación. La leyenda aún falta. |
| El control histórico declaraba aprobaciones sin vincularlas a una versión acreditada. | El historial conserva la evidencia de los commits y registra el estado de aprobación de cada versión. | La validación con interesados y la aprobación formal de 1.0 quedan pendientes. |

## 3. Verificación técnica ejecutada

El 25/11/2026 se compiló la aplicación y el proyecto de verificación con .NET SDK 10.0.403 en Windows, sobre el código de `cec25d1`. El código de esa revisión se conserva en la entrega 1.0. La compilación terminó con cero errores y advertencias. El ejecutable terminó con código 0 usando una carpeta nueva para los datos de prueba. Las pruebas se ejecutaron sobre los servicios, SQLite y la generación de PDF. Quedan pendientes el recorrido interactivo de Windows Forms, las pruebas de desconexión y cierre inesperado, la medición de RNF-01 y la inspección de la leyenda del PDF una vez incorporada.

| Evidencia | Resultado de la ejecución | Cobertura observada |
| --- | --- | --- |
| T01 | OK · Inicialización de SQLite e inventario de referencia | Configuración inicial, filas de inventario. |
| T02 | OK · Clientes, cédulas duplicadas y propiedad de vehículos | Alta, duplicados, normalización de placa y propietario. |
| T03 | OK · Agenda, transiciones y alta de orden con reversión transaccional | Agenda con cita, cancelación, vínculo con la orden y reversión. |
| T04 | OK · Compras, proveedores, cantidades, consumo y rechazo por falta de stock | Entrada, consumo, stock disponible, proveedor y rechazo sin aplicar cambios. |
| T05 | OK · Personal, catálogo, diagnóstico, cotización y avance de taller | Altas y duplicados. Cotización con mano de obra y repuestos, resultado comunicado por el cliente y avance. |
| T06 | OK · Una factura por orden, IVA configurable, redondeo, pagos y sobrepago | Emisión interna, redondeo, pagos parciales, liquidación y rechazo. |
| T07 | OK · Dos intentos simultáneos de cobro dejan un solo pago | Un cobro válido al intentar dos pagos simultáneos. |
| T08 | OK · Reapertura de SQLite, reporte y copia de seguridad | Reapertura regular, medidas del reporte, creación y lectura desde un respaldo. |
| T09 | OK · PDF de factura, reporte y comprobante de varias páginas | Apertura del PDF, título y paginación del ejemplo extenso. |

Reproducción desde la carpeta `AutoServiceManager/` con las dependencias restauradas.

```powershell
dotnet build .\Verificacion\AutoServiceManager.Verificacion.csproj --no-restore -v minimal
$datosDePrueba = Join-Path $env:TEMP ("AutoServiceManager-" + [Guid]::NewGuid().ToString())
dotnet .\Verificacion\bin\Debug\net10.0-windows\AutoServiceManager.Verificacion.dll $datosDePrueba
```

[Programa de pruebas](../../AutoServiceManager/Verificacion/Program.cs) · [Matriz de trazabilidad](01_Matriz_Trazabilidad.md)

## 4. Decisiones técnicas de la especificación

| Decisión | Referencias | Regla de la entrega 1.0 | Fundamento y estado comprobado |
| --- | --- | --- | --- |
| D-01 | C-01 · RF-01, RF-02, RNF-05 | El alta de clientes exige nombre, identificación y teléfono. Correo y dirección son opcionales. Los campos de vehículos permiten identificar la placa y al propietario. | INF-01, P-01, INF-02 y P-02 sustentan los datos mínimos. La implementación de clientes todavía exige correo y dirección. |
| D-02 | C-04 · RF-03, RF-05 | Recepción registra y modifica la cita. El turno está ocupado mientras la cita esté Programada o Confirmada. Reprogramar cambia la fecha y hora y conserva su estado activo. Atención y cancelación cierran la cita. | INF-03 y P-03 justifican la comprobación de disponibilidad. C-04 y la visión excluyen reservas por Internet. El código todavía carece de reprogramación y bloqueo de coincidencias. |
| D-03 | C-02 · RF-04, RF-05, RF-06, RD-01 | La orden requiere un cliente, su vehículo y al menos un servicio registrados. Empieza en Recibido. El avance admite conservar el estado o pasar a uno posterior y cierra en Entregado. La cotización y el cobro conservan sus estados propios. | INF-05, INF-07, P-06 y la secuencia refinada en PPE05. El código usa Ingresado y etiquetas operativas adicionales. |
| D-04 | C-03 · RF-07, RF-10, RF-12, RF-13, CU-12, HU-03 | La cotización suma servicios del catálogo y repuestos estimados. El consumo efectivo descuenta existencias. Mecánica registra la aceptación o el rechazo comunicado por el cliente en la cotización. | INF-06, INF-09, INF-11 y sus aclaraciones sustentan diagnóstico, estimación y consumo. CU-12, HU-03 y T05 respaldan el registro de decisión. El cálculo actual usa mano de obra digitada más repuestos. |
| D-05 | RF-08, RF-14, RD-02 | Facturación registra el cobro interno sin exigir el PDF. La exportación exige partidas con subtotal positivo. El comprobante presenta una tasa de IVA configurable y la leyenda de RD-02. | INF-12, INF-13, P-10, la exclusión de facturación tributaria en PPE03 y T06/T09. La leyenda falta en el PDF actual. |
| D-06 | RF-09, RF-16, RNF-02, RNF-04 | Administración mantiene el rol de cada empleado y puede generar respaldos. La persona se autentica con su cuenta. Cada rol accede a las funciones de la tabla de PPE06. | La matriz de interesados y los actores de PPE07 sustentan la asignación. La autenticación y las restricciones por rol quedan pendientes de implementación. |
| D-07 | RF-10, RF-11, RF-15, CU-17 | Administración mantiene las fichas de servicios y prepara para el propietario un reporte por estado, pagos y stock. Inventario registra proveedores locales y entradas por compra. | El alcance PPE03, INF-08, INF-10, INF-11, INF-14 y la función de compras observada en T04 fundamentan el alcance. El reporte actual carece de los desgloses indicados. |
| D-08 | RNF-01, RNF-03, RNF-05, RNF-06 | El ensayo de historial mide cinco consultas en Windows con 100 órdenes y 20 vehículos. La aceptación también incluye un recorrido por módulo sin red, recuperación tras cierre inesperado y verificación de campos inválidos. | PPE05, la restricción de operación local y el marco de prueba de PPE06 fijan las condiciones. Los ensayos de interfaz, desconexión, recuperación y tiempos siguen pendientes. |

## 5. Acuerdos y aprobación formal

La entrega 1.0 incorpora las decisiones D-01 a D-08 como conclusiones técnicas para la especificación. Los resultados T01 a T09 acreditan el alcance probado del producto. En los archivos y commits revisados no consta una aprobación del equipo o de los interesados sobre esta versión, ni un acuerdo ratificado para C-01 a C-04.

[Negociación de PPE07](../05_Modelado_y_Priorizacion/04_Matriz_Priorizacion_MoSCoW.md#negociación-y-planificación) · [Control de versiones y cronología](03_Control_Versiones_SRS.md)
