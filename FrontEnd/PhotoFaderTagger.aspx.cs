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

public partial class PhotoFaderTagger : BasePage
{
    FacebookPhoto photo;
    const string usemapKey = "usemap";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ClientScript.RegisterStartupScript(GetType(), "ScriptPhotoID", "var PhotoTagID = '" + imgPhotoToTag.ClientID + "';", true);
            ClientScript.RegisterStartupScript(GetType(), "FotoFaderInstructionsID", "var FotoFaderInstructionsID = '" + FotoFaderInstructions.ClientID + "';", true);
            ClientScript.RegisterStartupScript(GetType(), "TagBtnID", "var txtBtnTagID = '" + btnTag.ClientID + "';", true);
            ClientScript.RegisterStartupScript(GetType(), "showHelpOnFotoFadeID", "var showHelpOnFotoFadeID = '" + chkShowFotofadeOnStartFotoFading.ClientID + "';", true);
            //btnTag.Attributes.Add("onclick", "ValidateUserSelected();");
            btnTag.OnClientClick = "if(ValidateUserSelected() == false) {event.returnValue=false; return false; }else{ return true;};";
            //btnTag.Attributes.Add("onclick", "");
            imgPhotoToTag.Attributes.Add("name", imgPhotoToTag.ClientID);
            imgPhotoToTag.Attributes.Add("onmousemove", "outline(event, '" + txtCoordinates.ClientID + "', 'PhotoToTag')");

            ClientScript.RegisterStartupScript(GetType(), "ScriptTxtCoordinatesID", "txtCooridnatesID = '" + txtCoordinates.ClientID + "';", true);
            photo = FacebookPhoto.PhotoByID(Convert.ToInt64(Request.QueryString[Constants.PhotoIDQueryString]));

            imgPhotoToTag.ImageUrl = photo.BigSrc;

            if (photo.Caption.Length > 0)
            {
                ltlPhotoTitle.Text = "<span class=\"fotoFaderHeader2\">" + photo.Caption + @"</span>
<br />";
                this.Title = photo.Caption;
            }
            else
            {
                this.Title = "fotofader";
            }

            hlBackToAlbum.NavigateUrl = "albumpage.aspx?" + Constants.AlbumIDQueryString + "=" + photo.AlbumID.ToString();
            hlBackToAlbum.Text = "Back to " + photo.Album.Name + " album";

            hlBackToAllAlbums.NavigateUrl = "albums.aspx?" + Constants.FriendIDQueryString + "=" + photo.Album.OwnerID.ToString();
            hlBackToAllAlbums.Text = "Back to " + photo.Album.Owner.Name + "'s Albums";

            string idsList = Request.QueryString[Constants.PhotoListIDQueryString];

            if (idsList != null && idsList.Length > 0)
            {
                hlNextPhoto.NavigateUrl = "photofadertagger.aspx?" +
                    Constants.PhotoIDQueryString + "=" + General.NextID(idsList, Request.QueryString[Constants.PhotoIDQueryString]) + "&" +
                    Constants.PhotoListIDQueryString + "=" + idsList;

                hlPreviousPhoto.NavigateUrl = "photofadertagger.aspx?" +
                    Constants.PhotoIDQueryString + "=" + General.PreviousID(idsList, Request.QueryString[Constants.PhotoIDQueryString]) + "&" +
                    Constants.PhotoListIDQueryString + "=" + idsList;
            }
            else
            {
                hlNextPhoto.Visible = false;
                hlPreviousPhoto.Visible = false;
            }

            AddTagsToPage(photo.Tags, true);

            if (FacebookUser.LoggedInUser().ShowHelpOnStartFotoFading)
            {
                hlFotoFadeThisPhoto.NavigateUrl = "TaggingHelp.aspx?" + Constants.SetPageForTaggingQueryString + "=" + Constants.trueStringValue;
                hlFotoFadeThisPhoto.CssClass = "lbOn";
            }

