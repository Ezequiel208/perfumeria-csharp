// =====================================================================
//  SISTEMA DE PERFUMERÍA - ABM (Alta, Baja, Modificación y Borrado)
//  Lenguaje: C#
// =====================================================================
//
//  ¿Qué hace este programa?
//  Permite administrar los perfumes de una perfumería desde la consola:
//    1) ALTA          -> agregar un perfume nuevo.
//    2) BAJA          -> dar de baja un perfume (baja LÓGICA: el perfume
//                        sigue guardado pero queda marcado como "inactivo",
//                        por ejemplo porque se dejó de vender).
//    3) MODIFICACIÓN  -> cambiar los datos de un perfume existente.
//    4) BORRAR        -> eliminar un perfume DEFINITIVAMENTE de la lista
//                        (baja FÍSICA: desaparece y no se puede recuperar).
//    5) LISTAR        -> mostrar todos los perfumes.
//    6) REACTIVAR     -> volver a activar un perfume dado de baja.
//
//  Diferencia entre BAJA y BORRAR (es muy común que lo pregunten):
//    - Baja lógica: se "desactiva" el registro. Sirve para no perder la
//      historia (ej: saber que alguna vez se vendió ese perfume).
//    - Baja física (borrar): se elimina el registro de la memoria.
//
//  En C# para mostrar texto se usa Console.WriteLine() (equivale al cout
//  de C++ o al printf de C) y para leer lo que escribe el usuario se usa
//  Console.ReadLine() (equivale al cin de C++ o al scanf de C).
//  Console.ReadLine() SIEMPRE devuelve un texto (string), por eso cuando
//  necesitamos un número lo tenemos que convertir (ver LeerEntero).
// =====================================================================

// "using" importa espacios de nombres (bibliotecas) para no tener que
// escribir el nombre completo de cada cosa.
using System;                     // Console, Math, etc.
using System.Collections.Generic; // List<T> (lista dinámica)
using System.Globalization;       // CultureInfo (para leer decimales con coma o punto)

// Un "namespace" (espacio de nombres) agrupa clases relacionadas,
// como si fuera una carpeta que organiza el código.
namespace Perfumeria
{
    // =================================================================
    //  ¿QUÉ ES UNA CLASE?
    // =================================================================
    //  Una clase es un MOLDE o PLANTILLA que describe cómo es una "cosa"
    //  del mundo real dentro del programa. Define:
    //    - ATRIBUTOS / PROPIEDADES: los datos que tiene (nombre, precio...)
    //    - MÉTODOS: las acciones que puede hacer (mostrarse, etc.)
    //
    //  A partir de la clase se crean OBJETOS (también llamados
    //  "instancias"). Ejemplo: la clase Perfume es el molde, y
    //  "Chanel N°5" y "Acqua di Gio" son dos objetos distintos creados
    //  con ese molde, cada uno con sus propios valores.
    //
    //  Analogía: la clase es el plano de una casa; los objetos son las
    //  casas construidas con ese plano.
    // =================================================================
    class Perfume
    {
        // -------------------------------------------------------------
        //  PROPIEDADES (las "variables" de la clase)
        // -------------------------------------------------------------
        //  Cada propiedad guarda un dato del perfume.
        //  "public" significa que se puede acceder desde fuera de la clase.
        //  { get; set; } significa que se puede LEER (get) y ESCRIBIR (set).
        //  Si dice "private set", solo la propia clase puede modificarla.

        // Código único que identifica a cada perfume (como un DNI).
        // Es "private set" para que nadie lo cambie desde afuera: el
        // código no debería modificarse nunca una vez asignado.
        public int Codigo { get; private set; }

        // Nombre comercial del perfume. Ej: "Light Blue". (string = texto)
        public string Nombre { get; set; }

        // Marca que lo fabrica. Ej: "Dolce & Gabbana".
        public string Marca { get; set; }

        // Tamaño del frasco en mililitros. (int = número entero)
        public int Mililitros { get; set; }

        // Precio de venta. Usamos "decimal" porque es el tipo
        // recomendado para dinero: no tiene errores de redondeo
        // como float o double.
        public decimal Precio { get; set; }

        // Cantidad de unidades disponibles en el local.
        public int Stock { get; set; }

        // true = el perfume está activo (se vende).
        // false = fue dado de baja (baja lógica). (bool = verdadero/falso)
        public bool Activo { get; set; }

