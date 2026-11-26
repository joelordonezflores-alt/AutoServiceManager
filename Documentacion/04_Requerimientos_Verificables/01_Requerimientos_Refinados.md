# 01. Requerimientos Refinados y Verificables

**Proyecto:** AutoServiceManager · **Actividad:** PPE06 · **Versión documental:** 1.0 — Entrega académica final.

Estas fichas detallan los 25 requerimientos de PPE05. La [adquisición](../02_Adquisicion_e_Interesados/01_Adquisicion_Requerimientos.md) identifica fuentes simuladas y documentales. Los procedimientos CP definen cómo comprobar la aceptación. La [matriz de trazabilidad](../06_Trazabilidad_y_Validacion/01_Matriz_Trazabilidad.md) registra la cobertura observada. Las [decisiones técnicas de PPE08](../06_Trazabilidad_y_Validacion/02_Resultados_Validacion.md#4-decisiones-técnicas-de-la-especificación) fundamentan los enunciados de la entrega 1.0.

## Requerimientos funcionales

### RF-01 · Registro y consulta de clientes

- **Identificador:** RF-01
- **Nombre:** Registro y consulta de clientes
- **Enunciado:** El sistema deberá permitir a Recepción registrar y localizar clientes por identificación con nombre, identificación y teléfono obligatorios, y correo y dirección opcionales.
- **Fuente:** Recepcionista · INF-01, P-01, C-01
- **Prioridad:** Must
- **Criterio de aceptación:** Con nombre, identificación y teléfono se guarda un cliente localizable por identificación aunque falten correo y dirección. Si falta un dato obligatorio, se identifica el campo y se impide el alta.
- **Dependencias:** Ninguna.
- **Caso de uso o prueba:** CU-04, CU-05 · CP-01.
- **Estado del requisito:** Analizado.

### RF-02 · Registro de vehículos

- **Identificador:** RF-02
- **Nombre:** Registro de vehículos
- **Enunciado:** El sistema deberá permitir a Recepción registrar placa, marca, modelo y año de un vehículo asociado a un cliente registrado.
- **Fuente:** Administrador · INF-02, P-02
- **Prioridad:** Must
- **Criterio de aceptación:** El vehículo queda visible para su cliente con los cuatro datos; al indicar un cliente inexistente, el sistema rechaza el alta.
- **Dependencias:** RF-01, RD-03.
- **Caso de uso o prueba:** CU-20 · CP-02.
- **Estado del requisito:** Analizado.

### RF-03 · Gestión de citas

- **Identificador:** RF-03
- **Nombre:** Gestión de citas
- **Enunciado:** El sistema deberá permitir a Recepción gestionar altas, consultas, reprogramaciones y cancelaciones de citas con cliente, vehículo, fecha, hora y motivo, con un solo turno activo por fecha y hora en la agenda del taller.
- **Fuente:** Recepcionista · INF-03, P-03, C-04
- **Prioridad:** Should
- **Criterio de aceptación:** El alta válida queda Programada y aparece en la agenda. Programada y Confirmada ocupan el turno. Recepción puede mover una cita activa a un turno libre conservando su estado o cancelarla para liberar su turno. Atendida y Cancelada cierran la cita.
- **Dependencias:** RF-02.
- **Caso de uso o prueba:** CU-06 · CP-03.
- **Estado del requisito:** Analizado.

### RF-04 · Creación de órdenes de trabajo

- **Identificador:** RF-04
- **Nombre:** Creación de órdenes de trabajo
- **Enunciado:** El sistema deberá permitir a Recepción abrir una orden para un cliente y un vehículo registrados con al menos un servicio solicitado, con cita previa opcional.
- **Fuente:** Administrador · INF-05; alta sin cita en el catálogo anterior y HU-01
- **Prioridad:** Must
- **Criterio de aceptación:** Una orden válida queda en estado Recibido con o sin cita vinculada y muestra el servicio solicitado. Si se usa una cita pendiente del mismo cliente y vehículo, pasa a Atendida. Se rechaza una orden sin cliente, vehículo o servicio solicitado.
- **Dependencias:** RF-10, RD-01.
- **Caso de uso o prueba:** CU-07 · CP-04.
- **Estado del requisito:** Analizado.

### RF-05 · Consulta de estado e historial por placa

- **Identificador:** RF-05
- **Nombre:** Consulta de estado e historial por placa
- **Enunciado:** El sistema deberá mostrar el estado actual y el historial de órdenes y servicios de un vehículo identificado por su placa.
- **Fuente:** Mecánico · INF-04, INF-07, P-04
- **Prioridad:** Should
- **Criterio de aceptación:** Para una placa con órdenes se muestra primero la más reciente y su estado, con acceso a los servicios de cada orden. Para una placa sin órdenes se indica que no tiene historial.
- **Dependencias:** RF-04, RF-06.
- **Caso de uso o prueba:** CU-08 · CP-05.
- **Estado del requisito:** Analizado.

### RF-06 · Registro del avance de una orden

- **Identificador:** RF-06
- **Nombre:** Registro del avance de una orden
- **Enunciado:** El sistema deberá permitir a Mecánico u Operario registrar el avance de una orden activa con una observación y el estado Recibido, En diagnóstico, En reparación, Listo para entrega o Entregado.
- **Fuente:** Mecánico · INF-07, P-06 y C-02. Secuencia refinada en PPE05.
- **Prioridad:** Should
- **Criterio de aceptación:** Al guardar una observación, la orden conserva su estado o avanza a uno posterior en la secuencia Recibido → En diagnóstico → En reparación → Listo para entrega → Entregado. El avance queda visible en el historial. El sistema rechaza un retroceso o un nuevo avance sobre una orden Entregada.
- **Dependencias:** RF-04.
- **Caso de uso o prueba:** CU-19 · CP-06.
- **Estado del requisito:** Analizado.

### RF-07 · Consulta y actualización de inventario

- **Identificador:** RF-07
- **Nombre:** Consulta y actualización de inventario
- **Enunciado:** El sistema deberá permitir al Encargado de inventario mantener el catálogo y las existencias de repuestos con código, descripción, cantidad disponible y precio referencial.
- **Fuente:** Encargado de inventario · INF-08, INF-09, P-07
- **Prioridad:** Must
- **Criterio de aceptación:** Al dar de alta un repuesto se muestran código, descripción, cantidad y precio. Después de registrar un movimiento válido, el listado refleja la existencia resultante.
- **Dependencias:** Ninguna.
- **Caso de uso o prueba:** CU-17, CU-18 · CP-07.
- **Estado del requisito:** Analizado.

### RF-08 · Generación del comprobante en PDF

- **Identificador:** RF-08
- **Nombre:** Generación del comprobante en PDF
- **Enunciado:** El sistema deberá permitir al Personal de facturación exportar en PDF el comprobante de una orden con partidas y subtotal mayor que cero.
- **Fuente:** Personal de facturación · INF-13, P-10; Visión §2.2
- **Prioridad:** Should
- **Criterio de aceptación:** Con una orden con partidas y subtotal mayor que cero se genera un PDF con sus partidas, valores y campos de RD-02. Si faltan partidas o el subtotal es cero, se informa la causa y no se genera el PDF.
- **Dependencias:** RF-04, RD-02.
- **Caso de uso o prueba:** CU-09, CU-10 · CP-08.
- **Estado del requisito:** Analizado.

### RF-09 · Gestión de empleados

- **Identificador:** RF-09
- **Nombre:** Gestión de empleados
- **Enunciado:** El sistema deberá permitir a Administración registrar, consultar y actualizar empleados con nombre, identificación, teléfono y rol requeridos.
- **Fuente:** Administrador · Visión §2.1.7 y Matriz PPE03; insumo de PPE04 F-DOC
- **Prioridad:** Should
- **Criterio de aceptación:** El alta aparece en el listado con la identificación y el rol elegidos. Al modificar teléfono o rol, la consulta posterior muestra el cambio conservando la identificación.
- **Dependencias:** Ninguna.
- **Caso de uso o prueba:** CU-13 · CP-09.
- **Estado del requisito:** Analizado.

### RF-10 · Gestión del catálogo de servicios

- **Identificador:** RF-10
- **Nombre:** Gestión del catálogo de servicios
- **Enunciado:** El sistema deberá permitir a Administración registrar, consultar y actualizar los servicios del catálogo con nombre y precio referencial mayor que cero.
- **Fuente:** Administrador · Visión §2.1.7 y F-DOC; INF-11/P-09 sustentan los costos de cotización de forma indirecta
- **Prioridad:** Must
- **Criterio de aceptación:** El servicio aparece en el catálogo y al editar su precio se muestra el nuevo valor. Se rechaza un precio cero o negativo sin guardar ni modificar la ficha.
- **Dependencias:** Ninguna.
- **Caso de uso o prueba:** CU-14 · CP-15.
- **Estado del requisito:** Analizado.

### RF-11 · Reporte administrativo

- **Identificador:** RF-11
- **Nombre:** Reporte administrativo
- **Enunciado:** El sistema deberá permitir a Administración generar un reporte con número de órdenes por estado, total de pagos registrados y stock disponible por repuesto.
- **Fuente:** Propietario · INF-14 y P-11. El desglose de stock se deriva de INF-08.
- **Prioridad:** Could
- **Criterio de aceptación:** El reporte reproduce el conteo agrupado de órdenes, la suma de pagos y la existencia de cada repuesto para el mismo conjunto de datos.
- **Dependencias:** RF-04, RF-07, RF-14.
- **Caso de uso o prueba:** CU-15 · CP-16.
- **Estado del requisito:** Analizado.

### RF-12 · Diagnóstico y gestión de cotizaciones

- **Identificador:** RF-12
- **Nombre:** Diagnóstico y gestión de cotizaciones
- **Enunciado:** El sistema deberá permitir al Mecánico gestionar la cotización de una orden con su diagnóstico, costos estimados y decisión comunicada por el cliente.
- **Fuente:** Mecánico y Administrador · INF-06, P-05, INF-11 y P-09. El registro de la decisión procede de HU-03, CU-12 y la operación existente del módulo de taller.
- **Prioridad:** Must
- **Criterio de aceptación:** El diagnóstico contiene fallas, observaciones y trabajos recomendados. La cotización parte de al menos un servicio del catálogo, suma servicios y repuestos estimados, y conserva el stock. Un diagnóstico vacío impide emitirla. La emisión queda Pendiente. Mecánica registra la decisión del cliente como Aprobada o Rechazada. Cancelar el diálogo conserva el estado de la cotización.
- **Dependencias:** RF-04, RF-07, RF-10.
- **Caso de uso o prueba:** CU-01, CU-02, CU-03 (servicios), CU-12 · CP-17.
- **Estado del requisito:** Analizado.

### RF-13 · Consumo de repuestos en una orden

- **Identificador:** RF-13
- **Nombre:** Consumo de repuestos en una orden
- **Enunciado:** El sistema deberá permitir a Mecánico registrar en una orden el consumo de una cantidad entera positiva de un repuesto disponible con descuento de esa cantidad del inventario.
- **Fuente:** Encargado de inventario · INF-09, P-07, C-03
- **Prioridad:** Must
- **Criterio de aceptación:** Si hay stock suficiente, la orden incorpora la partida y el stock disminuye en la cantidad indicada. Si la cantidad es cero, negativa o superior al stock, se informa la causa y ambos registros quedan intactos.
- **Dependencias:** RF-04, RF-07.
- **Caso de uso o prueba:** CU-03 · CP-18.
- **Estado del requisito:** Analizado.

### RF-14 · Registro local de pagos

- **Identificador:** RF-14
- **Nombre:** Registro local de pagos
- **Enunciado:** El sistema deberá permitir al Personal de facturación registrar un pago local asociado a una orden por un monto positivo que no supere su saldo pendiente.
- **Fuente:** Personal de facturación · INF-12, P-10
- **Prioridad:** Must
- **Criterio de aceptación:** Un pago parcial reduce el saldo y un pago por el saldo lo lleva a cero. Un monto inválido se rechaza sin registrar pago. La exportación del PDF no es una precondición.
- **Dependencias:** RF-04.
- **Caso de uso o prueba:** CU-11; CU-09 de soporte · CP-19.
- **Estado del requisito:** Analizado.

### RF-15 · Registro de proveedores

- **Identificador:** RF-15
- **Nombre:** Registro de proveedores
- **Enunciado:** El sistema deberá permitir al Encargado de inventario registrar un proveedor con nombre, teléfono y correo electrónico para consulta local.
- **Fuente:** Encargado de inventario · INF-10, P-08
- **Prioridad:** Could
- **Criterio de aceptación:** Un proveedor ingresado con los tres datos aparece en el listado. Un campo obligatorio vacío impide el alta. El alta no genera pedidos externos.
- **Dependencias:** Ninguna.
- **Caso de uso o prueba:** CU-16 · CP-20.
- **Estado del requisito:** Analizado.

### RF-16 · Inicio de sesión de empleados

- **Identificador:** RF-16
- **Nombre:** Inicio de sesión de empleados
- **Enunciado:** El sistema deberá permitir a un empleado registrado iniciar sesión con su cuenta y acceder al panel asociado al rol asignado.
- **Fuente:** Derivado de RNF-02 · Informe PPE05 §A y matriz de interesados. La validación con interesados consta como pendiente en PPE08.
- **Prioridad:** Should
- **Criterio de aceptación:** Una cuenta habilitada con credenciales correctas abre el panel con su rol. Un usuario o contraseña incorrectos dejan el panel cerrado y muestran un error.
- **Dependencias:** RF-09.
- **Caso de uso o prueba:** CU-22 · CP-21.
- **Estado del requisito:** Analizado.

## Requerimientos no funcionales

### RNF-01 · Tiempo de respuesta del historial

- **Identificador:** RNF-01
- **Nombre:** Tiempo de respuesta del historial
- **Enunciado:** El sistema deberá mostrar el historial solicitado por placa en un máximo de tres segundos desde que se elige la consulta.
- **Fuente:** INF-04, INF-07 y P-04 fundamentan la consulta. El catálogo PPE05 fija el umbral de tres segundos.
- **Prioridad:** Should
- **Criterio de aceptación:** Cada una de cinco consultas muestra el historial o el aviso de ausencia dentro de tres segundos. La comprobación usa Windows con el runtime .NET 10 y una base sintética con 100 órdenes y 20 vehículos, uno sin órdenes. Se registra el equipo utilizado.
- **Dependencias:** RF-05.
- **Caso de uso o prueba:** CU-08 · CP-10.
- **Estado del requisito:** Analizado.

### RNF-02 · Control de acceso por rol

- **Identificador:** RNF-02
- **Nombre:** Control de acceso por rol
- **Enunciado:** El sistema deberá aplicar a cada función protegida los permisos del rol activo según la tabla de acceso de PPE06.
- **Fuente:** Administrador · Matriz PPE03 y F-DOC. La tabla de acceso se deriva de los actores asociados a cada caso de uso.
- **Prioridad:** Should
- **Criterio de aceptación:** Cada función de la tabla de acceso se abre con el rol habilitado. Con un rol distinto queda bloqueada y se muestra Acceso denegado. Sin sesión, el sistema solicita la autenticación de RF-16.
- **Dependencias:** RF-16.
- **Caso de uso o prueba:** CU-01 a CU-21, según la tabla de acceso, y CU-22 para el inicio de sesión · CP-11.
- **Estado del requisito:** Analizado.

### RNF-03 · Operación local y persistencia

- **Identificador:** RNF-03
- **Nombre:** Operación local y persistencia
- **Enunciado:** El sistema deberá operar sin conexión a Internet con conservación de las operaciones confirmadas ante un cierre inesperado de la aplicación.
- **Fuente:** Administrador · INF-15, P-12, Visión §3.2
- **Prioridad:** Must
- **Criterio de aceptación:** Con Windows desconectado, el recorrido de cada módulo incluido en la versión permite terminar las operaciones sin errores de conexión. Tras forzar el cierre después de confirmar un registro, el registro sigue visible al reabrir.
- **Dependencias:** Ninguna.
- **Caso de uso o prueba:** Transversal · CP-12.
- **Estado del requisito:** Analizado.

### RNF-04 · Almacenamiento SQLite y respaldo

- **Identificador:** RNF-04
- **Nombre:** Almacenamiento SQLite y respaldo
- **Enunciado:** El sistema deberá mantener los datos en SQLite local y reservar a Administración la generación de copias de seguridad en una ruta seleccionada.
- **Fuente:** Administrador · INF-15, P-12, Visión §3.2
- **Prioridad:** Should
- **Criterio de aceptación:** Una sesión autorizada selecciona un destino y crea un archivo SQLite abrible con los datos confirmados al momento de la copia; una sesión sin permiso no puede iniciarla.
- **Dependencias:** RNF-02.
- **Caso de uso o prueba:** CU-21 · CP-22.
- **Estado del requisito:** Analizado.

### RNF-05 · Validación de formularios

- **Identificador:** RNF-05
- **Nombre:** Validación de formularios
- **Enunciado:** El sistema deberá identificar el campo obligatorio vacío o inválido al intentar guardar un formulario y preservar el contenido previamente guardado ante el rechazo.
- **Fuente:** Recepcionista y Mecánico · C-01, C-02, INF-01/02/06/07
- **Prioridad:** Should
- **Criterio de aceptación:** Un teléfono obligatorio vacío, un año inválido y un importe inválido quedan identificados en el formulario correspondiente, por mensaje o restricción visible del control. Ningún intento inválido altera los datos persistidos.
- **Dependencias:** Ninguna.
- **Caso de uso o prueba:** CU-04, CU-11, CU-19, CU-20 · CP-23.
- **Estado del requisito:** Analizado.

### RNF-06 · Entorno Windows de escritorio

- **Identificador:** RNF-06
- **Nombre:** Entorno Windows de escritorio
- **Enunciado:** El sistema deberá ejecutarse como aplicación de escritorio Windows sobre .NET 10 sin depender de servicios externos de pago.
- **Fuente:** Equipo · Visión §§3.1 y 3.3; F-DOC, F-TEC para el soporte técnico
- **Prioridad:** Must
- **Criterio de aceptación:** En un equipo Windows con el runtime requerido se abre la aplicación y se completa el alta de un cliente con datos válidos sin contratar ni configurar un servicio externo de pago.
- **Dependencias:** Ninguna.
- **Caso de uso o prueba:** Transversal · CP-24.
- **Estado del requisito:** Analizado.

## Requerimientos de dominio

### RD-01 · Asociación de la orden al cliente y vehículo

- **Identificador:** RD-01
- **Nombre:** Asociación de la orden al cliente y vehículo
- **Enunciado:** El sistema deberá asociar cada orden de trabajo a un vehículo registrado y al cliente registrado como propietario de ese vehículo.
- **Fuente:** Administrador · INF-02, INF-05; Recepción opera CU-07
- **Prioridad:** Must
- **Criterio de aceptación:** Una orden válida muestra la placa y el cliente del vehículo. La creación con un cliente inexistente o con una placa asociada a otro cliente se rechaza.
- **Dependencias:** RF-02.
- **Caso de uso o prueba:** CU-07 · CP-13.
- **Estado del requisito:** Analizado.

### RD-02 · Contenido del comprobante de servicio

- **Identificador:** RD-02
- **Nombre:** Contenido del comprobante de servicio
- **Enunciado:** El sistema deberá incluir en el PDF número, fecha, datos del cliente y vehículo, detalle de servicios y repuestos, subtotal, IVA, total y la leyenda «Documento sin validez tributaria».
- **Fuente:** Personal de facturación · INF-13, P-10, Visión §2.2.7; detalle refinado en PPE05/06
- **Prioridad:** Should
- **Criterio de aceptación:** El documento presenta los campos enumerados y la leyenda visible. Con una tasa configurada para el ensayo, el total coincide con la suma del subtotal y el IVA redondeado a centavos.
- **Dependencias:** Ninguna.
- **Caso de uso o prueba:** CU-09, CU-10 · CP-14.
- **Estado del requisito:** Analizado.

### RD-03 · Placa única por vehículo

- **Identificador:** RD-03
- **Nombre:** Placa única por vehículo
- **Enunciado:** El sistema deberá identificar cada vehículo por una placa única en el sistema y admitir varios vehículos con placas distintas para el mismo cliente.
- **Fuente:** Administrador · INF-02, P-02
- **Prioridad:** Must
- **Criterio de aceptación:** Una placa existente se rechaza como alta nueva, incluso si cambia el uso de mayúsculas o los espacios exteriores. Dos placas distintas pueden asociarse al mismo cliente.
- **Dependencias:** Ninguna.
- **Caso de uso o prueba:** CU-20 · CP-25.
- **Estado del requisito:** Analizado.

## Tabla de acceso por rol

| Rol habilitado | Funciones protegidas | Casos de uso |
| --- | --- | --- |
| Recepcionista | Clientes, vehículos, citas, órdenes e historial por placa. | CU-04, CU-05, CU-06, CU-07, CU-08 y CU-20 |
| Mecánico | Diagnóstico, cotización, decisión del cliente, partidas de servicios y repuestos y avance de órdenes. | CU-01, CU-02, CU-03, CU-12 y CU-19 |
| Operario | Registro de avances de la orden. | CU-19 |
| Encargado de inventario | Catálogo y existencias de repuestos, proveedores y entradas por compra. | CU-16, CU-17 y CU-18 |
| Personal de facturación | Comprobante interno, exportación a PDF y registro de pagos. | CU-09, CU-10 y CU-11 |
| Administrador | Empleados, catálogo de servicios, reportes y copias de seguridad. | CU-13, CU-14, CU-15 y CU-21 |

CU-22 está disponible sin sesión para que el empleado pueda autenticarse. El rol activo se obtiene de la cuenta asociada al empleado. RNF-02 protege las funciones aunque se intente acceder a ellas desde otra pantalla.
