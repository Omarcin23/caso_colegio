// ============================================================
// MESA DE PARTES DIGITAL - I.E. Darío Arrús Cuestas N.° 5011
// ============================================================

Archivos.Cargar();
Pausa();

int opcion;
do
{
    MostrarMenuPrincipal();
    opcion = Validaciones.LeerEntero("");
    Console.WriteLine();

    switch (opcion)
    {
        case 1:
            Tramites.Registrar();
            Pausa();
            break;
        case 2:
            if (Usuarios.IniciarSesion())
            {
                Pausa();
                MenuUsuario();
            }
            else
            {
                Pausa();
            }
            break;
        case 3:
            Console.WriteLine("Gracias por usar el sistema.");
            break;
        default:
            Console.WriteLine("Opción incorrecta.");
            Pausa();
            break;
    }
} while (opcion != 3);

static void MenuUsuario()
{
    int opcion;
    do
    {
        Console.Clear();
        Console.WriteLine("Sesión: " + Usuarios.Etiqueta(Usuarios.Actual));
        Console.WriteLine();
        Console.WriteLine("[1] Mis trámites");
        Console.WriteLine("[2] Buscar expediente");
        Console.WriteLine("[3] Derivar trámite");
        Console.WriteLine("[4] Ordenar por expediente (más recientes primero)");
        if (Usuarios.EsDirectivo())
            Console.WriteLine("[5] Ver todos los trámites");
        Console.WriteLine("[6] Cerrar sesión");
        Console.Write("Ingresar opción: ");

        opcion = Validaciones.LeerEntero("");
        Console.WriteLine();

        switch (opcion)
        {
            case 1:
                Tramites.Mostrar(false);
                break;
            case 2:
                Tramites.Buscar();
                break;
            case 3:
                Tramites.Derivar();
                break;
            case 4:
                Tramites.Ordenar();
                Tramites.Mostrar(false);
                break;
            case 5:
                if (Usuarios.EsDirectivo())
                    Tramites.Mostrar(true);
                else
                    Console.WriteLine("Opción incorrecta.");
                break;
            case 6:
                Usuarios.CerrarSesion();
                Console.WriteLine("Sesión cerrada.");
                break;
            default:
                Console.WriteLine("Opción incorrecta.");
                break;
        }

        Pausa();
    } while (opcion != 6);
}

static void MostrarMenuPrincipal()
{
    Console.Clear();
    Console.WriteLine("***** MESA DE PARTES DIGITAL *****");
    Console.WriteLine("I.E. Darío Arrús Cuestas N.° 5011");
    Console.WriteLine();
    Console.WriteLine("[1] Registrar trámite (mesa de partes)");
    Console.WriteLine("[2] Iniciar sesión (director, coordinador, profesores)");
    Console.WriteLine("[3] Salir");
    Console.Write("Ingresar opción: ");
}

static void Pausa()
{
    Console.WriteLine();
    Console.WriteLine("Presione Enter para continuar...");
    Console.ReadLine();
}

public static class Datos
{
    public const int Max = 100;

    public static int[] Expedientes = new int[Max];
    public static string[] Solicitantes = new string[Max];
    public static string[] Dnis = new string[Max];
    public static string[] Tipos = new string[Max];
    public static string[] Destinos = new string[Max];      
    public static string[] Estados = new string[Max];
    public static string[] Fechas = new string[Max];        
    public static string[] RecibidoPor = new string[Max];   
    public static string[] Movimientos = new string[Max];   
    public static string[] Telefonos = new string[Max];    
    public static string[] Parentescos = new string[Max];   
    public static string[] Motivos = new string[Max];       

    public static int Contador = 0;

    public static readonly string[] ParentescosValidos =
        { "Madre", "Padre", "Tío(a)", "Abuelo(a)", "Hermano(a)", "Apoderado(a)", "Otro" };

    public static readonly string[] TiposValidos =
        { "justificacion", "vacante", "certificado", "constancia" };
}

public static class Validaciones
{
    // El DNI debe tener exactamente 8 dígitos numéricos.
    public static bool ValidarDNI(string dni)
    {
        if (dni.Length != 8)
        {
            Console.WriteLine("Error: el DNI debe tener 8 dígitos.");
            return false;
        }

        for (int i = 0; i < dni.Length; i++)
        {
            if (!char.IsDigit(dni[i]))
            {
                Console.WriteLine("Error: el DNI solo debe contener números.");
                return false;
            }
        }
        return true;
    }

