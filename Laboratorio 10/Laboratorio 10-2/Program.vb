Module Module1
    Sub Main()
        Console.WriteLine("Laboratorio 10-2: Condicionales en VB.NET")
        Console.Write("Ingrese su edad: ")
        Dim edad As Integer = Convert.ToInt32(Console.ReadLine())

        If edad >= 18 Then
            Console.WriteLine("Eres mayor de edad.")
        Else
            Console.WriteLine("Eres menor de edad.")
        End If

        Console.WriteLine()
        Console.WriteLine("Presione cualquier tecla para salir...")
        Console.ReadKey()
    End Sub
End Module
