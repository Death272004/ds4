internal interface ITemplate
{
    void ponerVariables(string nombre, string valor);
    void verHtml(string template);
}

class Template : ITemplate
{
    public void ponerVariables(string nombre, string valor)
    {
        Console.WriteLine($"Nombre: {nombre}, Valor: {valor}");
    }

    public void verHtml(string template)
    {
        Console.WriteLine(template);
    }
}