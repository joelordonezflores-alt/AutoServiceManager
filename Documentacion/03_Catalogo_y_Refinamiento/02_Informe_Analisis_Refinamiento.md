# 02. Informe de Análisis y Refinamiento de Requerimientos

**Aplicación:** AutoServiceManager  
**Asignatura:** Diseño y Programación de Software  
**Fase:** Ingeniería de Requerimientos  
**Entregable:** PPE05 - Catálogo de Requerimientos y Análisis de Refinamiento

**Integrantes:**
- Joel Ordoñez
- Ronald Veloz
- Pablo Cuasapaz

## A. Problemáticas detectadas

| ID | Problemática | Justificación | Corrección |
|---|---|---|---|
| RF-03 (versión inicial) | Ambigüedad / Incompletitud | "Gestionar citas" no indica qué operaciones incluye, qué datos se registran ni cómo se evitan coincidencias de horario. INF-03 y P-03 piden registrar cliente, vehículo, fecha, hora y servicio solicitado, y revisar la disponibilidad. Además, el conflicto C-04 deja sin precisar quién registra las citas. Sin estos datos no se puede diseñar el formulario ni probar el requerimiento. | Se reformuló el requerimiento: operaciones concretas (programar, consultar, reprogramar y cancelar), datos obligatorios, verificación de horario y registro únicamente por el personal del taller. Ver sección D. |
| RF-09 (versión inicial) | Ambigüedad / Requerimiento compuesto | Agrupa tres necesidades independientes (empleados, servicios y reportes) en una sola frase, y "reportes" no indica cuáles. Un requerimiento debe expresar una necesidad verificable. INF-14 y P-11 solo concretan reportes de servicios, órdenes de trabajo e ingresos. | Se dividió en RF-09 (empleados), RF-10 (catálogo de servicios) y RF-11 (reportes: general de órdenes de trabajo, ingresos por servicios e inventario de repuestos). |
| RD-02 (versión inicial) | Conflicto | Exige los datos de la "normativa tributaria vigente", pero el Entregable 1 (Visión §2.2, ítem 7) excluye la facturación electrónica oficial del SRI y P-10 confirma que solo se generarán comprobantes PDF sin autorización tributaria. Cumplir toda la normativa contradice el alcance definido. | Se acotó el requerimiento: el comprobante incluye los datos de cobro (número, fecha, cliente, vehículo, detalle, subtotal, IVA y total) e indica que no es un comprobante autorizado por el SRI. Se decidió mantener fuera del alcance la autorización tributaria. |
| RNF-03 (versión inicial) | Ambigüedad | "Disponible durante el horario de atención" no define el horario ni qué significa estar disponible. Al ser una aplicación local con base de datos SQLite (Visión §3.2), la disponibilidad no depende de un servidor, sino del equipo y de no perder los registros. Tal como está, no se puede verificar. | Se reformuló para exigir operación local sin Internet durante la jornada y conservación de los registros confirmados ante un cierre inesperado. Ver sección D. |
| RNF-02 (versión inicial) | Incompletitud (dependencia no cubierta) | Exige restringir el acceso según el rol de un "usuario autenticado", pero el catálogo inicial no contiene ningún requerimiento de inicio de sesión ni de asignación de roles. En la versión actual del proyecto, la aplicación abre directamente el panel principal sin solicitar credenciales. | Se agregó RF-16 (inicio de sesión y roles) y se reformuló RNF-02 para depender de él. Ver sección D. |
| RF-05 y RF-06 (versión inicial) | Conflicto | INF-07 y P-06 definen los estados pendiente, en proceso y finalizado, mientras que el documento de validación del repositorio (06_Trazabilidad_y_Validacion) maneja recibido, en diagnóstico, en reparación, listo para entrega y entregado. Dos listas distintas impiden definir de forma única el seguimiento de una orden. | Se adoptó una sola lista de cinco estados: pendiente equivale a Recibido o En diagnóstico, en proceso a En reparación y finalizado a Listo para entrega o Entregado. Se reflejó en RF-05 y RF-06. |

## B. Dependencias identificadas

