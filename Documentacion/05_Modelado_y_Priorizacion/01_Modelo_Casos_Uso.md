# 01. Modelo de Casos de Uso

**Proyecto:** AutoServiceManager · **Actividad:** PPE07 · **Versión documental:** 1.0 — Entrega académica final.

## 1. Actores

| Actor | Tipo | Rol |
| --- | --- | --- |
| Recepcionista | Persona | Registra clientes, vehículos, citas y órdenes. Comunica el estado al cliente. |
| Mecánico | Persona | Registra diagnóstico, prepara la cotización y añade partidas. |
| Operario | Persona | Actualiza el avance de la orden. Un mecánico puede desempeñar este rol. |
| Encargado de inventario | Persona | Consulta existencias y registra entradas y proveedores. |
| Personal de facturación | Persona | Emite comprobantes y registra pagos comunicados o recibidos en el taller. |
| Administrador | Persona | Mantiene personal, servicios y reportes, y genera respaldos. |
| Empleado registrado | Persona | Inicia sesión para ejercer las funciones del rol asignado. |

El cliente, el propietario y el proveedor son interesados indirectos. El cliente solicita citas, recibe el estado, comunica su decisión y realiza el pago fuera de la interacción con la aplicación. Los operadores del taller registran esas acciones. Cada rol inicia sesión y accede a las funciones descritas en la [tabla de acceso de PPE06](../04_Requerimientos_Verificables/01_Requerimientos_Refinados.md#tabla-de-acceso-por-rol).

## 2. Casos de uso

| ID | Nombre | Actor principal | Requerimientos asociados | Propósito |
| --- | --- | --- | --- | --- |
| CU-01 | Registrar diagnóstico | Mecánico | RF-12, RNF-02 | Guarda las fallas, observaciones y trabajos recomendados para cotizar una orden. |
| CU-02 | Generar cotización | Mecánico | RF-12, RNF-02 | Prepara el estimado para comunicarlo al cliente. |
| CU-03 | Agregar servicios y repuestos | Mecánico | RF-12 (servicios de la cotización), RF-13, RNF-02 | Selecciona partidas de la orden y registra el consumo de repuestos. |
| CU-04 | Registrar cliente | Recepcionista | RF-01, RNF-05, RNF-02 | Da de alta al cliente con sus datos de identificación y contacto. |
| CU-05 | Consultar clientes | Recepcionista | RF-01, RNF-02 | Localiza un cliente por su identificación. |
| CU-06 | Agendar y gestionar cita | Recepcionista | RF-03, RNF-02 | Programa, consulta, reprograma o cancela citas según disponibilidad. |
| CU-07 | Crear orden de trabajo | Recepcionista | RF-04, RD-01, RNF-02 | Recibe el vehículo para un servicio con cita opcional. |
| CU-08 | Consultar estado del vehículo | Recepcionista | RF-05, RNF-01, RNF-02 | Consulta por placa el estado y el historial. El cliente recibe el resultado a través del personal. |
| CU-09 | Generar comprobante interno | Personal de facturación | RF-08, RF-14 (soporte), RD-02, RNF-02 | Calcula y guarda los importes del cobro. Conserva el caso antes denominado Generar factura. |
| CU-10 | Exportar comprobante PDF | Personal de facturación | RF-08, RD-02, RNF-02 | Exporta y revisa el comprobante local. Conserva el caso antes denominado Generar factura PDF. |
| CU-11 | Registrar pago | Personal de facturación | RF-14, RNF-05, RNF-02 | Registra el monto recibido por el taller y actualiza el saldo. |
| CU-12 | Registrar decisión sobre la cotización | Mecánico | RF-12, RNF-02 | Registra la decisión comunicada por el cliente en la cotización asociada a la orden. |
| CU-13 | Gestionar empleados | Administrador | RF-09, RNF-02 | Registra, consulta y actualiza personal y rol. |
| CU-14 | Gestionar catálogo de servicios | Administrador | RF-10, RNF-02 | Mantiene las fichas y precios referenciales del catálogo. |
| CU-15 | Generar reporte general | Administrador | RF-11, RNF-02 | Prepara el reporte operativo para la gerencia del taller. |
| CU-16 | Registrar proveedor | Encargado de inventario | RF-15, RNF-02 | Registra los datos básicos del proveedor. |
| CU-17 | Registrar compra | Encargado de inventario | RF-07 (soporte), RNF-02 | Registra una entrada de repuestos para el control de existencias. Caso de soporte de RF-07, sin integrar pedidos externos. |
| CU-18 | Consultar inventario | Encargado de inventario | RF-07, RNF-02 | Localiza repuestos y consulta precios y existencias. |
| CU-19 | Registrar avance | Operario | RF-06, RNF-05, RNF-02 | Guarda la observación y el estado de la orden. Un mecánico puede cumplir este rol en el flujo. |
| CU-20 | Registrar vehículo | Recepcionista | RF-02, RNF-05, RD-03, RNF-02 | Vincula la placa y los datos del vehículo al cliente propietario. |
| CU-21 | Guardar y recuperar datos | Administrador | RNF-04, RNF-02 | Crea una copia SQLite y comprueba su apertura en una ejecución con datos alternativos. |
| CU-22 | Iniciar sesión | Empleado registrado | RF-16, RNF-02 | Inicia una sesión con su cuenta y su rol. RF-16 consta en el alcance y su implementación sigue pendiente. |

Se mantienen CU-01 a CU-21. CU-22 se añade para modelar RF-16. CU-09 y CU-10 conservan su ID y adoptan la terminología de comprobante de servicio. RNF-02 controla el acceso de cada rol. RNF-03 y RNF-06 se aplican a toda la solución. El [diagrama](02_Diagrama_Casos_Uso.png) muestra las asociaciones principales. Incluye la extensión opcional de CU-09 mediante CU-10 cuando se pide el PDF. El escenario de CU-07 detalla las alternativas con y sin alta de datos.

## 3. Escenario

### Escenario detallado de CU-07 · Crear orden de trabajo

**Actor principal:** Recepcionista. **Requisitos:** RF-04 y RD-01. **Historia:** HU-01. **Prueba:** CP-04 y CP-13.

**Precondiciones.** La persona que registra la orden tiene una sesión con rol Recepcionista. El catálogo ya contiene al menos un servicio.

1. Recepción abre la opción Crear orden y busca al cliente por su identificación.
2. Selecciona el cliente y un vehículo registrado a su nombre.
3. El sistema muestra las citas pendientes del vehículo. Recepción vincula una cita del mismo cliente y vehículo cuando corresponda.
4. Recepción selecciona al menos un servicio solicitado y confirma el registro.
5. El sistema verifica cliente, vehículo y servicio. Si hay cita, verifica su asociación y estado.
6. El sistema guarda la orden, asigna el identificador y muestra el estado Recibido. Si se vinculó una cita, la deja Atendida.
7. Recepción verifica la confirmación y encuentra la orden en la lista por placa.

**Flujos alternativos.**

- A1 · Cliente inexistente en el paso 1. Recepción completa CU-04 y vuelve a buscar al cliente. Si cancela el alta, termina sin crear la orden.
- A2 · Vehículo inexistente en el paso 2. Recepción completa CU-20 para ese cliente y vuelve a seleccionarlo. Si la placa pertenece a otro cliente, el sistema informa el conflicto y no guarda la orden.
- A3 · Atención sin cita en el paso 3. Recepción continúa al paso 4 con el vínculo vacío.
- A4 · Sin servicio o cita incompatible en el paso 5. El sistema identifica la causa y vuelve al paso que permita corregirla. No guarda la orden ni cambia la cita.

**Postcondiciones.** En el flujo exitoso queda una orden vinculada al cliente y al vehículo. Un fallo de validación deja la orden sin crear y la cita sin cambios.
