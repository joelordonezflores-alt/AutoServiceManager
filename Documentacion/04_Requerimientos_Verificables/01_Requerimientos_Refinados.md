# 01. Requerimientos Refinados y Verificables

**Aplicación:** AutoServiceManager  
**Asignatura:** Diseño y Programación de Software  
**Fase:** Ingeniería de Requerimientos  
**Entregable:** PPE06 - Requerimientos Refinados y Verificables

**Integrantes:**
- Joel Ordoñez
- Ronald Veloz
- Pablo Cuasapaz

## Requerimientos funcionales

### RF-01 — Registro de clientes

- **Identificador:** RF-01
- **Nombre:** Registro de clientes
- **Enunciado:** El sistema deberá permitir a la recepcionista registrar un cliente indicando nombre, identificación y teléfono como datos obligatorios y correo electrónico como dato opcional.
- **Fuente:** Recepcionista (INF-01, P-01); caso de uso CU-04 Registrar cliente.
- **Prioridad:** Must.
- **Criterio de aceptación:** Al registrar un cliente con nombre, identificación y teléfono, el cliente aparece en el listado de clientes; si falta alguno de esos tres datos, el sistema muestra un mensaje y no registra al cliente.
- **Dependencias:** Ninguna.

### RF-02 — Registro de vehículos

- **Identificador:** RF-02
- **Nombre:** Registro de vehículos
- **Enunciado:** El sistema deberá permitir a la recepcionista registrar un vehículo asociado a un cliente registrado, indicando placa, marca, modelo y año.
- **Fuente:** Administrador (INF-02, P-02); caso de uso CU-20 Registrar vehículo.
- **Prioridad:** Must.
- **Criterio de aceptación:** Al registrar un vehículo con placa, marca, modelo y año para un cliente registrado, el vehículo aparece asociado a ese cliente; si el cliente no está registrado, el sistema muestra un mensaje y no registra el vehículo.
- **Dependencias:** RF-01.

### RF-03 — Programación de citas

- **Identificador:** RF-03
- **Nombre:** Programación de citas
- **Enunciado:** El sistema deberá permitir a la recepcionista programar una cita indicando cliente, vehículo, fecha, hora y motivo, sin admitir dos citas en la misma fecha y hora.
- **Fuente:** Recepcionista (INF-03, P-03; conflicto C-04); caso de uso CU-06 Agendar y gestionar cita.
- **Prioridad:** Should.
- **Criterio de aceptación:** Al programar una cita con cliente, vehículo, fecha, hora y motivo, la cita queda registrada en estado Programada; al intentar programar otra cita en la misma fecha y hora, el sistema muestra un mensaje y no la registra.
- **Dependencias:** RF-01, RF-02.

### RF-04 — Creación de órdenes de trabajo

- **Identificador:** RF-04
- **Nombre:** Creación de órdenes de trabajo
- **Enunciado:** El sistema deberá permitir a la recepcionista crear una orden de trabajo para un cliente y un vehículo registrados, con o sin cita previa, indicando los servicios solicitados.
- **Fuente:** Administrador (INF-05, P-04); caso de uso CU-07 Crear orden de trabajo.
- **Prioridad:** Must.
- **Criterio de aceptación:** Al crear una orden para un cliente y un vehículo registrados con al menos un servicio solicitado, la orden queda registrada en estado Recibido y aparece en el listado de órdenes de trabajo.
- **Dependencias:** RF-01, RF-02, RD-01.

### RF-05 — Consulta de órdenes de trabajo por vehículo

- **Identificador:** RF-05
- **Nombre:** Consulta de órdenes de trabajo por vehículo
- **Enunciado:** El sistema deberá permitir consultar por placa las órdenes de trabajo de un vehículo, mostrando de cada una su estado y los servicios realizados.
- **Fuente:** Mecánico (INF-04, INF-07, P-04); caso de uso CU-08 Consultar estado del vehículo.
- **Prioridad:** Should.
- **Criterio de aceptación:** Al consultar una placa con órdenes registradas, el sistema muestra cada orden con su estado y sus servicios; si la placa no tiene órdenes registradas, el sistema muestra un mensaje que lo indica.
- **Dependencias:** RF-04, RF-06.

