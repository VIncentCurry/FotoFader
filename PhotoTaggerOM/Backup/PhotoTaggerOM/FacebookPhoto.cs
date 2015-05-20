using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using facebook;
using facebook.Components;
using System.Web;
using System.Web.SessionState;
using System.Xml.Linq;

namespace PhotoTaggerOM
{
    public class FacebookPhoto
    {
        #region Private Properties
        long photoID;
        long albumID;
        long ownerID;
        string smallSrc;
        string bigSrc;
        string src;
        string caption;
        string created;
        List<PhotoTag> tags;
        FacebookAlbum album;
        #endregion

        #region Constructor
        public FacebookPhoto(long id, long photoAlbumID,  long photoOwnerID, string photoSmallSrc, string photoBigSrc, string photoSrc, string photoCaption)
        {
            photoID = id;
            albumID = photoAlbumID;
            ownerID = photoOwnerID;
            smallSrc = photoSmallSrc;
            bigSrc = photoBigSrc;
            src = photoSrc;
            caption = photoCaption;
        }
        #endregion

        #region Public Properties
        public long ID
        {
            get { return photoID; }
        }

        public long AlbumID
        {
            get { return albumID; }
        }

        public long OwnerID
        {
            get { return ownerID; }
        }

        public string SmallSrc
        {
            get { return smallSrc; }
        }

        public string Src
        {
            get { return src; }
        }

        public string BigSrc
        {
            get { return bigSrc; }
        }

        public string Caption
        {
            get { return caption; }
        }

        public List<PhotoTag> Tags
        {
            get
            {
                return PhotoTag.GetPhotoTagsByPhotoID(photoID);
            }
        }

        public FacebookAlbum Album
        {
            get
            {
                if (album == null)
                    album = FacebookAlbum.AlbumByID(this.albumID);

                return album;
            }
        }
        #endregion

        #region Publishing Methods
        public void PublishTaggingDetails()
        {
            StoryPublisher publisher = (StoryPublisher)HttpContext.Current.Session[Constants.storyPublisherSessionKey];

            if (publisher != null)
                publisher.Publish();

            HttpContext.Current.Session[Constants.storyPublisherSessionKey] = null;

            /*List<long> targetIDs = new List<long>();

            foreach (PhotoTag tag in this.Tags)
            {
                if (tag.FacebookTagUserID != 0 && tag.FacebookTagUserID != PhotoTaggerFBService.PhotoTagFaceBookService.uid)
                    targetIDs.Add(tag.FacebookTagUserID);
            }

            //Add the entry to the stories...
            Dictionary<string, string> templateData = new Dictionary<string, string>();
            templateData.Add("src", this.SmallSrc);

            string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            string applicationPath = HttpContext.Current.Request.ApplicationPath;

            templateData.Add("href", @"http://" + serverName + applicationPath + @"PhotoFaderTagger.aspx?photoid=" + this.ID.ToString());

            PhotoTaggerFBService.PhotoTagFaceBookService.feed.publishUserAction(Constants.FotoTaggedTemplateBundleID, templateData, targetIDs, "", feed.PublishedStorySize.Full);*/
        }
        #endregion

        #region Loading Methods
        public static FacebookPhoto PhotoByID(long photoID)
        {
            FacebookPhoto photo = (FacebookPhoto)HttpContext.Current.Session[Constants.facebookPhotoSessionObjectPrefix + photoID.ToString()];

            if (photo == null)
            {
                photo = PopulateObjects(" pid = '" + photoID.ToString() + "'")[0];
            }

            return photo;
        }

        public static FacebookPhotos PhotosInAlbum(long albumID)
        {
            return PopulateObjects(" aid = " + albumID.ToString());
        }

        public static FacebookPhotos PhotosInAlbums(string albumIDs)
        {
            return PopulateObjects(" aid in(" + albumIDs + ")");
        }

        public static FacebookPhotos FotoFadeTaggedPhotosOfUser(long userID)
        {
            string photoIDs = "";

            //get the list of photofacebookids...
            List<PhotoTag> usersPhotoTags = PhotoTag.UsersPhotoTags(userID);

            foreach (PhotoTag tag in usersPhotoTags)
            {
                if (photoIDs.Length == 0)
                    photoIDs = tag.FacebookPhotoID.ToString();
                else
                    photoIDs += ", " + tag.FacebookPhotoID.ToString();
            }

            if (photoIDs.Length > 0)
                return (PopulateObjects("pid in (" + photoIDs + ")"));
            else
                return null;

        }

        private static FacebookPhotos  PopulateObjects(string whereClause)
        {
            FacebookPhotos photos = new FacebookPhotos();

            FacebookService fbService = PhotoTaggerFBService.PhotoTagFaceBookService;

            //string photosData = fbService.fql.query("Select pid, aid, owner, src_small, src_big, src, caption, created from photo where " + whereClause);

            //XElement photodetails = XElement.Parse(photosData);
                //PhotoTaggerFBContext.FacebookHttpContext.Fql.QueryXml("Select pid, aid, owner, src_small, src_big, src, caption, created from photo where " + whereClause);
            XElement photoDetails = PhotoTaggerFBService.ExecuteFQLReturnXElementData("Select pid, aid, owner, src_small, src_big, src, caption, created from photo where " + whereClause);

            foreach (XElement photoDetail in photoDetails.Elements())
            {

                List<string> dataValues = new List<string>();
                IEnumerable<XNode> photoData = photoDetail.Nodes();

                foreach (XNode dataItem in photoData)
                {
                    dataValues.Add(((XElement)dataItem).Value);
                }

                FacebookPhoto photo = new FacebookPhoto(
                    Convert.ToInt64(dataValues[0]),
                    Convert.ToInt64(dataValues[1]),
                    Convert.ToInt64(dataValues[2]),
                    dataValues[3],
                    dataValues[4],
                    dataValues[5],
                    dataValues[6]
                    );

                photos.Add(photo);

                HttpContext.Current.Session[Constants.facebookPhotoSessionObjectPrefix + photo.ID.ToString()] = photo;
            }

            return photos;
        }
        #endregion
    }

    public class FacebookPhotos : List<FacebookPhoto>
    {
        public FacebookPhotos() { }

        public string FacebookPhotoIDsAsString(string listDivider)
        {

            string facebookPhotoIDsAsString = "";

            foreach (FacebookPhoto photo in this)
            {
                if (facebookPhotoIDsAsString.Length == 0)
                    facebookPhotoIDsAsString = photo.ID.ToString();
                else
                    facebookPhotoIDsAsString += listDivider + photo.ID.ToString();
            }

            return facebookPhotoIDsAsString;
        }

        public FacebookPhoto PhotoByID(long photoID)
        {
            foreach (FacebookPhoto photo in this)
            {
                if (photo.ID == photoID)
                    return photo;
            }

            return null;
        }
    }
}
