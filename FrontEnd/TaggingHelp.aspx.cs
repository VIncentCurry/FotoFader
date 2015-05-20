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
using PhotoTaggerOM;

public partial class FullHelp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString[Constants.SetPageForTaggingQueryString] == Constants.trueStringValue)
            Page.ClientScript.RegisterStartupScript(this.GetType(), "SetPageForTagging", "SetPageForTagging(true);", true);

        chkShowInstructionsOnStartFotofade.Checked = FacebookUser.LoggedInUser().ShowHelpOnStartFotoFading;
    }
}
