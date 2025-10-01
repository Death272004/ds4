using System;

public class Program
{
    public static void Main(string[] args)
    {
        // Crear un objeto de la clase Aleatorios
        Aleatorios ale = new Aleatorios();

        // Generar un número aleatorio entre 1 y 10
        int numero = ale.GenerarNumero(1, 10);
        Console.WriteLine("Número aleatorio entre 1 y 10: " + numero);

        // Generar un arreglo de 5 números aleatorios entre 10 y 50
        int[] arreglo = ale.GenerarArreglo(5, 10, 50);

        Console.WriteLine("\nArreglo generado:");
        foreach (int n in arreglo)
        {
            Console.Write(n + " ");
        }
    }
}
