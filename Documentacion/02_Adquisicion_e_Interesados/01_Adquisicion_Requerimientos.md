# 01. Adquisición de Requerimientos

**Aplicación:** AutoServiceManager\
**Asignatura:** Diseño y Programación de Software\
**Fase:** Ingeniería de Requerimientos\
**Entregable:** PPE04 - Adquisición de Requerimientos

**Integrantes:**
- Joel Ordoñez
- Ronald Veloz
- Pablo Cuasapaz

## 1. Introducción

La adquisición de requerimientos constituye una actividad de la Ingeniería de Requerimientos que permite identificar las necesidades, problemas y expectativas de los interesados relacionados con un sistema de software.

En el proyecto AutoServiceManager, este proceso se enfoca en comprender las actividades administrativas y operativas de un taller automotriz, considerando la gestión de clientes, vehículos, citas, órdenes de trabajo, diagnósticos, inventario, facturación y reportes.

Para el presente ejercicio se documenta una simulación académica de adquisición de requerimientos, basada en escenarios representativos del funcionamiento de un taller automotriz, los interesados identificados en el Entregable 1 y las funcionalidades previstas para AutoServiceManager.

Los escenarios simulados permiten plantear necesidades iniciales, formular preguntas de aclaración e identificar posibles conflictos entre los interesados.

## 2. Propósito del documento

El propósito del presente documento es registrar de manera organizada el proceso de adquisición de requerimientos de AutoServiceManager, identificando las técnicas de elicitación, las fuentes de información, los responsables y las actividades planificadas.

Además, se busca documentar las necesidades detectadas, sus respectivas aclaraciones y los posibles conflictos entre interesados, con el fin de establecer una base para el posterior análisis, refinamiento y especificación de requerimientos del sistema.


## 3. Plan de adquisición

### 3.1. Técnicas y evidencia

| Técnica | Uso documentado | Evidencia y límite |
|---|---|---|
| Entrevista simulada | Formular y responder preguntas desde el rol de atención, taller, inventario, facturación o administración. | §5 conserva 12 preguntas, las respuestas hipotéticas y los registros que aclaran. |
| Observación simulada | Representar la agenda, recepción, reparación y movimientos de inventario en escenarios de escritorio. | Los resultados se reconstruyen a partir de §4 y §6. No hay una ficha de observación ni constancia de una sesión independiente. |
| Revisión documental y técnica | Contrastar los escenarios con la visión, los interesados y los módulos existentes. | F-DOC, F-TEC y la mención mixta en INF-15. Las referencias al repositorio identifican el insumo auditable. |
| Lluvia de ideas | Considerar alternativas a los conflictos y precisar preguntas para el refinamiento. | §6.2 conserva alternativas propuestas. El historial disponible no acredita una reunión de lluvia de ideas ni sus asistentes. |

### 3.2. Fuentes identificadas

| Código | Fuente registrada | Cobertura en la entrega |
|---|---|---|
| S-PRO | Propietario del taller, simulado | Reportes administrativos, INF-14. |
| S-ADM | Administrador del taller, simulado | Vehículos, órdenes, costos y persistencia, INF-02/05/11/15. |
| S-REC | Recepcionista, simulada | Clientes y citas, INF-01/03. |
| S-MEC | Mecánico, simulado | Historial, diagnóstico y avances, INF-04/06/07. |
| S-INV | Encargado de inventario, simulado | Repuestos, consumo y proveedores, INF-08/09/10. |
| S-FAC | Personal de facturación, simulado | Pagos y comprobantes, INF-12/13. |
| S-CLI | Cliente del taller, simulado | Expectativa de reservas y consultas en el conflicto C-04. |
| F-DOC | [Visión y Alcance](../01_Gestion_y_Alcance/01_Vision_y_Alcance.md) y [Matriz de Interesados](../01_Gestion_y_Alcance/02_Matriz_Interesados.md) | Referencia de roles, alcance y restricciones. Sirve para justificar derivaciones técnicas, con decisión pendiente cuando falte un relato equivalente en los INF. |
| F-TEC | [Proyecto Windows Forms](../../AutoServiceManager/AutoServiceManager.csproj) y [persistencia](../../AutoServiceManager/Persistencia/BaseDatos.cs), base Git `9d7df43` | Referencia técnica para confirmar la existencia de los módulos y el almacenamiento local. El plan original cita el proyecto como fuente. |

### 3.3. Cronograma por fases y responsables previstos

