
# 01. Visión y Alcance del Sistema

**Nombre de la aplicación:** AutoServiceManager  
**Asignatura:** Diseño y Programación de Software  
**Fase:** Ingeniería de Requerimientos  
**Entregable:** 1 - Configuración del Proyecto, Visión y Matriz de Interesados

## 1. Introducción

AutoServiceManager es una aplicación de escritorio propuesta para facilitar la administración de un taller de servicio automotriz. Su desarrollo busca responder a las necesidades de organización, control y seguimiento de las actividades que se realizan durante la atención y mantenimiento de vehículos.

En un taller automotriz, el manejo de información mediante registros manuales o archivos independientes puede generar dificultades para consultar los datos de los clientes, controlar los repuestos y conocer el estado de los trabajos realizados.

Por esta razón, se propone desarrollar un sistema que permita centralizar la información y mejorar la gestión de las operaciones del taller.

### 1.1. Propósito del documento

El presente documento tiene como propósito establecer la visión general de AutoServiceManager, definir las funcionalidades que formarán parte del sistema, identificar las actividades que estarán fuera de su alcance y establecer las restricciones que deberán considerarse durante su desarrollo.

### 1.2. Propósito del sistema

El propósito de AutoServiceManager es facilitar la gestión administrativa y operativa de un taller automotriz mediante el registro y consulta de información relacionada con clientes, vehículos, citas, órdenes de trabajo, inventario de repuestos y pagos.

Con esta aplicación se busca reducir los errores en el manejo de la información, mejorar el seguimiento de los servicios y facilitar las actividades del personal del taller.


## 2. Alcance del sistema

AutoServiceManager estará orientado a la gestión administrativa y operativa de un taller automotriz. El sistema permitirá organizar la información de los clientes, vehículos y servicios realizados, facilitando el seguimiento de las actividades del taller.

### 2.1. Funcionalidades incluidas

Las principales funcionalidades que se propone desarrollar son:

1. **Gestión de clientes:** Registrar y consultar la información de los clientes que solicitan servicios en el taller.

2. **Gestión de vehículos:** Registrar los vehículos asociados a cada cliente, incluyendo su placa y características principales.

3. **Gestión de citas:** Programar, consultar y administrar las citas para la atención de vehículos.

4. **Gestión de órdenes de trabajo:** Registrar las órdenes de servicio, consultar su información y realizar el seguimiento de los trabajos.

5. **Diagnóstico y seguimiento:** Registrar los diagnósticos, las actividades realizadas y los avances de los servicios automotrices.

6. **Gestión de inventario:** Registrar y consultar los repuestos disponibles, controlar las existencias y actualizar el inventario según su utilización.

7. **Gestión de servicios y personal:** Administrar la información de los servicios ofrecidos y del personal del taller.

8. **Facturación y pagos:** Calcular los valores correspondientes a los servicios y repuestos, registrar pagos y generar comprobantes en formato PDF.

9. **Reportes administrativos:** Consultar información general sobre las operaciones del taller para apoyar la toma de decisiones.

Estas funcionalidades corresponden al alcance propuesto para el desarrollo del sistema y deberán ser refinadas y validadas durante la fase de Ingeniería de Requerimientos.


### 2.2. Funcionalidades excluidas (NO alcance)

Para establecer los límites del proyecto, se definen las siguientes funcionalidades que no formarán parte de la primera versión de AutoServiceManager:

1. **Aplicación móvil:** No se desarrollará una aplicación para dispositivos Android o iOS.

2. **Plataforma web:** El sistema no permitirá gestionar las operaciones del taller mediante un navegador web, ya que estará orientado a computadoras con Windows.

3. **Reservas de citas en línea:** Los clientes no podrán agendar citas directamente desde Internet. El registro de las citas será realizado por el personal autorizado del taller.

4. **Pagos electrónicos en línea:** No se integrarán pasarelas de pago ni se procesarán transacciones bancarias desde la aplicación. El sistema se limitará a registrar los pagos realizados.

5. **Seguimiento GPS:** No se implementará el rastreo de vehículos mediante dispositivos GPS ni su ubicación en tiempo real.

6. **Integración con proveedores externos:** No se realizarán pedidos automáticos de repuestos ni conexiones directas con los sistemas informáticos de los proveedores.

7. **Facturación electrónica oficial:** La generación de comprobantes en PDF no incluirá la emisión, firma electrónica ni autorización de comprobantes tributarios mediante el Servicio de Rentas Internas (SRI).

