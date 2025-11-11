Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnMensaje_Click(sender As Object, e As EventArgs) Handles btnMensaje.Click
        lblMensaje.Text = "Hola Mundo en ASP.NET"
    End Sub
End Class