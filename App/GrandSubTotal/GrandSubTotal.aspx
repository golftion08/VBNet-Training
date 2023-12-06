<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GrandSubTotal.aspx.vb" Inherits="VBNet_Training.GrandSubTotal" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../CSS/GridviewCSS.css" rel="stylesheet" />
    <title>Grand and Sub total</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Grand and Sub Total</h1>
        <div>
            <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" OnDataBound="OnDataBound" >
                <AlternatingRowStyle BackColor="#FFFFFF" />
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#F875AA" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#F875AA" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
