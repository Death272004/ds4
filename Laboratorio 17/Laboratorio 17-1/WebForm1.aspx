<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Laboratorio171.WebForm1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Laboratorio 17 - Parte 2</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        .grid { border-collapse: collapse; width: 100%; }
        .grid th, .grid td { border: 1px solid #ddd; padding: 8px; text-align: left; }
        .grid th { background-color: #f2f2f2; }
        .grid tr:nth-child(even) { background-color: #f9f9f9; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Ventas por Categoría - Seafood</h2>
            <asp:GridView ID="GridV" runat="server" CssClass="grid" AutoGenerateColumns="true">
            </asp:GridView>
        </div>
    </form>
</body>
</html>