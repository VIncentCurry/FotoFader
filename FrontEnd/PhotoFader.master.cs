using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using PhotoTaggerOM;

public partial class PhotoFacer : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FacebookUser loggedInUser = FacebookUser.LoggedInUser();

                chkShowHelpOnStartUp.Checked = loggedInUser.ShowHelpOnStartUp;

                ddlFriends.Items.Insert(0, new ListItem(loggedInUser.Name + " (Me)", loggedInUser.ID.ToString()));

                foreach (FacebookUser friend in loggedInUser.Friends)
                {
                    ddlFriends.Items.Add(new ListItem(friend.Name, friend.ID.ToString()));
                }
            }

            string pageFileName = System.IO.Path.GetFileName(HttpContext.Current.Request.FilePath).ToLower();

            PhotoTaggerOM.PageVisit.RecordPageVisit(PhotoTaggerOM.FacebookUser.LoggedInUser().ID, pageFileName);

            lblUserName.Text = PhotoTaggerOM.FacebookUser.LoggedInUser().Name;

            Page.ClientScript.RegisterStartupScript(this.GetType(), "showHelpOnStartUpID", "var showHelpOnStartUpID;showHelpOnStartUpID = '" + chkShowHelpOnStartUp.ClientID + "';", true);

            bool popupGeneralHelp = (Session[Constants.showHelpSession].ToString()==Constants.trueStringValue);

            if (pageFileName == "photofadertagger.aspx")
            {
                ltlHelp.Text = "<a href=\"tagginghelp.aspx\" class=\"lbOn\" onclick=\"javascript:return false;\">Help</a>";

                if (popupGeneralHelp)
                    ltlHelp.Text += "<a href=\"initialhelp.aspx\" class=\"lbOn\" id=\"defaultLightbox\" style='display:none'>Help</a>";
            }
            else
            {
                if (popupGeneralHelp)
                    ltlHelp.Text = "<a href=\"initialhelp.aspx\" class=\"lbOn\" id=\"defaultLightbox\" onclick=\"javascript:return false;\">Help</a>";
                else
                    ltlHelp.Text = "<a href=\"initialhelp.aspx\" class=\"lbOn\" onclick=\"javascript:return false;\">Help</a>";
            }

            Session[Constants.showHelpSession] = Constants.falseStringValue;

        }
        catch (facebook.Utility.FacebookException sessionException)
        {
            General.RedirectSessionInvalidExceptions(sessionException);
        }
    }
    protected void chkShowHelpOnStartUp_CheckedChanged(object sender, EventArgs e)
    {
        PhotoTaggerOM.FacebookUser.LoggedInUser().ShowHelpOnStartUp = chkShowHelpOnStartUp.Checked;
        PhotoTaggerOM.FacebookUser.LoggedInUser().Save();
    }
    /*protected void ddlFriends_DataBound(object sender, EventArgs e)
    {
        FacebookUser loggedInUser = FacebookUser.LoggedInUser();

        ddlFriends.Items.Insert(0, new ListItem(loggedInUser.Name + " (Me)", loggedInUser.ID.ToString()));
    }*/

    protected void btnViewFriends_Click(object sender, EventArgs e)
    {
        Response.Redirect("Albums.aspx?FriendID=" + ddlFriends.SelectedValue);
    }
}
