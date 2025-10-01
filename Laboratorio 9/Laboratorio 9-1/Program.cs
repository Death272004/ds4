using System;
public class Program
{
    public static void Main(string[] args)
    {
        double precio;
        string formaPago;

        // Solicitar el precio del producto
        do
        {
            Console.Write("Introduzca el precio del producto (valor positivo): ");
            precio = Convert.ToDouble(Console.ReadLine());

            if (precio <= 0)
            {
                Console.WriteLine(" El precio debe ser un valor positivo.\n");
            }

        } while (precio <= 0);

        // Solicitar forma de pago
        Console.Write("Introduzca la forma de pago (efectivo o tarjeta): ");
        formaPago = Console.ReadLine().ToLower();

        // Si el pago es con tarjeta, solicitar número de cuenta
        if (formaPago == "tarjeta")
        {
            string numeroCuenta;

            do
            {
                Console.Write("Introduzca el número de cuenta (16 dígitos): ");
                numeroCuenta = Console.ReadLine();

                if (numeroCuenta.Length != 16 || !EsNumero(numeroCuenta))
                {
                    Console.WriteLine(" El número de cuenta debe tener exactamente 16 dígitos numéricos.\n");
                }

            } while (numeroCuenta.Length != 16 || !EsNumero(numeroCuenta));

            Console.WriteLine("\n Pago con tarjeta aceptado.");
            Console.WriteLine($"Precio del producto: ${precio}");
            Console.WriteLine($"Número de cuenta: {numeroCuenta}");
        }
        else if (formaPago == "efectivo")
        {
            Console.WriteLine("\n Pago en efectivo aceptado.");
            Console.WriteLine($"Precio del producto: ${precio}");
        }
        else
        {
            Console.WriteLine("\n Forma de pago no válida. Intente de nuevo.");
        }
    }

    // Método auxiliar para verificar si una cadena contiene solo dígitos
    public static bool EsNumero(string texto)
    {
        foreach (char c in texto)
        {
            if (!char.IsDigit(c))
                return false;
        }
        return true;
    }
}
