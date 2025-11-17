<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Laboratorio171.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
<asp:GridView id="MyGridView" DataSourceID="MyDataSource1"
    AllowSorting="True" AllowPaging="True"
    DataKeyNames="ProductID"
    AutoGenerateEditButton="True"
    Runat="Server"/>

<asp:SqlDataSource ID="MyDataSource1" runat="server"
    ConnectionString="Server=localhost;Database=Northwind;TrustServerCertificate=True;Integrated Security=SSPI;"
    ProviderName="System.Data.SqlClient"
    SelectCommand="SELECT ProductId, ProductName, UnitPrice From Products"
    UpdateCommand="Update Products Set [ProductName]=@ProductName, [UnitPrice]=@UnitPrice Where [ProductId]=@ProductId">
</asp:SqlDataSource>

</div>
    </form>
</body>
</html>