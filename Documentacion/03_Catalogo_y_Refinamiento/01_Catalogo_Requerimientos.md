# 01. Catálogo de Requerimientos

**Aplicación:** AutoServiceManager  
**Asignatura:** Diseño y Programación de Software  
**Fase:** Ingeniería de Requerimientos  
**Entregable:** PPE05 - Catálogo de Requerimientos y Análisis de Refinamiento

**Integrantes:**
- Joel Ordoñez
- Ronald Veloz
- Pablo Cuasapaz

## Catálogo

En la columna **Fuente** se indica el interesado y, entre paréntesis, el registro de la tabla de información del Entregable 2 (`INF-xx`, `P-xx`, `C-xx`) o el documento del Entregable 1 del que se deriva cada requerimiento.

| ID | Descripción | Tipo | Nivel | Fuente | Estado |
|---|---|---|---|---|---|
| RF-01 | El sistema deberá permitir registrar y consultar clientes, almacenando nombre, identificación y teléfono como datos obligatorios y el correo electrónico cuando esté disponible. | Funcional | Usuario | Recepcionista (INF-01, P-01) | Aprobado |
| RF-02 | El sistema deberá permitir registrar vehículos asociados a un cliente existente, con su placa y sus características principales. | Funcional | Usuario | Administrador (INF-02, P-02) | Aprobado |
| RF-03 | El sistema deberá permitir programar, consultar, reprogramar y cancelar citas, registrando cliente, vehículo, fecha, hora y motivo, y verificando que el horario no coincida con otra cita. Las citas serán registradas únicamente por el personal del taller. | Funcional | Usuario | Recepcionista (INF-03, P-03); conflicto C-04 | Analizado |
| RF-04 | El sistema deberá permitir crear órdenes de trabajo asociadas a un cliente y a un vehículo previamente registrados, con o sin cita previa, indicando los servicios solicitados y su estado. | Funcional | Usuario | Administrador (INF-05, P-04) | Aprobado |
| RF-05 | El sistema deberá permitir consultar el estado actual de un vehículo (Recibido, En diagnóstico, En reparación, Listo para entrega o Entregado) y su historial de servicios, a partir de las órdenes de trabajo asociadas a su placa. | Funcional | Usuario | Mecánico (INF-04, INF-07, P-04) | Analizado |
| RF-06 | El sistema deberá permitir registrar observaciones y avances del trabajo realizado sobre una orden activa y actualizar su estado entre Recibido, En diagnóstico, En reparación, Listo para entrega y Entregado. | Funcional | Usuario | Mecánico (INF-07, P-06); conflicto C-02 | Analizado |
| RF-07 | El sistema deberá permitir consultar y actualizar el inventario de repuestos, registrando código, descripción, cantidad disponible y precio referencial. | Funcional | Usuario | Encargado de inventario (INF-08) | Aprobado |
| RF-08 | El sistema deberá permitir generar el comprobante de cobro de una orden de trabajo en formato PDF, con los servicios y repuestos registrados y sus valores. | Funcional | Usuario | Personal de facturación (INF-13, P-10) | Analizado |
| RF-09 | El sistema deberá permitir registrar, consultar y actualizar los empleados del taller y su rol. | Funcional | Usuario | Administrador (Entregable 1: Visión §2.1 ítem 7 y Matriz de Interesados) | Analizado |
| RF-10 | El sistema deberá permitir administrar el catálogo de servicios ofrecidos por el taller, con su nombre y costo referencial. | Funcional | Usuario | Administrador (Entregable 1: Visión §2.1 ítem 7; INF-11) | Analizado |
| RF-11 | El sistema deberá permitir consultar reportes administrativos: reporte general de órdenes de trabajo, ingresos por servicios e inventario de repuestos. | Funcional | Usuario | Propietario del taller (INF-14, P-11) | Analizado |
| RF-12 | El sistema deberá permitir al mecánico registrar el diagnóstico de un vehículo (fallas detectadas, observaciones técnicas y trabajos recomendados) y generar una cotización con el costo estimado de servicios y repuestos. | Funcional | Usuario | Mecánico (INF-06, P-05); Administrador (INF-11, P-09) | Analizado |
| RF-13 | El sistema deberá relacionar los repuestos utilizados con la orden de trabajo y descontar la cantidad utilizada del inventario, mostrando una advertencia cuando las existencias sean insuficientes. | Funcional | Sistema | Encargado de inventario (INF-09, P-07); conflicto C-03 | Analizado |
| RF-14 | El sistema deberá permitir registrar los pagos realizados por los clientes, indicando el valor pagado y su relación con la orden de trabajo, sin procesar pagos bancarios en línea. | Funcional | Usuario | Personal de facturación (INF-12, P-10) | Analizado |
| RF-15 | El sistema deberá permitir registrar los datos básicos de los proveedores de repuestos, sin realizar pedidos automáticos. | Funcional | Usuario | Encargado de inventario (INF-10, P-08) | Aprobado |
| RF-16 | El sistema deberá permitir a los empleados iniciar sesión con usuario y contraseña, y asignarles un rol (Administrador, Recepcionista, Mecánico u Operario) que determine las funciones a las que pueden acceder. | Funcional | Sistema | Derivado del análisis de RNF-02 (ver Informe, problemática 5) | Propuesto |
| RNF-01 | El sistema deberá mostrar el estado actual de un vehículo en un tiempo máximo de 3 segundos. | No Funcional | Sistema | Recepcionista y Cliente (derivado de INF-04 e INF-07) | Aprobado |
| RNF-02 | El sistema deberá restringir el acceso a cada función según el rol del usuario que inició sesión (RF-16), mostrando un mensaje de acceso denegado ante intentos no autorizados. | No Funcional | Sistema | Administrador (Entregable 1: Matriz de Interesados) | Analizado |
| RNF-03 | El sistema deberá operar localmente, sin conexión a Internet, durante toda la jornada de atención del taller y conservar los registros ya confirmados ante un cierre inesperado de la aplicación. | No Funcional | Sistema | Administrador (INF-15; Entregable 1: Visión §3.2) | Analizado |
| RNF-04 | El sistema deberá almacenar la información en una base de datos local SQLite y permitir crear copias de seguridad del archivo de datos. | No Funcional | Sistema | Administrador (INF-15, P-12) | Aprobado |
| RNF-05 | Los formularios de registro deberán solicitar como obligatorios solo los datos esenciales de cada operación y validar los datos ingresados, indicando el campo que debe corregirse. | No Funcional | Usuario | Recepcionista y Mecánico (conflictos C-01 y C-02) | Analizado |
| RNF-06 | El sistema deberá ejecutarse como aplicación de escritorio en equipos con sistema operativo Windows, sin requerir servicios externos de pago. | No Funcional | Sistema | Equipo de desarrollo (Entregable 1: Visión §3.1 y §3.3) | Aprobado |
| RD-01 | Una orden de trabajo deberá estar asociada a un cliente y a un vehículo registrados. | Dominio | Sistema | Recepcionista (INF-02, INF-05) | Aprobado |
| RD-02 | El comprobante de cobro en PDF deberá incluir número, fecha, datos del cliente y del vehículo, detalle de servicios y repuestos, subtotal, IVA y total, e indicar que no es un comprobante tributario autorizado por el SRI. | Dominio | Sistema | Personal de facturación (INF-13, P-10; Entregable 1: Visión §2.2 ítem 7) | Analizado |
| RD-03 | Cada vehículo deberá identificarse por su placa, que no podrá repetirse en el sistema, y un mismo cliente podrá tener varios vehículos. | Dominio | Sistema | Administrador (INF-02, P-02) | Aprobado |
