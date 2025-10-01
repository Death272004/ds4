class Trabajador : Persona
{
    public int Sueldo;

    // Constructor (debe ser público para poder usarlo desde Main)
    public Trabajador(string nombre, int edad, string nif, int sueldo)
        : base(nombre, edad, nif) // llama al constructor de Persona
    {
        Sueldo = sueldo;
    }
}