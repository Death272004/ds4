using System;

namespace Laboratorio_12_3
{
    public class Area
    {
        public Area()
        {
        }

        public static double Calcular_Area(double a, double b, double c)
        {
            double s = (a + b + c) / 2;
            return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
        }
    }
}