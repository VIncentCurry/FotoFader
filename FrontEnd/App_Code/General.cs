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
using System.Web.Configuration;

/// <summary>
/// Summary description for General
/// </summary>
public class General
{
	public General()
	{
		//
		// TODO: Add constructor logic here
		//
	}
        public static string NextID(string idList, string currentID)
        {

            int startIndex = idList.IndexOf(currentID + Constants.idListSeperator) + (currentID+Constants.idListSeperator).Length;

            if (startIndex == (currentID + Constants.idListSeperator).Length - 1)
                startIndex = 0;

            int endIndex = idList.IndexOf(Constants.idListSeperator, startIndex);

            if (endIndex == -1)
                endIndex = idList.Length;
            
            return idList.Substring(startIndex, endIndex-startIndex);
        }

        public static string PreviousID(string idList, string currentID)
        {
            int endIndex = idList.IndexOf(Constants.idListSeperator + currentID);

            if (endIndex == -1)//we won't find currentid proceeded by a seperator if it is the first id in the list
                endIndex = idList.Length;

            int startIndex = idList.LastIndexOf(Constants.idListSeperator, endIndex - 1) + 1;

            return idList.Substring(startIndex, endIndex - startIndex);
        }

        public static void RedirectSessionInvalidExceptions(facebook.Utility.FacebookException facebookException)
        {
            if (facebookException is facebook.Utility.FacebookException)
            {
                if (facebookException.Message == "Session key invalid or no longer valid")
                {
                    HttpContext.Current.Server.ClearError();
                    HttpContext.Current.Session[PhotoTaggerOM.Constants.facebookSessionKeyIndex] = "";
                    HttpContext.Current.Response.Redirect(@"http://www.Facebook.com/login.php?api_key=" + PhotoTaggerOM.Constants.FacebookApplicationKey + @"&v=1.0&" + Constants.facebookNextPage + "=" + HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsoluteUri));
                }
            }

            throw facebookException;
        }
}

public class Constants
{
    public static string AlbumIDQueryString = "albumid";
    public static string PhotoIDQueryString = "photoid";
    public static string FriendIDQueryString = "FriendID";
    public static string PhotoListIDQueryString = "ps";
    public static string idListSeperator = "b";
    public static string facebookNextPage = "next"; //this is Facebook defined, so should not be changed.
    public static string showHelpSession = "help";
    public static string trueStringValue = "true";
    public static string falseStringValue = "false";
    public static string SetPageForTaggingQueryString = "spfts";

    public static string FacebookQuestionImage = WebConfigurationManager.AppSettings["FacebookQuestionImage"];
    public static string FaceBookApplicationKey = WebConfigurationManager.AppSettings["FacebookApplicationKey"];

    public static string TargetsTemplateBundleID = WebConfigurationManager.AppSettings["FotoTaggedTemplateBundleID"];
    public static string NoTargetsTemplateBundleID = WebConfigurationManager.AppSettings["FotoTaggedTemplateNoTargetsBundleID"];
}