<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PhotoFaderTagger.aspx.cs" Inherits="PhotoFaderTagger" MasterPageFile="~/PhotoFader.master" %>

<asp:Content ContentPlaceHolderID="PhotoFaderHead" ID="PhotoFaderScripts" runat="server">

    <script type="text/javascript" src="scripts/wz_jsgraphics.js"></script>
    <script type="text/javascript" src="scripts/Popups.js"></script>
    <script type="text/javascript" src="scripts/Fader.js"></script>
    <script type="text/javascript" src="scripts/DynamicPopup.js"></script></asp:Content>
<asp:Content ContentPlaceHolderID="fbPhotoFaderBody" ID="FotoFaderBody" runat="server">

    <script language="javascript" type="text/javascript">
    var recording = false;
    var deleting = false;
    var photoTaggingMode = false;
    var pos_x, pos_y, cur_x, cur_y, prev_x, prev_y, picture_pos_x, picture_pos_y, selector_x, selector_y;
    
    var txtCooridnatesID;
    var FotoFaderInstructionsID;
    var txtBtnTagID;
                    
    var popupSelector = new PopupWindow('tagSelector');
    popupSelector.offsetX = 0;
    popupSelector.offsetY = 0;
    
    document.onkeydown = HandleKeyPress;
    
    function outline(event)
    {
        if(recording && !deleting)
        { 
            if (!event) var event = window.event;
            if (event.pageX || event.pageY) 	{
	            pos_x = event.pageX;
	            pos_y = event.pageY;
            }
            else if (event.clientX || event.clientY) 	{
	            pos_x = event.clientX + document.body.scrollLeft
		            + document.documentElement.scrollLeft;
	            pos_y = event.clientY + document.body.scrollTop
		            + document.documentElement.scrollTop;
            }
            
            picture_pos_x = pos_x - findPosition(document.getElementById("PhotoToTag"))[0] ;
            picture_pos_y = pos_y- findPosition(document.getElementById("PhotoToTag"))[1];
            
            window.status = picture_pos_x + "," + picture_pos_y;
            document.getElementById(txtCooridnatesID).value += picture_pos_x + "," + picture_pos_y + ",";
            photoDrawing.drawLine(prev_x, prev_y, pos_x, pos_y);
            photoDrawing.paint();
                    
            prev_x = pos_x;
            prev_y = pos_y;

        }
        return true;
    }
    
    function findPosition( oElement ) {
    if( typeof( oElement.offsetParent ) != 'undefined' ) {
        
        return [ oElement.offsetLeft + oElement.offsetParent.offsetLeft, oElement.offsetTop + oElement.offsetParent.offsetTop ];
      } else {
        return [ oElement.x, oElement.y ];
      }
    }
    
    //This function is called when the line is clicked - here we simply call the PhotoClicked function.
    function DrawingClicked()
    {
        PhotoClicked();
    }
    
    function PhotoClicked()
    {
        if(photoTaggingMode)
        {
            if(deleting)
            {
                deleting = false;
            }
            else if(recording)
            {
                recording = false;
                document.body.style.cursor = 'default';
                
                //we're done with the tag - display the list of friends
                popupSelector.offsetX = TagSelectorXPosition();
                popupSelector.offsetY = TagSelectorYPosition();
                popupSelector.showPopup(PhotoTagID);
            }
            else
            {
                recording = true;
                document.body.style.cursor = 'crosshair';
            }
        }
        return true;
    }
    
    function ContinueDrawing()
    {
        popupSelector.hidePopup();
        return false;
    }
    
    function TagSelectorXPosition()
    {
        var tagSelectorWidth = document.getElementById("tagSelector").offsetWidth;
        var photoWidth = document.getElementById("PhotoToTag").offsetWidth;
        
        if(picture_pos_x > (photoWidth - tagSelectorWidth))
            return (photoWidth - tagSelectorWidth);
        else
            return picture_pos_x;
    }
    
    
    function TagSelectorYPosition()
    {
        var tagSelectorHeight = document.getElementById("tagSelector").offsetHeight;
        var photoHeight = document.getElementById("PhotoToTag").offsetHeight;
           
        if(picture_pos_y > (photoHeight - tagSelectorHeight))
            return (photoHeight - tagSelectorHeight);
        else
            return picture_pos_y;
    }
    
    function SetPageForTagging(on)
    {
        if(on)
        {
            photoTaggingMode = true;
            showElementByID(FotoFaderInstructionsID);
            
        }
        else
        {
            photoTaggingMode = false;
        }
    }
    
    function highlightPerson(filename)
    {    
        document.getElementById("PhotoToTag").style.background = "url('" + filename + "')";
        document.getElementById("PhotoToTag").style.backgroundRepeat="no-repeat";
        opacity(PhotoTagID, opacityValue(PhotoTagID), 40, 500);
    }

    function fadePerson()
    {
        opacity(PhotoTagID, opacityValue(PhotoTagID), 100, 500);
    }
    
    function ClearTagging()
    {
        if(submitted)
        {
            alert('The tag has already been submitted. However, you will be able to remove it.');
        }
        else
        {
            document.getElementById(txtCooridnatesID).value = "";
            photoDrawing.clear();
            popupSelector.hidePopup();
            recording = false;
            prev_x = "";
            prev_y = "";
        }
    }
    
    var displayNames = "";
    
    function showName(event, name)
    {
        if(displayNames.length == 0)
            displayNames = name;
        else
            displayNames += ", " + name;
        
        doTooltip(event, '', displayNames);
    }
    
    function hideName(event, name)
    {
        if(displayNames == name)
        {
            displayNames = "";
            hideTip();
        }
        else
        {
            displayNames = displayNames.replace("," + name, "");    
            doTooltip(event, '', displayNames);
        }
    }
    
    function EraseFinalPoint()
    {
        if(recording)
        {
            deleting = true;
            
            var coordinates = document.getElementById(txtCooridnatesID).value;
            var penultimateCommaPosition = coordinates.lastIndexOf(',', coordinates.length - 2);
            var secondLastCommaPosition = coordinates.lastIndexOf(',', penultimateCommaPosition - 1);
            
            document.getElementById(txtCooridnatesID).value = coordinates.substr(0, secondLastCommaPosition + 1);
            
            RedrawTaggingLine();
            /*coordinates = coordinates.substr(0, secondLastCommaPosition + 1);
            
            var points = coordinates.split(',');

            prev_x = points[0] * 1 + findPosition(document.getElementById("PhotoToTag"))[0] * 1;
            prev_y = points[1] * 1 + findPosition(document.getElementById("PhotoToTag"))[1] * 1;
            
            photoDrawing.clear();
            
            for(var i = 2; i <= points.length - 2; i+=2)
            {
                cur_x = points[i] * 1 + findPosition(document.getElementById("PhotoToTag"))[0] * 1 ;
                cur_y = points[i+1] * 1 + findPosition(document.getElementById("PhotoToTag"))[1] * 1;
                
                photoDrawing.drawLine(prev_x, prev_y, cur_x, cur_y);
                
                prev_x = cur_x;
                prev_y = cur_y;
            }
            
            photoDrawing.paint();
            
            document.getElementById(txtCooridnatesID).value = coordinates;*/
        }
    }
    
    function RedrawTaggingLine()
    {
        var coordinates = document.getElementById(txtCooridnatesID).value;
        
        var points = coordinates.split(',');

            prev_x = points[0] * 1 + findPosition(document.getElementById("PhotoToTag"))[0] * 1;
            prev_y = points[1] * 1 + findPosition(document.getElementById("PhotoToTag"))[1] * 1;
            
            photoDrawing.clear();
            
            for(var i = 2; i <= points.length - 2; i+=2)
            {
                cur_x = points[i] * 1 + findPosition(document.getElementById("PhotoToTag"))[0] * 1 ;
                cur_y = points[i+1] * 1 + findPosition(document.getElementById("PhotoToTag"))[1] * 1;
                
                photoDrawing.drawLine(prev_x, prev_y, cur_x, cur_y);
                
                prev_x = cur_x;
                prev_y = cur_y;
            }
            
            photoDrawing.paint();
    }
    
    function HandleKeyPress(event)
    {
        var zAsciiCode = 90;
	
	    if(event==undefined)
	    {
		    if(window.event.ctrlKey==1 && window.event.keyCode == zAsciiCode)
			    EraseFinalPoint();
	    }
	    else
	    {
		    if(event.ctrlKey==1 && event.keyCode == zAsciiCode)
			    EraseFinalPoint();
	    }
    }
    
    function ShowFotofadeHelpOnStartUp(show)
    {
        document.getElementById(showHelpOnFotoFadeID).checked = show;
        
        var prm = Sys.WebForms.PageRequestManager.getInstance();
    
        prm._doPostBack('<%= chkShowFotofadeOnStartFotoFading.ClientID %>', '');
    }
    
    function ValidateUserSelected()
    {
        var nameSelected;
        
        if(CheckSubmission())
        {
            for (var i=0; i<document.getElementsByName('<%=rdlFriends.ClientID.Replace("_", "$") %>').length; i++)
            {
                if (document.getElementsByName('<%=rdlFriends.ClientID.Replace("_", "$") %>')[i].checked)
                {
                    nameSelected = true;
                }
            }
            
            if(document.getElementById('<%=txtPhotoTagName.ClientID %>').value == '' && !nameSelected)
            {
                alert('You must select either a person or enter their name for tagging');
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            submitted = false;
            return false;
        }
    }
    
    </script>
<script src="http://static.ak.facebook.com/js/api_lib/v0.4/FeatureLoader.js.php" type="text/javascript"></script>
    <asp:HyperLink ID="hlBackToAlbum" runat="server">Back to Album</asp:HyperLink><span class="pipe">|</span><asp:HyperLink
        ID="hlBackToAllAlbums" runat="server">Back to all Albums</asp:HyperLink><asp:HyperLink ID="hlNextPhoto" runat="server" CssClass="pager">Next</asp:HyperLink><asp:HyperLink ID="hlPreviousPhoto" runat="server" CssClass="pager">Previous</asp:HyperLink><asp:Panel
            ID="FotoFaderInstructions" runat="server" CssClass="" Style="display:none;">
        Click on the 
            photo to start your tagging. Draw round the individual, using CTRL + Z to undo the last bit of drawing should you make a mistake. When you have drawn round the person and 
            returned to the start, click again. You can then choose a friend or type a name. Once you have completed your tagging, click the 
            done tagging button.<asp:Button 
                ID="btnDoneTagging" runat="server" CssClass="fbbutton" 
                onclick="btnDoneTagging_Click" OnClientClick="hideElementByID(FotoFaderInstructionsID);" Text="Done Tagging" />
                <!--</div>--></asp:Panel>
    <div id="PhotoToTag" name="PhotoToTag"> 
    
        <asp:Image ID="imgPhotoToTag" runat="server" onclick="PhotoClicked();" onmousemove="outline(event)" /><asp:TextBox ID="txtCoordinates" runat="server" style="visibility:hidden;display:none"></asp:TextBox>
    
    </div><script type="text/javascript" language="javascript">
    var photoDrawing = new jsGraphics("PhotoToTag");
    photoDrawing.setColor("white");
    photoDrawing.setStroke(2);
    </script>
    <asp:Literal ID="ltlPhotoTitle" runat="server"></asp:Literal><div><asp:HyperLink ID="hlFotoFadeThisPhoto" name="TagPosition" NavigateUrl="#" runat="server" onclick="SetPageForTagging(true);return false;">fotofade this photo</asp:HyperLink></div><div><asp:PlaceHolder
        ID="phPeopleLinks" runat="server"></asp:PlaceHolder></div>
        <asp:Literal ID="ltlMapArea" runat="server"></asp:Literal>
    <div id="tagSelector" name="tagSelector" style="visibility:collapse;position:absolute;overflow:hidden;">
    <label for="name" id="label_name" class="pts_name_input">Type any name or tag:</label><asp:TextBox
        ID="txtPhotoTagName" runat="server" cssclass="inputtext i_name" size="20" onkeypress="return clickButton(event, txtBtnTagID);"></asp:TextBox><span id="pts_choose_text">
        or choose a person:</span><div id="pts_userlist">
            <asp:RadioButtonList ID="rdlFriends" runat="server">
    </asp:RadioButtonList>
    <asp:ObjectDataSource ID="odsFriends" runat="server" 
    SelectMethod="LoggedInUsersFriends" TypeName="PhotoTaggerOM.FacebookUser"></asp:ObjectDataSource></div>
            <asp:Button ID="btnTag" runat="server" Text="Tag" onclick="btnTag_Click" CssClass="fbbutton" /><input
                id="btnCancel" type="button" value="Cancel" onclick="javascript:ClearTagging();"  class="fbbutton" /><input id="btnContinueDrawing" type="button" value="Continue" class="fbbutton" onclick="javascript:ContinueDrawing();" />
        </div>
    <asp:PlaceHolder ID="phFasterLoadingImages" runat="server"></asp:PlaceHolder>
    <asp:UpdatePanel ID="upShowFotoFaderInstructions" runat="server"><ContentTemplate>
        <asp:CheckBox ID="chkShowFotofadeOnStartFotoFading" runat="server"  
            AutoPostBack="true"
            OnCheckedChanged="chkShowFotofadeOnStartFotoFading_CheckedChanged" style="display:none;" /></ContentTemplate>
    </asp:UpdatePanel></asp:Content>
    
