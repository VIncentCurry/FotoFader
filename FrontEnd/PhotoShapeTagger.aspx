<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PhotoShapeTagger.aspx.cs" Inherits="PhotoShapeTagger" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Photo Trace Tagger</title>
    <script type="text/javascript" src="scripts/wz_jsgraphics.js"></script>
    <script type="text/javascript" src="scripts/Popups.js"></script>
    <script type="text/javascript" src="scripts/Fader.js"></script>
    <link rel="Stylesheet" type="text/css" href="css\AdvancedPhotoTagger.css" />
</head>
<body>
    <form id="frmPhotoTraceTagger" runat="server">
    <script language="javascript" type="text/javascript">
    var recording = false;
    var deleting = false;
    var photoTaggingMode = false;
    var pos_x, pos_y, cur_x, cur_y, prev_x, prev_y, prev_x2, prev_y2;
                    
    var popupSelector = new PopupWindow('tagSelector');
    popupSelector.offsetX = 0;
    popupSelector.offsetY = 0;
    
    document.onkeydown = HandleKeyPress;
    
    function outline(event)
    {
        //lblCoordinates.value = recording.toString(); //+ "; Recording = " + recording.toString();
        document.getElementById("txtPosition").value = recording;
        
        if(recording && !deleting)
        {
            cur_x = event.offsetX?(event.offsetX):event.pageX;
            cur_y = event.offsetY?(event.offsetY):event.pageY;
            /*pos_x = cur_x-document.getElementById("imgPhoto").offsetLeft;
            pos_y = cur_y-document.getElementById("imgPhoto").offsetTop;*/
            pos_x = cur_x-findPosition(document.getElementById("PhotoToTag"))[0] ;
            pos_y = cur_y-findPosition(document.getElementById("PhotoToTag"))[1];

            document.getElementById("txtCoordinates").value += pos_x + "," + pos_y + ",";
            
            if(prev_x!="")
            {
                photoDrawing.drawLine(prev_x, prev_y, cur_x, cur_y);
                photoDrawing.paint();
            }
            
            document.getElementById("txtPosition").value += " : " + pos_x + "," + pos_y + ";" + cur_x + "," + cur_y;
            
            
            prev_x = cur_x;
            prev_y = cur_y;
        }
        document.getElementById("txtPosition").value += "; All OK...";
        return true;
    }
    
    /*function point_it(event){
var pointer_div = document.getElementById("imgPhoto");

if (window.ActiveXObject) {
pos_x = window.event.offsetX;
pos_y = window.event.offsetY;
}
//for Firefox
else {
var top = 0, left = 0;
var elm = pointer_div;
while (elm) {
left = elm.offsetLeft;
top = elm.offsetTop;
elm = elm.offsetParent;
}

pos_x = event.pageX - left;
pos_y = event.pageY - top;
}
photoDrawing.drawLine(prev_x2, prev_y2, prev_x, prev_y);
            photoDrawing.paint();
            
            //document.getElementById("txtPosition").value += " : " + pos_x + "," + pos_y + ";" + cur_x + "," + cur_y;
            
            prev_x2 = prev_x;
            prev_y2 = prev_y;
            
            prev_x = pos_x;
            prev_y = pos_y;
}*/
    

    function EraseFinalPoint()
    {
        if(recording)
        {
            deleting = true;
            
            var coordinates = document.getElementById("txtCoordinates").value;
            var penultimateCommaPosition = coordinates.lastIndexOf(',', coordinates.length - 2);
            var secondLastCommaPosition = coordinates.lastIndexOf(',', penultimateCommaPosition - 1);
            
            coordinates = coordinates.substr(0, secondLastCommaPosition + 1);
            
            var points = coordinates.split(',');
            //alert(points[0] + "," + points[1]);
            prev_x = points[0] * 1 + findPosition(document.getElementById("PhotoToTag"))[0] * 1;
            prev_y = points[1] * 1 + findPosition(document.getElementById("PhotoToTag"))[1] * 1;
            
            //alert(findPosition(document.getElementById("PhotoToTag"))[0] + "," + findPosition(document.getElementById("PhotoToTag"))[1]);
            photoDrawing.clear();
            
            for(var i = 2; i <= points.length - 2; i+=2)
            {
                //alert(points[i] + "," + points[i+1]);
                
                cur_x = points[i] * 1 + findPosition(document.getElementById("PhotoToTag"))[0] * 1 ;
                cur_y = points[i+1] * 1 + findPosition(document.getElementById("PhotoToTag"))[1] * 1;
                
                //alert(cur_x + "," + cur_y);
                
                photoDrawing.drawLine(prev_x, prev_y, cur_x, cur_y);
                
                prev_x = cur_x;
                prev_y = cur_y;
            }
            
            photoDrawing.paint();
            
            document.getElementById("txtCoordinates").value = coordinates;
        }
    }

    function PhotoClicked()
    {
        /*alert("OffsetLeft:" + document.getElementById("PhotoToTag").offsetLeft);
        alert("Real left:" + getRealLeft("PhotoToTag"));
        alert("Find Position:" + findPosition(document.getElementById("imgPhoto"))[0] + "," + findPosition(document.getElementById("imgPhoto"))[1]);
        alert("Find Pos:" + findPos(document.getElementById("imgPhoto"))[0] + "," + findPos(document.getElementById("imgPhoto"))[1]);
        alert("Yahoo pos:" + yahooPosition(document.getElementById("imgPhoto"))[0] + "," + yahooPosition(document.getElementById("imgPhoto"))[1]);*/
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
              

                    // Show the window relative to the anchor name passed in
                    popupSelector.showPopup('imgPhoto');
                    

            }
            else
            {
                recording = true;
                document.body.style.cursor = 'crosshair';
            }
        }
        return true;
    }
    function DrawingClicked()
    {
        PhotoClicked();
    }
    
    function SetPageForTagging(on)
    {
        if(on)
        {
            photoTaggingMode = true;
            
        }
        else
        {
            photoTaggingMode = false;
        }
    }
    
    function getRealLeft(el){
    var L=0;
    var tempEl = document.getElementById(el);
    while(tempEl.parentNode)
    {
        L+= ( tempEl.offsetLeft)? tempEl.offsetLeft: 0;
        if(tempEl == document.body) break;
        tempEl = tempEl.parentNode;
    }
    return L;
    }
    
    /*function findPosition( oElement ) {
        if( typeof( oElement.offsetParent ) != 'undefined' ) {
            for( var posX = 0, posY = 0; oElement; oElement = oElement.offsetParent ) {
              posX += oElement.offsetLeft;
              posY += oElement.offsetTop;
            }
            return [ posX, posY ];
          } else {
            return [ oElement.x, oElement.y ];
          }
        }*/
        
    function findPosition( oElement ) {
        if( typeof( oElement.offsetParent ) != 'undefined' ) {
            /*for( var posX = 0, posY = 0; oElement; oElement = oElement.offsetParent ) {
              posX += oElement.offsetLeft;
              posY += oElement.offsetTop;
            }*/
            return [ oElement.offsetLeft + oElement.offsetParent.offsetLeft, oElement.offsetTop + oElement.offsetParent.offsetTop ];
          } else {
            return [ oElement.x, oElement.y ];
          }
        }
        
     function findPos(obj) {
	var curleft = curtop = 0;
	if (obj.offsetParent) {
	do {
			curleft += obj.offsetLeft;
			curtop += obj.offsetTop;
			} while (obj = obj.offsetParent);
			}
			return [curleft,curtop];
			}
			
	 function yahooPosition(el)  {
                // has to be part of document to have pageXY
                if ( (el.parentNode === null || el.offsetParent === null ||
                        this.getStyle(el, 'display') == 'none') && el != el.ownerDocument.body) {
                    YAHOO.log('getXY failed: element not available', 'error', 'Dom');
                    return false;
                }
                
                YAHOO.log('getXY returning ' + getXY(el), 'info', 'Dom');
                return getXY(el);
            }
            
      function highlightPerson(filename)
{
    document.getElementById("PhotoToTag").style.background = "url('" + filename + "')";
    document.getElementById("PhotoToTag").style.backgroundRepeat="no-repeat";
    opacity("imgPhoto", 100, 40, 500);
}

