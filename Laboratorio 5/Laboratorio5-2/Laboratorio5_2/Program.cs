using System;

namespace Laboratorio5_1
{
    class Program
    {
        private int[,] mat;

        public void Ingresar()
        {
            mat = new int[3, 4]; 
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write($"Ingrese valor en posición [{i + 1},{j + 1}]: ");
                    string linea = Console.ReadLine();
                    mat[i, j] = int.Parse(linea);
                }
            }
        }

        public void Imprimir()
        {
            Console.WriteLine("\nMatriz ingresada:");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(mat[i, j] + "\t");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }

        public static void Main(string[] args)
        {
            Program ma = new Program();
            ma.Ingresar();
            ma.Imprimir();
        }
    }
}
