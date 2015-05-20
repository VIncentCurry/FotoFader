using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ExceptionHandler;

/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : System.Web.UI.Page
{
	

    protected virtual void Page_Error(Object sender, EventArgs e)
    {
        /*Exception thrownException = Server.GetLastError();

        if (thrownException is facebook.Utility.FacebookException)
        {
            if (thrownException.Message == "Session key invalid or no longer valid")
            {
                Server.ClearError();
                Session[PhotoTaggerOM.Constants.facebookSessionKeyIndex] = "";
                Response.Redirect(@"http://www.Facebook.com/login.php?api_key=" + PhotoTaggerOM.Constants.FacebookApplicationKey + @"&v=1.0");
            }
        }*/
        ExceptionHandling.HandleException(Server.GetLastError());
    }
}
