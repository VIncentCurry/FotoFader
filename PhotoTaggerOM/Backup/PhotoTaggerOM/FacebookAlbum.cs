using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Xml.Linq;
using facebook.Components;

namespace PhotoTaggerOM
{
    public class FacebookAlbum
    {
        #region Private Properties
        long id;
        long coverPhotoID;
        long ownerID;
        FacebookUser owner;
        string name;
        DateTime created;
        DateTime modified;
        string description;
        string location;
        int numberOfPhotos;
        FacebookPhoto coverPhoto;
        FacebookPhotos photos;
        #endregion

        #region Constructors
        public FacebookAlbum(long albumID, long albumCoverID, long albumOwnerID, string albumName, /*DateTime albumCreated, DateTime albumModified,*/ string albumDescription, string albumLocation, int albumNumberOfPhotos)
        {
            id = albumID;
            coverPhotoID = albumCoverID;
            ownerID = albumOwnerID;
            name = albumName;
            //created = albumCreated;
            //modified = albumModified;
            description = albumDescription;
            location = albumLocation;
            numberOfPhotos = albumNumberOfPhotos;
        }
        #endregion

        #region Public Properties
        public long ID
        {
            get { return id; }
        }

        public long CoverPhotoID
        {
            get { return coverPhotoID; }
        }

        public long OwnerID
        {
            get { return ownerID; }
        }

        public string Name
        {
            get { return name; }
        }

        public DateTime Created
        {
            get { return created; }
        }

        public DateTime Modified
        {
            get { return modified; }
        }

        public string Description
        {
            get { return description; }
        }

        public string Location
        {
            get{return location;}
        }

        public int NumberOfPhotos
        {
            get { return numberOfPhotos; }
        }

        public FacebookPhoto CoverPhoto
        {
            get
            {
                if (NumberOfPhotos == 0)
                    return null;

                if (coverPhoto == null)
                {
                    if (photos == null)
                        coverPhoto = FacebookPhoto.PhotoByID(coverPhotoID);
                    else
                        coverPhoto = photos.PhotoByID(coverPhotoID);
                }
                return coverPhoto;
            }
        }

        public FacebookPhotos Photos
        {
            get
            {
                if (photos == null)
                {
                    photos = FacebookPhoto.PhotosInAlbum(id);
                }
                return photos;
            }
        }

        public void AddPhoto(FacebookPhoto photo)
        {
            if (this.photos == null)
                this.photos = new FacebookPhotos();

            this.photos.Add(photo);
        }

        public FacebookUser Owner
        {
            get
            {
                if (owner == null)
                    owner = FacebookUser.UserByID(ownerID);

                return owner;
            }
        }
        #endregion

        #region Loading Methods
        public static FacebookAlbum AlbumByID(long albumID)
        {
            FacebookAlbum album = (FacebookAlbum)HttpContext.Current.Session[Constants.facebookAlbumSessionObjectPrefix + albumID.ToString()];

            if (album == null)
            {
                album = PopulateObjects(" aid = " + albumID.ToString())[0];

                HttpContext.Current.Session[Constants.facebookAlbumSessionObjectPrefix + albumID.ToString()] = album;
            }

            return album;
        }

        public static List<FacebookAlbum> LoggedInUsersAlbums()
        {
            //FacebookService fbService = PhotoTaggerFBService.PhotoTagFaceBookService;

            return PopulateObjects("owner = '" + Convert.ToInt64(HttpContext.Current.Session[Constants.faceBookUserIDIndex]) + "'");
        }

        public static List<FacebookAlbum> UsersAlbums(long userID)
        {
            return PopulateObjects(" owner = " + userID.ToString());
        }

        private static List<FacebookAlbum> PopulateObjects(string whereClause)
        {
            FacebookAlbums albums = new FacebookAlbums();

            FacebookService fbService = PhotoTaggerFBService.PhotoTagFaceBookService;

            string fblAlbumQuery = "SELECT aid, cover_pid, owner, name, created, modified, description, location, size FROM album where " + whereClause;

            //string albumsData = fbService.fql.query(fblAlbumQuery);

            //XElement albumDetails = XElement.Parse(albumsData);
                //PhotoTaggerFBContext.FacebookHttpContext.Fql.QueryXml(fblAlbumQuery);
            XElement albumDetails = PhotoTaggerFBService.ExecuteFQLReturnXElementData(fblAlbumQuery);

            foreach (XElement albumDetail in albumDetails.Elements())
            {
                List<string> dataValues = new List<string>();
                IEnumerable<XNode> albumData = albumDetail.Nodes();

                foreach (XNode dataItem in albumData)
                {
                    dataValues.Add(((XElement)dataItem).Value);
                }

                FacebookAlbum album = new FacebookAlbum(Convert.ToInt64(dataValues[0]),
                    Convert.ToInt64(dataValues[1]),
                    Convert.ToInt64(dataValues[2]),
                    dataValues[3],
                    //Convert.ToDateTime(dataValues[4]),
                    //Convert.ToDateTime(dataValues[5]),
                    dataValues[6],
                    dataValues[7],
                    Convert.ToInt32(dataValues[8])
                    );

                albums.Add(album);
            }

            //Add the photos to the albums - this will reduce round trips to the faceboo server thus (hopefully) improving performance
            FacebookPhotos albumsPhotos = FacebookPhoto.PhotosInAlbums(albums.FacebookAlbumIDsAsString(","));

            foreach (FacebookPhoto photo in albumsPhotos)
                albums.AlbumByID(photo.AlbumID).AddPhoto(photo);


            return albums;
        }
        #endregion
    }

    public class FacebookAlbums : List<FacebookAlbum>
    {
        public FacebookAlbums() { }

        public string FacebookAlbumIDsAsString(string listDivider)
        {

            string facebookAlbumIDsAsString = "";

            foreach (FacebookAlbum album in this)
            {
                if (facebookAlbumIDsAsString.Length == 0)
                    facebookAlbumIDsAsString = album.ID.ToString();
                else
                    facebookAlbumIDsAsString += listDivider + album.ID.ToString();
            }

            return facebookAlbumIDsAsString;
        }

        public FacebookAlbum AlbumByID(long albumID)
        {
            foreach (FacebookAlbum album in this)
            {
                if (album.ID == albumID)
                    return album;
            }

            return null;
        }
    }
}
