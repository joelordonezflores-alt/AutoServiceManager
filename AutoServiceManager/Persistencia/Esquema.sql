CREATE TABLE configuracion (
    clave TEXT PRIMARY KEY,
    valor INTEGER NOT NULL
);
INSERT INTO configuracion VALUES ('iva', 1500);

CREATE TABLE secuencias (
    prefijo TEXT PRIMARY KEY,
    siguiente INTEGER NOT NULL CHECK (siguiente > 0)
);
CREATE TABLE clientes (
    cedula TEXT PRIMARY KEY COLLATE NOCASE,
    nombres TEXT NOT NULL,
    telefono TEXT NOT NULL,
    direccion TEXT NOT NULL,
    correo TEXT NOT NULL
);
CREATE TABLE vehiculos (
    placa TEXT PRIMARY KEY COLLATE NOCASE,
    marca TEXT NOT NULL,
    modelo TEXT NOT NULL,
    anio INTEGER NOT NULL,
    cedula TEXT NOT NULL COLLATE NOCASE REFERENCES clientes(cedula),
    UNIQUE (placa, cedula)
);
CREATE TABLE citas (
    id TEXT PRIMARY KEY COLLATE NOCASE,
    fecha TEXT NOT NULL,
    cedula TEXT NOT NULL COLLATE NOCASE REFERENCES clientes(cedula),
    placa TEXT NOT NULL COLLATE NOCASE,
    motivo TEXT NOT NULL,
    estado TEXT NOT NULL CHECK (estado IN ('Programada', 'Confirmada', 'Atendida', 'Cancelada')),
    FOREIGN KEY (placa, cedula) REFERENCES vehiculos(placa, cedula)
);
CREATE INDEX citas_cliente ON citas(cedula, fecha DESC);

CREATE TABLE ordenes (
    id TEXT PRIMARY KEY COLLATE NOCASE,
    fecha_ingreso TEXT NOT NULL,
    fecha_entrega TEXT,
    placa TEXT NOT NULL COLLATE NOCASE REFERENCES vehiculos(placa),
    estado TEXT NOT NULL,
    observaciones TEXT NOT NULL,
    tipo TEXT NOT NULL,
    cita_id TEXT UNIQUE COLLATE NOCASE REFERENCES citas(id)
);
CREATE INDEX ordenes_placa ON ordenes(placa);
CREATE TABLE partidas_orden (
    id INTEGER PRIMARY KEY,
    orden_id TEXT NOT NULL COLLATE NOCASE REFERENCES ordenes(id),
    tipo TEXT NOT NULL,
    codigo TEXT NOT NULL,
    nombre TEXT NOT NULL,
    categoria TEXT NOT NULL,
    cantidad INTEGER NOT NULL CHECK (cantidad > 0),
    precio INTEGER NOT NULL,
    origen TEXT NOT NULL DEFAULT ''
);
CREATE INDEX partidas_orden_orden ON partidas_orden(orden_id);
CREATE TABLE cotizaciones (
    orden_id TEXT PRIMARY KEY COLLATE NOCASE REFERENCES ordenes(id),
    id TEXT NOT NULL UNIQUE COLLATE NOCASE,
    fecha TEXT NOT NULL,
    diagnostico TEXT NOT NULL,
    mano_obra INTEGER NOT NULL,
    estimado INTEGER NOT NULL,
    estado TEXT NOT NULL CHECK (estado IN ('Pendiente', 'Aprobada', 'Rechazada'))
);
CREATE TABLE avances (
    id INTEGER PRIMARY KEY,
    orden_id TEXT NOT NULL COLLATE NOCASE REFERENCES ordenes(id),
    fecha TEXT NOT NULL,
    rol TEXT NOT NULL,
    estado TEXT NOT NULL,
    costo INTEGER NOT NULL
);
CREATE INDEX avances_orden ON avances(orden_id);

CREATE TABLE repuestos (
    id TEXT PRIMARY KEY COLLATE NOCASE,
    nombre TEXT NOT NULL,
    stock INTEGER NOT NULL CHECK (stock >= 0),
    precio INTEGER NOT NULL
);
CREATE TABLE proveedores (
    id TEXT PRIMARY KEY COLLATE NOCASE,
    nombre TEXT NOT NULL,
    telefono TEXT NOT NULL,
    correo TEXT NOT NULL
);
CREATE TABLE compras (
    id TEXT PRIMARY KEY COLLATE NOCASE,
    fecha TEXT NOT NULL,
    proveedor_id TEXT NOT NULL COLLATE NOCASE REFERENCES proveedores(id)
);
CREATE TABLE detalles_compra (
    id INTEGER PRIMARY KEY,
    compra_id TEXT NOT NULL COLLATE NOCASE REFERENCES compras(id),
    repuesto_id TEXT NOT NULL COLLATE NOCASE REFERENCES repuestos(id),
    cantidad INTEGER NOT NULL CHECK (cantidad > 0),
    costo INTEGER NOT NULL
);
CREATE INDEX detalles_compra_compra ON detalles_compra(compra_id);
CREATE TABLE empleados (
    cedula TEXT PRIMARY KEY COLLATE NOCASE,
    nombres TEXT NOT NULL,
    telefono TEXT NOT NULL,
    direccion TEXT NOT NULL,
    rol TEXT NOT NULL,
    sueldo INTEGER NOT NULL
);
CREATE TABLE servicios (
    id TEXT PRIMARY KEY COLLATE NOCASE,
    nombre TEXT NOT NULL,
    categoria TEXT NOT NULL,
    precio INTEGER NOT NULL
);

CREATE TABLE facturas (
    id TEXT PRIMARY KEY COLLATE NOCASE,
    orden_id TEXT NOT NULL UNIQUE COLLATE NOCASE REFERENCES ordenes(id),
    fecha TEXT NOT NULL,
    tasa_iva INTEGER NOT NULL,
    subtotal INTEGER NOT NULL,
    iva INTEGER NOT NULL,
    total INTEGER NOT NULL,
    cedula TEXT NOT NULL,
    cliente TEXT NOT NULL,
    telefono TEXT NOT NULL,
    direccion TEXT NOT NULL,
    correo TEXT NOT NULL,
    placa TEXT NOT NULL,
    marca TEXT NOT NULL,
    modelo TEXT NOT NULL,
    anio INTEGER NOT NULL
);
CREATE TABLE partidas_factura (
    id INTEGER PRIMARY KEY,
    factura_id TEXT NOT NULL COLLATE NOCASE REFERENCES facturas(id),
    tipo TEXT NOT NULL,
    codigo TEXT NOT NULL,
    nombre TEXT NOT NULL,
    categoria TEXT NOT NULL,
    cantidad INTEGER NOT NULL CHECK (cantidad > 0),
    precio INTEGER NOT NULL
);
CREATE INDEX partidas_factura_factura ON partidas_factura(factura_id);
CREATE TABLE pagos (
    id TEXT PRIMARY KEY COLLATE NOCASE,
    factura_id TEXT NOT NULL COLLATE NOCASE REFERENCES facturas(id),
    fecha TEXT NOT NULL,
    metodo TEXT NOT NULL,
    monto INTEGER NOT NULL CHECK (monto > 0)
);
CREATE INDEX pagos_factura ON pagos(factura_id);