    // El teléfono debe tener 9 dígitos numéricos y empezar con 9 (celular).
    public static bool ValidarTelefono(string telefono)
    {
        if (telefono.Length != 9)
        {
            Console.WriteLine("Error: el teléfono debe tener 9 dígitos.");
            return false;
        }

        for (int i = 0; i < telefono.Length; i++)
        {
            if (!char.IsDigit(telefono[i]))
            {
                Console.WriteLine("Error: el teléfono solo debe contener números.");
                return false;
            }
        }

        if (telefono[0] != '9')
        {
            Console.WriteLine("Error: el número de celular debe empezar con 9.");
            return false;
        }
        return true;
    }

    public static int BuscarOpcion(string[] opciones, string texto)
    {
        string buscado = Normalizar(texto);
        for (int i = 0; i < opciones.Length; i++)
        {
            if (Normalizar(opciones[i]) == buscado)
                return i;
        }
        return -1;
    }

    public static string Normalizar(string texto)
    {
        string t = texto.Trim().ToLower();
        t = t.Replace('á', 'a').Replace('é', 'e').Replace('í', 'i')
             .Replace('ó', 'o').Replace('ú', 'u');
        return t;
    }

    public static string LimpiarTexto(string texto)
    {
        return texto.Replace('|', ' ').Trim();
    }

    public static string LeerTexto(string mensaje)
    {
        string valor;
        do
        {
            Console.Write(mensaje);
            valor = LimpiarTexto(Console.ReadLine() ?? "");
            if (valor == "")
                Console.WriteLine("Error: este campo no puede estar vacío.");
        } while (valor == "");
        return valor;
    }

    public static int LeerEntero(string mensaje)
    {
        int numero;
        Console.Write(mensaje);
        while (!int.TryParse(Console.ReadLine(), out numero))
        {
            Console.WriteLine("Error: ingrese un número válido.");
            Console.Write(mensaje);
        }
        return numero;
    }

    public static string LeerClave(string mensaje)
    {
        Console.Write(mensaje);

        if (Console.IsInputRedirected)
            return Console.ReadLine() ?? "";

        string clave = "";
        ConsoleKeyInfo tecla;
        do
        {
            tecla = Console.ReadKey(true);
            if (tecla.Key == ConsoleKey.Backspace && clave.Length > 0)
            {
                clave = clave.Substring(0, clave.Length - 1);
                Console.Write("\b \b");
            }
            else if (!char.IsControl(tecla.KeyChar))
            {
                clave += tecla.KeyChar;
                Console.Write("*");
            }
        } while (tecla.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return clave;
    }

    public static string FechaActual()
    {
        return DateTime.Now.ToString("dd/MM/yyyy HH:mm");
    }
}

public static class Usuarios
{
    public static readonly string[] Cuentas =
        { "director", "coordinador", "prof1", "prof2", "prof3", "prof4", "prof5" };

    public static readonly string[] Claves =
        { "dir2026", "coord2026", "prof1234", "prof2345", "prof3456", "prof4567", "prof5678" };

    public static readonly string[] Cargos =
        { "Director", "Coordinador", "Profesor", "Profesor", "Profesor", "Profesor", "Profesor" };

    public static readonly string[] Nombres =
        { "Luis Salazar", "Marta Vega", "Carmen Ruiz", "Jorge Paredes",
          "Elena Castro", "Daniel Rojas", "Sofía León" };

    public static int Actual = -1;

    public static int BuscarCuenta(string cuenta)
    {
        return Validaciones.BuscarOpcion(Cuentas, cuenta);
    }

    public static string Etiqueta(int pos)
    {
        return Cargos[pos] + " " + Nombres[pos];
    }
                                                                                                                
    public static string EtiquetaDeCuenta(string cuenta)
    {
        int pos = BuscarCuenta(cuenta);
        if (pos == -1)
            return cuenta;
        return Etiqueta(pos);
    }

    // El director y el coordinador pueden ver todos los trámites.
    public static bool EsDirectivo()
    {
        return Actual != -1 && Cargos[Actual] != "Profesor";
    }

    // Pide usuario y contraseña (máximo 3 intentos). Devuelve true si entró.
    public static bool IniciarSesion()
    {
        Console.WriteLine("*** INICIAR SESIÓN ***");
        Console.WriteLine("(Deje el usuario vacío y presione Enter para cancelar)");

        for (int intento = 1; intento <= 3; intento++)
        {
            Console.Write("Usuario: ");
            string cuenta = (Console.ReadLine() ?? "").Trim();

            if (cuenta == "")
            {
                Console.WriteLine("Inicio de sesión cancelado.");
                return false;
            }

            string clave = Validaciones.LeerClave("Contraseña: ");

            int pos = BuscarCuenta(cuenta);
            if (pos != -1 && Claves[pos] == clave)
            {
                Actual = pos;
                Console.WriteLine("Bienvenido(a), " + Etiqueta(pos) + ".");
                return true;
            }

            Console.WriteLine($"Usuario o contraseña incorrectos. Intento {intento} de 3.");
        }

        Console.WriteLine("Demasiados intentos fallidos.");
        return false;
    }

