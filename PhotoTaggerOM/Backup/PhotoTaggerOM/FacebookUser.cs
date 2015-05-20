using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Xml.Linq;
using DataServer;
using System.Data.SqlClient;
using System.Data;
using facebook.Components;

namespace PhotoTaggerOM
{
    public class FacebookUser
    {
        #region Private Properties
        long id;
        string firstName;
        string lastName;
        private List<FacebookAlbum> faceBookAlbums;
        private List<FacebookUser> faceBookFriends;
        //Properties from the fotofader database
        bool fotofaderValuesLoaded = false;
        bool isDirty = false;
        bool askedPublishPermission;
        bool showHelpOnStartUp;
        bool showHelpOnStartFotoFading;
        #endregion

        #region Public Properties
        public long ID
        {
            get { return id; }
        }

        public string FirstName
        {
            get { return firstName; }
        }

        public string LastName
        {
            get { return lastName; }
        }

        public string Name
        {
            get { return firstName + " " + lastName; }
        }

        public bool AskedPublishPermission
        {
            get
            {
                if (!fotofaderValuesLoaded)
                    PopulateFotofaderDatabaseValues();

                return askedPublishPermission;
            }
            set
            {
                isDirty = true;
                askedPublishPermission = value;
            }
        }

        public bool ShowHelpOnStartUp
        {
            get
            {
                if (!fotofaderValuesLoaded)
                    PopulateFotofaderDatabaseValues();

                return showHelpOnStartUp;
            }
            set
            {
                isDirty = true;
                showHelpOnStartUp = value;
            }
        }

        public bool ShowHelpOnStartFotoFading
        {
            get
            {
                if (!fotofaderValuesLoaded)
                    PopulateFotofaderDatabaseValues();
                
                return showHelpOnStartFotoFading;
            }
            set
            {
                isDirty = true;
                showHelpOnStartFotoFading = value;
            }
        }

        public List<FacebookAlbum> Albums
        {
            get
            {
                if (faceBookAlbums == null)
                {
                    faceBookAlbums = FacebookAlbum.UsersAlbums(id);
                }
                return faceBookAlbums;
            }
        }

        public List<FacebookUser> Friends
        {
            get
            {
                if (faceBookFriends == null)
                {
                    faceBookFriends = PopulateObjects("uid in (" + LoggedInUsersFriendsAsIDsList() + ") order by name");
                }

                return faceBookFriends;
            }
        }
        #endregion

        #region Constructors
        public FacebookUser(long userID, string userFirstName, string userLastName)
        {
            id = userID;
            firstName = userFirstName;
            lastName = userLastName;
        }

        #endregion

        #region Loading Methods

        public static List<FacebookUser> LoggedInUsersFriends()
        {
            return (PopulateObjects("uid in (" + LoggedInUsersFriendsAsIDsList() + ") order by name"));
        }

        public static FacebookUser UserByID(long id)
        {
            return (PopulateObjects("uid = " + id.ToString())[0]);
        }

        public static List<FacebookUser> UsersByIDs(string ids)
        {
            return PopulateObjects("uid in (" + ids + ")");
        }

        public static FacebookUser LoggedInUser()
        {
            if(HttpContext.Current.Session[Constants.faceBookLoggedInUserSession]==null)
            {
                FacebookUser loggedInUser = PopulateObjects("uid = " + PhotoTaggerFBService.PhotoTagFaceBookService.users.getLoggedInUser().ToString())[0];
                HttpContext.Current.Session[Constants.faceBookLoggedInUserSession]=loggedInUser;
                return loggedInUser;
            }
            else
            {
                return (FacebookUser)HttpContext.Current.Session[Constants.faceBookLoggedInUserSession];
            }
        }

        private static string LoggedInUsersFriendsAsIDsList()
        {

            string idsInStringList = "";

            foreach (long id in PhotoTaggerFBService.PhotoTagFaceBookService.friends.get())
            {
                if (idsInStringList.Length == 0)
                    idsInStringList = id.ToString();
                else
                    idsInStringList += ", " + id.ToString();
            }

            return idsInStringList;
        }

        private string FriendsAsIDsList()
        {
            string idsInStringList="";

            foreach (long id in PhotoTaggerFBService.PhotoTagFaceBookService.friends.get(this.ID)) 
            {
                if(idsInStringList.Length==0)
                    idsInStringList=id.ToString();
                else
                    idsInStringList+=", " + id.ToString();
            }

            return idsInStringList;
        }

        private static List<FacebookUser> PopulateObjects(string whereClause)
        {
            List<FacebookUser> users = new List<FacebookUser>();

            FacebookService fbService = PhotoTaggerFBService.PhotoTagFaceBookService;

            /*string usersData = fbService.fql.query("select uid, first_name, last_name from user where " + whereClause);

            XElement userdetails = XElement.Parse(usersData);*/
            XElement userdetails = PhotoTaggerFBService.ExecuteFQLReturnXElementData("select uid, first_name, last_name from user where " + whereClause);

            //XElement userdetails = PhotoTaggerFBContext.FacebookHttpContext.Fql.QueryXml("select uid, first_name, last_name from user where " + whereClause);

            foreach (XElement userDetail in userdetails.Elements())
            {

                List<string> dataValues = new List<string>();
                IEnumerable<XNode> userData = userDetail.Nodes();

                foreach (XNode dataItem in userData)
                {
                    dataValues.Add(((XElement)dataItem).Value);
                }

                FacebookUser user = new FacebookUser(
                    Convert.ToInt64(dataValues[0]),
                    dataValues[1],
                    dataValues[2]
                    );

                users.Add(user);
            }

            return users;
        }

        private void PopulateFotofaderDatabaseValues()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SQLDataServer.AddParameter(ref parameters, "FacebookUserID", this.ID, SqlDbType.BigInt, 4);
            SqlDataReader usersData = SQLDataServer.ExecuteSPReturnDataReader("apsp_GetUsersApplicationData", Constants.AdvancedPhotoTaggerConnectionString, parameters);


            if (usersData.HasRows)
            {
                usersData.Read();

                askedPublishPermission = Convert.ToBoolean(usersData["CheckedPublishPermission"]);
                showHelpOnStartUp = Convert.ToBoolean(usersData["ShowHelpOnStartUp"]);
                showHelpOnStartFotoFading = Convert.ToBoolean(usersData["ShowHelpOnStartFotofading"]);

            }
            else
            {
                askedPublishPermission = false;
                showHelpOnStartFotoFading = true;
                showHelpOnStartUp = true;
            }

            usersData.Close();

            fotofaderValuesLoaded=true;
        }
        #endregion

        #region Saving the fotofader values
        public void Save()
        {
            if (isDirty)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                SQLDataServer.AddParameter(ref parameters, "@FacebookUserID", this.ID, SqlDbType.BigInt, 4);
                SQLDataServer.AddParameter(ref parameters, "@CheckedPublishPermission", askedPublishPermission, SqlDbType.Bit, 1);
                SQLDataServer.AddParameter(ref parameters, "@ShowHelpOnStartUp", showHelpOnStartUp, SqlDbType.Bit,1);
	            SQLDataServer.AddParameter(ref parameters, "@ShowHelpOnStartFotofading", showHelpOnStartFotoFading, SqlDbType.Bit,1);

                SQLDataServer.ExecuteSP("apsp_SaveUsersValues", Constants.AdvancedPhotoTaggerConnectionString, parameters);
            }
        }
        #endregion
    }
}
