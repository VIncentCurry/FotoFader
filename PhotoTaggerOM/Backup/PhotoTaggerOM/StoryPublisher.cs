using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using facebook;

namespace PhotoTaggerOM
{
    public class StoryPublisher
    {
        List<long> targetIDs = new List<long>();
        List<FacebookPhoto> photos = new List<FacebookPhoto>();
        List<long> photoIDs = new List<long>();

        public void AddTarget(long targetID)
        {

            if (targetID != 0 && targetID != Convert.ToInt64(HttpContext.Current.Session[Constants.faceBookUserIDIndex]) && !targetIDs.Contains(targetID))
                targetIDs.Add(targetID);
        }

        public void AddPhotoID(long photoID)
        {
            if (!photoIDs.Contains(photoID))
                photoIDs.Add(photoID);
        }

        public string TargetIDsAsString()
        {
            string returnTargetIDs = "";

            foreach (long targetID in targetIDs)
            {
                if (returnTargetIDs.Length == 0)
                    returnTargetIDs = targetID.ToString();
                else
                    returnTargetIDs += "," + targetID;
            }

            return returnTargetIDs;
        }

        public string TargetIDsAsString(string seperator, string quotations)
        {
            string returnTargetIDs = "";

            foreach (long targetID in targetIDs)
            {
                if (returnTargetIDs.Length == 0)
                    returnTargetIDs = quotations + targetID.ToString() + quotations;
                else
                    returnTargetIDs += seperator + quotations + targetID + quotations;
            }

            return returnTargetIDs;
        }

        public void Publish()
        {
            Dictionary<string, string> templateData = new Dictionary<string, string>();
            Dictionary<string, string> submitData = new Dictionary<string,string>();

            string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            string applicationPath = HttpContext.Current.Request.ApplicationPath;


            foreach (long photoID in photoIDs)
            {
                FacebookPhoto photo = FacebookPhoto.PhotoByID(photoID);
                templateData.Add("src", photo.SmallSrc);
                templateData.Add("href", @"http://" + serverName + applicationPath + @"PhotoFaderTagger.aspx?photoid=" + photo.ID.ToString());
            }

            submitData.Add("images", "[" + facebook.Utility.JSONHelper.ConvertToJSONAssociativeArray(templateData) + "]");

            PhotoTaggerFBService.PhotoTagFaceBookService.feed.publishUserAction(
                Constants.FotoTaggedTemplateBundleID,
                submitData,
                targetIDs, 
                "",
                feed.PublishedStorySize.Full
                );
        }

        public string TemplateData
        {
            get
            {
                Dictionary<string, string> templateData = new Dictionary<string, string>();
                Dictionary<string, string> submitData = new Dictionary<string, string>();

                string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                string applicationPath = HttpContext.Current.Request.ApplicationPath;


                foreach (long photoID in photoIDs)
                {
                    FacebookPhoto photo = FacebookPhoto.PhotoByID(photoID);
                    templateData.Add("src", photo.SmallSrc);
                    templateData.Add("href", @"http://" + serverName + applicationPath + @"PhotoFaderTagger.aspx?photoid=" + photo.ID.ToString());
                }

                submitData.Add("images", "[" + facebook.Utility.JSONHelper.ConvertToJSONAssociativeArray(templateData) + "]");

                return "\"images\":" + "[" + facebook.Utility.JSONHelper.ConvertToJSONAssociativeArray(templateData) + "]";
            }
        }

    }
}
