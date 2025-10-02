Public Class Persona
    Public Nombre As String
    Public Edad As Integer

    Public Sub New(nombre As String, edad As Integer)
        Me.Nombre = nombre
        Me.Edad = edad
    End Sub

    Public Sub MostrarDatos()
        Console.WriteLine("Nombre: " & Nombre)
        Console.WriteLine("Edad: " & Edad)
    End Sub
End Class

Module Module1
    Sub Main()
        Console.WriteLine("Laboratorio 10-4: Clases y Objetos en VB.NET")
        Console.Write("Ingrese su nombre: ")
        Dim nombre As String = Console.ReadLine()

        Console.Write("Ingrese su edad: ")
        Dim edad As Integer = Convert.ToInt32(Console.ReadLine())

        Dim persona1 As New Persona(nombre, edad)
        Console.WriteLine()
        Console.WriteLine("Datos de la persona:")
        persona1.MostrarDatos()

        Console.WriteLine()
        Console.WriteLine("Presione cualquier tecla para salir...")
        Console.ReadKey()
    End Sub
End Module
