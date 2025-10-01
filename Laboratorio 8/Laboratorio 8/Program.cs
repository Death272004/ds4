using System;

class Program
{
    public static void Main()
    {
        // Crear un objeto de tipo Trabajador
        Trabajador p = new Trabajador("Josan", 22, "77588250-z", 100000);

        // Mostrar los datos del trabajador
        Console.WriteLine("Nombre = " + p.Nombre);
        Console.WriteLine("Edad = " + p.Edad);
        Console.WriteLine("NIF = " + p.NIF);
        Console.WriteLine("Sueldo = " + p.Sueldo);

        Console.ReadKey();
    }
}