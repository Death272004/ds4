<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WebForm1.aspx.vb" Inherits="WebForm1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Saludo Personalizado</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Introduzca un Texto</h2>
            <asp:TextBox ID="TextBox1" runat="server" Width="200px"></asp:TextBox>
            <br /><br />
            <asp:Button ID="Button1" runat="server" Text="Enviar Saludo!" OnClick="Button1_Click" />
        </div>
    </form>
</body>
</html>