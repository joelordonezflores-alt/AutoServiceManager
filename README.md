# AutoServiceManager


## 1. Descripción general

AutoServiceManager es un proyecto de aplicación de escritorio orientado a la gestión administrativa y operativa de talleres automotrices. Su propósito es facilitar la organización de la información relacionada con clientes, vehículos, citas, órdenes de trabajo, inventario, servicios y pagos.

El sistema se desarrollará como parte de la asignatura Diseño y Programación de Software, aplicando los conocimientos de Ingeniería de Requerimientos, diseño de software y programación mediante formularios.

## 2. Problemática

En los talleres automotrices que utilizan registros manuales o archivos independientes pueden presentarse dificultades para organizar y consultar la información de sus operaciones.

Estas dificultades incluyen errores en el registro de clientes y vehículos, problemas para realizar el seguimiento de los servicios, falta de control de repuestos y demoras al consultar los costos de los trabajos realizados.

La ausencia de información centralizada puede afectar la coordinación entre el personal administrativo y operativo, dificultando el seguimiento de las actividades del taller.

## 3. Solución propuesta

Se propone desarrollar AutoServiceManager, una aplicación de escritorio que permitirá centralizar la información del taller automotriz y facilitar la administración de sus principales procesos.

La aplicación estará orientada al registro y consulta de clientes y vehículos, gestión de citas, seguimiento de órdenes de trabajo, control de inventario, registro de pagos y generación de reportes administrativos.

Con esta solución se busca mejorar la organización de las operaciones, reducir errores en el manejo de información y facilitar las actividades del personal del taller.


## 4. Integrantes del equipo

El proyecto AutoServiceManager es desarrollado por estudiantes de la carrera de Ingeniería en Tecnologías de la Información de la Escuela Politécnica Nacional.

El equipo propone utilizar el marco de trabajo Scrum para organizar las actividades y responsabilidades durante el desarrollo del proyecto.

| Integrante | Rol |
|---|---|
| Joel Ordoñez | Product Owner y desarrollador |
| Ronald Veloz | Scrum Master y desarrollador |
| Pablo Cuasapaz | Desarrollador |

El Product Owner será responsable de priorizar las necesidades del sistema, el Scrum Master facilitará la organización del trabajo y los integrantes participarán en el desarrollo y verificación de la aplicación.


## 5. Tecnologías utilizadas

Para el desarrollo de AutoServiceManager se utilizarán herramientas y tecnologías que permitan implementar una aplicación de escritorio basada en formularios, con almacenamiento de información y control de versiones.

| Tecnología | Descripción y utilización |
|---|---|
| C# | Lenguaje de programación utilizado para desarrollar la lógica y las funcionalidades del sistema. |
| Windows Forms | Framework utilizado para diseñar e implementar la interfaz gráfica de escritorio. |
| .NET 10 | Plataforma de desarrollo sobre la cual se encuentra configurada la solución de Windows Forms. |
| Visual Studio | Entorno de desarrollo utilizado para crear, programar, compilar y ejecutar la aplicación. |
| SQLite | Base de datos local propuesta para almacenar la información de clientes, vehículos, servicios, inventario y pagos. |
| Git | Herramienta de control de versiones utilizada para registrar y administrar los cambios del proyecto. |
| GitHub | Plataforma utilizada para alojar el repositorio y facilitar el trabajo colaborativo del equipo. |
| Markdown | Formato utilizado para elaborar y organizar la documentación del proyecto. |

### 5.1. Metodología de desarrollo

El equipo propone utilizar Scrum como marco de trabajo ágil para organizar las actividades y responsabilidades durante el desarrollo de AutoServiceManager.

El trabajo se realizará de manera colaborativa, considerando la identificación de necesidades, la priorización de funcionalidades, el desarrollo incremental y la revisión de los resultados obtenidos.

### 5.2. Estado tecnológico del proyecto

La solución inicial está configurada para utilizar C# con Windows Forms y .NET 10. La utilización de SQLite se contempla como parte de la implementación de la persistencia de datos y deberá verificarse durante el desarrollo.

Las funcionalidades descritas corresponden a los objetivos del proyecto y no implican que todas se encuentren implementadas en esta etapa.


## 6. Estructura del repositorio

El repositorio de AutoServiceManager está organizado para mantener separados los archivos de la aplicación, la documentación de Ingeniería de Requerimientos y los archivos de configuración del proyecto.

```text
AutoServiceManager/
|
|-- AutoServiceManager/
|   |-- AutoServiceManager.csproj
|   |-- ... (archivos de la aplicación)
|
|-- Documentacion/
|   |-- 01_Gestion_y_Alcance/
|   |   |-- 01_Vision_y_Alcance.md
|   |   |-- 02_Matriz_Interesados.md
|   |
|   |-- 02_Adquisicion_e_Interesados/
|   |-- 03_Catalogo_y_Refinamiento/
|   |-- 04_Requerimientos_Verificables/
|   |-- 05_Modelado_y_Priorizacion/
|   |-- 06_Trazabilidad_y_Validacion/
|
|-- AutoServiceManager.slnx
|-- README.md
|-- .gitignore
|-- .gitattributes
```

### 6.1. Descripción de las carpetas y archivos

| Carpeta o archivo | Descripción |
|---|---|
| `AutoServiceManager/` | Contiene el proyecto de Windows Forms y sus archivos de código fuente. |
| `Documentacion/` | Almacena los documentos correspondientes a las actividades de Ingeniería de Requerimientos. |
| `01_Gestion_y_Alcance/` | Contiene los documentos de visión, alcance y matriz de interesados del Entregable 1. |
| `02_Adquisicion_e_Interesados/` | Carpeta destinada a documentar la adquisición de requerimientos y el análisis de interesados. |
| `03_Catalogo_y_Refinamiento/` | Carpeta destinada al catálogo, análisis y refinamiento de requerimientos. |
| `04_Requerimientos_Verificables/` | Carpeta destinada a la especificación de requerimientos verificables. |
| `05_Modelado_y_Priorizacion/` | Carpeta destinada al modelado y la priorización de requerimientos. |
| `06_Trazabilidad_y_Validacion/` | Carpeta destinada a documentar la trazabilidad y validación de requerimientos. |
| `AutoServiceManager.slnx` | Archivo de solución utilizado para abrir el proyecto en Visual Studio. |
| `README.md` | Documento principal que describe el proyecto, sus integrantes, tecnologías y estructura. |
| `.gitignore` | Archivo que establece qué elementos no deben incluirse en el control de versiones. |
| `.gitattributes` | Archivo que define atributos de los archivos administrados mediante Git. |

Las carpetas de documentación permitirán organizar los diferentes entregables del proyecto de acuerdo con las actividades establecidas durante la asignatura.
```
