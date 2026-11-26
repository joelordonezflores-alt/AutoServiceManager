# AutoServiceManager

Aplicación de escritorio para la gestión administrativa y operativa de talleres automotrices. Proyecto de Diseño y Programación de Software, Escuela Politécnica Nacional.

## Problemática

Los registros manuales o dispersos dificultan localizar clientes y vehículos, coordinar citas y reparaciones, controlar existencias y consultar los importes cobrados.

## Solución propuesta

AutoServiceManager centraliza clientes, vehículos, citas, órdenes de trabajo, diagnósticos, inventario, cotizaciones, comprobantes y pagos. Ofrece consultas operativas y reportes administrativos. La primera versión se plantea para el personal del taller y almacenamiento local. El [alcance y las exclusiones](Documentacion/01_Gestion_y_Alcance/01_Vision_y_Alcance.md) detallan las capacidades previstas.

## Integrantes y responsabilidades

| Integrante | Rol |
|---|---|
| Joel Ordoñez | Product Owner y desarrollador |
| Ronald Veloz | Scrum Master y desarrollador |
| Pablo Cuasapaz | Desarrollador |

Joel Ordoñez coordina alcance y adquisición. Ronald Veloz mantiene el catálogo de requerimientos y la planificación. Pablo Cuasapaz coordina el modelado, la trazabilidad y las pruebas. El equipo usa Scrum como referencia para organizar el trabajo.

## Tecnologías

| Tecnología | Uso en el proyecto |
|---|---|
| C# y Windows Forms | Aplicación de escritorio y formularios. |
| .NET 10 | Plataforma del proyecto con destino `net10.0-windows`. |
| SQLite mediante Microsoft.Data.Sqlite | Persistencia local, consultas y copias de seguridad. |
| PDFsharp-MigraDoc | Comprobantes de servicio y exportación de reportes. |
| Visual Studio | Apertura de la solución y compilación en Windows. |
| Git, GitHub y Markdown | Versionado, colaboración y documentación. |

## Estructura del repositorio

| Ruta | Contenido |
|---|---|
| [AutoServiceManager.sln](AutoServiceManager.sln) y [AutoServiceManager.slnx](AutoServiceManager.slnx) | Soluciones de Visual Studio. |
| [AutoServiceManager/](AutoServiceManager/) | Formularios, modelos, servicios, esquema SQLite y recursos. |
| [AutoServiceManager/Verificacion/](AutoServiceManager/Verificacion/) | Comprobaciones automatizadas de los servicios. |
| [Documentacion/01_Gestion_y_Alcance/](Documentacion/01_Gestion_y_Alcance/) | PPE03 · visión y matriz de interesados. |
| [Documentacion/02_Adquisicion_e_Interesados/](Documentacion/02_Adquisicion_e_Interesados/) | PPE04 · adquisición, aclaraciones y conflictos. |
| [Documentacion/03_Catalogo_y_Refinamiento/](Documentacion/03_Catalogo_y_Refinamiento/) | PPE05 · catálogo y análisis. |
| [Documentacion/04_Requerimientos_Verificables/](Documentacion/04_Requerimientos_Verificables/) | PPE06 · fichas y criterios verificables. |
| [Documentacion/05_Modelado_y_Priorizacion/](Documentacion/05_Modelado_y_Priorizacion/) | PPE07 · casos de uso, diagrama, historias y prioridades. |
| [Documentacion/06_Trazabilidad_y_Validacion/](Documentacion/06_Trazabilidad_y_Validacion/) | PPE08 · trazabilidad, validación, versiones y registro cronológico. |
| [Documentacion/SRS_Final.md](Documentacion/SRS_Final.md) | Documento maestro, versión 1.0 — Entrega académica final. |

El código implementa persistencia local y módulos de operación del taller. La [matriz de trazabilidad](Documentacion/06_Trazabilidad_y_Validacion/01_Matriz_Trazabilidad.md) indica la evidencia, las pruebas y las brechas de cada requerimiento.
