<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InitialHelp.aspx.cs" Inherits="InitialHelp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>fotofader Help</title>
</head>
<body>
    <form id="frmFotoFaderHelp" runat="server">
    <div id="defaultLightbox">
    <p>The fotofader application has been created to allow clearer photo tagging.</p><p>In order to see it in action, browse to a photo (assuming you are not there already) and then put your mouse over the names at the bottom of the page. You will see the rest of the photo fading out, whilst the person remains at normal view.</p><p>If you want to fotofade tag your own photos, simply click the "fotofade tag this photo" link at the bottom of the page.</p><p><a href="#" class="lbAction" rel="deactivate">Close Help</a></p><p>
        <asp:CheckBox ID="chkShowHelpOnStartUp" onclick="javascript:ShowHelpOnStartUp(this.checked)" runat="server" Text="Show this screen when starting fotofader" /></p>
        
    </div>
    </form>
</body>
</html>
