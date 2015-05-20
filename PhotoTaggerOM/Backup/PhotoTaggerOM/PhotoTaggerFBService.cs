using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using facebook.Components;
using System.Web;
using System.Web.SessionState;
using facebook;
using System.Xml.Linq;

namespace PhotoTaggerOM
{
    class PhotoTaggerFBService
    {
        const string faceBookUserIDIndex = "Facebook_userId";
        const string faceBookSessionExpiresIndex = "Facebook_session_expires";

        public static FacebookService PhotoTagFaceBookService
        {
            get
            {
                if (HttpContext.Current.Session[Constants.faceBookSessionObject] == null)
                {
                    FacebookService fbService = new FacebookService();

                    fbService.ApplicationKey = Constants.FacebookApplicationKey;
                    fbService.Secret = Constants.FacebookSecretKey;
                    fbService.IsDesktopApplication = false;

                    string sessionKey = HttpContext.Current.Session[Constants.facebookSessionKeyIndex] as string;
                    long userID = Convert.ToInt64(HttpContext.Current.Session[faceBookUserIDIndex]);

                    string authToken = HttpContext.Current.Request["auth_token"];

                    if (!String.IsNullOrEmpty(sessionKey))
                    {
                        fbService.SessionKey = sessionKey;
                        fbService.uid = userID;
                    }
                    else if (!String.IsNullOrEmpty(authToken))
                    {
                        fbService.CreateSession(authToken);
                        HttpContext.Current.Session[Constants.facebookSessionKeyIndex] = fbService.SessionKey;
                        HttpContext.Current.Session[faceBookUserIDIndex] = fbService.uid;
                        HttpContext.Current.Session[faceBookSessionExpiresIndex] = fbService.SessionExpires;
                    }
                    else
                    {
                        string nextPageURLEncoded = HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsoluteUri);
                        HttpContext.Current.Response.Redirect(@"http://www.Facebook.com/login.php?api_key=" + fbService.ApplicationKey + @"&v=1.0&next=" + nextPageURLEncoded);
                    }

                    HttpContext.Current.Session[Constants.faceBookSessionObject] = fbService;

                    return fbService;
                }
                else
                {
                    return (FacebookService)HttpContext.Current.Session[Constants.faceBookSessionObject];
                }
            }
        }

        public static XElement ExecuteFQLReturnXElementData(string fql)
        {
            int numberOfTries = 0;

            while (numberOfTries<2)
            {
                try
                {
                    string rawData = PhotoTagFaceBookService.fql.query(fql);

                    return XElement.Parse(rawData);
                }
                catch (Exception dataAccessError)
                {
                    if (ExceptionIsTimeOut(dataAccessError))
                    {
                        numberOfTries += 1;
                    }
                    else
                    {
                        throw dataAccessError;
                    }
                }
            }

            throw new FacebookTimeOutException();
        }

        private static bool ExceptionIsTimeOut(Exception exception)
        {
            if (exception.Message == "The underlying connection was closed: An unexpected error occurred on a receive."
                || exception.Message == "The underlying connection was closed: A connection that was expected to be kept alive was closed by the server.")
                return true;
            else
                return false;
        }
    }
}
