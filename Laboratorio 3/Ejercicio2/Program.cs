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
}

internal class Program
{
    private static void Main(string[] args)
    {
        // ===== EJERCICIO 1 =====
        Console.WriteLine("Introduce el primer número:");
        int primerNumero = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Introduce el segundo número:");
        int segundoNumero = Convert.ToInt32(Console.ReadLine());

        CalculosMatematicos calc = new CalculosMatematicos();
        int resultado = calc.Calcular(primerNumero, segundoNumero);

        Console.WriteLine("El resultado de (a+b)*(a-b) es: " + resultado);

        // ===== EJERCICIO 2 =====
        Console.WriteLine("\nAhora a calcular el área de un círculo.");
        Console.WriteLine("Introduce el valor del radio:");
        double radio = Convert.ToDouble(Console.ReadLine());

        double area = calc.CalculoArea(radio);
        Console.WriteLine("El área del círculo con radio " + radio + " es: " + area);
    }
}
