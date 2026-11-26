# 02. Matriz de Interesados

**Nombre de la aplicación:** AutoServiceManager\
**Asignatura:** Diseño y Programación de Software\
**Fase:** Ingeniería de Requerimientos\
**Entregable:** 1 - Configuración del Proyecto, Visión y Matriz de Interesados

## 1. Introducción

La identificación de los interesados constituye una actividad importante dentro de la Ingeniería de Requerimientos, ya que permite reconocer a las personas, grupos u organizaciones que utilizan el sistema, participan en su desarrollo o pueden verse afectados por su funcionamiento.

En el proyecto AutoServiceManager, los interesados están relacionados con las actividades administrativas y operativas de un taller automotriz. Cada uno presenta necesidades, expectativas e intereses que deben analizarse para definir adecuadamente los requerimientos del sistema.

El presente documento tiene como propósito identificar y clasificar a los principales interesados de AutoServiceManager, establecer sus niveles de interés e influencia, reconocer sus necesidades y analizar los posibles conflictos que podrían surgir durante el desarrollo del proyecto.

## 2. Criterios de clasificación

Para elaborar la matriz de interesados se utilizarán los siguientes criterios:

- **Interno:** Interesado que pertenece a la organización donde funcionará el sistema.
- **Externo:** Interesado que no pertenece a la organización, pero mantiene una relación con ella o puede verse afectado por el sistema.
- **Directo:** Interesado que interactúa directamente con la aplicación durante sus actividades.
- **Indirecto:** Interesado que no utiliza directamente la aplicación, pero tiene necesidades relacionadas con ella o recibe sus resultados.
- **Interés:** Nivel de importancia que tiene el sistema para el interesado. Se clasifica en alto, medio o bajo.
- **Influencia:** Capacidad del interesado para afectar las decisiones, los requerimientos o el desarrollo del sistema. Se clasifica en alta, media o baja.


## 3. Matriz de Interesados

La siguiente matriz identifica a los principales interesados de AutoServiceManager, considerando su relación con el taller automotriz, su nivel de interés e influencia y las necesidades que esperan satisfacer mediante el sistema.

| Interesado | Clasificación | Interés | Influencia | Necesidad principal | Posibles conflictos |
|---|---|---|---|---|---|
| Propietario del taller | Interno / Indirecto | Alto | Alta | Supervisar las operaciones y los ingresos mediante reportes preparados por el administrador. | Puede solicitar reportes detallados que aumenten la complejidad y el tiempo de desarrollo. |
| Administrador del taller | Interno / Directo | Alto | Alta | Gestionar clientes, vehículos, servicios, personal y operaciones generales del taller. | Puede priorizar el control administrativo frente a la rapidez de las operaciones diarias. |
| Recepcionista | Interno / Directo | Alto | Media | Registrar clientes, vehículos, citas y órdenes de trabajo de manera organizada. | Puede requerir registros sencillos, mientras que la administración solicita información más detallada. |
| Mecánico | Interno / Directo | Alto | Media | Consultar órdenes de trabajo, registrar diagnósticos y actualizar los avances de los servicios. | Puede considerar excesiva la información que debe registrar durante sus actividades. |
| Encargado de inventario | Interno / Directo | Alto | Media | Controlar las existencias de repuestos y registrar su utilización. | Puede existir desacuerdo con los mecánicos sobre el registro y actualización de repuestos utilizados. |
| Personal de facturación | Interno / Directo | Alto | Media | Consultar los valores de los servicios, registrar pagos y generar comprobantes. | Puede necesitar información completa de las órdenes antes de registrar los pagos. |
| Cliente del taller | Externo / Indirecto | Alto | Baja | Recibir atención organizada, conocer el estado de su vehículo y obtener información clara sobre los costos. | Puede solicitar consultas y reservas en línea que no están contempladas en la primera versión. |
| Proveedor de repuestos | Externo / Indirecto | Medio | Baja | Mantener un registro organizado de los repuestos suministrados al taller. | Puede solicitar integración automática de pedidos, funcionalidad excluida del alcance inicial. |
| Equipo de desarrollo | Externo / Indirecto | Alto | Alta | Comprender los requerimientos y desarrollar una aplicación que cumpla con el alcance establecido. | Las solicitudes adicionales pueden superar el tiempo y los recursos disponibles para el proyecto. |

La clasificación directa se refiere a la operación del taller. El equipo de desarrollo configura y comprueba el producto durante el proyecto académico. El propietario y el cliente reciben información a través del personal. El acceso de cada rol a las funciones se define en la tabla de permisos de [PPE06](../04_Requerimientos_Verificables/01_Requerimientos_Refinados.md#tabla-de-acceso-por-rol). Los niveles se mantienen como valoración inicial.


## 4. Resolución de Conflictos

Durante el desarrollo de AutoServiceManager pueden surgir diferencias entre las necesidades y expectativas de los interesados. Por ejemplo, el administrador del taller puede requerir información detallada para mantener un mayor control de las operaciones, mientras que el personal operativo necesita realizar registros rápidos y sencillos para no interrumpir sus actividades.

Para resolver estos conflictos, el equipo de desarrollo identificará las necesidades de cada interesado mediante técnicas de adquisición de requerimientos, como entrevistas y reuniones de trabajo. Posteriormente, se analizarán las diferencias encontradas, considerando las reglas del negocio, la importancia de cada necesidad y las restricciones del proyecto.

Cuando existan requerimientos contradictorios, se promoverá la negociación entre los interesados para establecer acuerdos que permitan satisfacer las necesidades principales sin afectar el funcionamiento del sistema. Las decisiones alcanzadas serán documentadas y consideradas durante la especificación y validación de los requerimientos de AutoServiceManager.

De esta manera, se buscará mantener la coherencia de los requerimientos, evitar contradicciones y desarrollar una solución que responda a las necesidades reales del taller automotriz.
