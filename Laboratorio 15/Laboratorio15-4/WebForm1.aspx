<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WebForm1.aspx.vb" Inherits="WebForm1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sumar Números</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Calcular 2 números</h2>
            
            <p>Ingrese el primer número:</p>
            <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="false"></asp:TextBox>
            
            <p>Ingrese el segundo número:</p>
            <asp:TextBox ID="TextBox2" runat="server" AutoPostBack="false"></asp:TextBox>
            
            <br /><br />
            <asp:Button ID="Button1" runat="server" Text="Sumar" OnClick="Button1_Click" />
            
            <br /><br />
            <table border="1">
                <tr>
                    <td><strong>Resultado</strong></td>
                    <td>
                        <asp:Label ID="LabelResultado" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>