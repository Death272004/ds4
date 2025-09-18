using System;

namespace Laboratorio46
{
    class Program
    {
        const double PI = Math.PI;

        static void Main(string[] args)
        {
            Console.Write("Ingrese el radio del círculo: ");
            double radio = double.Parse(Console.ReadLine());

            double area = PI * Math.Pow(radio, 2);

            Console.WriteLine($"El área del círculo con radio {radio} es: {area}");
        }
    }
}
