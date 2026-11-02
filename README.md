# Sistema de Perfumería en C# (ABM)

Programa de consola en C# para administrar los perfumes de una perfumería: **Alta, Baja, Modificación y Borrado**.

Hecho por **Ezequiel Paez** como material de apoyo para estudiantes de primer año. Todo el código está comentado línea por línea explicando qué es una clase, qué es un constructor, para qué sirve cada variable y cómo funciona el menú.

## Funciones del menú

| Opción | Qué hace |
|---|---|
| 1. Alta | Agrega un perfume nuevo (código automático) |
| 2. Baja | Baja **lógica**: marca el perfume como inactivo, sin borrarlo |
| 3. Modificación | Cambia los datos; Enter vacío conserva el valor actual |
| 4. Borrar | Baja **física**: elimina el perfume definitivamente |
| 5. Listar | Muestra todos los perfumes en una tabla |
| 6. Reactivar | Vuelve a activar un perfume dado de baja |
| 0. Salir | Termina el programa |

## Conceptos que se explican en el código

- **Clase** y **objeto** (`Perfume`)
- **Constructor**
- **Propiedades** y tipos de datos (`int`, `string`, `decimal`, `bool`)
- **Métodos**
- **Lista** (`List<Perfume>`) y `foreach`
- **Menú** con `while` + `switch`
- Entrada y salida con `Console.WriteLine` / `Console.ReadLine` (en lugar de `cout`/`cin` o `printf`/`scanf`)
- Validación de datos con `int.TryParse`
- Diferencia entre **baja lógica** y **baja física**

## Cómo ejecutarlo

### Opción 1: Visual Studio
1. Crear un proyecto nuevo de tipo **Aplicación de consola (C#)**.
2. Reemplazar el contenido de `Program.cs` por el de este repositorio.
3. Presionar **F5**.

### Opción 2: Terminal (con [.NET SDK](https://dotnet.microsoft.com/download) instalado)
```bash
git clone https://github.com/Ezequiel208/perfumeria-csharp.git
cd perfumeria-csharp
dotnet run
```

El programa arranca con tres perfumes de ejemplo para poder probar todas las opciones.
