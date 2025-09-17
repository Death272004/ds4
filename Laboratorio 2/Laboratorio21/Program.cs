using System;

namespace Laboratorio21
{
    class Program
    {
        public static void Main()
        {
            // Asignando valor a variable estática.
            MyClass.Valor = 1;
            Console.WriteLine(MyClass.Valor);
        }
    }

    public class MyClass
    {
        // Declarando variable estática.
        public static int Valor;
    }
}
