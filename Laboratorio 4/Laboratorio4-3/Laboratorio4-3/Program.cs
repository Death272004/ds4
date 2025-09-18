using System;

namespace Laboratorio43
{
    class Program
    {
        static void Main(string[] args)
        {
            int numero, suma = 0;

            do
            {
                Console.Write("Ingrese un número (0 para salir): ");
                numero = int.Parse(Console.ReadLine());
                suma += numero;
            } while (numero != 0);

            Console.WriteLine($"La suma total es: {suma}");
        }
    }
}