    public static void CerrarSesion()
    {
        Actual = -1;
    }

    public static void MostrarLista()
    {
        for (int i = 0; i < Cuentas.Length; i++)
        {
            Console.WriteLine($"[{i + 1}] {Etiqueta(i)}");
        }
    }

    public static int ElegirDestinatario(string mensaje)
    {
        MostrarLista();
        int numero;
        do
        {
            numero = Validaciones.LeerEntero(mensaje);
            if (numero < 1 || numero > Cuentas.Length)
                Console.WriteLine("Error: elija un número de la lista.");
        } while (numero < 1 || numero > Cuentas.Length);
        return numero - 1;
    }
}

public static class Tramites
{

    private static int SiguienteExpediente()
    {
        int mayor = 0;
        for (int i = 0; i < Datos.Contador; i++)
        {
            if (Datos.Expedientes[i] > mayor)
                mayor = Datos.Expedientes[i];
        }
        return mayor + 1;
    }

    public static int BuscarPosicion(int numero)
    {
        for (int i = 0; i < Datos.Contador; i++)
        {
            if (Datos.Expedientes[i] == numero)
                return i;
        }
        return -1;
    }

    private static bool PuedeVer(int i)
    {
        if (Usuarios.EsDirectivo())
            return true;
        return Datos.Destinos[i] == Usuarios.Cuentas[Usuarios.Actual];
    }

    private static string ElegirParentesco()
    {
        for (int i = 0; i < Datos.ParentescosValidos.Length; i++)
        {
            Console.WriteLine($"[{i + 1}] {Datos.ParentescosValidos[i]}");
        }

        int numero;
        do
        {
            numero = Validaciones.LeerEntero("Elegir parentesco: ");
            if (numero < 1 || numero > Datos.ParentescosValidos.Length)
                Console.WriteLine("Error: elija un número de la lista.");
        } while (numero < 1 || numero > Datos.ParentescosValidos.Length);

        string parentesco = Datos.ParentescosValidos[numero - 1];
        if (parentesco == "Otro")
            parentesco = Validaciones.LeerTexto("Especifique el parentesco: ");

        return parentesco;
    }

    public static void Registrar()
    {
        Console.WriteLine("*** REGISTRO DE TRÁMITE ***");

        if (Datos.Contador >= Datos.Max)
        {
            Console.WriteLine("Capacidad máxima alcanzada.");
            return;
        }

        string solicitante = Validaciones.LeerTexto("Ingresar nombre del solicitante: ");

        Console.WriteLine();
        Console.WriteLine("¿Qué es el solicitante del alumno?");
        string parentesco = ElegirParentesco();
        Console.WriteLine();

        string dni;
        do
        {
            Console.Write("Ingresar DNI: ");
            dni = (Console.ReadLine() ?? "").Trim();
        } while (!Validaciones.ValidarDNI(dni));

        string telefono;
        do
        {
            Console.Write("Ingresar teléfono (9 dígitos): ");
            telefono = (Console.ReadLine() ?? "").Trim();
        } while (!Validaciones.ValidarTelefono(telefono));

        string tipo;
        int posTipo;
        do
        {
            Console.Write("Ingresar tipo de trámite (justificación, vacante, certificado, constancia): ");
            tipo = (Console.ReadLine() ?? "").Trim();
            posTipo = Validaciones.BuscarOpcion(Datos.TiposValidos, tipo);
            if (posTipo == -1)
                Console.WriteLine("Error: tipo de trámite no válido.");
        } while (posTipo == -1);

        string motivo = "-";
        if (Datos.TiposValidos[posTipo] == "justificacion")
            motivo = Validaciones.LeerTexto("Ingresar motivo de la justificación: ");

        Console.WriteLine();
        Console.WriteLine("¿A quién va dirigido el trámite?");
        int posDestino = Usuarios.ElegirDestinatario("Elegir destinatario: ");
        Console.WriteLine();

        string encargado = Validaciones.LeerTexto("Ingresar nombre de quien recibe el documento: ");

        int pos = Datos.Contador;
        int expediente = SiguienteExpediente();
        string ahora = Validaciones.FechaActual();

        Datos.Expedientes[pos] = expediente;
        Datos.Solicitantes[pos] = solicitante;
        Datos.Dnis[pos] = dni;
        Datos.Tipos[pos] = Datos.TiposValidos[posTipo];
        Datos.Destinos[pos] = Usuarios.Cuentas[posDestino];
        Datos.Estados[pos] = "Recibido";
        Datos.Fechas[pos] = ahora;
        Datos.RecibidoPor[pos] = encargado;
        Datos.Movimientos[pos] = ahora;
        Datos.Telefonos[pos] = telefono;
        Datos.Parentescos[pos] = parentesco;
        Datos.Motivos[pos] = motivo;
        Datos.Contador++;

        Archivos.Guardar();
        Console.WriteLine("Trámite registrado. N° de expediente: " + expediente);
        Console.WriteLine("Dirigido a: " + Usuarios.Etiqueta(posDestino));
    }

