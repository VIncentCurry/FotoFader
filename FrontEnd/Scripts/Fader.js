nereidFadeObjects = new Object();
nereidFadeTimers = new Object();

var fadeArray = [];

function nereidFade(object, destOp, rate, delta){

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
    delta=Math.min(direction*diff,delta);
    //object.filters.alpha.opacity+=direction*delta;
    ChangeOpacityValue(object, direction*delta);

    //if (object.filters.alpha.opacity != destOp){
    if(opacityValue(object) != destOp){
        nereidFadeObjects[object.sourceIndex]=object;
        nereidFadeTimers[object.sourceIndex]=setTimeout("nereidFade(nereidFadeObjects["+object.sourceIndex+"],"+destOp+","+rate+","+delta+")",rate);
    }
}

//This function will return the opacity value of the object as a decimal between 0 and 100, whatever browser we are using.
function opacityValue(id)
{
    var object = document.getElementById(id);
    
    if (object.style.MozOpacity != null)
		return object.style.MozOpacity * 100;
    else if (object.style.KhtmlOpacity != null)
		return object.style.KhtmlOpacity;
	else if (object.style.opacity&&!obj.style.filters)
		return object.style.opacity;
	else if (object.filters != null)
	    return GetIEOpacity(object);
}

function GetIEOpacity(object)
{
    for (var i = 90; i<= 100; i++)
    {
        if(object.filter = 'alpha(opacity=' + i + ')')
            return i;
    }
}

function SetOpacityValue(object, value)
{
    if (object.style.MozOpacity)
		object.style.MozOpacity = value / 100;
    else if (object.style.KhtmlOpacity)
		object.style.KhtmlOpacity = value;
	else if (object.filters.alpha.opacity)
	    object.filters.alpha.opacity = value;
}


function ChangeOpacityValue(object, value)
{
    if (object.MozOpacity)
		object.MozOpacity += value / 100;
    else if (object.KhtmlOpacity)
		object.KhtmlOpacity += value;
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
    
function hideElementByID(elementName)
{
    document.getElementById(elementName).style.display='collapse';
}
    
function showElementByID(elementName)
{
    document.getElementById(elementName).style.display='block';
}

function clickButton(e, buttonid){

      var evt = e ? e : window.event;

      var bt = document.getElementById(buttonid);

      if (bt){

          if (evt.keyCode == 13){

                bt.click();

                return false;

          }

      }

}