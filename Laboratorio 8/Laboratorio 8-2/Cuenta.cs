public class Cuenta
{
    private string idCuenta;

    public Cuenta(string prmtIdCuenta)
    {
        this.idCuenta = prmtIdCuenta;
        Console.WriteLine("Constructor de Clase base para cuenta {0}", prmtIdCuenta);
    }

    public virtual void CalcularInteres()
    {
        Console.WriteLine("Calculo de Interes en clase base para cuenta {0}", this.idCuenta);
    }

    public string getidCuenta()
    {
        return this.idCuenta;
    }
}