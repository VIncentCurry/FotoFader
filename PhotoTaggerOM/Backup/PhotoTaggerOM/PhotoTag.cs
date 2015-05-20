using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using DataServer;
using System.Drawing;
using facebook;
using System.Web;

namespace PhotoTaggerOM
{
    public class PhotoTag
    {
        #region Private Properties
        private Guid id;
        private long facebookPhotoID;
        private long facebookTagUserId;
        private string tag;
        private int midXPoint;
        private int midYPoint;
        private Guid parentPhotoID;
        private string polygonCoordinates;
        private string backgroundImageFileName;
        private string backgroundImageFilePath;
        private long taggerFacebookUserID;
        private FacebookUser taggedUser;
        private FacebookPhoto parentPhoto;
        private FacebookUser tagger;
        #endregion

        #region Constructors
        public PhotoTag(Guid id, long facebookTagUserID, string tag, long facebookPhotoID, string polygonCoordinates, string imageFileName, string imageFilePath, long taggerFacebookUserID)
        {
            this.id = id;
            this.facebookPhotoID = facebookPhotoID;
            this.facebookTagUserId = facebookTagUserID;
            this.tag = tag;
            this.polygonCoordinates = polygonCoordinates;
            this.backgroundImageFileName = imageFileName;
            this.backgroundImageFilePath = imageFilePath;
            this.taggerFacebookUserID = taggerFacebookUserID;
        }

        public PhotoTag(long facebookTagUserID, string tag, long facebookPhotoID, string polygonCoordinates)
        {
            this.facebookPhotoID = facebookPhotoID;
            this.facebookTagUserId = facebookTagUserID;
            this.tag = tag;
            this.taggerFacebookUserID = Convert.ToInt64(HttpContext.Current.Session[Constants.faceBookUserIDIndex]);
            if (polygonCoordinates.EndsWith(","))
                polygonCoordinates = polygonCoordinates.Substring(0, polygonCoordinates.Length - 1);
            
            this.polygonCoordinates = polygonCoordinates;

            CreateBackgroundImage();

            StoryPublisher publisher;

            if (HttpContext.Current.Session[Constants.storyPublisherSessionKey] == null)
                publisher = new StoryPublisher();
            else
                publisher = (StoryPublisher)HttpContext.Current.Session[Constants.storyPublisherSessionKey];

            publisher.AddPhotoID(facebookPhotoID);

            if (facebookTagUserID != 0)
                publisher.AddTarget(facebookTagUserID);

            HttpContext.Current.Session[Constants.storyPublisherSessionKey] = publisher;
        }
        #endregion

        private void CreateBackgroundImage()
        {
            Image originalPhoto;
            Bitmap tagImage;
            Graphics graph;

            string[] coordinates = polygonCoordinates.Split(new Char[] { ',' });
            Line tagLine = new Line();

            if (coordinates.Length % 2 != 0)
                throw new Exception("We have a non-even number of coordinates...");

            for (int i = 0; i < coordinates.Length; i = i + 2)
            {
                tagLine.AddPoint(Convert.ToInt32(coordinates[i]), Convert.ToInt32(coordinates[i + 1]));
            }

            System.Net.WebClient wc = new System.Net.WebClient();

            byte[] photoData = wc.DownloadData(this.ParentPhoto.BigSrc);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(photoData);

            originalPhoto = System.Drawing.Image.FromStream(stream);
            tagLine.InvertArea(originalPhoto.Width, originalPhoto.Height);
            
            tagImage = new System.Drawing.Bitmap(originalPhoto.Width, originalPhoto.Height);
            graph = System.Drawing.Graphics.FromImage(tagImage);
            
            Brush whiteBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
            graph.DrawImage(originalPhoto, new System.Drawing.Rectangle(0, 0, originalPhoto.Width, originalPhoto.Height));
            graph.FillPolygon(whiteBrush, tagLine.Points);
            backgroundImageFileName = this.facebookPhotoID.ToString() + this.facebookTagUserId.ToString() + this.tag + ".jpg";
            tagImage.Save(Constants.BackgroundFileSaveLocation + backgroundImageFileName);
            backgroundImageFilePath = Constants.BackgroundFileSaveLocation;

            this.ApplyEdit();
        }

        #region Public Properties
        public Guid ID
        {
            get
            {
                return id;
            }
        }

        public long FacebookPhotoID
        {
            get { return this.facebookPhotoID; }
            set { this.facebookPhotoID = value; }
        }

        public long FacebookTagUserID
        {
            get
            {
                return facebookTagUserId;
            }
            set
            {
                facebookTagUserId = value;
            }
        }

        public string Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }

        public string PolygonCoordinates
        {
            get { return polygonCoordinates; }
            set { polygonCoordinates = value; }
        }


        public int MidXPoint
        {
            get
            {
                return midXPoint;
            }
            set
            {
                midXPoint = value;
            }
        }

        public int MidYPoint
        {
            get { return midYPoint; }
            set { midYPoint = value;}
        }

        public Guid ParentPhotoID
        {
            get { return parentPhotoID; }
            set { parentPhotoID = value; }
        }

        public string BackgroundImageFileName
        {
            get { return Constants.BackgroundImagesHTTPPath + backgroundImageFileName; }
        }

        //currently just the tag - will add functionality to get the name from the facebook user id.
        public string DisplayName
        {
            get 
            {
                if (tag.Length > 0)
                    return tag;
                else
                    return TaggedUser.Name;
            }
        }

        public FacebookUser TaggedUser
        {
            get 
            {
                if (taggedUser==null)
                    taggedUser=FacebookUser.UserByID(facebookTagUserId);

                return taggedUser;
            }
            set
            {
                taggedUser = value;
            }
        }