### RF-06 — Registro del avance de una orden

- **Identificador:** RF-06
- **Nombre:** Registro del avance de una orden
- **Enunciado:** El sistema deberá permitir al mecánico o al operario registrar el avance de una orden activa, indicando una observación y el nuevo estado entre Recibido, En diagnóstico, En reparación, Listo para entrega y Entregado.
- **Fuente:** Mecánico (INF-07, P-06; conflicto C-02); caso de uso CU-19 Registrar avance.
- **Prioridad:** Should.
- **Criterio de aceptación:** Al registrar una observación y un nuevo estado sobre una orden en estado distinto de Entregado, la orden muestra la observación y su estado actualizado; si la orden está en estado Entregado, el sistema no permite registrar avances.
- **Dependencias:** RF-04.

### RF-07 — Consulta del inventario de repuestos

- **Identificador:** RF-07
- **Nombre:** Consulta del inventario de repuestos
- **Enunciado:** El sistema deberá permitir al encargado de inventario consultar los repuestos registrados, mostrando código, descripción, cantidad disponible y precio referencial.
- **Fuente:** Encargado de inventario (INF-08); caso de uso CU-18 Consultar inventario.
- **Prioridad:** Should.
- **Criterio de aceptación:** Al abrir la consulta de inventario, el sistema muestra cada repuesto registrado con su código, descripción, cantidad disponible y precio referencial.
- **Dependencias:** Ninguna.

### RF-08 — Generación del comprobante de cobro en PDF

- **Identificador:** RF-08
- **Nombre:** Generación del comprobante de cobro en PDF
- **Enunciado:** El sistema deberá permitir al personal de facturación generar en formato PDF el comprobante de cobro de una orden de trabajo, con los servicios y repuestos registrados y sus valores.
- **Fuente:** Personal de facturación (INF-13, P-10); caso de uso CU-10 Generar factura PDF.
- **Prioridad:** Should.
- **Criterio de aceptación:** Al solicitar el comprobante de una orden con al menos un servicio o un repuesto, el sistema genera un archivo PDF que lista cada servicio y cada repuesto con su valor; si la orden no tiene servicios ni repuestos, el sistema muestra un mensaje y no genera el archivo.
- **Dependencias:** RF-04, RF-12, RF-13, RD-02.

### RF-09 — Registro de empleados

- **Identificador:** RF-09
- **Nombre:** Registro de empleados
- **Enunciado:** El sistema deberá permitir al administrador registrar un empleado indicando nombre, identificación, teléfono, rol (Administrador, Recepcionista, Mecánico u Operario), usuario y contraseña.
- **Fuente:** Administrador (Entregable 1: Visión §2.1 ítem 7 y Matriz de Interesados); caso de uso CU-13 Gestionar empleados.
- **Prioridad:** Could.
- **Criterio de aceptación:** Al registrar un empleado con todos los datos indicados, el empleado aparece en el listado de empleados con el rol asignado; si falta alguno de los datos, el sistema muestra un mensaje y no registra al empleado.
- **Dependencias:** Ninguna.

### RF-10 — Registro de servicios del catálogo

- **Identificador:** RF-10
- **Nombre:** Registro de servicios del catálogo
- **Enunciado:** El sistema deberá permitir al administrador registrar un servicio del catálogo indicando su nombre y su costo referencial.
- **Fuente:** Administrador (Entregable 1: Visión §2.1 ítem 7; INF-11); caso de uso CU-14 Gestionar catálogo de servicios.
- **Prioridad:** Could.
- **Criterio de aceptación:** Al registrar un servicio con nombre y costo referencial, el servicio aparece en el catálogo de servicios con ambos datos; si el costo no es un número mayor que cero, el sistema muestra un mensaje y no registra el servicio.
- **Dependencias:** Ninguna.