        // =============================================================
        //  ¿QUÉ ES UN CONSTRUCTOR?
        // =============================================================
        //  Es un método ESPECIAL que se ejecuta automáticamente cuando
        //  se crea un objeto con la palabra "new". Por ejemplo:
        //
        //      Perfume p = new Perfume(1, "Light Blue", "D&G", 100, 50000m, 10);
        //
        //  Características:
        //    - Se llama EXACTAMENTE igual que la clase (Perfume).
        //    - NO tiene tipo de retorno (ni siquiera void).
        //  ¿Para qué sirve?
        //    - Para INICIALIZAR el objeto: darle valores iniciales a sus
        //      propiedades, asegurando que nunca exista un perfume
        //      "a medio hacer" (sin nombre, sin código, etc.).
        // =============================================================
        public Perfume(int codigo, string nombre, string marca, int mililitros, decimal precio, int stock)
        {
            // Asignamos los valores recibidos (parámetros) a las propiedades.
            Codigo = codigo;
            Nombre = nombre;
            Marca = marca;
            Mililitros = mililitros;
            Precio = precio;
            Stock = stock;
            Activo = true; // todo perfume nuevo arranca activo
        }

        // -------------------------------------------------------------
        //  MÉTODO de la clase: una acción que el objeto sabe hacer.
        //  Este muestra los datos del perfume en una sola línea.
        // -------------------------------------------------------------
        public void Mostrar()
        {
            // El operador ?: es un "if" corto: condición ? siVerdadero : siFalso
            string estado = Activo ? "ACTIVO" : "DE BAJA";

            // El signo $ antes de las comillas permite meter variables
            // dentro de {} (se llama "interpolación de cadenas").
            // ,-20 significa "ocupar 20 caracteres alineado a la izquierda"
            // (sirve para que la tabla quede prolija).
            // :N2 muestra el número con 2 decimales y separador de miles.
            Console.WriteLine($"{Codigo,-6} {Nombre,-20} {Marca,-18} {Mililitros,5} ml  ${Precio,12:N2}  {Stock,6}   {estado}");
        }
    }

    // =================================================================
    //  CLASE PRINCIPAL DEL PROGRAMA
    // =================================================================
    //  Acá vive el método Main (el punto de entrada) y los métodos que
    //  realizan cada operación del menú.
    // =================================================================
    class Program
    {
        // -------------------------------------------------------------
        //  VARIABLES DE LA CLASE (campos "static")
        // -------------------------------------------------------------
        //  "static" significa que pertenecen a la clase Program y no a un
        //  objeto en particular, así todos los métodos static (como Main)
        //  pueden usarlas directamente.

        // La LISTA donde guardamos todos los perfumes.
        // List<Perfume> es como un vector que crece solo: podemos agregar
        // y quitar elementos sin preocuparnos por el tamaño.
        static List<Perfume> perfumes = new List<Perfume>();

        // Contador para generar códigos únicos automáticamente.
        // Cada vez que damos de alta un perfume se le asigna este número
        // y luego se incrementa. Así nunca se repiten códigos.
        static int proximoCodigo = 1;

        // =============================================================
        //  MAIN: PUNTO DE ENTRADA
        // =============================================================
        //  Es lo PRIMERO que se ejecuta al correr el programa.
        // =============================================================
        static void Main(string[] args)
        {
            // Permite que se vean bien los acentos y la ñ en la consola.
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Cargamos algunos perfumes de ejemplo para no arrancar vacíos.
            CargarDatosDeEjemplo();

            // Variable que controla si el programa sigue funcionando.
            bool salir = false;

            // =========================================================
            //  ¿CÓMO FUNCIONA EL MENÚ? ¿POR QUÉ SE HACE ASÍ?
            // =========================================================
            //  Usamos un bucle "while" que se repite MIENTRAS el usuario
            //  no elija salir. En cada vuelta:
            //    1) Se muestra el menú.
            //    2) Se lee la opción elegida.
            //    3) Un "switch" decide qué método ejecutar.
            //    4) Se vuelve a mostrar el menú.
            //
            //  ¿Por qué así?
            //    - El bucle permite hacer MUCHAS operaciones sin tener que
            //      volver a ejecutar el programa cada vez.
            //    - El switch es más claro y ordenado que muchos if/else
            //      encadenados cuando comparamos una variable contra
            //      varios valores fijos.
            //    - Cada opción llama a un MÉTODO separado: así el código
            //      queda dividido en partes pequeñas, fáciles de leer,
            //      probar y corregir (esto se llama "modularizar").
            //  (También se podría usar do-while, que garantiza que el
            //   menú se muestre al menos una vez; acá da el mismo
            //   resultado porque "salir" arranca en false.)
            // =========================================================
            while (!salir) // "!" significa NO: "mientras NO salir"
            {
                MostrarMenu();
                int opcion = LeerEntero("Elegí una opción: ");
                Console.WriteLine();

                switch (opcion)
                {
                    case 1: Alta();         break; // break termina el case
                    case 2: Baja();         break;
                    case 3: Modificacion(); break;
                    case 4: Borrar();       break;
                    case 5: Listar();       break;
                    case 6: Reactivar();    break;
                    case 0:
                        salir = true; // cambia la condición: el while termina
                        Console.WriteLine("¡Gracias por usar el sistema de la perfumería!");
                        break;
                    default: // se ejecuta si ningún case coincidió
                        Console.WriteLine("Opción inválida. Intentá de nuevo.");
                        break;
                }

                if (!salir)
                {
                    Pausa(); // espera un Enter antes de volver al menú
                }
            }
        }