- **RF-02 depende de RF-01:** un vehículo se registra asociado a un cliente existente (INF-02).
- **RF-03 depende de RF-01 y RF-02:** una cita requiere cliente y vehículo registrados (INF-03, P-03).
- **RF-04 depende de RF-01, RF-02 y RD-01:** la orden de trabajo debe estar asociada a un cliente y a un vehículo registrados. No depende de RF-03, porque la orden puede crearse con o sin cita previa.
- **RF-06 y RF-12 dependen de RF-04:** el avance y el diagnóstico se registran sobre una orden de trabajo existente.
- **RF-05 depende de RF-04 y RF-06:** el estado actual y el historial se obtienen de las órdenes asociadas a la placa y de su avance (P-04).
- **RF-12 depende de RF-10 y RF-13:** la cotización suma el costo de los servicios del catálogo y de los repuestos utilizados (P-09).
- **RF-13 depende de RF-04 y RF-07:** el repuesto utilizado se relaciona con una orden y se descuenta del inventario (P-07).
- **RF-08 depende de RF-04, RF-12, RF-13 y RD-02:** el comprobante muestra los servicios y repuestos de la orden y debe cumplir los datos definidos en RD-02.
- **RF-14 depende de RF-08:** el pago se aplica sobre el valor del comprobante de la orden y actualiza su saldo pendiente.
- **RF-11 depende de RF-04, RF-07 y RF-14:** los reportes de órdenes, inventario e ingresos se calculan con esos registros.
- **RF-16 depende de RF-09:** los usuarios que inician sesión corresponden a empleados registrados con su rol.
- **RNF-02 depende de RF-16:** no se puede restringir el acceso por rol sin autenticar al usuario.
- **RNF-01 depende de RF-05:** el tiempo máximo de 3 segundos aplica a la consulta del estado del vehículo.
- **RF-15 es independiente:** solo almacena datos básicos de proveedores, sin pedidos automáticos (P-08).

## C. Análisis de viabilidad

Se analizan dos requerimientos críticos: **RF-08** (comprobante de cobro en PDF), por su relación con el dinero y con el alcance tributario, y **RF-16** (inicio de sesión y roles), por ser el único requerimiento nuevo y no estar implementado en el proyecto actual.

### C.1. RF-08: Comprobante de cobro en PDF

| Criterio | Análisis |
|---|---|
| Viabilidad técnica | **Viable.** C# con Windows Forms sobre .NET permite generar PDF mediante bibliotecas como PDFsharp-MigraDoc, que el repositorio ya incluye como dependencia junto con un módulo generador de PDF. Los datos del comprobante (cliente, vehículo, servicios y repuestos) están disponibles en la base de datos SQLite. |
| Viabilidad económica | **Viable.** La biblioteca es gratuita, por lo que no genera costos de licencia y respeta la restricción económica del Entregable 1 (§3.3). |
| Viabilidad temporal | **Viable.** El requerimiento se limita a formatear datos ya registrados en un documento. No requiere integraciones externas ni aprobaciones, por lo que cabe en el tiempo de la asignatura. |
| Viabilidad organizacional | **Viable con un riesgo.** El personal de facturación ya entrega comprobantes y solo cambia el medio. El riesgo es que el cliente lo confunda con una factura electrónica del SRI; se mitiga con RD-02, que obliga a indicar que no es un comprobante autorizado. |

**Conclusión:** se mantiene en el alcance de la primera versión, con la limitación definida en RD-02.

### C.2. RF-16: Inicio de sesión y roles

| Criterio | Análisis |
|---|---|
| Viabilidad técnica | **Viable.** Requiere una tabla de usuarios en SQLite, un formulario de inicio de sesión y el almacenamiento de contraseñas con un algoritmo de derivación de claves incluido en .NET (PBKDF2), sin dependencias externas. |
| Viabilidad económica | **Viable.** No requiere herramientas ni servicios de pago. |
| Viabilidad temporal | **Viable con alcance mínimo.** Implica modificar los formularios existentes para habilitar u ocultar opciones según el rol, lo que puede tomar un sprint. Se recomienda implementar solo inicio de sesión y cuatro roles, y dejar para una versión posterior la recuperación de contraseñas y el bloqueo por intentos fallidos. |
| Viabilidad organizacional | **Viable con un riesgo.** El taller tiene pocos usuarios, y el administrador puede crear las cuentas. El personal operativo podría considerar lento iniciar sesión en cada uso (conflicto C-01 y C-02), por lo que se propone mantener la sesión abierta durante la jornada. |

