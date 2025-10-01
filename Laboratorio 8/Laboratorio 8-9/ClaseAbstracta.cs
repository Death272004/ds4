using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

abstract class ClaseAbstracta
{
    //Se fuerza la herencia de la clase para definir estos métodos
    abstract protected string tomarValor();
    abstract public string prefixValor(string prefix);
    //Método común
    public void printOut()
    {
        Console.WriteLine(tomarValor());
    }
}