### RF-11 — Generación del reporte administrativo

- **Identificador:** RF-11
- **Nombre:** Generación del reporte administrativo
- **Enunciado:** El sistema deberá permitir al administrador generar un reporte administrativo que muestre el número de órdenes de trabajo por estado, el total de ingresos por servicios y la cantidad disponible de cada repuesto.
- **Fuente:** Propietario del taller (INF-14, P-11); caso de uso CU-15 Generar reporte general.
- **Prioridad:** Could.
- **Criterio de aceptación:** Al generar el reporte, el sistema muestra el número de órdenes de trabajo por estado, el total de ingresos por servicios registrados y la cantidad disponible de cada repuesto, con valores iguales a los de las órdenes, los pagos y el inventario registrados.
- **Dependencias:** RF-04, RF-07, RF-14.

### RF-12 — Generación de cotizaciones

- **Identificador:** RF-12
- **Nombre:** Generación de cotizaciones
- **Enunciado:** El sistema deberá permitir al mecánico generar la cotización de una orden de trabajo registrando el diagnóstico (fallas detectadas, observaciones técnicas y trabajos recomendados) y los servicios del catálogo, con un total igual a la suma del costo de los servicios y de los repuestos de la orden.
- **Fuente:** Mecánico (INF-06, P-05); Administrador (INF-11, P-09); casos de uso CU-01 Registrar diagnóstico y CU-02 Generar cotización.
- **Prioridad:** Must.
- **Criterio de aceptación:** Al registrar el diagnóstico con fallas, observaciones y trabajos recomendados y seleccionar al menos un servicio del catálogo, el sistema genera la cotización con un total igual a la suma del costo de los servicios y de los repuestos de la orden; si el diagnóstico está vacío, el sistema muestra un mensaje y no genera la cotización.
- **Dependencias:** RF-04, RF-10, RF-13.

### RF-13 — Uso de repuestos en una orden

- **Identificador:** RF-13
- **Nombre:** Uso de repuestos en una orden
- **Enunciado:** El sistema deberá permitir al mecánico agregar a una orden de trabajo un repuesto cuya cantidad solicitada no supere las existencias del inventario, descontando esa cantidad de las existencias.
- **Fuente:** Encargado de inventario (INF-09, P-07; conflicto C-03); caso de uso CU-03 Agregar servicios y repuestos.
- **Prioridad:** Must.
- **Criterio de aceptación:** Al agregar a una orden un repuesto con existencias suficientes, el repuesto aparece en la orden y las existencias del inventario disminuyen en la cantidad agregada; si la cantidad solicitada supera las existencias, el sistema muestra una advertencia y no agrega el repuesto.
- **Dependencias:** RF-04, RF-07.

### RF-14 — Registro de pagos

- **Identificador:** RF-14
- **Nombre:** Registro de pagos
- **Enunciado:** El sistema deberá permitir al personal de facturación registrar el pago de una orden de trabajo indicando el valor pagado.
- **Fuente:** Personal de facturación (INF-12, P-10); caso de uso CU-11 Registrar pago.
- **Prioridad:** Must.
- **Criterio de aceptación:** Al registrar un valor pagado mayor que cero y menor o igual al saldo pendiente de la orden, el sistema guarda el pago asociado a la orden y actualiza el saldo pendiente; si el valor es mayor que el saldo pendiente, no es numérico o es menor o igual a cero, el sistema muestra un mensaje y no registra el pago.
- **Dependencias:** RF-08.

### RF-15 — Registro de proveedores

