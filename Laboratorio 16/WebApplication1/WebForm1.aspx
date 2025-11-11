<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .cal {
            position: absolute;
            top: 50px;
            left: 150px;
            right: 400px;
            height: 600px;
            bottom: 100px;
            background-color: dodgerblue;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="cal">
            <asp:Label ID="lblTitulo" Text="CALCULADORA BASICA" runat="server" Style="margin-left: 50px"
                Font-Bold="True" Font-Italic="False" ForeColor="White" Font-Size="25px"></asp:Label>
            
            <asp:TextBox ID="txtDisplay" runat="server" Style="margin-left: 50px; margin-top: 24px;"
                Width="335px" Height="41px"></asp:TextBox>
            
            <br />
            <asp:Button ID="btn1" Text="1" runat="server" Height="37px" Style="margin-left: 50px"
                Width="57px" OnClick="btn1_Click" />
            <asp:Button ID="btn2" Text="2" runat="server" Height="37px" Style="margin-left: 0px"
                Width="57px" OnClick="btn2_Click" />
            <asp:Button ID="btn3" Text="3" runat="server" Height="37px" Style="margin-left: 0px"
                Width="57px" OnClick="btn3_Click" />
            <asp:Button ID="btnAdd" Text="+" runat="server" Height="37px" Style="margin-left: 0px; margin-top: 0px;"
                Width="57px" OnClick="btnAdd_Click" />
            
            <br />
            <asp:Button ID="btn4" Text="4" runat="server" Height="37px" Style="margin-left: 50px"
                Width="57px" OnClick="btn4_Click" />
            <asp:Button ID="btn5" Text="5" runat="server" Height="37px" Style="margin-left: 0px"
                Width="57px" OnClick="btn5_Click" />
            <asp:Button ID="btn6" Text="6" runat="server" Height="37px" Style="margin-left: 0px"
                Width="57px" OnClick="btn6_Click" />
            <asp:Button ID="btnSub" Text="-" runat="server" Height="37px" Style="margin-left: 0px"
                Width="57px" OnClick="btnSub_Click" />
            
            <br />
            <asp:Button ID="btn7" Text="7" runat="server" Height="37px" Style="margin-left: 50px"
                Width="57px" OnClick="btn7_Click" />
            <asp:Button ID="btn8" Text="8" runat="server" Height="37px" Style="margin-left: 0px"
                Width="57px" OnClick="btn8_Click" />
            <asp:Button ID="btn9" Text="9" runat="server" Height="37px" Style="margin-left: 0px"
                Width="57px" OnClick="btn9_Click" />
            <asp:Button ID="btnMul" Text="*" runat="server" Height="37px" Style="margin-left: 0px"
                Width="57px" OnClick="btnMul_Click" />
            
            <br />
            <asp:Button ID="btn0" runat="server" Text="0" Height="37px" Style="margin-left: 50px"
                Width="57px" OnClick="btn0_Click" />
            <asp:Button ID="btnClr" runat="server" Text="CLR" Height="37px" Style="margin-left: 0px"
                Width="57px" OnClick="btnClr_Click" />
            <asp:Button ID="btnEq" runat="server" Text="=" Height="37px" Style="margin-left: 0px"
                Width="57px" OnClick="btnEq_Click" />
            <asp:Button ID="btnDiv" Text="/" runat="server" Height="37px" Style="margin-left: 0px"
                Width="57px" OnClick="btnDiv_Click" />
        </div>
    </form>
</body>
</html>