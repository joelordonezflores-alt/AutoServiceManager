import ec.edu.epn.autoservice.persistencia.DatosSistema;
import java.io.IOException;
import java.io.ObjectInputFilter;
import java.io.ObjectInputStream;
import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.StandardOpenOption;
import java.util.Date;
import java.util.List;

/** Conversor de archivos locales creados por AutoService Manager Java. */
public final class ExportarDatosJava {
    private static final String MODELO = "ec.edu.epn.autoservice.modelo.";

    public static void main(String[] args) throws Exception {
        if (args.length != 2) {
            throw new IllegalArgumentException("Uso: ExportarDatosJava archivo.ser exportacion.json");
        }
        DatosSistema datos;
        try (var entrada = new ObjectInputStream(Files.newInputStream(Path.of(args[0])))) {
            entrada.setObjectInputFilter(ExportarDatosJava::filtrar);
            datos = (DatosSistema) entrada.readObject();
        }
        var json = new StringBuilder("{\"version\":1,\"datos\":");
        escribir(json, datos);
        json.append('}');
        Files.writeString(Path.of(args[1]), json, StandardCharsets.UTF_8,
                StandardOpenOption.CREATE_NEW, StandardOpenOption.WRITE);
        System.out.println("Exportación Java completada.");
    }

    private static ObjectInputFilter.Status filtrar(ObjectInputFilter.FilterInfo info) {
        if (info.depth() > 32 || info.references() > 2_000_000
                || info.streamBytes() > 134_217_728 || info.arrayLength() > 1_000_000) {
            return ObjectInputFilter.Status.REJECTED;
        }
        Class<?> clase = info.serialClass();
        if (clase == null) return ObjectInputFilter.Status.UNDECIDED;
        if (clase == DatosSistema.class || clase.getName().startsWith(MODELO)
                || clase == java.util.ArrayList.class || clase == Date.class || clase == Enum.class
                || clase == Object[].class || clase == String.class) {
            return ObjectInputFilter.Status.ALLOWED;
        }
        return ObjectInputFilter.Status.REJECTED;
    }

    private static void escribir(StringBuilder json, Object valor) throws ReflectiveOperationException, IOException {
        if (valor == null) {
            json.append("null");
        } else if (valor instanceof String texto) {
            cadena(json, texto);
        } else if (valor instanceof Number numero) {
            if (!Double.isFinite(numero.doubleValue())) throw new IOException("Importe no válido.");
            json.append(numero);
        } else if (valor instanceof Date fecha) {
            json.append(fecha.getTime());
        } else if (valor instanceof Enum<?> opcion) {
            cadena(json, opcion.name());
        } else if (valor instanceof List<?> lista) {
            json.append('[');
            boolean primero = true;
            for (Object item : lista) {
                if (!primero) json.append(',');
                escribir(json, item);
                primero = false;
            }
            json.append(']');
        } else if (valor instanceof DatosSistema || valor.getClass().getName().startsWith(MODELO)) {
            json.append("{\"clase\":");
            cadena(json, valor.getClass().getSimpleName());
            for (Class<?> tipo = valor.getClass(); tipo != Object.class; tipo = tipo.getSuperclass()) {
                for (Field campo : tipo.getDeclaredFields()) {
                    if (Modifier.isStatic(campo.getModifiers()) || campo.isSynthetic()) continue;
                    campo.setAccessible(true);
                    json.append(',');
                    cadena(json, campo.getName());
                    json.append(':');
                    escribir(json, campo.get(valor));
                }
            }
            json.append('}');
        } else {
            throw new IOException("Tipo inesperado en el archivo: " + valor.getClass().getName());
        }
    }

    private static void cadena(StringBuilder json, String texto) {
        json.append('"');
        for (char caracter : texto.toCharArray()) {
            switch (caracter) {
                case '"' -> json.append("\\\"");
                case '\\' -> json.append("\\\\");
                default -> {
                    if (caracter < 32 || Character.isSurrogate(caracter)) {
                        json.append(String.format("\\u%04x", (int) caracter));
                    } else {
                        json.append(caracter);
                    }
                }
            }
        }
        json.append('"');
    }
}
