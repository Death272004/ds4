namespace Ejercicio5App
{
    using System;
    using System.Collections.Generic;

    public class Program
    {
        public static void Main(string[] args)
        {
            Aleatorios ale = new Aleatorios();
            int[] arreglo = GenerarArregloNoRepetido(ale, 10, 1, 20);

            Console.WriteLine("Arreglo de números aleatorios NO repetidos:");
            foreach (int n in arreglo)
                Console.Write(n + " ");
        }

        public static int[] GenerarArregloNoRepetido(Aleatorios ale, int cantidad, int minimo, int maximo)
        {
            if (cantidad > (maximo - minimo + 1))
                throw new ArgumentException("Cantidad mayor al rango disponible.");

            HashSet<int> numeros = new HashSet<int>();
            while (numeros.Count < cantidad)
            {
                int num = ale.GenerarNumero(minimo, maximo);
                numeros.Add(num);
            }

            int[] arreglo = new int[cantidad];
            numeros.CopyTo(arreglo);
            return arreglo;
        }
    }
}
