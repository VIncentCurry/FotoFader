<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AlbumPage.aspx.cs" Inherits="AlbumPage" Title="Album Page" MasterPageFile="~/PhotoFader.master" %>

<asp:Content ContentPlaceHolderID="fbPhotoFaderBody" runat="server" ID="AlbumPageBody">
    <asp:HyperLink ID="hlBackToAlbums" runat="server">Back to all albums</asp:HyperLink>
        <asp:PlaceHolder ID="phPhotos" runat="server">
            <asp:Table ID="tblPhotoGrid" runat="server">
            </asp:Table>
        </asp:PlaceHolder></asp:Content>
    
