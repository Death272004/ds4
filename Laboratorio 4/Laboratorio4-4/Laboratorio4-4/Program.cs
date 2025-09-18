using System;

namespace Laboratorio44
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Ingrese un número: ");
            int n = int.Parse(Console.ReadLine());

            if (n > 0)
                Console.WriteLine("El número es positivo.");
            else if (n < 0)
                Console.WriteLine("El número es negativo.");
            else
                Console.WriteLine("El número es cero.");
        }
    }
}