    public static void Mostrar(bool todos)
    {
        if (todos)
            Console.WriteLine("*** TODOS LOS TRÁMITES ***");
        else
            Console.WriteLine("*** MIS TRÁMITES ***");

        int mostrados = 0;
        for (int i = 0; i < Datos.Contador; i++)
        {
            bool mio = Datos.Destinos[i] == Usuarios.Cuentas[Usuarios.Actual];
            if (!todos && !mio)
                continue;

            Console.WriteLine(
                $"Exp. {Datos.Expedientes[i]} | {Datos.Solicitantes[i]} | {Datos.Tipos[i]} | " +
                $"Dirigido a: {Usuarios.EtiquetaDeCuenta(Datos.Destinos[i])} | " +
                $"Estado: {Datos.Estados[i]} | {Datos.Fechas[i]}");

            if (Datos.Tipos[i] == "justificacion")
                Console.WriteLine("   Motivo: " + Datos.Motivos[i]);

            mostrados++;
        }

        if (mostrados == 0)
            Console.WriteLine("No hay trámites para mostrar.");
    }

    public static void Buscar()
    {
        Console.WriteLine("*** BUSCAR EXPEDIENTE ***");

        if (Datos.Contador == 0)
        {
            Console.WriteLine("No hay trámites registrados.");
            return;
        }

        int numero = Validaciones.LeerEntero("Ingresar número de expediente: ");
        int i = BuscarPosicion(numero);

        if (i == -1)
        {
            Console.WriteLine("Expediente no encontrado.");
            return;
        }

        if (!PuedeVer(i))
        {
            Console.WriteLine("Este expediente no está dirigido a usted.");
            return;
        }

        Console.WriteLine("Solicitante: " + Datos.Solicitantes[i] + " (" + Datos.Parentescos[i] + " del alumno)");
        Console.WriteLine("DNI: " + Datos.Dnis[i]);
        Console.WriteLine("Teléfono: " + Datos.Telefonos[i]);
        Console.WriteLine("Trámite: " + Datos.Tipos[i]);
        if (Datos.Tipos[i] == "justificacion")
            Console.WriteLine("Motivo: " + Datos.Motivos[i]);
        Console.WriteLine("Recibido por: " + Datos.RecibidoPor[i]);
        Console.WriteLine("Fecha de registro: " + Datos.Fechas[i]);
        Console.WriteLine("Dirigido a: " + Usuarios.EtiquetaDeCuenta(Datos.Destinos[i]));
        Console.WriteLine("Estado: " + Datos.Estados[i]);
        Console.WriteLine("Último movimiento: " + Datos.Movimientos[i]);
    }

    public static void Derivar()
    {
        Console.WriteLine("*** DERIVAR TRÁMITE ***");

        if (Datos.Contador == 0)
        {
            Console.WriteLine("No hay trámites registrados.");
            return;
        }

        int numero = Validaciones.LeerEntero("Ingresar número de expediente: ");
        int i = BuscarPosicion(numero);

        if (i == -1)
        {
            Console.WriteLine("Expediente no encontrado.");
            return;
        }

        if (Datos.Destinos[i] != Usuarios.Cuentas[Usuarios.Actual])
        {
            Console.WriteLine("Solo puede derivar trámites dirigidos a usted.");
            return;
        }

        Console.WriteLine("Dirigido actualmente a: " + Usuarios.EtiquetaDeCuenta(Datos.Destinos[i]));
        Console.WriteLine();
        Console.WriteLine("¿A quién desea derivarlo?");

        int posDestino;
        do
        {
            posDestino = Usuarios.ElegirDestinatario("Derivar a: ");
            if (posDestino == Usuarios.Actual)
                Console.WriteLine("Error: el trámite ya está dirigido a usted.");
        } while (posDestino == Usuarios.Actual);

        Datos.Destinos[i] = Usuarios.Cuentas[posDestino];
        Datos.Estados[i] = "Derivado";
        Datos.Movimientos[i] = Validaciones.FechaActual();

        Archivos.Guardar();
        Console.WriteLine("Trámite derivado a: " + Usuarios.Etiqueta(posDestino));
    }