        public FacebookPhoto ParentPhoto
        {
            get
            {
                if (parentPhoto == null)
                    parentPhoto = FacebookPhoto.PhotoByID(facebookPhotoID);

                return parentPhoto;
            }
        }

        public bool PhotoIsLoggedOnUsers
        {
            get
            {
                return (ParentPhoto.OwnerID == Convert.ToInt64(HttpContext.Current.Session[Constants.faceBookUserIDIndex]));
            }
        }

        public bool TagIsLoggedOnUser
        {
            get
            {
                return (facebookTagUserId == Convert.ToInt64(HttpContext.Current.Session[Constants.faceBookUserIDIndex]));
            }
        }

        public bool TaggerIsLoggedInUser
        {
            get
            {
                return (taggerFacebookUserID == Convert.ToInt64(HttpContext.Current.Session[Constants.faceBookUserIDIndex]));
            }
        }
        #endregion

        #region Saving to the database

        public void ApplyEdit()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            SQLDataServer.AddParameter(ref parameters, "@PhotoFacebookID", this.facebookPhotoID, SqlDbType.BigInt, 8);
            SQLDataServer.AddParameter(ref parameters, "@FacebookUserID", this.facebookTagUserId, SqlDbType.BigInt, 8);
            SQLDataServer.AddParameter(ref parameters, "@Name", this.tag, SqlDbType.VarChar, 50);
            SQLDataServer.AddParameter(ref parameters, "@PolygonCoordinates", this.polygonCoordinates, SqlDbType.VarChar, 8000);
            SQLDataServer.AddParameter(ref parameters, "@BackgroundImageFileName", this.backgroundImageFileName, SqlDbType.NVarChar,100);
            SQLDataServer.AddParameter(ref parameters, "@BackgroundImageFilePath", this.backgroundImageFilePath, SqlDbType.NVarChar, 200);
            SQLDataServer.AddParameter(ref parameters, "@TaggerFacebookUserID", this.taggerFacebookUserID, SqlDbType.Int, 4);

            this.id = SQLDataServer.ExecuteSPReturnGuid("apsp_SavePhotoTag", Constants.AdvancedPhotoTaggerConnectionString, parameters);
        }

        public static void Delete(Guid id)
        {
            List<SqlParameter> deleteParameter = new List<SqlParameter>();
            SQLDataServer.AddParameter(ref deleteParameter, "PhotoTagID", id, SqlDbType.UniqueIdentifier, 16);

            SQLDataServer.ExecuteSP("apsp_DeletePhotoTag", Constants.AdvancedPhotoTaggerConnectionString, deleteParameter);
        }
        #endregion
    
        public static List<PhotoTag> GetPhotoTagsByPhotoID(long photoID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SQLDataServer.AddParameter(ref parameters, "PhotoFacebookID", photoID, SqlDbType.BigInt, 8);

            List<PhotoTag> photoTags = PopulateItems("apsp_GetTagsForPhoto", parameters);

            //We're also going to populate the Facebook User items in one shot to minimize FQL calls...

            string userIDList = "";

            foreach (PhotoTag photoTag in photoTags)
            {
                if (userIDList.Length == 0)
                    userIDList = photoTag.FacebookTagUserID.ToString();
                else
                    userIDList += "," + photoTag.FacebookTagUserID.ToString();
            }

            if (userIDList.Length > 0)
            {
                List<FacebookUser> taggedPersons = FacebookUser.UsersByIDs(userIDList);

                foreach (FacebookUser taggedPerson in taggedPersons)
                {
                    foreach (PhotoTag photoTag in photoTags)
                    {
                        if (taggedPerson.ID == photoTag.FacebookTagUserID)
                        {
                            photoTag.TaggedUser = taggedPerson;
                        }
                    }
                }
            }

            return photoTags;
        }

        public static List<PhotoTag> UsersPhotoTags(long userID)
        {
            List<SqlParameter> userParameter = new List<SqlParameter>();
            SQLDataServer.AddParameter(ref userParameter, "@FacebookUserID", userID, SqlDbType.BigInt, 8);

            return PopulateItems("apsp_GetTagsForUser", userParameter);
        }

        private static List<PhotoTag> PopulateItems(string storedProcedure, List<SqlParameter> parameters)
        {
            SqlDataReader tagData = SQLDataServer.ExecuteSPReturnDataReader(storedProcedure, Constants.AdvancedPhotoTaggerConnectionString, parameters);
            List<PhotoTag> photoTags = new List<PhotoTag>();

            while (tagData.Read())
            {
                PhotoTag photoTag = new PhotoTag(
                    tagData.GetGuid(0),
                    Convert.ToInt64(tagData["FacebookUserID"]),
                    Convert.ToString(tagData["Name"]),
                    Convert.ToInt64(tagData["PhotoFacebookID"]),
                    Convert.ToString(tagData["PolygonCoordinates"]),
                    Convert.ToString(tagData["BackgroundImageFileName"]),
                    Convert.ToString(tagData["BackgroundImageFilePath"]),
                    Convert.ToInt64(tagData["TaggerFacebookUserID"])
                    );

                photoTags.Add(photoTag); 
            }

            tagData.Close();

            return photoTags;
        }
    }

    public class PhotoTags : List<PhotoTag>
    {
        public PhotoTags(){}

        public string FacebookPhotoIDsAsString(string listDivider)
        {

            string facebookPhotoIDsAsString = "";

            foreach (PhotoTag tag in this)
            {
                if (facebookPhotoIDsAsString.Length == 0)
                    facebookPhotoIDsAsString = tag.FacebookPhotoID.ToString();
                else
                    facebookPhotoIDsAsString += listDivider + tag.FacebookPhotoID.ToString();
            }

            return facebookPhotoIDsAsString;
        }
    }
}
