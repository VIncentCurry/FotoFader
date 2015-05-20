<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Friends.aspx.cs" Inherits="Friends" Title="Friends" MasterPageFile="~/PhotoFader.master" %>

<asp:Content ContentPlaceHolderID="fbPhotoFaderBody" ID="FriendsBody" runat="server">
        <asp:HyperLink ID="hlOwnAlbums1" runat="server" NavigateUrl="~/Albums.aspx">Return to your own albums</asp:HyperLink>
        <asp:ObjectDataSource ID="odsFriends" runat="server" 
            SelectMethod="LoggedInUsersFriends" TypeName="PhotoTaggerOM.FacebookUser"></asp:ObjectDataSource>
        <asp:GridView ID="gvwFriends" runat="server" AutoGenerateColumns="False" 
            DataSourceID="odsFriends">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="ID" 
                    DataNavigateUrlFormatString="Albums.aspx?FriendID={0}" DataTextField="Name" 
                    DataTextFormatString="{0}" />
            </Columns>
        </asp:GridView><br />
        <asp:HyperLink ID="hlOwnAlbums" runat="server" NavigateUrl="~/Albums.aspx">Return to your own albums</asp:HyperLink></asp:Content>
    
