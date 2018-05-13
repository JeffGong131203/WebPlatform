<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComDebug.aspx.cs" Inherits="SerialPortSvc.ComDebug" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="当前串口："></asp:Label>
            <asp:Label ID="lblComName" runat="server"></asp:Label>
            <asp:Label ID="Label2" runat="server" Text="是否打开："></asp:Label>
            <asp:Label ID="lblStatus" runat="server"></asp:Label>
            <asp:Button ID="btnOpen" runat="server" OnClick="btnOpen_Click" Text="Open" />
            <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Text="Close" />
            <br />
            <asp:TextBox ID="txtSendData" runat="server" Width="336px"></asp:TextBox>
            <asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" Text="Send" />
            <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="Refresh" />
            <br />
            <asp:TextBox ID="txtRecive" runat="server" Height="123px" ReadOnly="True" TextMode="MultiLine" Width="335px"></asp:TextBox>
        </div>
    </form>
</body>
</html>
