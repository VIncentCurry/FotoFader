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

public partial class Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Server.GetLastError().GetType() == new PhotoTaggerOM.FacebookTimeOutException().GetType())
            lblErrorMessage.Text = "Hmmm... we're having problems getting data back from Facebook. Very annoying, can we suggest you try again later?";
    }
}
