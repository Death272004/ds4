using System;

namespace Parcial_1
{
    class Parcial_1
    {
        private int[,] arreglo;
        int N;
        int suma = 0;

        public void Cargar()
        {
            do
            {
                Console.Write("Ingrese un numero mayor o igual a 6: ");
                N = int.Parse(Console.ReadLine());

                if (N < 6)
                {
                    Console.WriteLine("Error, el número debe ser mayor o igual a 6.");
                }
                else if (N % 2 != 0)
                {
                    Console.WriteLine("Error, el número debe ser par.");
                }
            } while (N < 6 || N % 2 != 0);

            arreglo = new int[N, N];
            Random rand = new Random();

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    // 1. Esquinas de 2x2
                    bool esquinaSuperiorIzquierda = (i < 2 && j < 2);
                    bool esquinaSuperiorDerecha = (i < 2 && j >= N - 2);
                    bool esquinaInferiorIzquierda = (i >= N - 2 && j < 2);
                    bool esquinaInferiorDerecha = (i >= N - 2 && j >= N - 2);

                    // 2. Solo la diagonal secundaria (de esquina inferior izquierda a superior derecha)
                    bool diagonalSecundaria = (i + j == N - 1);

                    // Excluir las posiciones que ya están en las esquinas
                    bool enDiagonal = diagonalSecundaria &&
                                     !(i < 2 && j >= N - 2) &&  // Excluir esquina superior derecha
                                     !(i >= N - 2 && j < 2);    // Excluir esquina inferior izquierda

                    bool debeTenerValor = esquinaSuperiorIzquierda || esquinaSuperiorDerecha ||
                                         esquinaInferiorIzquierda || esquinaInferiorDerecha ||
                                         enDiagonal;

                    if (debeTenerValor)
                    {
                        arreglo[i, j] = rand.Next(101, 201);
                        suma += arreglo[i, j];
                    }
                    else
                    {
                        arreglo[i, j] = 0;
                    }
                }
            }
        }

        public void Imprimir()
        {
            Console.WriteLine("\nMatriz generada:");
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write(arreglo[i, j].ToString().PadLeft(4) + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine($"\nLa suma de los números aleatorios es: {suma}");
            Console.WriteLine("\nPresione una tecla para finalizar...");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            Parcial_1 pc = new Parcial_1();
            pc.Cargar();
            pc.Imprimir();
        }
    }
}