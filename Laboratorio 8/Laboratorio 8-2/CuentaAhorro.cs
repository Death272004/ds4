public class CuentaAhorro : Cuenta
{
    public CuentaAhorro(string prmtIdCuenta) : base(prmtIdCuenta) { }

    public override void CalcularInteres()
    {
        Console.WriteLine("Calculo de Interes en clase derivada para cuenta {0}", this.getidCuenta());
    }
}