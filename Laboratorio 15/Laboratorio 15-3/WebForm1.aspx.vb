Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim texto As String = TextBox1.Text
        Page.ClientScript.RegisterClientScriptBlock(GetType(Page), "MessageBox", "window.alert('Hola: " + texto + "');", True)
    End Sub

End Class