using System;

public class CalculosMatematicos
{
    public int Calcular(int a, int b)
    {
        return (a + b) * (a - b);
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Introduce el primer número:");
        int primerNumero = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Introduce el segundo número:");
        int segundoNumero = Convert.ToInt32(Console.ReadLine());

        CalculosMatematicos calc = new CalculosMatematicos();
        int resultado = calc.Calcular(primerNumero, segundoNumero);

        Console.WriteLine("El resultado de (a+b)*(a-b) es: " + resultado);
    }
}
