<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TaggingHelp.aspx.cs" Inherits="FullHelp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tagging Help</title>
</head>
<body>
    <form id="frmTaggingHelp" runat="server">
    <div>
    <ol><li>Click on the "fotofade tag this photo" link.</li><li>Click on the photo where you would like to start drawing around your subject. The cursor will change to a cross-hair.</li><li>Draw round the person you wish to tag. A white line will be drawn on the photo, indicating the area you have tagged.</li><li>If you should make an error in your drawing, use CTRL + Z to undo the drawing - you will see the white line being erased. Once you have deleted the tagging you didn't want, move the cursor back to where you wanted to tag, and click the mouse.</li><li>Once you've drawn round the person you want to tag, click the mouse again, and a list of your firends will pop-up, as well as a text box. Either check your friend's name or type their name into the text box and click 'Tag'.</li><li>Should you wish your tag someone up to the <!--side of the photo: <br /><img alt="" src="images/sideinstructions1_small.jpg" /><br />then, when outlining the subject, move your cursor up to the edge:<br /><img alt="" src="images/sideinstructions2_small.jpg" /><br />and then off the photo back round to where the subject hits the edge of the photo:<br /><img alt="" src="Images/SideInstructions3_small.jpg" /><br />The tagger will now join the points where you left and entered the photo.--><a href="sidetagging.aspx" class="lbOn">side of the photo</a> then drag the cursor off the photo and round to where the 
        person starts in the photo again. The tagging will then draw a line between the two points. It should be noted that you should move off and back onto the photo fairly slowly, otherwise you will miss out a portion of the person.</li></ol><a href="#" class="lbAction" rel="deactivate">Close Help.</a><p>
            <asp:CheckBox ID="chkShowInstructionsOnStartFotofade" runat="server" onclick="javascript:ShowFotofadeHelpOnStartUp(this.checked);" Text="Show these insturctions when I fotofade a photo" />
            </p>

    </div>
    </form>
</body>
</html>
