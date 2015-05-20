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
using System.Collections.Generic;
using PhotoTaggerOM;

public partial class Albums : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhotoTaggerOM.FacebookUser loggedInUser = PhotoTaggerOM.FacebookUser.LoggedInUser();

            if (loggedInUser.AskedPublishPermission || PhotoTaggerOM.General.UserHasGrantedPublishPermission())
            {
                List<FacebookAlbum> albums;
                FacebookPhotos fotoTaggedPhotos;
                string name;
                string possesivename;

                if (Request.QueryString[Constants.FriendIDQueryString] == null)
                {
                    albums = loggedInUser.Albums;
                    fotoTaggedPhotos = FacebookPhoto.FotoFadeTaggedPhotosOfUser(loggedInUser.ID);
                    name = "you";
                    possesivename = "your";
                }
                else
                {
                    albums = FacebookAlbum.UsersAlbums(Convert.ToInt64(Request.QueryString[Constants.FriendIDQueryString]));
                    fotoTaggedPhotos = FacebookPhoto.FotoFadeTaggedPhotosOfUser(Convert.ToInt64(Request.QueryString[Constants.FriendIDQueryString]));
                    name = FacebookUser.UserByID(Convert.ToInt64(Request.QueryString[Constants.FriendIDQueryString])).FirstName;
                    possesivename = name + "'s";
                }

                if (fotoTaggedPhotos != null)
                {
                    Label lblTaggedPhotos = new Label();
                    lblTaggedPhotos.Text = "fotofade tagged photos of " + name + ":";
                    lblTaggedPhotos.CssClass = "fotoFaderHeader2";
                    phTaggedPhotosTitle.Controls.Add(lblTaggedPhotos);

                    pgPersonsTaggedPhotos.DisplayNPhotosIntable(fotoTaggedPhotos, 0, fotoTaggedPhotos.Count - 1, 4);
                    
                }

                Label lblYourAlbums = new Label();
                lblYourAlbums.Text = possesivename + " albums";
                lblYourAlbums.CssClass = "fotoFaderHeader2";
                phImages.Controls.Add(lblYourAlbums);

                agAlbums.DisplayNAlbumsIntable(albums, 0, albums.Count - 1, 4);
            }
            else
            {
                loggedInUser.AskedPublishPermission = true;
                string nextPageURLEncoded;
                if (Request.QueryString[Constants.facebookNextPage] == null || Request.QueryString[Constants.facebookNextPage].Length==0)
                    nextPageURLEncoded = Server.UrlEncode(Request.Url.AbsoluteUri);
                else
                    nextPageURLEncoded = Request.QueryString[Constants.facebookNextPage];

                Response.Redirect("http://www.facebook.com/authorize.php?api_key="
                    + Constants.FaceBookApplicationKey + "&v=1.0&ext_perm=publish_stream"
                    + "&" + Constants.facebookNextPage + "=" + nextPageURLEncoded 
                    + "&next_cancel=" + nextPageURLEncoded);
            }
        }
        catch (facebook.Utility.FacebookException facebookException)
        {
            General.RedirectSessionInvalidExceptions(facebookException);
        }
    }

}
