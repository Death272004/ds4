using System;

namespace Laboratorio5_1
{
    class Program
    {
        private int[] sueldos;
        public void Cargar()
        {
            sueldos = new int[5]; 
            for (int f = 0; f < sueldos.Length; f++)
            {
                Console.Write("Ingrese sueldo del operario " + (f + 1) + ": ");
                string linea = Console.ReadLine();
                sueldos[f] = int.Parse(linea);
            }
        }

        // Método para imprimir los sueldos
        public void Imprimir()
        {
            Console.WriteLine("\nLos 5 sueldos de los operarios son:");
            for (int f = 0; f < sueldos.Length; f++)
            {
                Console.WriteLine("Operario " + (f + 1) + ": " + sueldos[f]);
            }
            Console.ReadKey();
        }

        // Main principal
        static void Main(string[] args)
        {
            Program pv = new Program();
            pv.Cargar();
            pv.Imprimir();
        }
    }
}
