using System;

namespace Laboratorio47
{
    class Program
    {
        static void Main(string[] args)
        {
            Saludar("Richard");
            Saludar("Ana", "¡Hola!");
        }

        static void Saludar(string nombre, string saludo = "Buenos días")
        {
            Console.WriteLine($"{saludo}, {nombre}!");
        }
    }
}
