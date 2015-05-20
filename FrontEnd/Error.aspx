<%@ Page Language="C#" MasterPageFile="~/PhotoFader.master" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" Title="FotoFader Error" %>

<asp:Content ID="ErrorHead" ContentPlaceHolderID="PhotoFaderHead" Runat="Server">
</asp:Content>
<asp:Content ID="ErrorBody" ContentPlaceHolderID="fbPhotoFaderBody" Runat="Server">
    <asp:Label ID="lblErrorMessage" runat="server" Text="Oh dear! An error has occurred! The details have been logged and we will try and solve the problem. In the meantime, you could click the back button in your browser and try again."></asp:Label>
</asp:Content>

