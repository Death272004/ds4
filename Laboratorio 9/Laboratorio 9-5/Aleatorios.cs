using System;
using System.Collections.Generic;

public class Aleatorios
{
    private Random random;

    public Aleatorios()
    {
        random = new Random();
    }

    public int GenerarNumero(int minimo, int maximo)
    {
        return random.Next(minimo, maximo + 1);
    }

    public int[] GenerarArreglo(int cantidad, int minimo, int maximo)
    {
        int[] arreglo = new int[cantidad];
        for (int i = 0; i < cantidad; i++)
            arreglo[i] = GenerarNumero(minimo, maximo);
        return arreglo;
    }
}