            if (!IsPostBack)
            {
                FacebookUser loggedInUser = FacebookUser.LoggedInUser();

                chkShowFotofadeOnStartFotoFading.Checked = loggedInUser.ShowHelpOnStartFotoFading;

                rdlFriends.Items.Insert(0, new ListItem(loggedInUser.Name + " (Me)", loggedInUser.ID.ToString()));

                foreach (FacebookUser friend in loggedInUser.Friends)
                {
                    rdlFriends.Items.Add(new ListItem(friend.Name, friend.ID.ToString()));
                }
            }
        }
        catch (facebook.Utility.FacebookException facebookException)
        {
            General.RedirectSessionInvalidExceptions(facebookException);
        }
    }

    private void AddTagToPage(PhotoTag photoTag)
    {
        List<PhotoTag> photoTags = new List<PhotoTag>();
        photoTags.Add(photoTag);

        AddTagsToPage(photoTags, false);
    }

    private void AddTagsToPage(List<PhotoTag> photoTags, bool addRemoveLinks)
    {
        string mapArea = "";

        if(addRemoveLinks)
            mapArea = @"<map name=""PhotoTags"">";

        foreach (PhotoTag tag in photoTags)
            mapArea += @"<area shape=polygon coords=""" + tag.PolygonCoordinates + @""" onMouseOver=""javascript:showName(event, '" + tag.DisplayName + @"');highlightPerson('" + tag.BackgroundImageFileName + @"');"" onMouseOut=""javascript:hideName(event, '" + tag.DisplayName + @"');fadePerson();"" />";
        
        mapArea += @"</map>";

        imgPhotoToTag.Attributes.Add(usemapKey, "#PhotoTags");

        if (addRemoveLinks)
            ltlMapArea.Text = mapArea;
        else
            ltlMapArea.Text = ltlMapArea.Text.Remove(ltlMapArea.Text.IndexOf(@"</map>")) + mapArea;

        foreach (PhotoTag tag in photoTags)
        {
            HyperLink hlShowPerson = new HyperLink();

            if (tag.FacebookTagUserID == 0)
                hlShowPerson.NavigateUrl = "";
            else
                hlShowPerson.NavigateUrl = "http://www.facebook.com/profile.php?id=" + tag.FacebookTagUserID.ToString();

            hlShowPerson.Attributes.Add("onMouseOver", "highlightPerson('" + tag.BackgroundImageFileName + "');");
            hlShowPerson.Attributes.Add("onMouseOut", "fadePerson();");
            hlShowPerson.Text = tag.DisplayName;

            if (phPeopleLinks.Controls.Count > 0)
            {
                Label comma = new Label();
                comma.Text = ", ";
                phPeopleLinks.Controls.Add(comma);
            }

            phPeopleLinks.Controls.Add(hlShowPerson);

            //download the image to the page
            Image tagImage = new Image();
            tagImage.ImageUrl = tag.BackgroundImageFileName;
            tagImage.Style.Add("visibility", "hidden");
            phFasterLoadingImages.Controls.Add(tagImage);


            if (addRemoveLinks && (tag.PhotoIsLoggedOnUsers || tag.TagIsLoggedOnUser || tag.TaggerIsLoggedInUser))
            {
                Label lblOpenBracket = new Label();
                lblOpenBracket.Text = " (";
                phPeopleLinks.Controls.Add(lblOpenBracket);

                LinkButton lbRemoveTag = new LinkButton();
                lbRemoveTag.Command += new CommandEventHandler(RemoveTag);
                lbRemoveTag.CommandArgument = tag.ID.ToString();
                lbRemoveTag.Text = "remove tag";
                lbRemoveTag.ID = tag.ID.ToString();
                
                phPeopleLinks.Controls.Add(lbRemoveTag);

                Label lblCloseBracket = new Label();
                lblCloseBracket.Text = ")";
                phPeopleLinks.Controls.Add(lblCloseBracket);

            }
        }
    }

    protected void TagFriend()
    {
        bool repeatID = false;

        foreach (PhotoTag tag in photo.Tags)
        {
            if (Convert.ToInt64(rdlFriends.SelectedValue) == tag.FacebookTagUserID)
                repeatID = true;
        }

        if (!repeatID)
        {
            PhotoTag personTagged = new PhotoTag(Convert.ToInt64(rdlFriends.SelectedValue), "", photo.ID, txtCoordinates.Text);
            AddTagToPage(personTagged);
            txtCoordinates.Text = "";
        }
        else
        {
            ClientScript.RegisterStartupScript(GetType(), "IDAlreadyUsed", "alert('You have already tagged " + rdlFriends.SelectedItem + ". Please remove the original if you wish to retag.');", true);
            txtCoordinates.Text = "";
        }
        KeepInstructionsVisibleAndTaggingModeOn();
        rdlFriends.SelectedIndex = -1;
        txtPhotoTagName.Text = "";
    }

    protected void KeepInstructionsVisibleAndTaggingModeOn()
    {
        ClientScript.RegisterStartupScript(GetType(), "KeepInstructionsVisible", "SetPageForTagging(true);", true);
    }

    protected void rdlFriends_DataBound(object sender, EventArgs e)
    {
        FacebookUser loggedInUser = FacebookUser.LoggedInUser();

        rdlFriends.Items.Insert(0, new ListItem(loggedInUser.Name + " (Me)", loggedInUser.ID.ToString()));
    }

    protected void btnTag_Click(object sender, EventArgs e)
    {
        if (rdlFriends.SelectedIndex > -1 || txtPhotoTagName.Text.Length > 0)
        {
            if (rdlFriends.SelectedIndex != -1)
                TagFriend();
            else
            {
                bool repeatName = false;

                foreach (PhotoTag tag in photo.Tags)
                {
                    if (txtPhotoTagName.Text == tag.Tag)
                        repeatName = true;
                }

                if (!repeatName)
                {
                    PhotoTag namedPersonTagged = new PhotoTag(0, txtPhotoTagName.Text, photo.ID, txtCoordinates.Text);
                    AddTagToPage(namedPersonTagged);
                    txtCoordinates.Text = "";
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "NameAlreadyUsed", "alert('You have already tagged the name " + txtPhotoTagName.Text + " in this photo. Please delete the original if you wish to retag this person.');", true);
                    txtCoordinates.Text = "";
                }

                KeepInstructionsVisibleAndTaggingModeOn();
                rdlFriends.SelectedIndex = -1;
                txtPhotoTagName.Text = "";
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "SetPageForTagging", "SetPageForTagging(true);", true);
            ClientScript.RegisterStartupScript(this.GetType(), "RedrawLine", "RedrawTaggingLine();", true);
            ClientScript.RegisterStartupScript(this.GetType(), "InformUser", "alert('You must select either a friend or enter their name before tagging.');", true);
        }
    }

    protected void RemoveTag(object sender, CommandEventArgs e)
    {
        PhotoTag.Delete(new Guid(e.CommandArgument.ToString()));

        //Clear the tags and then reload them...this is done as the RemoveTag event is fired after th
        imgPhotoToTag.Attributes.Remove(usemapKey);
        ltlMapArea.Text = "";
        phFasterLoadingImages.Controls.Clear();
        phPeopleLinks.Controls.Clear();

        photo = FacebookPhoto.PhotoByID(Convert.ToInt64(Request.QueryString[Constants.PhotoIDQueryString]));
        AddTagsToPage(photo.Tags, true);
    }


    protected void btnDoneTagging_Click(object sender, EventArgs e)
    {
        string templateID;

        StoryPublisher publisher = (StoryPublisher)Session[PhotoTaggerOM.Constants.storyPublisherSessionKey];

        if (publisher != null)
        {
            if (publisher.TargetIDsAsString().Length == 0)
                templateID = Constants.NoTargetsTemplateBundleID;
            else
                templateID = Constants.TargetsTemplateBundleID;

            //Get all the targets apart from the first one - we are going to include these in the message
            string whereClause = publisher.TargetIDsAsString(",", "'");
            string additionalUsers = "";

            if (whereClause.IndexOf(",") > -1)
            {
                whereClause = whereClause.Substring(whereClause.IndexOf(",") + 1);
                List<FacebookUser> additionalTargets = FacebookUser.UsersByIDs(whereClause);

                foreach (FacebookUser additionalTarget in additionalTargets)
                {
                    if (additionalUsers.Length == 0)
                        additionalUsers = additionalTarget.Name;
                    else
                        additionalUsers += ", " + additionalTarget.Name;
                }

                additionalUsers = "Also in this photo: " + additionalUsers;
            }



            ClientScript.RegisterClientScriptBlock(
                this.GetType(),
                "FeedPrompter",
                "function callback(){}FB_RequireFeatures([\"XFBML\"],function(){ FB.Facebook.init('" + Constants.FaceBookApplicationKey + "', 'xd_receiver.htm', null);FB.Connect.showFeedDialog(" + templateID + ", {" + publisher.TemplateData + "}, [" + publisher.TargetIDsAsString(",", "") + "],'" + additionalUsers + "',null, FB.RequireConnect.doNotRequire, callback);});",
                true);

            Session[PhotoTaggerOM.Constants.storyPublisherSessionKey] = null;
        }


    }

    protected void cstSelectPerson_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = (rdlFriends.SelectedIndex > -1 || txtPhotoTagName.Text.Length > 0);
    }

    protected void chkShowFotofadeOnStartFotoFading_CheckedChanged(object sender, EventArgs e)
    {
        FacebookUser loggedInUser = FacebookUser.LoggedInUser();

        loggedInUser.ShowHelpOnStartFotoFading = chkShowFotofadeOnStartFotoFading.Checked;
        loggedInUser.Save();
    }
}
