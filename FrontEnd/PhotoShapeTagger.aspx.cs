using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

using PhotoTaggerOM;

public partial class PhotoShapeTagger : System.Web.UI.Page
{
    List<PhotoTag> picturesPhotoTags;
    const string photoTagsObject = "schnarf";

    protected void Page_Load(object sender, EventArgs e)
    {
        picturesPhotoTags = new List<PhotoTag>();
        //picturesPhotoTags = PhotoTag.GetPhotoTagsByPhotoID("facebook");

        /*string mapArea;

        mapArea = @"<map name=""PhotoTags"">";
        foreach (PhotoTag tag in picturesPhotoTags)
            mapArea += @"<area shape=poly coords=""" + tag.PolygonCoordinates + @""" onMouseOver=""showname('" + tag.DisplayName + @"');"" onmouseout=""showname('');"">";
        mapArea += @"</map>";

        imgPhoto.Attributes.Add("usemap", "#PhotoTags");
        ltlMapArea.Text = mapArea;

        foreach (PhotoTag tag in picturesPhotoTags)
        {
            HyperLink hlShowPerson = new HyperLink();

            hlShowPerson.NavigateUrl = "";
            hlShowPerson.Attributes.Add("onMouseOver", "highlightPerson('" + tag.BackgroundImageFileName + "');");
            hlShowPerson.Attributes.Add("onMouseOut", "fadePerson();");
            hlShowPerson.Text = tag.DisplayName;

            phPeopleLinks.Controls.Add(hlShowPerson);
        }

        txtCoordinates.Text = "";*/

        //Session[photoTagsObject] = picturesPhotoTags;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        AddTagsToPage();
    }

    protected void tag_Click(object sender, EventArgs e)
    {
        //picturesPhotoTags = (List<PhotoTag>)Session[photoTagsObject];

        if(picturesPhotoTags==null)
            picturesPhotoTags = new List<PhotoTag>();
        
        //PhotoTag photoTag = new PhotoTag("", txtPhotoTagName.Text, "facebookID", txtCoordinates.Text);

        //picturesPhotoTags.Add(photoTag);

        /*string mapArea;

        mapArea = @"<map name=""PhotoTags"">";
        foreach(PhotoTag tag in picturesPhotoTags)
            mapArea += @"<area shape=poly coords=""" + tag.PolygonCoordinates + @""" onMouseOver=""showname('" + tag.DisplayName + @"');"" onmouseout=""showname('');"">";
        mapArea += @"</map>";

        imgPhoto.Attributes.Add("usemap", "#PhotoTags");
        ltlMapArea.Text = mapArea;

        foreach (PhotoTag tag in picturesPhotoTags)
        {
            HyperLink hlShowPerson = new HyperLink();

            hlShowPerson.NavigateUrl = "";
            hlShowPerson.Attributes.Add("onMouseOver", "highlightPerson('" + tag.BackgroundImageFileName + "');");
            hlShowPerson.Attributes.Add("onMouseOut", "fadePerson();");
            hlShowPerson.Text = tag.DisplayName;

            phPeopleLinks.Controls.Add(hlShowPerson);
        }

        txtCoordinates.Text = "";

        Session[photoTagsObject] = picturesPhotoTags;*/
    }

    private void AddTagsToPage()
    {
        string mapArea;

        mapArea = @"<map name=""PhotoTags"">";
        foreach (PhotoTag tag in picturesPhotoTags)
            mapArea += @"<area shape=poly coords=""" + tag.PolygonCoordinates + @""" onMouseOver=""showname('" + tag.DisplayName + @"');"" onmouseout=""showname('');"">";
        mapArea += @"</map>";

        imgPhoto.Attributes.Add("usemap", "#PhotoTags");
        ltlMapArea.Text = mapArea;

        foreach (PhotoTag tag in picturesPhotoTags)
        {
            HyperLink hlShowPerson = new HyperLink();

            hlShowPerson.NavigateUrl = "";
            hlShowPerson.Attributes.Add("onMouseOver", "highlightPerson('" + tag.BackgroundImageFileName + "');");
            hlShowPerson.Attributes.Add("onMouseOut", "fadePerson();");
            hlShowPerson.Text = tag.DisplayName;

            phPeopleLinks.Controls.Add(hlShowPerson);
        }

        txtCoordinates.Text = "";
    }
}
