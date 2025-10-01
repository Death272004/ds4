using System;

class Laboratorio7_1
{
    static void Main(string[] args)
    {
        Banco banco1 = new Banco();
        banco1.Operar();
        banco1.DepositosTotales();
        Console.ReadKey();
    }
}