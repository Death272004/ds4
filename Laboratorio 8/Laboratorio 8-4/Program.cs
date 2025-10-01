internal class Program
{
    private static void Main(string[] args)
    {
        Empleado empleado = new Empleado();
        empleado.Nombre = "John Doe";
        Console.WriteLine($" Nombre del empleado: {empleado.Nombre}");

        CuentaBancaria cta = new CuentaBancaria();
        cta.Saldo = 100;
        Console.WriteLine($" Saldo de la cuenta bancaria: {cta.Saldo}");

        Cobertura c = new Cobertura(5);
        Console.WriteLine($" COn una Cobertura: {c.Radio}");

    }
}