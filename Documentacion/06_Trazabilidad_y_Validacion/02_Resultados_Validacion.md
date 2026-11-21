# Resultados de Validación — AutoService Manager

## 1. Checklist de Validación (Lista de Comprobación)

Aplicada sobre los requerimientos clave del sistema (`RF-01` Registro de clientes, `RF-02` Registro de vehículos, `RF-04` Crear orden de trabajo y flujo general del sistema)[cite: 14, 15]:

| Criterio / Pregunta de Validación | RF-01 (Registrar clientes) | RF-02 (Registrar vehículos) | RF-04 (Crear orden de trabajo) | Estado General |
| :--- | :---: | :---: | :---: | :---: |
| ¿Representa una necesidad real del taller? | Sí | Sí | Sí | Cumple[cite: 15] |
| ¿Es claro y sin términos ambiguos? | Sí | Sí | Sí | Cumple[cite: 15] |
| ¿Es consistente con los demás requerimientos y casos de uso? | Sí | Sí | Sí | Cumple[cite: 15] |
| ¿Está completo (datos y flujo definidos)? | Sí | Sí | Sí | Cumple[cite: 15] |
| ¿Es factible técnica y operativamente? | Sí | Sí | Sí | Cumple[cite: 15] |
| ¿Es verificable mediante casos de prueba? | Sí | Sí | Sí | Cumple[cite: 15] |
| ¿Tiene fuente (interesado) identificada? | Sí | Sí | Sí | Cumple[cite: 16] |
| ¿Tiene prioridad asignada (MoSCoW)? | Sí | Sí | Sí | Cumple[cite: 16] |
| ¿Fue revisado y validado con los interesados? | Validado | Validado | Validado | Cumple[cite: 16] |

---

## 2. Observaciones de la Validación

Durante la revisión de requerimientos, casos de uso y consultas con los interesados se identificaron los siguientes puntos[cite: 16]:

* **Estados del vehículo:** Era necesario precisar la lista exacta de estados del vehículo para el seguimiento operativo (`Recibido`, `En diagnóstico`, `En reparación`, `Listo para entrega`, `Entregado`)[cite: 10, 16].
* **Campos obligatorios en órdenes de trabajo:** Se observó la necesidad de definir los datos mínimos obligatorios (cliente registrado, vehículo registrado y detalle del servicio/problema) para evitar demoras en la atención del recepcionista[cite: 6, 12, 13, 16].
* **Gestión de citas y reportes:** En el análisis inicial (`RF-03` y `RF-09`) faltaba especificar los campos de las citas (fecha y hora) y el alcance de los reportes administrativos[cite: 4, 9, 16].
* **Alineación con el diagrama de casos de uso:** Se verificó la correspondencia entre los requerimientos y los actores finales del modelo (`Recepcionista`, `Mecánico`, `Cliente`, `Administrador`, `Operario`)[cite: 19].

---

## 3. Correcciones Derivadas

A partir de las observaciones anteriores se aplicaron las siguientes correcciones en la documentación del SRS[cite: 2, 16]:

* **Refinamiento de `RF-05` (Consulta de estado de vehículos):** Se definieron explícitamente los cinco estados operativos del vehículo (`Recibido`, `En diagnóstico`, `En reparación`, `Listo para entrega`, `Entregado`) y un tiempo máximo de respuesta de 3 segundos (`RNF-01`)[cite: 10, 14].
* **Refinamiento de `RF-07` (Inventario de repuestos):** Se detallaron las operaciones de entradas, salidas, consulta de stock disponible y alertas por stock mínimo[cite: 10].
* **Reglas de integridad en órdenes de trabajo (`RD-01`):** Se formalizó la restricción de que toda orden de trabajo debe asociarse a un cliente y vehículo previamente registrados, permitiendo su alta mediante flujos alternativos si no existen[cite: 8, 13].
* **Actualización de trazabilidad:** Se unificaron los nombres de actores y casos de uso entre el diagrama (`02_Diagrama_Casos_Uso.png`), las historias de usuario (`03_Historias_Usuario.md`) y la matriz de trazabilidad (`01_Matriz_Trazabilidad.md`)[cite: 1, 2, 19].
