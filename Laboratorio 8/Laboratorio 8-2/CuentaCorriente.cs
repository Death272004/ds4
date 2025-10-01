public class CuentaCorriente : Cuenta
{
    public CuentaCorriente(string prmtIdCuenta) : base(prmtIdCuenta) { }

    public override void CalcularInteres()
    {
        Console.WriteLine("Calculo de Interes en clase derivada para cuenta {0}", this.getidCuenta());
    }
}