| Fase | Actividad | Responsables previstos en el plan | Rastro verificable |
|---|---|---|---|
| 1 | Revisar visión, interesados y módulos. | Joel Ordoñez | F-DOC y F-TEC identificados en §3.2. Fecha de ejecución individual pendiente. |
| 2 | Elaborar escenarios de entrevista simulada. | Joel Ordoñez y Ronald Veloz | Registros de §4 y preguntas de §5. El contenido publicado no atribuye cada respuesta a una sesión. |
| 3 | Analizar escenarios y formular alternativas. | Ronald Veloz y Pablo Cuasapaz | Conflictos de §6 con hipótesis explícitas. Fecha de sesión pendiente. |
| 4 | Organizar la adquisición para el refinamiento. | Joel Ordoñez, Ronald Veloz y Pablo Cuasapaz | Git acredita la primera incorporación de este documento el 21/11/2026 en `0b39034`. |

Joel coordina la revisión de alcance. Ronald facilita el seguimiento y la elaboración de escenarios. Pablo revisa la correspondencia con las funciones existentes. Estas asignaciones vienen del plan. Las fechas y resultados sustentados se recogen en el [registro cronológico de PPE08](../06_Trazabilidad_y_Validacion/03_Control_Versiones_SRS.md#cronología-documentada).

## 4. Tabla de Información Registrada

La siguiente tabla presenta las necesidades y situaciones identificadas durante el análisis de escenarios simulados de un taller automotriz. Cada registro incluye su identificador, la información planteada, la fuente utilizada como referencia y una aclaración obtenida dentro de la simulación académica.

Las aclaraciones corresponden a respuestas hipotéticas elaboradas para precisar las necesidades del sistema y serán utilizadas como base para el posterior análisis de requerimientos.

| ID | Información obtenida | Fuente | Aclaración / Respuesta | Rastro de fuente y aclaración |
|---|---|---|---|---|
| INF-01 | Es necesario registrar los datos de identificación y contacto de los clientes. | Recepcionista (simulada) | Se plantea registrar nombre, identificación, teléfono y correo electrónico cuando esté disponible. | S-REC / P-01 |
| INF-02 | Cada vehículo debe estar relacionado con su respectivo propietario. | Administrador (simulado) | Se utilizará la placa como dato de identificación del vehículo y se permitirá asociar varios vehículos a un mismo cliente. | S-ADM / P-02 |
| INF-03 | Se requiere organizar las citas para evitar coincidencias en los horarios de atención. | Recepcionista (simulada) | Se propone registrar fecha, hora, cliente, vehículo y servicio solicitado. | S-REC / P-03 |
| INF-04 | El personal necesita consultar el historial de servicios realizados a un vehículo. | Mecánico (simulado) | El historial se consultará a partir de las órdenes de trabajo asociadas con cada vehículo. | S-MEC / P-04 |
| INF-05 | Es necesario registrar órdenes de trabajo para controlar las reparaciones. | Administrador (simulado) | Cada orden estará asociada a un vehículo e incluirá los servicios solicitados y su estado. | S-ADM / P-04 (historial de órdenes) |
| INF-06 | Los mecánicos necesitan registrar los diagnósticos de los vehículos. | Mecánico (simulado) | Se propone incluir observaciones sobre las fallas detectadas y los trabajos recomendados. | S-MEC / P-05 |
| INF-07 | Se requiere actualizar el avance de las reparaciones. | Mecánico (simulado) | Se utilizarán estados como pendiente, en proceso y finalizado para facilitar el seguimiento. | S-MEC / P-06 |
| INF-08 | El inventario debe mostrar los repuestos disponibles en el taller. | Encargado de inventario (simulado) | Cada repuesto tendrá un código, descripción, cantidad disponible y precio referencial. | S-INV / P-07 |
| INF-09 | Se necesita registrar la utilización de repuestos durante las reparaciones. | Encargado de inventario (simulado) | La utilización de un repuesto deberá relacionarse con una orden de trabajo y reflejarse en las existencias. | S-INV / P-07 |
| INF-10 | El taller necesita mantener información de sus proveedores de repuestos. | Encargado de inventario (simulado) | Se registrarán datos básicos del proveedor, sin integrar pedidos automáticos externos. | S-INV / P-08 |
| INF-11 | El personal necesita elaborar cotizaciones antes de realizar determinados trabajos. | Administrador (simulado) | La cotización considerará el costo estimado de los servicios y repuestos necesarios. | S-ADM / P-09 |
| INF-12 | Es necesario registrar los pagos realizados por los clientes. | Personal de facturación (simulado) | Se almacenará el valor pagado y su relación con el servicio, sin procesar pagos bancarios en línea. | S-FAC / P-10 |
| INF-13 | Se requiere generar un comprobante de los valores cobrados. | Personal de facturación (simulado) | Se propone generar comprobantes en PDF, sin incluir autorización tributaria electrónica del SRI. | S-FAC / P-10 |
| INF-14 | El propietario necesita consultar información general de las operaciones del taller. | Propietario (simulado) | Se propone consultar reportes sobre servicios, órdenes de trabajo e ingresos registrados. | S-PRO / P-11 |
| INF-15 | Es importante disponer de mecanismos para conservar la información del sistema. | Administrador (simulado) y revisión del proyecto | Se contempla el almacenamiento local y la creación de copias de seguridad para reducir el riesgo de pérdida de información. | S-ADM + F-TEC / P-12 |

Los registros presentados constituyen una primera aproximación a las necesidades del sistema. Su contenido deberá analizarse y refinarse en las siguientes etapas de Ingeniería de Requerimientos.


## 5. Preguntas de Aclaración y Respuestas Obtenidas

Durante la adquisición de requerimientos pueden identificarse necesidades que requieren información adicional para comprender su funcionamiento y establecer las condiciones que deberá cumplir el sistema.

Para AutoServiceManager se formularon preguntas de aclaración dentro de los escenarios académicos simulados. Las respuestas presentadas son hipotéticas y permiten precisar las necesidades registradas anteriormente.

| ID | Registro relacionado | Pregunta de aclaración | Respuesta simulada |
|---|---|---|---|
| P-01 | INF-01 | ¿Qué datos de los clientes son necesarios para registrarlos en el sistema? | Se consideran necesarios el nombre, identificación y teléfono. El correo electrónico podrá registrarse cuando esté disponible. |
| P-02 | INF-02 | ¿Un cliente puede registrar varios vehículos? | Sí. Un mismo cliente podrá tener varios vehículos asociados, pero cada vehículo deberá identificarse mediante su placa. |
| P-03 | INF-03 | ¿Qué información se necesita para programar una cita? | Se deberá registrar el cliente, vehículo, fecha, hora y servicio solicitado. También se deberá revisar la disponibilidad del horario. |
| P-04 | INF-04 e INF-05 | ¿Cómo se consultará el historial de servicios de un vehículo? | Se utilizarán las órdenes de trabajo registradas anteriormente, relacionadas con la identificación del vehículo. |
| P-05 | INF-06 | ¿Qué información deberá registrar el mecánico durante el diagnóstico? | Se registrarán las fallas identificadas, las observaciones técnicas y los trabajos recomendados para el vehículo. |
| P-06 | INF-07 | ¿Qué estados permitirán identificar el avance de una reparación? | Se consideran los estados pendiente, en proceso y finalizado, para facilitar el seguimiento de las órdenes de trabajo. |
| P-07 | INF-08 e INF-09 | ¿Qué debe suceder cuando se utiliza un repuesto en una reparación? | El sistema deberá relacionar el repuesto con la orden de trabajo y actualizar la cantidad disponible en el inventario. |
| P-08 | INF-10 | ¿El sistema realizará pedidos automáticos a los proveedores? | No. Únicamente se registrará la información básica de los proveedores, de acuerdo con el alcance inicial del proyecto. |
| P-09 | INF-11 | ¿Cómo se calculará el valor estimado de una reparación? | Se considerarán los costos de los servicios y repuestos necesarios para elaborar una cotización antes de realizar el trabajo. |
| P-10 | INF-12 e INF-13 | ¿El sistema permitirá cobrar en línea y generar facturas electrónicas autorizadas por el SRI? | No. Se registrarán los pagos realizados y se generarán comprobantes PDF, sin integrar pagos bancarios en línea ni autorización tributaria electrónica. |
| P-11 | INF-14 | ¿Qué información necesita consultar el propietario del taller? | Se propone consultar reportes de servicios realizados, órdenes de trabajo e ingresos registrados para apoyar la administración. |
| P-12 | INF-15 | ¿Cómo se podrá reducir el riesgo de pérdida de información? | Se contempla utilizar almacenamiento local y copias de seguridad que permitan conservar los registros del taller. |

### 5.1. Análisis de las aclaraciones

Las preguntas formuladas permitieron precisar las necesidades propuestas para los diferentes módulos de AutoServiceManager.

En la gestión de clientes y vehículos se establecieron los datos principales de registro y su relación. Para las citas y órdenes de trabajo se identificó la información necesaria para organizar la atención y realizar el seguimiento de los servicios.

En el área de inventario se aclaró la relación entre los repuestos utilizados y las órdenes de trabajo. Asimismo, se establecieron límites para la gestión de proveedores, los pagos y la generación de comprobantes, manteniendo la coherencia con el alcance definido en el PPE03.

Las respuestas obtenidas dentro de la simulación constituyen una propuesta inicial que deberá revisarse durante las etapas posteriores de análisis y validación de requerimientos.


## 6. Registro de Conflictos Detectados

Durante la adquisición de requerimientos pueden identificarse diferencias entre las necesidades y expectativas de los interesados. Estas situaciones deben analizarse para evitar contradicciones y establecer acuerdos sobre las funcionalidades del sistema.

En AutoServiceManager se identifican los siguientes conflictos hipotéticos mediante el análisis de escenarios simulados. Se consideran las necesidades de los interesados y las restricciones establecidas en el PPE03.

### 6.1. Tabla de conflictos

| ID | Conflicto detectado | Interesados involucrados | Descripción | Estado |
|---|---|---|---|---|
| C-01 | Cantidad de información requerida en los registros. | Administrador y recepcionista. | El administrador necesita registros detallados de clientes y vehículos, mientras que la recepcionista requiere un proceso rápido para evitar demoras durante la atención. Se relaciona con INF-01 e INF-02. | Pendiente |
| C-02 | Registro de avances de las reparaciones. | Administrador y mecánico. | El administrador necesita conocer el estado actualizado de cada orden de trabajo, mientras que el mecánico busca reducir el tiempo destinado al registro de información para concentrarse en las reparaciones. Se relaciona con INF-05, INF-06 e INF-07. | Pendiente |
| C-03 | Control de utilización de repuestos. | Encargado de inventario y mecánico. | El encargado de inventario necesita mantener actualizadas las existencias, mientras que el mecánico requiere disponer de los repuestos sin retrasar sus actividades. Esto puede generar diferencias sobre el momento y la responsabilidad del registro. Se relaciona con INF-08 e INF-09. | Pendiente |
| C-04 | Disponibilidad de funcionalidades en línea. | Cliente y administrador del taller. | El cliente podría solicitar reservas y consultas de servicios por Internet, mientras que el administrador debe respetar el alcance y los recursos disponibles para la primera versión de AutoServiceManager. Se relaciona con INF-03 y las restricciones del PPE03. | Pendiente |

Los estados corresponden al análisis inicial del ejercicio académico. Los conflictos se mantienen pendientes porque las soluciones propuestas todavía no han sido negociadas ni validadas con interesados reales.

### 6.2. Propuestas para la resolución de conflictos

Para resolver los conflictos identificados se plantean las siguientes alternativas:

- **C-01:** Establecer campos obligatorios para registrar la información esencial de clientes y vehículos, permitiendo completar los datos adicionales cuando sea necesario.

- **C-02:** Utilizar estados definidos para las órdenes de trabajo y permitir el registro de observaciones técnicas sin exigir información excesiva en cada actualización.

- **C-03:** Relacionar los repuestos utilizados con las órdenes de trabajo y establecer un procedimiento sencillo que permita mantener actualizado el inventario.

- **C-04:** Mantener la programación de citas a cargo del personal del taller durante la primera versión, considerando las reservas en línea como una posible mejora futura.

### 6.3. Estrategia de negociación y seguimiento

El equipo de desarrollo analizará los conflictos considerando las necesidades de cada interesado, la importancia de los requerimientos y las restricciones del proyecto.

Se propone realizar reuniones de revisión para comparar las alternativas de solución y establecer acuerdos entre los interesados involucrados. Las decisiones deberán documentarse y considerarse durante el análisis, refinamiento y validación de requerimientos.

Cuando se alcance un acuerdo, el estado correspondiente podrá actualizarse de "Pendiente" a "Resuelto". Si todavía existen diferencias, se registrará como "En negociación".

Las alternativas surgieron de la simulación. La [negociación de PPE07](../05_Modelado_y_Priorizacion/04_Matriz_Priorizacion_MoSCoW.md#negociación-y-planificación) recoge la solución que adopta la especificación para cada conflicto. El estado Pendiente indica que falta la ratificación de los interesados.
