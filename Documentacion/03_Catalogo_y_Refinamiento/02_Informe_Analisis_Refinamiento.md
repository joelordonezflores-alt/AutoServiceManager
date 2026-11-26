# 02. Informe de Análisis y Refinamiento de Requerimientos

**Proyecto:** AutoServiceManager · **Actividad:** PPE05 · **Versión documental:** 1.0 — Entrega académica final.

## A. Problemáticas detectadas

| ID | Problemática | Justificación | Corrección en esta revisión |
|---|---|---|---|
| RF-03 | Incompletitud y ambigüedad | PPE06 redujo la agenda a programar. INF-03 y P-03 requieren disponibilidad y el catálogo ya cubría cuatro operaciones. | Se recupera la gestión de citas. Programada y Confirmada bloquean el mismo turno, y reprogramar conserva el estado activo. D-02. |
| RF-04, RF-05 y RF-06 | Estados incompatibles | La adquisición usa tres estados, las fichas usan cinco y la implementación usa otros, incluido Ingresado. | Se adopta la lista de PPE05 como secuencia de operación. Recibido es el estado inicial y Entregado cierra el seguimiento. D-03. |
| RF-09, RF-10 y RF-11 | Agrupación y recorte | El informe inicial separó empleados, servicios y reportes. Las fichas omitieron parte de la gestión y el reporte de stock es una derivación. | Se mantienen los tres IDs y se explicita la cobertura y el origen parcial. D-06 y D-07. |
| RF-12 y RF-13 | Dependencia incorrecta | La cotización previa del escenario INF-11/P-09 no equivale al consumo efectivo del escenario INF-09/P-07. | La estimación consulta precios. El descuento se reserva para el registro de consumo. D-04. |
| RF-08, RF-14 y RD-02 | Conflicto de alcance y dependencia | El alcance excluye emisión tributaria oficial. El registro del pago depende del importe interno y puede realizarse sin PDF. | Se diferencian comprobante interno, pago y exportación. Se conserva la leyenda documental. D-05. |
| RF-16 y RNF-02 | Dependencia y fuente incompletas | El acceso por rol necesita una sesión. F-DOC da contexto de roles y falta un acuerdo sobre la incorporación y los permisos. | Se analiza RF-16 y se modela en CU-22. RNF-02 depende de RF-16 y usa la tabla de acceso de PPE06. La autenticación y la protección de funciones quedan pendientes de implementación. D-06. |
| RNF-03 y RNF-04 | Recuperación y prueba incompletas | La ficha anterior reducía la operación sin red a tres recorridos y omitía el cierre inesperado. | Se recupera la conservación de registros y se distingue la reapertura normal de la prueba de interrupción. D-08. |

