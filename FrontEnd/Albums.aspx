<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Albums.aspx.cs" Inherits="Albums" MasterPageFile="~/PhotoFader.master" Title="Albums" %>
<%@ Register src="PhotosGrid.ascx" tagname="PhotosGrid" tagprefix="uc1" %>
<%@ Register src="AlbumGrid.ascx" tagname="AlbumGrid" tagprefix="uc2" %>
<asp:Content ContentPlaceHolderID="fbPhotoFaderBody" ID="AlbumsBody" runat="server">
        <asp:PlaceHolder ID="phTaggedPhotosTitle" runat="server"></asp:PlaceHolder>
        <uc1:PhotosGrid ID="pgPersonsTaggedPhotos" runat="server" />
        <asp:PlaceHolder ID="phImages" runat="server"></asp:PlaceHolder>
        <uc2:AlbumGrid ID="agAlbums" runat="server" />

        </asp:Content>
