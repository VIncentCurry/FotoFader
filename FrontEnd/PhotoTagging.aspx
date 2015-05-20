<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="PhotoTagging.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Photo Tagging</title>
    <script type="text/javascript" src="scripts/wz_jsgraphics.js"></script>
    <link type="text/css" rel="Stylesheet" href="css/PhotoFaderStyles.css" />
</head>
<body>	
    <form id="frmPhotoTagger" runat="server">
    <script src="http://static.ak.connect.facebook.com/js/api_lib/v0.4/FeatureLoader.js.php" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
    var recording = true;
    var pos_x, pos_y, cur_x, cur_y, prev_x, prev_y;
    var facebookApplicationKey;
    
    alert('this happens during the page load event...');
    
    
    function outline(event)
    {
        if(recording)
        {
            cur_x = event.offsetX?(event.offsetX):event.pageX;
            cur_y = event.offsetY?(event.offsetY):event.pageY;
            pos_x = cur_x-document.getElementById("imgPhoto").offsetLeft;
            pos_y = cur_y-document.getElementById("imgPhoto").offsetTop;
            
            window.status = pos_x + "," + pos_y;
            document.getElementById("txtCoordinates").value += pos_x + "," + pos_y + ",";
            photoDrawing.drawLine(prev_x, prev_y, pos_x, pos_y);
            photoDrawing.paint();
            
            prev_x = pos_x;
            prev_y = pos_y;
            
        }
    }
    
    //this script works!!!!
function doSomething(e) {
	var posx = 0;
	var posy = 0;
	if (!e) var e = window.event;
	if (e.pageX || e.pageY) 	{
		posx = e.pageX;
		posy = e.pageY;
	}
	else if (e.clientX || e.clientY) 	{
		posx = e.clientX + document.body.scrollLeft
			+ document.documentElement.scrollLeft;
		posy = e.clientY + document.body.scrollTop
			+ document.documentElement.scrollTop;
	}
	window.status = posx + "," + posy;
            document.getElementById("txtCoordinates").value += posx + "," + posy + ",";
            photoDrawing.drawLine(prev_x, prev_y, posx, posy);
            photoDrawing.paint();
            
            prev_x = posx;
            prev_y = posy;
}
function showPermissionsDialog() 
{ 
alert("show permissions");
    FB.Bootstrap.requireFeatures(["Connect"], function() { 
            FB.Facebook.init(facebookApplicationKey, "xd_receiver.htm");
            FB.Connect.showPermissionDialog('publish_stream, offline_access, user_photos, user_photo_video_tags', function() {window.open("http://localhost:3529/AdvancedPhotoTagger/", "_parent") });
        });
}
    </script>	
    
    <div id="PhotoToTag">
        <asp:Image ID="imgPhoto" runat="server" ImageUrl="fotofaderpatentpicture.jpg" onmousemove="doSomething(event)" /><asp:TextBox ID="txtCoordinates" runat="server" style="visibility:hidden"></asp:TextBox><asp:Button
                ID="btnSaveTag" runat="server" Text="Save Tagging data" 
        onclick="btnSaveTag_Click" />
    </div>
    <asp:Label ID="lblCoordinates" runat="server"></asp:Label>
    <asp:Literal ID="ltlMapArea" runat="server"></asp:Literal>
    </form>
    <script type="text/javascript" language="javascript">
    alert('This happens after the page is loaded...');
    var photoDrawing = new jsGraphics("PhotoToTag");
    photoDrawing.setColor("blue");
    photoDrawing.setStroke(1);
    showPermissionsDialog();
    </script>
</body>
</html>