function fadePerson()
{
    opacity("imgPhoto", 40, 100, 500);
}

function showname(name){

document.getElementById("txtName").value = name;

}


function HandleKeyPress(e)
{
    var zAsciiCode = 90;
    
    if(e.ctrlKey && e.which == zAsciiCode)
        EraseFinalPoint();

}

function ClearTagging()
    {
        document.getElementById("txtCoordinates").value = "";
        photoDrawing.clear();
        popupSelector.hidePopup();
        recording = false;
        prev_x = "";
        prev_y = "";
    }

    </script>
    <div id="PhotoToTag" name="PhotoToTag">
        <asp:Image ID="imgPhoto" name="imgPhoto" runat="server" 
            ImageUrl="n716167974_702569_7138.jpg" onclick="PhotoClicked();" onmousemove="outline(event)" /><asp:TextBox ID="txtCoordinates" runat="server" style="visibility:hidden"></asp:TextBox>
        <asp:TextBox ID="txtPosition" runat="server" style="display:none"></asp:TextBox>
    <asp:Literal ID="ltlMapArea" runat="server"></asp:Literal>
    </div>
    <input id="txtName" type="text" />
    <script type="text/javascript" language="javascript">
    var photoDrawing = new jsGraphics("PhotoToTag");
    photoDrawing.setColor("blue");
    photoDrawing.setStroke(3);
    </script>