        // -------------------------------------------------------------
        //  Dibuja el menú en pantalla.
        // -------------------------------------------------------------
        static void MostrarMenu()
        {
            Console.Clear(); // limpia la pantalla para que quede prolijo
            Console.WriteLine("==========================================");
            Console.WriteLine("        PERFUMERÍA - MENÚ PRINCIPAL        ");
            Console.WriteLine("==========================================");
            Console.WriteLine(" 1. Alta de perfume");
            Console.WriteLine(" 2. Baja de perfume (desactivar)");
            Console.WriteLine(" 3. Modificar perfume");
            Console.WriteLine(" 4. Borrar perfume (eliminar definitivo)");
            Console.WriteLine(" 5. Listar perfumes");
            Console.WriteLine(" 6. Reactivar perfume dado de baja");
            Console.WriteLine(" 0. Salir");
            Console.WriteLine("==========================================");
        }

        // =============================================================
        //  1) ALTA: agregar un perfume nuevo
        // =============================================================
        static void Alta()
        {
            Console.WriteLine("--- ALTA DE PERFUME ---");

            // Pedimos cada dato usando los métodos auxiliares, que se
            // encargan de validar que el usuario escriba algo correcto.
            string nombre = LeerTexto("Nombre: ");
            string marca = LeerTexto("Marca: ");
            int ml = LeerEnteroPositivo("Mililitros: ");
            decimal precio = LeerDecimalPositivo("Precio: ");
            int stock = LeerEnteroPositivo("Stock: ");

            // Acá se usa el CONSTRUCTOR: "new Perfume(...)" crea el objeto
            // y ejecuta el constructor con los datos que le pasamos.
            Perfume nuevo = new Perfume(proximoCodigo, nombre, marca, ml, precio, stock);

            perfumes.Add(nuevo); // lo guardamos en la lista
            proximoCodigo++;      // preparamos el código para el próximo

            Console.WriteLine($"\nPerfume agregado con éxito. Código asignado: {nuevo.Codigo}");
        }

        // =============================================================
        //  2) BAJA (lógica): marcar un perfume como inactivo
        // =============================================================
        static void Baja()
        {
            Console.WriteLine("--- BAJA DE PERFUME ---");
            Perfume p = PedirYBuscarPerfume();
            if (p == null) return; // null = no se encontró; salimos del método

            if (!p.Activo)
            {
                Console.WriteLine("Ese perfume ya estaba dado de baja.");
                return;
            }

            if (Confirmar($"¿Dar de baja \"{p.Nombre}\"? (s/n): "))
            {
                p.Activo = false; // no se borra, solo se desactiva
                Console.WriteLine("Perfume dado de baja correctamente.");
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
            }
        }

        // =============================================================
        //  3) MODIFICACIÓN: cambiar los datos de un perfume
        // =============================================================
        //  Truco útil: si el usuario deja el campo vacío y presiona Enter,
        //  se conserva el valor anterior. Así no tiene que reescribir todo.
        // =============================================================
        static void Modificacion()
        {
            Console.WriteLine("--- MODIFICAR PERFUME ---");
            Perfume p = PedirYBuscarPerfume();
            if (p == null) return;

            Console.WriteLine("\nDatos actuales:");
            MostrarEncabezado();
            p.Mostrar();
            Console.WriteLine("\n(Dejá vacío y presioná Enter para mantener el valor actual)\n");

            // --- Nombre ---
            Console.Write($"Nombre [{p.Nombre}]: "); // Write NO salta de línea
            string entrada = Console.ReadLine();
            // IsNullOrWhiteSpace devuelve true si está vacío o solo tiene espacios
            if (!string.IsNullOrWhiteSpace(entrada))
                p.Nombre = entrada.Trim(); // Trim quita espacios sobrantes

            // --- Marca ---
            Console.Write($"Marca [{p.Marca}]: ");
            entrada = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(entrada))
                p.Marca = entrada.Trim();

