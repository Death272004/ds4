using System;

public class CalculosMatematicos
{
    public int Calcular(int a, int b)
    {
        return (a + b) * (a - b);
    }

    public double CalculoArea(double radio)
    {
        return Math.PI * Math.Pow(radio, 2);
    }

    public double CalcularPerimetroRectangulo(double largo, double ancho)
    {
        return 2 * (largo + ancho);
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        CalculosMatematicos calc = new CalculosMatematicos();

        // ===== EJERCICIO 1 =====
        Console.WriteLine("Introduce el primer número:");
        int primerNumero = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Introduce el segundo número:");
        int segundoNumero = Convert.ToInt32(Console.ReadLine());

        int resultado = calc.Calcular(primerNumero, segundoNumero);
        Console.WriteLine("El resultado de (a+b)*(a-b) es: " + resultado);

        // ===== EJERCICIO 2 =====
        Console.WriteLine("\nAhora vamos a calcular el área de un círculo.");
        Console.Write("Introduce el valor del radio: ");
        double radio = Convert.ToDouble(Console.ReadLine());

        double area = calc.CalculoArea(radio);
        Console.WriteLine("El área del círculo con radio " + radio + " es: " + area);

        // ===== EJERCICIO 3 =====
        Console.WriteLine("\nAhora vamos a calcular el perímetro de un rectángulo.");
        Console.Write("Introduce el largo: ");
        double largo = Convert.ToDouble(Console.ReadLine());

        Console.Write("Introduce el ancho: ");
        double ancho = Convert.ToDouble(Console.ReadLine());

        double perimetro = calc.CalcularPerimetroRectangulo(largo, ancho);
        Console.WriteLine("El perímetro del rectángulo es: " + perimetro);
    }
}
