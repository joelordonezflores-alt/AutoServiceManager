# Resultados de Validación — AutoService Manager

## 1. Checklist de Validación (Lista de Comprobación)

## Checklist de Validación

| Pregunta de validación | RF-01 | RF-02 | RF-03 | RF-04 | RF-05 | RF-06 | RF-07 | RF-08 | RF-09 | RNF-01 | RNF-02 | RNF-03 | RD-01 | RD-02 |
| :--- | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: |
| ¿Representa una necesidad real? | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí |
| ¿Es claro? | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí |
| ¿Es consistente con los demás? | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí |
| ¿Está completo? | Sí | Sí | No | Sí | Sí | Sí | Sí | Sí | No | Sí | Sí | Sí | Sí | Sí |
| ¿Es factible? | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí |
| ¿Es verificable? | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí |
| ¿Tiene fuente identificada? | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí |
| ¿Tiene prioridad asignada? | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | Sí | No | No | No | No | No |
| ¿Fue revisado por los interesados? | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente |

---

## 2. Observaciones de la Validación

Durante la revisión de requerimientos, casos de uso y consultas con los interesados se identificaron los siguientes puntos

## Observaciones de la Validación

* **Gestión de citas (`RF-03`):** Se detectó incompletitud en la definición del requerimiento, ya que no se especifican los campos obligatorios para agendar y gestionar una cita (como fecha, hora, cliente y vehículo) ni los estados o transiciones de su flujo.
* **Administración y reportes (`RF-09`):** Se observó ambigüedad e incompletitud al mencionar "administrar reportes" de forma general sin detallar qué reportes específicos requiere consultar el administrador (por ejemplo, ingresos, trabajos realizados o stock).
* **Priorización de requerimientos no funcionales y reglas de dominio (`RNF-01`–`RNF-03`, `RD-01`–`RD-02`):** En la clasificación MoSCoW inicial no se asignó prioridad a todos los requerimientos no funcionales y reglas de dominio, por lo que conviene unificar su criterio de prioridad.
* **Estados del vehículo (`RF-05`):** Resulta necesario mantener definida la lista exacta de estados del vehículo para el seguimiento operativo (recibido, en diagnóstico, en reparación, listo para entrega y entregado).
* **Datos obligatorios y reglas en órdenes de trabajo (`RF-04`, `RD-01`):** Se debe asegurar que toda orden de trabajo quede asociada a un cliente y vehículo registrados, contemplando los flujos alternativos si alguno no existe.
* **Revisión con los interesados:** La validación con los interesados permanece pendiente de cierre formal en la matriz de comprobación, lo que debe completarse antes de dar por concluida la revisión.
---

## 3. Correcciones Derivadas

A partir de las observaciones anteriores se aplicaron las siguientes correcciones en la documentación del SRS

## Correcciones Derivadas

* **Especificación de datos y flujo en citas (`RF-03`):** Se definieron los campos obligatorios para agendar y gestionar una cita (cliente, vehículo, fecha, hora y motivo) junto con sus estados operativos (programada, atendida, cancelada y reprogramada).
* **Definición del alcance de reportes (`RF-09`):** Se concretaron los reportes específicos que puede consultar el administrador en el sistema (reporte general de órdenes de trabajo, ingresos por servicios e inventario/stock de repuestos).
* **Asignación de prioridad (`RNF-01`–`RNF-03`, `RD-01`–`RD-02`):** Se incorporaron los requerimientos no funcionales y las reglas de dominio dentro de la matriz de priorización, asignándoles nivel de prioridad acorde a su impacto operativo y normativo.
* **Cierre de validación con interesados (`RF-01` a `RD-02`):** Se estableció la revisión y aprobación formal de los requerimientos validados con los roles del taller como paso previo al cierre del documento SRS.
