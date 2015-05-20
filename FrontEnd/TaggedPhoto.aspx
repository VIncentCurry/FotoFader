 <%@ Page Language="C#" AutoEventWireup="true" CodeFile="TaggedPhoto.aspx.cs" Inherits="TaggedPhoto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<SCRIPT language="JavaScript" type="text/javascript">

<!--
nereidFadeObjects = new Object();
nereidFadeTimers = new Object();

var fadeArray = [];

function nereidFade(object, destOp, rate, delta){
DebugOutput(opacityValue(object));
/*if (!document.all)
return*/
    if (object != "[object]"){  
        setTimeout("nereidFade("+object+","+destOp+","+rate+","+delta+")",0);
        return;
    }   
    //clearTimeout(nereidFadeTimers[object.sourceIndex]);
    //DebugOutput('We have cleared the timers???');
    //diff = destOp-object.filters.alpha.opacity;
    diff = destOp - opacityValue(object);
    direction = 1;
    //if (object.filters.alpha.opacity > destOp){
    if(opacityValue(object) > destOp){
        direction = -1;
    }
    DebugOutput('Delta before min:' + delta);
    delta=Math.min(direction*diff,delta);
    //object.filters.alpha.opacity+=direction*delta;
    DebugOutput('Delta = ' + delta);
    ChangeOpacityValue(object, direction*delta);

    //if (object.filters.alpha.opacity != destOp){
    if(opacityValue(object) != destOp){
        nereidFadeObjects[object.sourceIndex]=object;
        nereidFadeTimers[object.sourceIndex]=setTimeout("nereidFade(nereidFadeObjects["+object.sourceIndex+"],"+destOp+","+rate+","+delta+")",rate);
    }
}

//This function will return the opacity value of the object as a decimal between 0 and 100, whatever browser we are using.
function opacityValue(object)
{
    if (object.style.MozOpacity)
		return object.style.MozOpacity * 100;
    else if (object.style.KhtmlOpacity)
		return object.style.KhtmlOpacity;
	else if (object.filters.alpha.opacity)
	    return object.filters.alpha.opacity;
	/*else if (object.style.opacity&&!obj.filters)
		return object.style.opacity;*/
}

function SetOpacityValue(object, value)
{
    DebugOutput('SettingOpacity: ' + value);
    if (object.style.MozOpacity)
		object.style.MozOpacity = value / 100;
    else if (object.style.KhtmlOpacity)
		object.style.KhtmlOpacity = value;
	else if (object.filters.alpha.opacity)
	    object.filters.alpha.opacity = value;
}


function ChangeOpacityValue(object, value)
{
    DebugOutput('Adjusting Opacity: ' + value);
    if (object.style.MozOpacity)
		object.style.MozOpacity += value / 100;
    else if (object.style.KhtmlOpacity)
		object.style.KhtmlOpacity += value;
	else if (object.filters.alpha.opacity)
	    object.filters.alpha.opacity += value;
}

function opacity(id, opacStart, opacEnd, millisec) {
    //speed for each frame
    var speed = Math.round(millisec / 100);
    var timer = 0;
    
    clearPendingOpacities();

    //determine the direction for the blending, if start and end are the same nothing happens
    if(opacStart > opacEnd) {
        for(i = opacStart; i >= opacEnd; i--) {
            fadeArray.push(setTimeout("changeOpac(" + i + ",'" + id + "')",(timer * speed)));
            timer++;
        }
    } else if(opacStart < opacEnd) {
        for(i = opacStart; i <= opacEnd; i++)
            {
            fadeArray.push(setTimeout("changeOpac(" + i + ",'" + id + "')",(timer * speed)));
            timer++;
        }
    }
}

function clearPendingOpacities()
{
    for(var i=0; i < fadeArray.length; i++)
    {
        clearTimeout(fadeArray[i]);
    }
    
    fadeArray = [];
}

//change the opacity for different browsers
function changeOpac(opacity, id) {
    var object = document.getElementById(id).style;
    object.opacity = (opacity / 100);
    object.MozOpacity = (opacity / 100);
    object.KhtmlOpacity = (opacity / 100);
    object.filter = "alpha(opacity=" + opacity + ")"; 
    }

function highlightPerson(filename)
{
    document.getElementById("backgroundPicture").style.background = "url('" + filename + "')";
    document.getElementById("backgroundPicture").style.backgroundRepeat="no-repeat";
    //nereidFade(document.getElementById("imgTaggedPhoto"),30,5,5);
    opacity("imgTaggedPhoto", opacityValue("imgTaggedPhoto"), 40, 500);
}

function fadePerson()
{
    opacity("imgTaggedPhoto", opacityValue("imgTaggedPhoto"), 100, 500);
    //nereidFade(document.getElementById("imgTaggedPhoto"),100,20,10)
}

function DebugOutput(message)
{
    document.getElementById("debugOutput").innerHTML += '<br/ >' + message;
}
//-->

</SCRIPT>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tagged Photo</title>
    <link type="text/css" rel="Stylesheet" href="css\PhotoFaderStyles.css" />
</head>
<body>
    <form id="frmTaggedPhoto" runat="server">
    <div id="backgroundPicture" style="background:url('fotofaderpatentpictureperson2.jpg'); background-repeat:no-repeat;" >
        <asp:Image ID="imgTaggedPhoto" runat="server" ImageUrl="fotofaderpatentpicture.jpg"/></div>
    <asp:HyperLink ID="hlCeline" NavigateUrl="" runat="server" onMouseOver="highlightPerson('fotofaderpatentpictureperson2.jpg');" onMouseOut="fadePerson();">Celine</asp:HyperLink>&nbsp;<asp:HyperLink
        ID="hlIlda" runat="server" NavigateUrl="" onMouseOver="highlightPerson('fotofaderpatentpictureperson.jpg');" onMouseOut="fadePerson();">Ilda</asp:HyperLink><div id="debugOutput"></div>
    </form>
</body>
</html>
