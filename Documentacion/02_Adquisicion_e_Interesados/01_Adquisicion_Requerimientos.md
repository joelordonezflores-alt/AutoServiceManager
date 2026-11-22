
# 01. Adquisición de Requerimientos

**Aplicación:** AutoServiceManager  
**Asignatura:** Diseño y Programación de Software  
**Fase:** Ingeniería de Requerimientos  
**Entregable:** PPE04 - Adquisición de Requerimientos

**Integrantes:**
- Joel Ordoñez
- Ronald Veloz
- Pablo Cuasapaz

## 1. Introducción

La adquisición de requerimientos constituye una actividad de la Ingeniería de Requerimientos que permite identificar las necesidades, problemas y expectativas de los interesados relacionados con un sistema de software.

En el proyecto AutoServiceManager, este proceso se enfoca en comprender las actividades administrativas y operativas de un taller automotriz, considerando la gestión de clientes, vehículos, citas, órdenes de trabajo, diagnósticos, inventario, facturación y reportes.

Para el presente ejercicio se utilizará una simulación académica de adquisición de requerimientos, basada en escenarios representativos del funcionamiento de un taller automotriz, los interesados identificados en el Entregable 1 y las funcionalidades previstas para AutoServiceManager.

La información obtenida mediante los escenarios simulados permitirá plantear necesidades iniciales, formular preguntas de aclaración e identificar posibles conflictos entre los interesados.

## 2. Propósito del documento

El propósito del presente documento es registrar de manera organizada el proceso de adquisición de requerimientos de AutoServiceManager, identificando las técnicas de elicitación, las fuentes de información, los responsables y las actividades planificadas.

Además, se busca documentar las necesidades detectadas, sus respectivas aclaraciones y los posibles conflictos entre interesados, con el fin de establecer una base para el posterior análisis, refinamiento y especificación de requerimientos del sistema.


## 3. Plan de Adquisición

El plan de adquisición de requerimientos de AutoServiceManager establece las actividades necesarias para identificar, recopilar y aclarar las necesidades de los interesados relacionados con la gestión de un taller automotriz.

Para este ejercicio académico se combinará la revisión de documentación existente con técnicas de elicitación aplicadas a escenarios simulados. Esto permitirá analizar las necesidades de los diferentes interesados y establecer una base para la posterior especificación de requerimientos.

### 3.1. Técnicas de elicitación

Se contemplan las siguientes técnicas de adquisición:

| Técnica | Descripción | Objetivo |
|---|---|---|
| Entrevista simulada | Formulación de preguntas y respuestas hipotéticas desde la perspectiva de los interesados del taller. | Identificar necesidades relacionadas con la administración, atención de clientes y mantenimiento de vehículos. |
| Observación simulada | Análisis de escenarios representativos de las actividades diarias de un taller automotriz. | Reconocer problemas en el registro de información y seguimiento de los servicios. |
| Revisión documental | Análisis de los documentos del PPE03 y de la estructura y funcionalidades del proyecto AutoServiceManager. | Identificar información existente y mantener la coherencia entre los entregables. |
| Lluvia de ideas | Generación y evaluación de propuestas de funcionalidades a partir de los problemas identificados. | Identificar posibles soluciones y necesidades que requieran aclaración. |

Las entrevistas y observaciones se desarrollarán como simulaciones académicas, sin representar consultas realizadas a trabajadores de un taller real.

### 3.2. Fuentes de información

Las fuentes consideradas para la adquisición de requerimientos son las siguientes:

| Fuente | Tipo | Información esperada |
|---|---|---|
| Propietario del taller (simulado) | Interesado interno | Necesidades de supervisión, control de ingresos y reportes. |
| Administrador del taller (simulado) | Interesado interno | Gestión de clientes, vehículos, personal y servicios. |
| Recepcionista (simulado) | Interesado interno | Registro de clientes, programación de citas y recepción de vehículos. |
| Mecánico (simulado) | Interesado interno | Diagnósticos, órdenes de trabajo y seguimiento de reparaciones. |
| Encargado de inventario (simulado) | Interesado interno | Control de existencias y utilización de repuestos. |
| Personal de facturación (simulado) | Interesado interno | Registro de pagos y generación de comprobantes. |
| Cliente del taller (simulado) | Interesado externo | Información sobre servicios, costos y estado del vehículo. |
| Documentación PPE03 | Fuente documental | Visión, alcance, restricciones y matriz de interesados. |
| Proyecto AutoServiceManager | Fuente técnica | Estructura de formularios, módulos y funcionalidades existentes. |

### 3.3. Cronograma de adquisición

La adquisición se organiza en cuatro fases para facilitar la recopilación, aclaración y organización de la información.