            // --- Mililitros ---
            p.Mililitros = LeerEnteroOpcional($"Mililitros [{p.Mililitros}]: ", p.Mililitros);

            // --- Precio ---
            p.Precio = LeerDecimalOpcional($"Precio [{p.Precio:N2}]: ", p.Precio);

            // --- Stock ---
            p.Stock = LeerEnteroOpcional($"Stock [{p.Stock}]: ", p.Stock);

            Console.WriteLine("\nPerfume modificado correctamente.");
        }

        // =============================================================
        //  4) BORRAR (baja física): eliminar de la lista para siempre
        // =============================================================
        static void Borrar()
        {
            Console.WriteLine("--- BORRAR PERFUME ---");
            Perfume p = PedirYBuscarPerfume();
            if (p == null) return;

            Console.WriteLine("ATENCIÓN: esta acción no se puede deshacer.");
            if (Confirmar($"¿Borrar definitivamente \"{p.Nombre}\"? (s/n): "))
            {
                perfumes.Remove(p); // lo quita de la lista
                Console.WriteLine("Perfume borrado definitivamente.");
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
            }
        }

        // =============================================================
        //  5) LISTAR: mostrar todos los perfumes
        // =============================================================
        static void Listar()
        {
            Console.WriteLine("--- LISTADO DE PERFUMES ---\n");

            // .Count devuelve cuántos elementos tiene la lista
            if (perfumes.Count == 0)
            {
                Console.WriteLine("No hay perfumes cargados.");
                return;
            }

            MostrarEncabezado();

            // "foreach" recorre la lista elemento por elemento:
            // en cada vuelta, "p" es un perfume distinto.
            foreach (Perfume p in perfumes)
            {
                p.Mostrar(); // cada objeto sabe mostrarse a sí mismo
            }

            Console.WriteLine($"\nTotal de perfumes: {perfumes.Count}");
        }

        // =============================================================
        //  6) REACTIVAR: deshacer una baja lógica
        //  (esto es justamente la ventaja de la baja lógica: se puede
        //   revertir, cosa que con "borrar" es imposible)
        // =============================================================
        static void Reactivar()
        {
            Console.WriteLine("--- REACTIVAR PERFUME ---");
            Perfume p = PedirYBuscarPerfume();
            if (p == null) return;

            if (p.Activo)
            {
                Console.WriteLine("Ese perfume ya está activo.");
            }
            else
            {
                p.Activo = true;
                Console.WriteLine($"\"{p.Nombre}\" fue reactivado.");
            }
        }

        // =============================================================
        //  MÉTODOS AUXILIARES
        // =============================================================
        //  Son métodos "ayudantes" que se reutilizan en varios lugares.
        //  Escribirlos una sola vez evita repetir código (principio
        //  DRY: "Don't Repeat Yourself" = no te repitas).
        // =============================================================

        // Busca un perfume por su código. Devuelve el perfume o null si
        // no existe. (null significa "nada", "ningún objeto").
        static Perfume BuscarPorCodigo(int codigo)
        {
            foreach (Perfume p in perfumes)
            {
                if (p.Codigo == codigo)
                    return p; // lo encontramos: lo devolvemos y salimos
            }
            return null; // recorrimos todo y no estaba
        }

        // Muestra la lista, pide un código y busca el perfume.
        // Lo usan Baja, Modificación, Borrar y Reactivar.
        static Perfume PedirYBuscarPerfume()
        {
            if (perfumes.Count == 0)
            {
                Console.WriteLine("No hay perfumes cargados.");
                return null;
            }

            MostrarEncabezado();
            foreach (Perfume item in perfumes)
                item.Mostrar();
            Console.WriteLine();

            int codigo = LeerEntero("Ingresá el código del perfume: ");
            Perfume p = BuscarPorCodigo(codigo);

            if (p == null)
                Console.WriteLine("No existe un perfume con ese código.");

            return p;
        }

        // Encabezado de la tabla (los títulos de las columnas).
        static void MostrarEncabezado()
        {
            Console.WriteLine($"{"CÓD",-6} {"NOMBRE",-20} {"MARCA",-18} {"TAMAÑO",8}  {"PRECIO",13}  {"STOCK",6}   ESTADO");
            Console.WriteLine(new string('-', 92)); // una línea de 92 guiones
        }