**Conclusión:** es viable si se limita a su versión mínima, y es necesario para poder cumplir RNF-02.

## D. Versión refinada de los requerimientos afectados

| ID | Versión inicial | Versión refinada |
|---|---|---|
| RF-03 | El sistema deberá permitir gestionar citas de atención. | El sistema deberá permitir programar, consultar, reprogramar y cancelar citas, registrando cliente, vehículo, fecha, hora y motivo, y verificando que el horario no coincida con otra cita. Las citas serán registradas únicamente por el personal del taller. |
| RF-05 | El sistema deberá permitir consultar el estado actual de los vehículos. | El sistema deberá permitir consultar el estado actual de un vehículo (Recibido, En diagnóstico, En reparación, Listo para entrega o Entregado) y su historial de servicios, a partir de las órdenes de trabajo asociadas a su placa. |
| RF-06 | El sistema deberá permitir registrar observaciones y avances del trabajo realizado sobre una orden activa. | El sistema deberá permitir registrar observaciones y avances del trabajo realizado sobre una orden activa y actualizar su estado entre Recibido, En diagnóstico, En reparación, Listo para entrega y Entregado. |
| RF-08 | El sistema deberá permitir generar facturas por los servicios realizados. | El sistema deberá permitir generar el comprobante de cobro de una orden de trabajo en formato PDF, con los servicios y repuestos registrados y sus valores. |
| RF-09 | El sistema deberá permitir administrar empleados, servicios y reportes. | **RF-09:** El sistema deberá permitir registrar, consultar y actualizar los empleados del taller y su rol. **RF-10:** El sistema deberá permitir administrar el catálogo de servicios ofrecidos por el taller, con su nombre y costo referencial. **RF-11:** El sistema deberá permitir consultar reportes administrativos: reporte general de órdenes de trabajo, ingresos por servicios e inventario de repuestos. |
| RNF-02 | El sistema deberá restringir el acceso a las funciones según el rol del usuario autenticado. | El sistema deberá restringir el acceso a cada función según el rol del usuario que inició sesión (RF-16), mostrando un mensaje de acceso denegado ante intentos no autorizados. |
| RNF-03 | El sistema deberá estar disponible durante el horario de atención del taller. | El sistema deberá operar localmente, sin conexión a Internet, durante toda la jornada de atención del taller y conservar los registros ya confirmados ante un cierre inesperado de la aplicación. |
| RD-02 | La factura deberá incluir los datos exigidos por la normativa tributaria vigente. | El comprobante de cobro en PDF deberá incluir número, fecha, datos del cliente y del vehículo, detalle de servicios y repuestos, subtotal, IVA y total, e indicar que no es un comprobante tributario autorizado por el SRI. |
| RF-16 | No existía. | El sistema deberá permitir a los empleados iniciar sesión con usuario y contraseña, y asignarles un rol (Administrador, Recepcionista, Mecánico u Operario) que determine las funciones a las que pueden acceder. |

## Conclusiones y recomendaciones

**Conclusiones**

- El catálogo contiene 16 requerimientos funcionales, 6 no funcionales y 3 de dominio, por encima del mínimo exigido (10, 5 y 2).
- Se detectaron y corrigieron seis problemáticas: dos ambigüedades, una incompletitud y tres conflictos. Una de las correcciones dividió un requerimiento compuesto y otra agregó un requerimiento faltante (RF-16).
- Todos los registros INF-01 a INF-15 del Entregable 2 quedaron cubiertos por al menos un requerimiento.

**Recomendaciones**

- Agregar a la tabla del Entregable 2 los registros que hoy no existen para personal, catálogo de servicios, seguridad por roles, rendimiento y disponibilidad, porque RF-09, RF-10, RNF-01, RNF-02 y RNF-03 se rastrean solo de forma indirecta.
- Actualizar la matriz de trazabilidad de la carpeta 06 con los identificadores nuevos (RF-10 a RF-16, RNF-04 a RNF-06 y RD-03) y con las versiones refinadas de este informe.
- Validar los requerimientos en estado Analizado y Propuesto con los interesados antes de pasarlos a Aprobado, ya que las aclaraciones del Entregable 2 son respuestas simuladas.
- Planificar RF-16 en su versión mínima antes de las funciones que dependen de los roles.
