<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Laboratorio20.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tabla de Multiplicar - Laboratorio 20</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 40px; }
        .container { max-width: 600px; margin: 0 auto; }
        .input-group { margin: 20px 0; }
        .table-container { margin-top: 20px; }
        table { border-collapse: collapse; width: 100%; }
        th, td { border: 1px solid #ddd; padding: 8px; text-align: center; }
        th { background-color: #f2f2f2; }
        tr:nth-child(even) { background-color: #f9f9f9; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Laboratorio 20 - Problema 1</h1>
            <h2>Tabla de Multiplicar</h2>
            
            <div class="input-group">
                <asp:Label ID="lblInstruccion" runat="server" Text="Ingrese un número para generar su tabla de multiplicar:"></asp:Label>
                <br /><br />
                <asp:TextBox ID="txtNumero" runat="server" Width="200px" placeholder="Ej: 5" TextMode="Number"></asp:TextBox>
                <asp:Button ID="btnGenerar" runat="server" Text="Generar Tabla" OnClick="btnGenerar_Click" />
            </div>

            <div class="table-container">
                <asp:Label ID="lblResultado" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>