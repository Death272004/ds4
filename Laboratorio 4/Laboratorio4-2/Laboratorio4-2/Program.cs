using System;

namespace Laboratorio42
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Ingrese un número entero: ");
            int n = int.Parse(Console.ReadLine());
            long factorial = 1;

            for (int i = 1; i <= n; i++)
            {
                factorial *= i;
            }

            Console.WriteLine($"El factorial de {n} es: {factorial}");
        }
    }
}