- **Identificador:** RF-15
- **Nombre:** Registro de proveedores
- **Enunciado:** El sistema deberá permitir al encargado de inventario registrar un proveedor de repuestos indicando su nombre, teléfono y correo electrónico.
- **Fuente:** Encargado de inventario (INF-10, P-08); caso de uso CU-16 Registrar proveedor.
- **Prioridad:** Could.
- **Criterio de aceptación:** Al registrar un proveedor con nombre, teléfono y correo electrónico, el proveedor aparece en el listado de proveedores; si falta alguno de esos datos, el sistema muestra un mensaje y no registra al proveedor.
- **Dependencias:** Ninguna.

### RF-16 — Inicio de sesión

- **Identificador:** RF-16
- **Nombre:** Inicio de sesión
- **Enunciado:** El sistema deberá permitir a un empleado registrado iniciar sesión con su usuario y su contraseña.
- **Fuente:** Derivado del análisis de RNF-02 (Informe de Análisis y Refinamiento, problemática 5); caso de uso por definir en el Modelo de Casos de Uso.
- **Prioridad:** Should.
- **Criterio de aceptación:** Al ingresar un usuario y una contraseña registrados, el sistema abre el panel principal con el rol del empleado; al ingresar un usuario o una contraseña no registrados, el sistema muestra un mensaje de error y no abre el panel principal.
- **Dependencias:** RF-09.

## Requerimientos no funcionales

### RNF-01 — Tiempo de respuesta de la consulta de vehículo

- **Identificador:** RNF-01
- **Nombre:** Tiempo de respuesta de la consulta de vehículo
- **Enunciado:** El sistema deberá mostrar el resultado de la consulta de órdenes de trabajo de un vehículo en un máximo de 3 segundos desde que el usuario confirma la consulta.
- **Fuente:** Recepcionista y Cliente (derivado de INF-04 e INF-07); caso de uso CU-08 Consultar estado del vehículo.
- **Prioridad:** Should.
- **Criterio de aceptación:** Al consultar una placa con órdenes registradas, el resultado se muestra en 3 segundos o menos, medidos con un cronómetro desde que el usuario confirma la consulta hasta que aparece el resultado.
- **Dependencias:** RF-05.

### RNF-02 — Restricción de acceso por rol

- **Identificador:** RNF-02
- **Nombre:** Restricción de acceso por rol
- **Enunciado:** El sistema deberá denegar el acceso a las funciones de administración de empleados, servicios y reportes a los empleados cuyo rol sea distinto de Administrador.
- **Fuente:** Administrador (Entregable 1: Matriz de Interesados); caso de uso CU-13 Gestionar empleados.
- **Prioridad:** Should.
- **Criterio de aceptación:** Al iniciar sesión con rol Recepcionista, Mecánico u Operario y seleccionar una función de administración, el sistema muestra el mensaje "Acceso denegado" y no abre la función; al iniciar sesión con rol Administrador, la función se abre.
- **Dependencias:** RF-16.

### RNF-03 — Operación sin conexión a Internet

- **Identificador:** RNF-03
- **Nombre:** Operación sin conexión a Internet
- **Enunciado:** El sistema deberá ejecutar todas sus funciones sin conexión a Internet.
- **Fuente:** Administrador (INF-15); Entregable 1: Visión §3.2.
- **Prioridad:** Must.
- **Criterio de aceptación:** Con el equipo desconectado de Internet, el empleado completa el registro de un cliente, la creación de una orden de trabajo y la generación de un comprobante PDF sin que el sistema muestre errores de conexión.
- **Dependencias:** Ninguna.

### RNF-04 — Copia de seguridad de la base de datos

- **Identificador:** RNF-04
- **Nombre:** Copia de seguridad de la base de datos
- **Enunciado:** El sistema deberá permitir al administrador crear una copia de seguridad de la base de datos local SQLite en la ubicación que él seleccione.
- **Fuente:** Administrador (INF-15, P-12); caso de uso CU-21 Guardar y recuperar datos.
- **Prioridad:** Should.
- **Criterio de aceptación:** Al solicitar la copia de seguridad y seleccionar una carpeta, el sistema crea en ella un archivo con los mismos registros que la base de datos al momento de la copia y muestra un mensaje de confirmación.
- **Dependencias:** Ninguna.