    public static void Ordenar()
    {
        for (int i = 0; i < Datos.Contador - 1; i++)
        {
            for (int j = 0; j < Datos.Contador - 1 - i; j++)
            {
                if (Datos.Expedientes[j] < Datos.Expedientes[j + 1])
                {
                    (Datos.Expedientes[j], Datos.Expedientes[j + 1]) = (Datos.Expedientes[j + 1], Datos.Expedientes[j]);
                    Intercambiar(Datos.Solicitantes, j);
                    Intercambiar(Datos.Dnis, j);
                    Intercambiar(Datos.Tipos, j);
                    Intercambiar(Datos.Destinos, j);
                    Intercambiar(Datos.Estados, j);
                    Intercambiar(Datos.Fechas, j);
                    Intercambiar(Datos.RecibidoPor, j);
                    Intercambiar(Datos.Movimientos, j);
                    Intercambiar(Datos.Telefonos, j);
                    Intercambiar(Datos.Parentescos, j);
                    Intercambiar(Datos.Motivos, j);
                }
            }
        }

        Archivos.Guardar();
        Console.WriteLine("Trámites ordenados por expediente (más recientes primero).");
    }

    private static void Intercambiar(string[] arreglo, int j)
    {
        string temp = arreglo[j];
        arreglo[j] = arreglo[j + 1];
        arreglo[j + 1] = temp;
    }
}

public static class Archivos
{
    private static readonly string Carpeta =
        Path.Combine(AppContext.BaseDirectory, "datos");

    private static readonly string Ruta =
        Path.Combine(Carpeta, "tramites.txt");

    public static void Guardar()
    {
        try
        {
            Directory.CreateDirectory(Carpeta);

            string[] lineas = new string[Datos.Contador];
            for (int i = 0; i < Datos.Contador; i++)
            {
                lineas[i] = string.Join("|",
                    Datos.Expedientes[i],
                    Datos.Solicitantes[i],
                    Datos.Dnis[i],
                    Datos.Tipos[i],
                    Datos.Destinos[i],
                    Datos.Estados[i],
                    Datos.Fechas[i],
                    Datos.RecibidoPor[i],
                    Datos.Movimientos[i],
                    Datos.Telefonos[i],
                    Datos.Parentescos[i],
                    Datos.Motivos[i]);
            }
            File.WriteAllLines(Ruta, lineas);
        }
        catch (IOException ex)
        {
            Console.WriteLine("Error al guardar el archivo: " + ex.Message);
        }
    }

    public static void Cargar()
    {
        Datos.Contador = 0;

        if (!File.Exists(Ruta))
            return;

        try
        {
            string[] lineas = File.ReadAllLines(Ruta);
            for (int i = 0; i < lineas.Length && Datos.Contador < Datos.Max; i++)
            {
                string[] campos = lineas[i].Split('|');
                if (campos.Length != 12 || !int.TryParse(campos[0], out int exp))
                    continue; 

                if (Usuarios.BuscarCuenta(campos[4]) == -1)
                    continue; 

                int pos = Datos.Contador;
                Datos.Expedientes[pos] = exp;
                Datos.Solicitantes[pos] = campos[1];
                Datos.Dnis[pos] = campos[2];
                Datos.Tipos[pos] = campos[3];
                Datos.Destinos[pos] = campos[4];
                Datos.Estados[pos] = campos[5];
                Datos.Fechas[pos] = campos[6];
                Datos.RecibidoPor[pos] = campos[7];
                Datos.Movimientos[pos] = campos[8];
                Datos.Telefonos[pos] = campos[9];
                Datos.Parentescos[pos] = campos[10];
                Datos.Motivos[pos] = campos[11];
                Datos.Contador++;
            }
            Console.WriteLine($"Se cargaron {Datos.Contador} trámite(s) desde el archivo.");
        }
        catch (IOException ex)
        {
            Console.WriteLine("Error al leer el archivo: " + ex.Message);
        }
    }

    public static string RutaArchivo()
    {
        return Ruta;
    }
}