Estas exclusiones corresponden a los límites establecidos para la versión inicial del proyecto. Su posible incorporación en futuras versiones dependerá de nuevas necesidades y de la evaluación de su viabilidad.


## 3. Restricciones del sistema

El desarrollo de AutoServiceManager estará sujeto a restricciones que delimitan las condiciones bajo las cuales se diseñará e implementará la aplicación.

### 3.1. Restricciones tecnológicas

- La aplicación se desarrollará utilizando el lenguaje de programación C# y Windows Forms sobre la plataforma .NET.
- El sistema estará orientado a equipos con sistema operativo Windows.
- Se utilizará Visual Studio como entorno principal de desarrollo.
- El código fuente y la documentación serán administrados mediante Git y GitHub.

### 3.2. Restricciones técnicas

- El sistema utilizará una base de datos local SQLite para almacenar la información del taller, conforme a la tecnología propuesta para el proyecto.
- La aplicación no dependerá de una conexión permanente a Internet para realizar las operaciones locales.
- El funcionamiento del sistema estará limitado a los recursos disponibles en el equipo donde se instale.

### 3.3. Restricciones económicas

- El proyecto se desarrollará con los recursos tecnológicos disponibles para los integrantes del equipo.
- Se priorizará la utilización de herramientas gratuitas o que se encuentren disponibles mediante licencias académicas.
- No se contempla la adquisición de infraestructura especializada ni la contratación de servicios externos de pago.

### 3.4. Restricciones temporales

- El desarrollo se realizará durante el período académico establecido para la asignatura Diseño y Programación de Software.
- Las funcionalidades serán planificadas de acuerdo con las fechas de entrega de las actividades y evaluaciones.
- Las características que no puedan desarrollarse dentro del tiempo disponible deberán evaluarse para versiones posteriores.

### 3.5. Restricciones organizacionales

- El desarrollo será realizado de manera colaborativa por los integrantes del equipo.
- Las modificaciones del código fuente y la documentación se gestionarán mediante el repositorio compartido de GitHub.
- Las funcionalidades propuestas deberán responder a las necesidades de los interesados y respetar el alcance definido para la primera versión.


## 4. Definiciones y abreviaturas

En esta sección se presentan los principales términos utilizados en el contexto de AutoServiceManager, con el propósito de facilitar la comprensión del sistema y evitar interpretaciones ambiguas.

### 4.1. Definiciones

| Término | Definición |
|---|---|
| Cliente | Persona que solicita servicios de mantenimiento o reparación para un vehículo. |
| Vehículo | Automóvil registrado en el sistema y asociado a un cliente del taller. |
| Cita | Registro de una fecha y hora programadas para la atención de un vehículo. |
| Orden de trabajo | Documento que registra los servicios solicitados, las actividades realizadas y el seguimiento de un vehículo dentro del taller. |
| Diagnóstico | Evaluación realizada por el personal técnico para identificar el estado de un vehículo y los trabajos necesarios. |
| Servicio automotriz | Actividad de mantenimiento, revisión o reparación realizada sobre un vehículo. |
| Repuesto | Pieza o componente utilizado durante el mantenimiento o reparación de un vehículo. |
| Inventario | Registro de los repuestos disponibles y sus respectivas existencias. |
| Cotización | Estimación del costo de los servicios y repuestos necesarios para realizar un trabajo. |
| Factura o comprobante | Documento que presenta los valores correspondientes a los servicios y repuestos registrados. |
| Pago | Registro de un valor entregado por el cliente para cubrir el costo de los servicios realizados. |
| Reporte administrativo | Resumen de información sobre las operaciones del taller que permite consultar y analizar su funcionamiento. |
| Interesado (Stakeholder) | Persona, grupo u organización que utiliza el sistema, tiene interés en él o puede verse afectado por su funcionamiento. |

### 4.2. Abreviaturas

| Abreviatura | Significado |
|---|---|
| IR | Ingeniería de Requerimientos. |
| SRS | Especificación de Requerimientos de Software (Software Requirements Specification). |
| RF | Requerimiento Funcional. |
| RNF | Requerimiento No Funcional. |
| UI | Interfaz de Usuario (User Interface). |
| BD | Base de Datos. |
| PDF | Formato de Documento Portátil (Portable Document Format). |
| SRI | Servicio de Rentas Internas. |
| .NET | Plataforma de desarrollo de software utilizada para implementar la aplicación. |
