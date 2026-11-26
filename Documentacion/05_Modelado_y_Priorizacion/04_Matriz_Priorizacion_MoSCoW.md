# 04. Matriz de Priorización MoSCoW

**Proyecto:** AutoServiceManager · **Actividad:** PPE07 · **Versión documental:** 1.0 — Entrega académica final.

Must identifica el núcleo de la primera iteración. Should identifica capacidades importantes para la segunda iteración. Could identifica complementos. Won’t se reserva para exclusiones acordadas. Todos los IDs del catálogo permanecen en las primeras tres categorías. Las exclusiones se gestionan en la visión.

| ID | Requerimiento | Prioridad | Justificación |
| --- | --- | --- | --- |
| RF-01 | Registro y consulta de clientes | Must | Base de atención y asociación de vehículos. |
| RF-02 | Registro de vehículos | Must | Identifica el vehículo antes de recibir el trabajo. |
| RF-03 | Gestión de citas | Should | La orden admite atención sin cita. |
| RF-04 | Creación de órdenes de trabajo | Must | Es el registro central del trabajo y cobro. |
| RF-05 | Consulta de estado e historial por placa | Should | Apoya el seguimiento diario del taller. |
| RF-06 | Registro del avance de una orden | Should | El trabajo puede ingresar antes de habilitar el seguimiento completo. |
| RF-07 | Consulta y actualización de inventario | Must | Proporciona las existencias que usa RF-13. |
| RF-08 | Generación del comprobante en PDF | Should | Entrega el comprobante; el cobro interno puede registrarse antes. |
| RF-09 | Gestión de empleados | Should | Se entrega antes del inicio de sesión que depende del registro de personal. |
| RF-10 | Gestión del catálogo de servicios | Must | RF-12 requiere servicios y precios de referencia. |
| RF-11 | Reporte administrativo | Could | Es análisis administrativo después de estabilizar los módulos fuente. |
| RF-12 | Diagnóstico y gestión de cotizaciones | Must | La cotización comunica el costo antes del trabajo. |
| RF-13 | Consumo de repuestos en una orden | Must | Evita desajustes de inventario durante el trabajo. |
| RF-14 | Registro local de pagos | Must | Es la operación central del cobro. |
| RF-15 | Registro de proveedores | Could | Es apoyo del inventario; puede programarse después del control de stock. |
| RF-16 | Inicio de sesión de empleados | Should | La prioridad coincide con la del control de acceso dependiente. |
| RNF-01 | Tiempo de respuesta del historial | Should | Es una meta de calidad del seguimiento. |
| RNF-02 | Control de acceso por rol | Should | Se verifica una vez disponible RF-16. |
| RNF-03 | Operación local y persistencia | Must | Sostiene la operación cotidiana del taller. |
| RNF-04 | Almacenamiento SQLite y respaldo | Should | Aporta recuperación ante pérdida del archivo de trabajo. |
| RNF-05 | Validación de formularios | Should | Reduce errores de captura en el taller. |
| RNF-06 | Entorno Windows de escritorio | Must | Define el entorno de ejecución de la primera versión. |
| RD-01 | Asociación de la orden al cliente y vehículo | Must | Asegura la integridad del historial. |
| RD-02 | Contenido del comprobante de servicio | Should | Protege la claridad del comprobante dentro del alcance de PPE03. |
| RD-03 | Placa única por vehículo | Must | Evita duplicidad de expedientes y sostiene el historial. |

### Negociación y planificación

| Conflicto | Solución técnica incorporada en la entrega | Requerimientos | Coordinación interna asignada | Ratificación de interesados |
|---|---|---|---|---|
| C-01 · Detalle de registros | Nombre, identificación y teléfono son obligatorios para clientes. Correo y dirección pueden omitirse en el alta. En vehículos se conservan placa, marca, modelo, año y propietario registrado. | RF-01, RF-02, RNF-05 | Joel Ordoñez | Sin constancia |
| C-02 · Tiempo de registro de avances | Se guarda la observación y el estado de la orden. La secuencia de RF-06 admite observaciones en el mismo estado y cierra en Entregado. Los importes de mano de obra se gestionan como partidas. | RF-04, RF-05, RF-06, RF-12 | Ronald Veloz | Sin constancia |
| C-03 · Control de repuestos | Inventario registra entradas. Mecánica estima repuestos en la cotización y descuenta existencias al registrar el consumo en la orden. | RF-07, RF-12, RF-13 | Pablo Cuasapaz | Sin constancia |
| C-04 · Funciones en línea | Recepción opera la agenda del taller. Programada y Confirmada ocupan el turno. La consulta y comunicación con el cliente se realizan por medio del personal. | RF-03, RF-05, RNF-03 | Joel Ordoñez | Sin constancia |

Las soluciones resuelven el alcance de la especificación académica a partir de los conflictos simulados de PPE04. La coordinación asignada identifica la responsabilidad documental. El estado de ratificación se consigna en el control de versiones.

| Iteración | Alcance | Orden de trabajo |
|---|---|---|
| I1 · Operación básica | RF-01, RF-02, RF-04, RF-07, RF-10, RF-12, RF-13, RF-14, RNF-03, RNF-06, RD-01, RD-03 | Establecer el entorno, clientes, vehículos y catálogos. Después habilitar órdenes, cotización, consumo y pagos. |
| I2 · Agenda, seguimiento y administración | RF-03, RF-05, RF-06, RF-08, RF-09, RF-16, RNF-01, RNF-02, RNF-04, RNF-05, RD-02 | Implementar RF-09 antes de RF-16 y RNF-02. Aplicar RNF-02 antes de restringir el respaldo. Implementar RF-06 antes de RF-05 y medir RNF-01 después. Completar RD-02 antes de aceptar el PDF. |
| I3 · Complementos | RF-11, RF-15 | Consolidar el reporte con datos reales de los módulos y habilitar el registro de proveedores. Verificar CU-17 con un proveedor registrado. |

I1 comprueba RF-07 mediante el alta y la actualización por consumo. I3 verifica la entrada por compra con proveedor. Los permisos de RNF-02 se integran transversalmente en I2 antes de aceptar los módulos protegidos. La secuencia expresa la planificación académica. El estado real de cada funcionalidad consta en la matriz de trazabilidad.
