using System;

namespace Laboratorio5_1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] frutas = { "Manzana", "Platano", "Cereza", "Naranja" };

            foreach (string fruta in frutas)
            {
                Console.WriteLine(fruta);
            }

            Console.ReadKey();
        }
    }
}