<a onclick="SetPageForTagging(true);" id="TagPosition" name="TagPosition" href="#">Shape Tag This Photo</a>
    <asp:PlaceHolder ID="phPeopleLinks" runat="server"></asp:PlaceHolder>
    <div id="tagSelector" name="tagSelector" style="visibility:hidden;position:absolute;">
    <label for="name" id="label_name" class="pts_name_input">Type any name or tag:</label><asp:TextBox
        ID="txtPhotoTagName" runat="server" cssclass="inputtext i_name" size="20"></asp:TextBox><span id="pts_choose_text">or choose a person:</span><div id="pts_userlist"><label><input onclick="PhotoTagSelector._checkboxClick(this, 685860805, 'Vincent Curry (Me)')" type="checkbox">Vincent Curry (Me)</label><br><hr><label><input onclick="PhotoTagSelector._checkboxClick(this, 777240342, 'Aaron Eudell')" type="checkbox">Aaron Eudell</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 1183070352, 'Abi Sirokh')" type="checkbox">Abi Sirokh</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 502365845, 'Adam Blemings')" type="checkbox">Adam Blemings</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 502297453, 'Adam Lovie')" type="checkbox">Adam Lovie</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 614678573, 'Alasdair MacLennan')" type="checkbox">Alasdair MacLennan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 617295582, 'Alastair Euan McCullough')" type="checkbox">Alastair Euan McCullough</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 718455373, 'Alex McGuckian')" type="checkbox">Alex McGuckian</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 200901554, 'Andrea Boden')" type="checkbox">Andrea Boden</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 805805031, 'Andrea Havill')" type="checkbox">Andrea Havill</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 513942353, 'Andrew Ashley')" type="checkbox">Andrew Ashley</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 757151788, 'Andrew Bootle-Wilbraham')" type="checkbox">Andrew Bootle-Wilbraham</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 807770296, 'Andrew Joint')" type="checkbox">Andrew Joint</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 503905057, 'Andrew Khan')" type="checkbox">Andrew Khan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 558117651, 'Andrew Wyeth')" type="checkbox">Andrew Wyeth</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 576066967, 'Andy Coster')" type="checkbox">Andy Coster</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 1184239384, 'Andy Funnell')" type="checkbox">Andy Funnell</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 549710613, 'Andy MacDonald')" type="checkbox">Andy MacDonald</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 591235950, 'Andy McEwen')" type="checkbox">Andy McEwen</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 556074457, 'Andy Portsmore')" type="checkbox">Andy Portsmore</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 713687628, 'Andy Preston')" type="checkbox">Andy Preston</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 506411729, 'Andy Smith')" type="checkbox">Andy Smith</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 651917226, 'Andy Young')" type="checkbox">Andy Young</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 517456276, 'Anna Miles')" type="checkbox">Anna Miles</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 504833038, 'Anna Reid')" type="checkbox">Anna Reid</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 517791038, 'Anthea Small')" type="checkbox">Anthea Small</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 517790584, 'Anthony Poole')" type="checkbox">Anthony Poole</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 515893420, 'Arran Peck')" type="checkbox">Arran Peck</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 526586463, 'Ashley Doran Northup')" type="checkbox">Ashley Doran Northup</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 732145996, 'Barrie Chalk')" type="checkbox">Barrie Chalk</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 536104186, 'Barry Tuck')" type="checkbox">Barry Tuck</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 509452712, 'Beca Picton')" type="checkbox">Beca Picton</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 651705159, 'Ben Marshall')" type="checkbox">Ben Marshall</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 523062142, 'Ben Strange')" type="checkbox">Ben Strange</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 554683071, 'Ben-oni Le Roux')" type="checkbox">Ben-oni Le Roux</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 515340985, 'Beth Pearsons')" type="checkbox">Beth Pearsons</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 760403781, 'Beverley Griffiths')" type="checkbox">Beverley Griffiths</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 655978589, 'Bindi Dholakia')" type="checkbox">Bindi Dholakia</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 538842029, 'Brea Millen')" type="checkbox">Brea Millen</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 504599796, 'Brenden Kent')" type="checkbox">Brenden Kent</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 590221173, 'Bryn Morgan')" type="checkbox">Bryn Morgan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 535621282, 'Carinna Castriotis')" type="checkbox">Carinna Castriotis</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 602602641, 'Caroline Hyland')" type="checkbox">Caroline Hyland</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 580267680, 'Catherine Stead')" type="checkbox">Catherine Stead</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 716167974, 'Celine Rojon')" type="checkbox">Celine Rojon</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 742000618, 'Charles Daw')" type="checkbox">Charles Daw</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 511930468, 'Charlie Cooper')" type="checkbox">Charlie Cooper</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 589940443, 'Charlotte Conrad')" type="checkbox">Charlotte Conrad</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 688501797, 'Charlotte Scott- Borchardt')" type="checkbox">Charlotte Scott- Borchardt</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 754015900, 'Choy Lau')" type="checkbox">Choy Lau</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 705051433, 'Chris Duncan')" type="checkbox">Chris Duncan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 613595332, 'Chris Gabbett')" type="checkbox">Chris Gabbett</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 659190442, 'Chris Gliddon')" type="checkbox">Chris Gliddon</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 697240297, 'Chris King')" type="checkbox">Chris King</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 592685133, 'Chris Middlemiss')" type="checkbox">Chris Middlemiss</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 712557135, 'Chris Upham')" type="checkbox">Chris Upham</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 621712150, 'Claire Taylor')" type="checkbox">Claire Taylor</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 824470500, 'Clare Major')" type="checkbox">Clare Major</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 719295181, 'Clifford Street')" type="checkbox">Clifford Street</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 626855403, 'Colin Manson')" type="checkbox">Colin Manson</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 596429830, 'Cress Fotherly')" type="checkbox">Cress Fotherly</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 530131618, 'Damien Watson')" type="checkbox">Damien Watson</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 517950272, 'Daniel Beaumont')" type="checkbox">Daniel Beaumont</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 1177980749, 'David Burroughes')" type="checkbox">David Burroughes</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 1342564086, 'David Christopher')" type="checkbox">David Christopher</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 720408571, 'David Engwell')" type="checkbox">David Engwell</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 644510878, 'David John Rhymes')" type="checkbox">David John Rhymes</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 537312081, 'David Ralson')" type="checkbox">David Ralson</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 524803968, 'David Rowlands')" type="checkbox">David Rowlands</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 618560057, 'David-roger Smith')" type="checkbox">David-roger Smith</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 814710503, 'Denise McAvinia')" type="checkbox">Denise McAvinia</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 586960840, 'Dirk Schwaebisch')" type="checkbox">Dirk Schwaebisch</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 504654229, 'Doug Rodman')" type="checkbox">Doug Rodman</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 506597092, 'Duncan Jennings')" type="checkbox">Duncan Jennings</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 502426984, 'Duncan Traynor')" type="checkbox">Duncan Traynor</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 1431343420, 'Eamon Nolan')" type="checkbox">Eamon Nolan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 515772024, 'Ed Dorling')" type="checkbox">Ed Dorling</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 505602524, 'Ed Mitchell')" type="checkbox">Ed Mitchell</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 677194852, 'Ed Schwalbe')" type="checkbox">Ed Schwalbe</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 683471646, 'Edward Cadwallader')" type="checkbox">Edward Cadwallader</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 564690477, 'Elliot James Whitham')" type="checkbox">Elliot James Whitham</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 570560288, 'Emile Hörr')" type="checkbox">Emile Hörr</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 701195645, 'Ewan Jeffrey')" type="checkbox">Ewan Jeffrey</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 513549903, 'Famous PMAC')" type="checkbox">Famous PMAC</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 711281460, 'Farakh Masood')" type="checkbox">Farakh Masood</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 568305581, 'Fay Swinton-Berry')" type="checkbox">Fay Swinton-Berry</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 515550142, 'Fintan Quinn')" type="checkbox">Fintan Quinn</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 789271461, 'Fumiaki Ito')" type="checkbox">Fumiaki Ito</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 898570530, 'Gabriele Frederick')" type="checkbox">Gabriele Frederick</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 856560203, 'Gareth Brown')" type="checkbox">Gareth Brown</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 786985693, 'Gareth Williams-Gardner')" type="checkbox">Gareth Williams-Gardner</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 721506945, 'Gavin Fox')" type="checkbox">Gavin Fox</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 533913083, 'Georgina McWhirter')" type="checkbox">Georgina McWhirter</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 669376158, 'Geraint Morgan')" type="checkbox">Geraint Morgan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 567937108, 'Gordon Bonifacio')" type="checkbox">Gordon Bonifacio</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 669262171, 'Gordon Dorricott')" type="checkbox">Gordon Dorricott</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 508270362, 'Grace Wallace')" type="checkbox">Grace Wallace</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 829835248, 'Graham Gandy')" type="checkbox">Graham Gandy</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 683051998, 'Graham Holden')" type="checkbox">Graham Holden</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 713191769, 'Graham Wilson')" type="checkbox">Graham Wilson</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 730305212, 'Gregory Young')" type="checkbox">Gregory Young</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 223301924, 'Hannah Savory')" type="checkbox">Hannah Savory</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 716253171, 'Hayley McClelland')" type="checkbox">Hayley McClelland</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 687389967, 'Hazel Grice')" type="checkbox">Hazel Grice</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 856635225, 'Heidi Oxley')" type="checkbox">Heidi Oxley</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 507099037, 'Helen Payne')" type="checkbox">Helen Payne</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 501311964, 'Henry Weeks')" type="checkbox">Henry Weeks</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 508651207, 'Horatio Smoot')" type="checkbox">Horatio Smoot</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 506816338, 'Hugo Watney')" type="checkbox">Hugo Watney</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 573870389, 'Ian Stokes-Rees')" type="checkbox">Ian Stokes-Rees</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 684955475, 'Ian Tucker')" type="checkbox">Ian Tucker</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 659853155, 'Irina Miksa')" type="checkbox">Irina Miksa</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 646391473, 'Isabelle Rojon')" type="checkbox">Isabelle Rojon</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 591179923, 'Ivan Slade')" type="checkbox">Ivan Slade</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 654406173, 'J Bryn Morgan')" type="checkbox">J Bryn Morgan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 512416404, 'Jack Rowland')" type="checkbox">Jack Rowland</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 756829866, 'James Abbott')" type="checkbox">James Abbott</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 701420875, 'James Brown')" type="checkbox">James Brown</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 578861604, 'James Cure')" type="checkbox">James Cure</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 902935690, 'James Debenham')" type="checkbox">James Debenham</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 504629090, 'James Hayden')" type="checkbox">James Hayden</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 776680485, 'James Ryan')" type="checkbox">James Ryan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 725277106, 'James Wilson')" type="checkbox">James Wilson</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 525852007, 'Jamie Sharp')" type="checkbox">Jamie Sharp</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 1137024383, 'Jason Knight')" type="checkbox">Jason Knight</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 578601290, 'Jennifer Monkman')" type="checkbox">Jennifer Monkman</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 855665590, 'Jenny Burns')" type="checkbox">Jenny Burns</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 627031609, 'Jessica Manson')" type="checkbox">Jessica Manson</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 595597212, 'Joe Stych')" type="checkbox">Joe Stych</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 514893039, 'John Curry')" type="checkbox">John Curry</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 36904603, 'John Kendall')" type="checkbox">John Kendall</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 804935723, 'Johnny Maher')" type="checkbox">Johnny Maher</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 504650314, 'Jon Clarke')" type="checkbox">Jon Clarke</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 589670006, 'Jon Guy')" type="checkbox">Jon Guy</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 644835511, 'Jon Hilditch')" type="checkbox">Jon Hilditch</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 564639991, 'Jonathan Roome')" type="checkbox">Jonathan Roome</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 222408524, 'Jonathan Underwood')" type="checkbox">Jonathan Underwood</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 653130683, 'Jonny Adamson')" type="checkbox">Jonny Adamson</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 505708394, 'Jonny Olley')" type="checkbox">Jonny Olley</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 686323803, 'Julia Sutherland')" type="checkbox">Julia Sutherland</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 639930623, 'Kai Blankley')" type="checkbox">Kai Blankley</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 511695733, 'Kate Henville')" type="checkbox">Kate Henville</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 570146770, 'Katerina Bairaktari')" type="checkbox">Katerina Bairaktari</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 525181891, 'Katherine Quigley')" type="checkbox">Katherine Quigley</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 563236050, 'Kathleen Hicks')" type="checkbox">Kathleen Hicks</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 502219035, 'Kenny MacLennan')" type="checkbox">Kenny MacLennan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 614096656, 'Kieron O\'Connor')" type="checkbox">Kieron O'Connor</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 693885611, 'Kimberly Whattler')" type="checkbox">Kimberly Whattler</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 223411675, 'Kirsty Protherough')" type="checkbox">Kirsty Protherough</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 665445267, 'Laura Fidler')" type="checkbox">Laura Fidler</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 576500867, 'Laura Skipper')" type="checkbox">Laura Skipper</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 634314465, 'Laurence Mallinson')" type="checkbox">Laurence Mallinson</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 609260765, 'Lee Gibson')" type="checkbox">Lee Gibson</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 729574502, 'Leila Jackson')" type="checkbox">Leila Jackson</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 654620925, 'Lezanne Jansen')" type="checkbox">Lezanne Jansen</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 737860343, 'Lindsey Brown')" type="checkbox">Lindsey Brown</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 700894777, 'Lorraine Olley')" type="checkbox">Lorraine Olley</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 733038660, 'Lou Clements')" type="checkbox">Lou Clements</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 743930719, 'Lucy Cooper')" type="checkbox">Lucy Cooper</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 517787971, 'Lucy Oxley')" type="checkbox">Lucy Oxley</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 748445606, 'Luke Jones')" type="checkbox">Luke Jones</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 658646344, 'Luke Ramsdale')" type="checkbox">Luke Ramsdale</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 548375167, 'Magda Jedraszczyk')" type="checkbox">Magda Jedraszczyk</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 625920927, 'Magnus MacLennan')" type="checkbox">Magnus MacLennan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 691526279, 'Marc Rodriguez')" type="checkbox">Marc Rodriguez</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 555423265, 'Marcus Idle')" type="checkbox">Marcus Idle</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 718155183, 'Marcus McCluggage')" type="checkbox">Marcus McCluggage</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 616456382, 'Marek Izmajlowicz')" type="checkbox">Marek Izmajlowicz</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 805655486, 'Marian Foley')" type="checkbox">Marian Foley</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 877315692, 'Marika Edelsten')" type="checkbox">Marika Edelsten</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 846990443, 'Martin Jackson')" type="checkbox">Martin Jackson</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 511694927, 'Martin O\'Callaghan')" type="checkbox">Martin O'Callaghan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 500207896, 'Martin Vivian Pearse')" type="checkbox">Martin Vivian Pearse</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 712495346, 'Matt Dowse')" type="checkbox">Matt Dowse</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 199709807, 'Matt T')" type="checkbox">Matt T</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 508213722, 'Matt Terry')" type="checkbox">Matt Terry</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 886280150, 'Meg Fletcher')" type="checkbox">Meg Fletcher</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 1386847010, 'Michael Walker')" type="checkbox">Michael Walker</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 36904257, 'Michal Stefan Zaremba-Tymieniecki')" type="checkbox">Michal Stefan Zaremba-Tymieniecki</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 723106850, 'Mike Allen')" type="checkbox">Mike Allen</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 720065423, 'Mike Castle')" type="checkbox">Mike Castle</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 675775319, 'Nadeem Hussain')" type="checkbox">Nadeem Hussain</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 512278482, 'Natasha Tucker')" type="checkbox">Natasha Tucker</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 199705407, 'Nathan O\'Grady')" type="checkbox">Nathan O'Grady</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 1190725345, 'Neil Giffin')" type="checkbox">Neil Giffin</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 586012904, 'Neil Wallace')" type="checkbox">Neil Wallace</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 731550496, 'Niall Lear')" type="checkbox">Niall Lear</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 512262591, 'Nick Allen-Perry')" type="checkbox">Nick Allen-Perry</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 507875152, 'Nick Duncan')" type="checkbox">Nick Duncan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 582351860, 'Nick Dutnall')" type="checkbox">Nick Dutnall</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 535751402, 'Nick Funnell')" type="checkbox">Nick Funnell</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 584945148, 'Nick Hall')" type="checkbox">Nick Hall</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 690171351, 'Nick Harlock')" type="checkbox">Nick Harlock</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 518587842, 'Nigel Rowles')" type="checkbox">Nigel Rowles</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 582801219, 'Noel Anderson')" type="checkbox">Noel Anderson</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 856950175, 'Nuno Bernardes')" type="checkbox">Nuno Bernardes</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 500130472, 'Oli Hire')" type="checkbox">Oli Hire</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 805840649, 'Paul H McKenna')" type="checkbox">Paul H McKenna</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 627522679, 'Paul Hughes')" type="checkbox">Paul Hughes</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 671030843, 'Peter Bacon')" type="checkbox">Peter Bacon</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 684795032, 'Peter Bryden')" type="checkbox">Peter Bryden</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 885925012, 'Peter Crompton')" type="checkbox">Peter Crompton</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 865120463, 'Peter Curry')" type="checkbox">Peter Curry</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 811045256, 'Peter Dalton')" type="checkbox">Peter Dalton</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 721760124, 'Peter Maitland')" type="checkbox">Peter Maitland</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 568741417, 'Philippa Rae')" type="checkbox">Philippa Rae</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 521930403, 'Phill Hall')" type="checkbox">Phill Hall</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 597271396, 'Pippa Rollitt')" type="checkbox">Pippa Rollitt</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 559601448, 'Quinten Coetzer')" type="checkbox">Quinten Coetzer</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 547918618, 'Rebecca Burchmore')" type="checkbox">Rebecca Burchmore</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 663990010, 'Rhona McKenzie')" type="checkbox">Rhona McKenzie</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 567006020, 'Richard \'Bob\' Hoskins')" type="checkbox">Richard 'Bob' Hoskins</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 574356083, 'Richard Allen')" type="checkbox">Richard Allen</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 596680773, 'Richard Crompton')" type="checkbox">Richard Crompton</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 553150822, 'Richard Gaylord')" type="checkbox">Richard Gaylord</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 508531000, 'Richard Mahony')" type="checkbox">Richard Mahony</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 503963772, 'Richard McKeown')" type="checkbox">Richard McKeown</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 655230292, 'Richard Onslow')" type="checkbox">Richard Onslow</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 733555499, 'Richard Pilgrim')" type="checkbox">Richard Pilgrim</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 1107174687, 'Richard Warburton')" type="checkbox">Richard Warburton</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 686275448, 'Rob Auld')" type="checkbox">Rob Auld</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 628385377, 'Rob De Bresser')" type="checkbox">Rob De Bresser</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 731025830, 'Rob Sedman')" type="checkbox">Rob Sedman</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 681022652, 'Rob Sheldon')" type="checkbox">Rob Sheldon</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 502326422, 'Robert Hume')" type="checkbox">Robert Hume</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 675861114, 'Robin Gregson')" type="checkbox">Robin Gregson</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 862300362, 'Robson Miller')" type="checkbox">Robson Miller</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 533496264, 'Ros Jennings')" type="checkbox">Ros Jennings</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 502117623, 'Ross Alexander McClelland')" type="checkbox">Ross Alexander McClelland</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 577033384, 'Ross Burgess')" type="checkbox">Ross Burgess</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 674870044, 'Roy Maxwell Turner')" type="checkbox">Roy Maxwell Turner</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 535760951, 'Ruaidhri Garvey')" type="checkbox">Ruaidhri Garvey</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 36804772, 'Rupert Try')" type="checkbox">Rupert Try</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 816145721, 'Russell Forster')" type="checkbox">Russell Forster</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 506976606, 'Sabine Schwaebisch')" type="checkbox">Sabine Schwaebisch</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 605391469, 'Sally Amberton')" type="checkbox">Sally Amberton</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 745800236, 'Sam Stead')" type="checkbox">Sam Stead</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 541162418, 'Sam Sweeney')" type="checkbox">Sam Sweeney</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 193107962, 'Sam Ward')" type="checkbox">Sam Ward</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 565685304, 'Sara Alainna McKechnie')" type="checkbox">Sara Alainna McKechnie</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 628736926, 'Sara Kidd')" type="checkbox">Sara Kidd</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 666899847, 'Sarah Engwell')" type="checkbox">Sarah Engwell</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 824902, 'Sarah Heaney')" type="checkbox">Sarah Heaney</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 596765259, 'Sarah McLellan')" type="checkbox">Sarah McLellan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 563587196, 'Sarah Wallace')" type="checkbox">Sarah Wallace</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 688152847, 'Sarah-Jane Abbott')" type="checkbox">Sarah-Jane Abbott</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 633371034, 'Sass Willis')" type="checkbox">Sass Willis</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 591380343, 'Scott Chatterton')" type="checkbox">Scott Chatterton</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 576746900, 'Seda Danaci')" type="checkbox">Seda Danaci</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 656066151, 'Shaun Justice')" type="checkbox">Shaun Justice</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 547317110, 'Silpa Gohil')" type="checkbox">Silpa Gohil</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 736667844, 'Simon Burnand')" type="checkbox">Simon Burnand</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 710866882, 'Simon Goldsmith')" type="checkbox">Simon Goldsmith</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 726686275, 'Somine Rabe')" type="checkbox">Somine Rabe</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 755338596, 'Sonya Jane Bennion')" type="checkbox">Sonya Jane Bennion</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 738355547, 'Stephan Becker')" type="checkbox">Stephan Becker</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 643351459, 'Stephen McKenna')" type="checkbox">Stephen McKenna</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 882575436, 'Steve Kearns')" type="checkbox">Steve Kearns</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 623255037, 'Steve Mckinstry')" type="checkbox">Steve Mckinstry</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 727104466, 'Steven Trenerry')" type="checkbox">Steven Trenerry</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 728987929, 'Sue Morgan')" type="checkbox">Sue Morgan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 508561466, 'Susanna Rhoslyn Ellis')" type="checkbox">Susanna Rhoslyn Ellis</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 579605355, 'Susie Lanson Carr')" type="checkbox">Susie Lanson Carr</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 625315730, 'Suzanne Barnes')" type="checkbox">Suzanne Barnes</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 606175481, 'Tamara Foster')" type="checkbox">Tamara Foster</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 509968089, 'Tara Kent')" type="checkbox">Tara Kent</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 674931580, 'Tarz Heffernan')" type="checkbox">Tarz Heffernan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 530159282, 'Tim Burchmore')" type="checkbox">Tim Burchmore</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 501832551, 'Tom Cuff-Burnett')" type="checkbox">Tom Cuff-Burnett</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 673056852, 'Tom Digweed')" type="checkbox">Tom Digweed</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 677428022, 'Tom Pettit')" type="checkbox">Tom Pettit</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 516989151, 'Toni McCloggan')" type="checkbox">Toni McCloggan</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 506216985, 'Tony Paul')" type="checkbox">Tony Paul</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 655490152, 'Tracey Clare Dunlop')" type="checkbox">Tracey Clare Dunlop</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 587075760, 'Tyrone Warburton')" type="checkbox">Tyrone Warburton</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 539105552, 'Verity Skinner')" type="checkbox">Verity Skinner</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 667251509, 'Will Day')" type="checkbox">Will Day</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 759835342, 'Will Houston')" type="checkbox">Will Houston</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 199702870, 'William Turner')" type="checkbox">William Turner</label><br><label><input onclick="PhotoTagSelector._checkboxClick(this, 684722311, 'Zachary Watson')" type="checkbox">Zachary Watson</label><br></div><div id="pts_invite_section" style="display: none;">Enter <span id="pts_invite_name"></span>'s email address. We'll send a link to this photo and add them to your friends list.<div><label for="pts_invite_email" id="label_pts_invite_email">Email:</label><input class="inputtext" id="pts_invite_email" name="pts_invite_email" value="" type="text"></div></div><div class="buttons">
        <asp:Button ID="tag" runat="server" Text="Tag" CssClass="inputbutton" name="tag" OnClick="tag_Click" /><input class="inputbutton inputaux" id="cancel" name="cancel" value="Cancel" type="button"  onclick="javascript:ClearTagging();return false;"></div></div>
    </form>
</body>
</html>