        // -------------------------------------------------------------
        //  LECTURA Y VALIDACIÓN DE DATOS
        // -------------------------------------------------------------
        //  ¿Por qué validar? Porque si el usuario escribe "hola" cuando
        //  pedimos un número, una conversión directa como int.Parse()
        //  haría que el programa se cierre con un error.
        //  int.TryParse() en cambio INTENTA convertir: devuelve true si
        //  pudo y false si no, sin romper el programa. El resultado de la
        //  conversión queda en la variable indicada con "out".
        // -------------------------------------------------------------

        // Lee un número entero cualquiera; repite hasta que sea válido.
        static int LeerEntero(string mensaje)
        {
            int numero;
            while (true) // se repite hasta que hagamos "return"
            {
                Console.Write(mensaje);
                string texto = Console.ReadLine();

                if (int.TryParse(texto, out numero))
                    return numero;

                Console.WriteLine("Error: tenés que ingresar un número entero.");
            }
        }

        // Lee un entero que sea mayor o igual a 0.
        static int LeerEnteroPositivo(string mensaje)
        {
            while (true)
            {
                int numero = LeerEntero(mensaje);
                if (numero >= 0)
                    return numero;
                Console.WriteLine("Error: el número no puede ser negativo.");
            }
        }

        // Lee un número decimal >= 0 (acepta coma o punto como separador).
        static decimal LeerDecimalPositivo(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string texto = Console.ReadLine();

                if (IntentarConvertirDecimal(texto, out decimal numero) && numero >= 0)
                    return numero;

                Console.WriteLine("Error: ingresá un número válido y no negativo (ej: 45000 o 45000,50).");
            }
        }

        // Para MODIFICACIÓN: si el usuario deja vacío, devuelve el valor actual.
        static int LeerEnteroOpcional(string mensaje, int valorActual)
        {
            while (true)
            {
                Console.Write(mensaje);
                string texto = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(texto))
                    return valorActual; // no escribió nada: se mantiene

                if (int.TryParse(texto, out int numero) && numero >= 0)
                    return numero;

                Console.WriteLine("Error: ingresá un entero no negativo o dejá vacío.");
            }
        }

        // Igual que el anterior pero para decimales.
        static decimal LeerDecimalOpcional(string mensaje, decimal valorActual)
        {
            while (true)
            {
                Console.Write(mensaje);
                string texto = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(texto))
                    return valorActual;

                if (IntentarConvertirDecimal(texto, out decimal numero) && numero >= 0)
                    return numero;

                Console.WriteLine("Error: ingresá un número válido o dejá vacío.");
            }
        }

        // Convierte texto a decimal aceptando "45000,50" o "45000.50".
        // Reemplazamos la coma por punto y convertimos con la cultura
        // "invariante", que siempre usa el punto como separador decimal.
        static bool IntentarConvertirDecimal(string texto, out decimal numero)
        {
            numero = 0;
            if (texto == null) return false;
            texto = texto.Trim().Replace(',', '.');
            return decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out numero);
        }

        // Lee un texto que no puede quedar vacío.
        static string LeerTexto(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string texto = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(texto))
                    return texto.Trim();

                Console.WriteLine("Error: este campo no puede quedar vacío.");
            }
        }

        // Pregunta sí/no. Devuelve true solo si responde "s" o "si".
        static bool Confirmar(string mensaje)
        {
            Console.Write(mensaje);
            string respuesta = (Console.ReadLine() ?? "").Trim().ToLower();
            // ?? "" significa: si ReadLine devolvió null, usar "" en su lugar
            return respuesta == "s" || respuesta == "si" || respuesta == "sí";
        }

        // Espera a que el usuario presione Enter.
        static void Pausa()
        {
            Console.WriteLine("\nPresioná Enter para volver al menú...");
            Console.ReadLine();
        }

        // Carga algunos perfumes iniciales para probar el programa.
        static void CargarDatosDeEjemplo()
        {
            perfumes.Add(new Perfume(proximoCodigo++, "Light Blue", "Dolce & Gabbana", 100, 85000m, 12));
            perfumes.Add(new Perfume(proximoCodigo++, "Acqua di Gio", "Giorgio Armani", 100, 120000m, 8));
            perfumes.Add(new Perfume(proximoCodigo++, "Good Girl", "Carolina Herrera", 80, 135000m, 5));
            // La "m" al final del número indica que es de tipo decimal.
            // proximoCodigo++ usa el valor actual y DESPUÉS le suma 1.
        }
    }
}
