Module Module1
    ' Función que suma dos números
    Function Sumar(a As Integer, b As Integer) As Integer
        Return a + b
    End Function

    Sub Main()
        Console.WriteLine("Laboratorio 10-3: Uso de funciones en VB.NET")
        Console.Write("Ingrese el primer número: ")
        Dim n1 As Integer = Convert.ToInt32(Console.ReadLine())
        Console.Write("Ingrese el segundo número: ")
        Dim n2 As Integer = Convert.ToInt32(Console.ReadLine())

        Dim resultado As Integer = Sumar(n1, n2)
        Console.WriteLine("La suma de los números es: " & resultado)

        Console.WriteLine()
        Console.WriteLine("Presione cualquier tecla para salir...")
        Console.ReadKey()
    End Sub
End Module
