using System;

namespace Laboratorio2
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            //Ejemplo untilizando las variables de instacia de clase.
            client.FirstName = "Su_Nombre";
            client.LastName = "Su_Apellido";
            client.Age = 15; // Su_Edad
            client.id = 1;
            Console.WriteLine(client.GetFullName());
        }
    }
}

public class Client
{
    // Declaracion de variables de instancia en clase.
    public int id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ushort Age { get; set; }

    public string GetFullName()
    {
        //Utilizando las variables de instancia dentro de metodos de la clase.
        return FirstName + " " + LastName;
    }
}