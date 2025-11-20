<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Laboratorio203.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gestión de Laptops - Laboratorio 20</title>

    <style>
        body { font-family: Arial, sans-serif; margin: 40px; }
        .container { max-width: 800px; margin: 0 auto; }
        .form-group { margin: 15px 0; }
        .form-group label { display: inline-block; width: 100px; font-weight: bold; }
        .form-group input { padding: 8px; }
        .toolbar { margin: 1px 0; }
        .toolbar img { cursor: pointer; margin-right: 5px; }
        .grid-view { margin-top: 20px; width: 100%; }
        .message { padding: 10px; margin: 10px 0; border-radius: 4px; }
        .success { background-color: #d4edda; color: #155724; border: 1px solid #c3e6cb; }
        .error { background-color: #f8d7da; color: #721c24; border: 1px solid #f5c6cb; }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Gestión de Laptops - Laboratorio 20</h1>

            <div class="form-group">

        <div class="toolbar">

            <asp:ImageButton ID="btnNuevo" runat="server"
                ImageUrl="~/Img2/nuevo.png"
                OnClick="btnNuevo_Click"
                Height="32px" Width="32px"
                ToolTip="Nuevo registro"
                CssClass="image-button" />

            <asp:ImageButton ID="btnGuardar" runat="server"
                ImageUrl="~/Img2/guardar.png"
                OnClick="btnGuardar_Click"
                Enabled="false"
                Height="32px" Width="32px"
                ToolTip="Guardar registro"
                CssClass="image-button" />

            <asp:ImageButton ID="btnCancelar" runat="server"
                ImageUrl="~/Img2/cancelar.png"
                OnClick="btnCancelar_Click"
                Enabled="false"
                Height="32px" Width="32px"
                ToolTip="Cancelar operación"
                CssClass="image-button" />

            <asp:ImageButton ID="btnEliminar" runat="server"
                ImageUrl="~/Img2/eliminar.png"
                OnClick="btnEliminar_Click"
                Enabled="false"
                Height="32px" Width="32px"
                ToolTip="Eliminar registro"
                CssClass="image-button" />

                <strong>Buscar por ID:</strong>
                <asp:TextBox ID="txtBuscarId" runat="server" Width="80px"></asp:TextBox>
                <asp:ImageButton ID="btnBuscar" runat="server"
                    ImageUrl="~/Img2/buscar.png"
                    OnClick="btnBuscar_Click"
                    Height="24px" Width="24px"
                    ToolTip="Buscar por ID"
                    CssClass="image-button" />

            <asp:Button ID="btnSalir" runat="server" Text="Salir" 
                OnClick="btnSalir_Click"
                CssClass="btn-salir" />
        </div>
            </div>

            <!-- FORMULARIO -->
            <div class="form-group">
                <label>ID:</label>
                <asp:TextBox ID="txtId" runat="server" Enabled="false" BackColor="#f0f0f0"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>Nombre:</label>
                <asp:TextBox ID="txtNombre" runat="server" Enabled="false"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>Precio:</label>
                <asp:TextBox ID="txtPrecio" runat="server" Enabled="false" TextMode="Number"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>Stock:</label>
                <asp:TextBox ID="txtStock" runat="server" Enabled="false" TextMode="Number"></asp:TextBox>
            </div>

            <asp:Label ID="lblMensaje" runat="server" Text="" CssClass="message"></asp:Label>

            <asp:GridView ID="gvLaptops" runat="server" AutoGenerateColumns="true"
                CssClass="grid-view" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                OnSelectedIndexChanged="gvLaptops_SelectedIndexChanged">
                <HeaderStyle BackColor="#007bff" ForeColor="White" Font-Bold="true" />
                <RowStyle BackColor="#f8f9fa" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

        </div>
    </form>
</body>
</html>