| Fase | Actividad planificada | Responsable | Resultado esperado |
|---|---|---|---|
| Fase 1 | Revisar los documentos del PPE03 y las funcionalidades existentes de AutoServiceManager. | Joel Ordoñez | Identificación de fuentes y necesidades iniciales. |
| Fase 2 | Desarrollar entrevistas simuladas con los diferentes interesados del taller. | Joel Ordoñez y Ronald Veloz | Registro de necesidades y problemas identificados en los escenarios. |
| Fase 3 | Analizar escenarios de operación del taller mediante observación simulada y lluvia de ideas. | Ronald Veloz y Pablo Cuasapaz | Identificación de necesidades adicionales y situaciones ambiguas. |
| Fase 4 | Formular preguntas de aclaración, analizar posibles conflictos y organizar la información. | Joel Ordoñez, Ronald Veloz y Pablo Cuasapaz | Consolidación del documento de adquisición de requerimientos. |

### 3.4. Responsables del proceso

Las responsabilidades propuestas para el desarrollo del ejercicio se distribuyen considerando los roles establecidos previamente en el equipo.

- **Joel Ordoñez (Product Owner y desarrollador):** Coordinar la identificación de necesidades, organizar la información obtenida y verificar su relación con el alcance del sistema.
- **Ronald Veloz (Scrum Master y desarrollador):** Facilitar la organización de las actividades, participar en la elaboración de escenarios simulados y apoyar la identificación de conflictos.
- **Pablo Cuasapaz (Desarrollador):** Analizar las funcionalidades técnicas del proyecto, participar en la identificación de necesidades y colaborar en la documentación y revisión de los registros.

Los tres integrantes participarán en la revisión de la información para mantener la coherencia entre las necesidades identificadas y las funcionalidades propuestas para AutoServiceManager.


## 4. Tabla de Información Registrada

La siguiente tabla presenta las necesidades y situaciones identificadas durante el análisis de escenarios simulados de un taller automotriz. Cada registro incluye su identificador, la información planteada, la fuente utilizada como referencia y una aclaración obtenida dentro de la simulación académica.

Las aclaraciones corresponden a respuestas hipotéticas elaboradas para precisar las necesidades del sistema y serán utilizadas como base para el posterior análisis de requerimientos.

| ID | Información obtenida | Fuente | Aclaración / Respuesta |
|---|---|---|---|
| INF-01 | Es necesario registrar los datos de identificación y contacto de los clientes. | Recepcionista (simulada) | Se plantea registrar nombre, identificación, teléfono y correo electrónico cuando esté disponible. |
| INF-02 | Cada vehículo debe estar relacionado con su respectivo propietario. | Administrador (simulado) | Se utilizará la placa como dato de identificación del vehículo y se permitirá asociar varios vehículos a un mismo cliente. |
| INF-03 | Se requiere organizar las citas para evitar coincidencias en los horarios de atención. | Recepcionista (simulada) | Se propone registrar fecha, hora, cliente, vehículo y servicio solicitado. |
| INF-04 | El personal necesita consultar el historial de servicios realizados a un vehículo. | Mecánico (simulado) | El historial se consultará a partir de las órdenes de trabajo asociadas con cada vehículo. |
| INF-05 | Es necesario registrar órdenes de trabajo para controlar las reparaciones. | Administrador (simulado) | Cada orden estará asociada a un vehículo e incluirá los servicios solicitados y su estado. |
| INF-06 | Los mecánicos necesitan registrar los diagnósticos de los vehículos. | Mecánico (simulado) | Se propone incluir observaciones sobre las fallas detectadas y los trabajos recomendados. |
| INF-07 | Se requiere actualizar el avance de las reparaciones. | Mecánico (simulado) | Se utilizarán estados como pendiente, en proceso y finalizado para facilitar el seguimiento. |
| INF-08 | El inventario debe mostrar los repuestos disponibles en el taller. | Encargado de inventario (simulado) | Cada repuesto tendrá un código, descripción, cantidad disponible y precio referencial. |
| INF-09 | Se necesita registrar la utilización de repuestos durante las reparaciones. | Encargado de inventario (simulado) | La utilización de un repuesto deberá relacionarse con una orden de trabajo y reflejarse en las existencias. |
| INF-10 | El taller necesita mantener información de sus proveedores de repuestos. | Encargado de inventario (simulado) | Se registrarán datos básicos del proveedor, sin integrar pedidos automáticos externos. |
| INF-11 | El personal necesita elaborar cotizaciones antes de realizar determinados trabajos. | Administrador (simulado) | La cotización considerará el costo estimado de los servicios y repuestos necesarios. |
| INF-12 | Es necesario registrar los pagos realizados por los clientes. | Personal de facturación (simulado) | Se almacenará el valor pagado y su relación con el servicio, sin procesar pagos bancarios en línea. |
| INF-13 | Se requiere generar un comprobante de los valores cobrados. | Personal de facturación (simulado) | Se propone generar comprobantes en PDF, sin incluir autorización tributaria electrónica del SRI. |
| INF-14 | El propietario necesita consultar información general de las operaciones del taller. | Propietario (simulado) | Se propone consultar reportes sobre servicios, órdenes de trabajo e ingresos registrados. |
| INF-15 | Es importante disponer de mecanismos para conservar la información del sistema. | Administrador (simulado) y revisión del proyecto | Se contempla el almacenamiento local y la creación de copias de seguridad para reducir el riesgo de pérdida de información. |

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

Las alternativas descritas constituyen propuestas del ejercicio de simulación y no representan acuerdos confirmados con personas reales.