### RNF-05 — Mensajes de validación en formularios

- **Identificador:** RNF-05
- **Nombre:** Mensajes de validación en formularios
- **Enunciado:** El sistema deberá mostrar un mensaje que indique el campo incorrecto cuando un empleado intente guardar un formulario de registro con un dato obligatorio vacío o con un formato inválido.
- **Fuente:** Recepcionista y Mecánico (conflictos C-01 y C-02); casos de uso CU-04 Registrar cliente y CU-19 Registrar avance.
- **Prioridad:** Should.
- **Criterio de aceptación:** Al guardar el formulario de registro de clientes con el teléfono vacío, el sistema muestra un mensaje que nombra el campo teléfono y no guarda el registro; al ingresar texto en el campo año del vehículo, el sistema muestra un mensaje que nombra ese campo y no guarda el registro.
- **Dependencias:** Ninguna.

### RNF-06 — Plataforma de ejecución

- **Identificador:** RNF-06
- **Nombre:** Plataforma de ejecución
- **Enunciado:** El sistema deberá ejecutarse como aplicación de escritorio en computadoras con sistema operativo Windows.
- **Fuente:** Equipo de desarrollo (Entregable 1: Visión §3.1); aplica a todos los casos de uso.
- **Prioridad:** Must.
- **Criterio de aceptación:** La aplicación se abre en una computadora con sistema operativo Windows y permite completar el registro de un cliente.
- **Dependencias:** Ninguna.

## Requerimientos de dominio

### RD-01 — Asociación de la orden a cliente y vehículo

- **Identificador:** RD-01
- **Nombre:** Asociación de la orden a cliente y vehículo
- **Enunciado:** El sistema deberá asociar cada orden de trabajo a un cliente registrado y a un vehículo registrado.
- **Fuente:** Recepcionista (INF-02, INF-05); caso de uso CU-07 Crear orden de trabajo.
- **Prioridad:** Must.
- **Criterio de aceptación:** Al intentar crear una orden sin un cliente registrado o sin un vehículo registrado, el sistema muestra un mensaje y no crea la orden; toda orden creada muestra el cliente y la placa del vehículo asociados.
- **Dependencias:** RF-01, RF-02.

### RD-02 — Contenido del comprobante de cobro

- **Identificador:** RD-02
- **Nombre:** Contenido del comprobante de cobro
- **Enunciado:** El sistema deberá incluir en el comprobante de cobro en PDF el número de comprobante, la fecha, los datos del cliente y del vehículo, el detalle de servicios y repuestos, el subtotal, el IVA, el total y la leyenda "Documento sin validez tributaria".
- **Fuente:** Personal de facturación (INF-13, P-10; Entregable 1: Visión §2.2 ítem 7); casos de uso CU-09 Generar factura y CU-10 Generar factura PDF.
- **Prioridad:** Should.
- **Criterio de aceptación:** El PDF generado para una orden contiene todos los elementos indicados en el enunciado, y el total es igual al subtotal más el IVA.
- **Dependencias:** Ninguna.

### RD-03 — Identificación única del vehículo por placa

- **Identificador:** RD-03
- **Nombre:** Identificación única del vehículo por placa
- **Enunciado:** El sistema deberá identificar cada vehículo por una placa única, sin limitar la cantidad de vehículos por cliente.
- **Fuente:** Administrador (INF-02, P-02); caso de uso CU-20 Registrar vehículo.
- **Prioridad:** Must.
- **Criterio de aceptación:** Al intentar registrar un vehículo con una placa ya registrada, el sistema muestra un mensaje y no lo registra; al registrar dos vehículos con placas distintas para un mismo cliente, ambos quedan asociados a ese cliente.
- **Dependencias:** Ninguna.
