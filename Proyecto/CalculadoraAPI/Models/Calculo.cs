using System;

namespace CalculadoraAPI.Models
{
    public class Calculo
    {
        public int ID { get; set; }
        public string OPERACION { get; set; }
        public string RESULTADO { get; set; }
        public string TIPOS { get; set; }
        public DateTime FECHA { get; set; }
    }
}