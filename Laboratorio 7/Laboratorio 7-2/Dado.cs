using System;

public class Dado
{
    private int valor;
    private static Random aleatorio;
    l
    public Dado()
    {
        aleatorio = new Random();
    }

    public void Tirar()
    {
        valor = aleatorio.Next(1, 7);  // Números del 1 al 6
    }

    public void Imprimir()
    {
        Console.WriteLine("El valor del dado es:" + valor);
    }

    public int RetornarValor()
    {
        return valor;
    }
}