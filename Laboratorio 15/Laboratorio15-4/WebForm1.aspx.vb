Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim num1 As Double
        Dim num2 As Double
        Dim resultado As Double

        ' Convertir los textos a números y sumar
        If Double.TryParse(TextBox1.Text, num1) AndAlso Double.TryParse(TextBox2.Text, num2) Then
            resultado = num1 + num2
            LabelResultado.Text = resultado.ToString()
        Else
            LabelResultado.Text = "Error: Ingrese números válidos"
        End If
    End Sub
End Class