Las decisiones D-01 a D-08 constan en los [resultados de validación](../06_Trazabilidad_y_Validacion/02_Resultados_Validacion.md#4-decisiones-técnicas-de-la-especificación). El [catálogo](01_Catalogo_Requerimientos.md) identifica las fuentes. Los 15 INF tienen cobertura; algunos requisitos también provienen de PPE03 o del análisis técnico y conservan esa distinción.

## B. Dependencias identificadas

| Requisito | Depende de | Motivo |
|---|---|---|
| RF-02 | RF-01, RD-03 | Usa un cliente registrado y la placa única. |
| RF-03 | RF-02 | La cita registra el vehículo ya asociado a su cliente. |
| RF-04 | RF-10, RD-01 | Selecciona servicios del catálogo y exige el vínculo entre vehículo y propietario. |
| RF-05 | RF-04, RF-06 | Consulta las órdenes y el estado actualizado mediante los avances. |
| RF-06 | RF-04 | Actualiza una orden existente. |
| RF-08 | RF-04, RD-02 | Exporta datos de la orden con la estructura del comprobante. |
| RF-11 | RF-04, RF-07, RF-14 | Obtiene órdenes, existencias y pagos de los registros de sus módulos. |
| RF-12 | RF-04, RF-07, RF-10 | Parte de una orden y consulta precios de repuestos y servicios. La estimación deja el stock intacto. |
| RF-13 | RF-04, RF-07 | Registra una partida sobre una orden y descuenta existencias. |
| RF-14 | RF-04 | El saldo y el pago pertenecen a una orden. Puede generarse el comprobante interno cuando se registra el primer pago. |
| RF-16 | RF-09 | Una cuenta se asocia al empleado y su rol asignado en administración. |
| RNF-01 | RF-05 | Mide la duración de la consulta por placa. |
| RNF-02 | RF-16 | El permiso se evalúa con la identidad y el rol de la sesión. |
| RNF-04 | RNF-02 | La operación de respaldo requiere determinar si el rol tiene permiso. |
| RD-01 | RF-02 | La relación entre placa y propietario se establece al registrar el vehículo. |

Las demás fichas no necesitan otro requerimiento como precondición de entrega. RNF-03, RNF-05 y RNF-06 se aplican a las funciones alcanzadas por su ámbito. La [planificación](../05_Modelado_y_Priorizacion/04_Matriz_Priorizacion_MoSCoW.md) sitúa cada dependencia en la misma iteración o en una anterior.

## C. Análisis de viabilidad

### C.1. RF-08 y RD-02 · Comprobante PDF

| Dimensión | Evaluación |
|---|---|
| Técnica | El proyecto referencia PDFsharp-MigraDoc y el generador exporta partidas, subtotales e IVA. La leyenda de RD-02 falta en el código. La prueba T09 generó y abrió el archivo. |
| Económica | La implementación local aprovecha la dependencia existente y el guardado en disco. Este alcance carece de integración tributaria y de servicios externos de pago. |
| Temporal | El ajuste documental está terminado. Agregar la leyenda y comprobar una muestra, una orden sin partidas y una salida de varias páginas requieren estimación por el equipo antes de la entrega. |
| Organizacional | Facturación controla la exportación y comunica al cliente el motivo de la leyenda. Su asignación se deriva de INF-12 e INF-13 y figura en D-05. |

### C.2. RF-16 y RNF-02 · Inicio de sesión y permisos

| Dimensión | Evaluación |
|---|---|
| Técnica | C# y SQLite permiten integrar cuentas locales y vincularlas con empleados. El panel abre hoy sin sesión. Faltan la persistencia de cuentas, la pantalla de acceso y la comprobación de permisos. |
| Económica | La propuesta es local. Su costo principal es el tiempo de desarrollo y verificación. |
| Temporal | Debe planificarse después del registro de empleados y antes de las pruebas de permisos y respaldo. Falta estimar el esfuerzo de la matriz de roles. |
| Organizacional | Administración gestiona el rol de cada empleado. PPE06 establece las funciones por rol. La validación formal con los operadores requiere constancia. |

## D. Comparación y control de cambios por ID

| ID | Versión anterior o diferencia | Ajuste y rastro |
| --- | --- | --- |
| RF-01 | Registrar y consultar clientes, con tres campos mínimos y correo opcional. | Restituye la consulta de PPE05 y conserva los tres campos de INF-01/P-01. El código exige además dirección y correo. D-01. |
| RF-02 | Registrar un vehículo asociado al cliente. | Conserva placa, marca, modelo y año. Explicita la dependencia de la unicidad de RD-03. |
| RF-03 | PPE05 incluía cuatro operaciones; PPE06 solo programaba. | Recupera consulta, reprogramación y cancelación. Programa el turno con Programada o Confirmada y mantiene ese estado al reprogramar. D-02. |
| RF-04 | Crear orden con o sin cita y usar Recibido como estado inicial. | Conserva la regla de cliente/vehículo previo y exige la selección de servicio. Vincula cita opcional, su estado Atendida y las alternativas de alta. D-03. |
| RF-05 | Consultar el estado y el historial por placa. | Precisa el orden cronológico, el acceso al detalle y el resultado vacío. D-03 para los estados. |
| RF-06 | Registrar observaciones, avance y estado de una orden activa. | Conserva la observación y los cinco estados de PPE05. Permite conservar el estado o avanzar y bloquea el retroceso y los avances tras Entregado. D-03. |
| RF-07 | PPE05 consultaba y actualizaba inventario; PPE06 solo consultaba. | Recupera el alta y la actualización por movimientos. Pasa a Must porque habilita RF-13. |
| RF-08 | Generar un comprobante PDF con partidas. | Separa emisión interna de exportación, precisa el subtotal positivo y enlaza el contenido de RD-02. D-05. |
| RF-09 | PPE05 gestionaba empleados; PPE06 solo registraba y exigía credenciales. | Conserva alta, consulta, edición y rol. El uso de credenciales queda en RF-16. Pasa a Should porque es su dependencia. D-06. |
| RF-10 | Administrar servicios; la ficha solo registraba el servicio. | Incorpora consulta y edición como operaciones de gestión y mantiene el precio positivo de la ficha anterior. Must por RF-04 y RF-12. D-07. |
| RF-11 | Consultar reportes generales de órdenes, ingresos e inventario. | Conserva el reporte y explicita agrupación por estado, pagos registrados y stock por repuesto. D-07. |
| RF-12 | Una ficha mezclaba diagnóstico, cotización y repuestos utilizados. | Integra diagnóstico, estimación y registro de la decisión existente en HU-03, CU-12 y el módulo de taller. La cotización conserva el stock. El consumo efectivo corresponde a RF-13. D-04. |
| RF-13 | Agregar repuestos descontando inventario. | Precisa la cantidad entera positiva. Mantiene el descuento y explicita el efecto de un intento inválido. |
| RF-14 | Registrar pagos con dependencia del PDF. | Conserva el cobro local y los límites del saldo. Quita la dependencia de la exportación y conserva el registro interno de cobro. D-05. |
| RF-15 | Alta de proveedores con datos básicos. | Conserva nombre, teléfono y correo, y deja fuera los pedidos a sistemas externos. |
| RF-16 | Inicio de sesión y asignación de rol propuestos. | Establece el inicio de sesión en CU-22 con el rol de RF-09, como requisito analizado y pendiente de implementación. D-06. |
| RNF-01 | Respuesta de hasta 3 segundos. | Conserva el umbral y fija cinco mediciones con 100 órdenes y 20 vehículos. Se debe registrar el equipo de prueba. D-08. |
| RNF-02 | PPE05 protegía funciones por rol; PPE06 comprobaba solo administración. | Establece la tabla de acceso por rol en PPE06 y comprueba el bloqueo de funciones con rol ajeno o sin sesión. D-06. |
| RNF-03 | PPE05 conservaba datos confirmados tras un cierre; PPE06 probaba tres funciones sin red. | Recupera la condición de persistencia y propone un recorrido por módulo más una prueba de cierre inesperado. D-08. |
| RNF-04 | PPE05 pedía SQLite y respaldo; PPE06 se concentraba en la copia. | Consolida el almacenamiento y la copia seleccionable. La propuesta mantiene la función reservada a Administración. D-06. |
| RNF-05 | Pocos campos esenciales y mensaje al detectar uno inválido. | Conserva la validación por campo y el rechazo sin cambios. Acepta la identificación visible por mensaje o control. D-01. |
| RNF-06 | Escritorio Windows y ausencia de servicios externos de pago. | Recupera la restricción de alcance y explicita .NET 10, verificado en el proyecto. |
| RD-01 | Asociación de órdenes a cliente y vehículo. | Precisa que el cliente es propietario del vehículo y atribuye correctamente INF-02/05 al Administrador. |
| RD-02 | PDF con detalle, IVA y leyenda de ausencia de validez tributaria. | Mantiene el contenido. Usa una tasa de prueba configurable y preserva la leyenda exigida. D-05. |
| RD-03 | Placa única y varios vehículos por cliente. | Conserva la regla y hace explícita la normalización que ya usa el código. |

La versión 1.0 conserva los 25 IDs y clasifica todos los requerimientos como Analizado. El análisis de RF-16 incluye su criterio de aceptación y los permisos aplicables. La aprobación formal requiere respaldo documental. Los estados de implementación se registran por separado en la [matriz de trazabilidad](../06_Trazabilidad_y_Validacion/01_Matriz_Trazabilidad.md).
