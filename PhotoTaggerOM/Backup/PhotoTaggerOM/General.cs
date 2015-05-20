using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using facebook;
using System.Xml.Linq;

namespace PhotoTaggerOM
{
    public class Constants
    {
        public static string AdvancedPhotoTaggerConnectionString = WebConfigurationManager.ConnectionStrings["AdvancedPhotoTaggerDB"].ConnectionString;
        public static string BackgroundFileSaveLocation = WebConfigurationManager.AppSettings["BackgroundFileSaveLocation"];
        public static string BackgroundImagesHTTPPath = WebConfigurationManager.AppSettings["BackGroundImagesHTTPPath"];
        public static string FacebookApplicationKey = WebConfigurationManager.AppSettings["FacebookApplicationKey"];
        public static string FacebookSecretKey = WebConfigurationManager.AppSettings["FacebookSecret"];

        public const string facebookSessionKeyIndex = "Facebook_session_key";
        public const string storyPublisherSessionKey = "storyPublisher";
        public const string faceBookUserIDIndex = "Facebook_userId";

        public const string faceBookSessionObject = "facebookSession";
        public const string faceBookLoggedInUserSession = "userSession";

        public const string facebookPhotoSessionObjectPrefix = "ph";
        public const string facebookAlbumSessionObjectPrefix = "al";

        public static long FotoTaggedTemplateBundleID = Convert.ToInt64(WebConfigurationManager.AppSettings["FotoTaggedTemplateBundleID"]);
    }

    public class General
    {
        public static bool UserHasGrantedPublishPermission()
        {
            
            
            /*string hasAppPermission = PhotoTaggerFBService.PhotoTagFaceBookService.fql.query("select publish_stream from permissions where uid = " + PhotoTaggerFBService.PhotoTagFaceBookService.uid.ToString());

            XElement userdetails = XElement.Parse(hasAppPermission);*/

            XElement userdetails = PhotoTaggerFBService.ExecuteFQLReturnXElementData("select publish_stream from permissions where uid = " + PhotoTaggerFBService.PhotoTagFaceBookService.uid.ToString());
            
            foreach (XElement userDetail in userdetails.Elements())
            {

                List<string> dataValues = new List<string>();
                IEnumerable<XNode> userData = userDetail.Nodes();

                foreach (XNode dataItem in userData)
                {
                    dataValues.Add(((XElement)dataItem).Value);
                }


                if (dataValues[0] == "0")
                    return false;
                else
                    return true;
            }
            return false;
        }
    }
}
