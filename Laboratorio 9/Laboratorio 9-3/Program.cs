using System;

public class Program
{
    public static void Main(string[] args)
    {
        double lado1, lado2, lado3;

        Console.WriteLine("Ingrese el primer lado del triángulo:");
        lado1 = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Ingrese el segundo lado del triángulo:");
        lado2 = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Ingrese el tercer lado del triángulo:");
        lado3 = Convert.ToDouble(Console.ReadLine());

        // Verificar si los lados pueden formar un triángulo
        if ((lado1 + lado2 > lado3) && (lado1 + lado3 > lado2) && (lado2 + lado3 > lado1))
        {
            Console.WriteLine("\nLos lados forman un triángulo.");

            // Determinar el tipo de triángulo
            if (lado1 == lado2 && lado2 == lado3)
            {
                Console.WriteLine("Es un triángulo equilátero (todos los lados iguales).");
            }
            else if (lado1 == lado2 || lado1 == lado3 || lado2 == lado3)
            {
                Console.WriteLine("Es un triángulo isósceles (dos lados iguales).");
            }
            else
            {
                Console.WriteLine("Es un triángulo escaleno (todos los lados diferentes).");
            }
        }
        else
        {
            Console.WriteLine("\nLos valores ingresados no forman un triángulo.");
            Console.WriteLine("La suma de dos lados debe ser mayor que el tercero.");
        }
    }
}
