<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Laboratorio202.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Matriz Diagonal Inversa - Laboratorio 20</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 40px; }
        .container { max-width: 800px; margin: 0 auto; }
        .input-group { margin: 20px 0; }
        .matrix-container { margin-top: 20px; overflow-x: auto; }
        .matrix-table { border-collapse: collapse; margin: 10px 0; }
        .matrix-table td { 
            width: 35px; 
            height: 35px; 
            text-align: center; 
            border: 1px solid #333; 
            font-weight: bold;
        }
        .diagonal-cell { background-color: #ffeb3b; }
        .regular-cell { background-color: #e3f2fd; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Laboratorio 20 - Problema 2</h1>
            <h2>Matriz Diagonal Inversa</h2>
            
            <div class="input-group">
                <asp:Label ID="lblInstruccion" runat="server" 
                    Text="Ingrese la dimensión N para generar una matriz N x N:"></asp:Label>
                <br /><br />
                <asp:TextBox ID="txtDimension" runat="server" Width="200px" 
                    placeholder="Ej: 5" TextMode="Number"></asp:TextBox>
                <asp:Button ID="btnGenerarMatriz" runat="server" Text="Generar Matriz" OnClick="btnGenerarMatriz_Click" />
            </div>

            <div class="matrix-container">
                <asp:Label ID="lblMatriz" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>