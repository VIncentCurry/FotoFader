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

public partial class AlbumPage : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            long albumID = Convert.ToInt64(Request.QueryString[Constants.AlbumIDQueryString]);

            FacebookAlbum album = FacebookAlbum.AlbumByID(albumID);

            DisplayNPhotosIntable(album, 0, album.Photos.Count - 1, 4, tblPhotoGrid);

            hlBackToAlbums.NavigateUrl = "albums.aspx?" + Constants.FriendIDQueryString + "=" + album.OwnerID.ToString();
            hlBackToAlbums.Text = "Back to " + album.Owner.Name + "'s Albums";

        }
        catch (facebook.Utility.FacebookException facebookException)
        {
            General.RedirectSessionInvalidExceptions(facebookException);
        }
    }

    private void DisplayNPhotosIntable(FacebookAlbum album, int startIndex, int endIndex, int numberOfColumns, Table tableToDisplayIn)
    {
        int columnCount = 0;

        TableRow photoGridRow = new TableRow();

        for (int i = startIndex; i <= endIndex; i++)
        {

            FacebookPhoto photo = album.Photos[i];

            Image imgPhoto = new Image();
            imgPhoto.ImageUrl = photo.Src;
            imgPhoto.ToolTip = photo.Caption;
            imgPhoto.CssClass = "PhotoInImageGrid";

            HyperLink hlPhoto = new HyperLink();
            hlPhoto.NavigateUrl = "PhotoFaderTagger.aspx?" + Constants.PhotoIDQueryString + "=" + photo.ID.ToString() + "&"
                + Constants.PhotoListIDQueryString + "=" + album.Photos.FacebookPhotoIDsAsString(Constants.idListSeperator);
            hlPhoto.Controls.Add(imgPhoto);

            TableCell photoCell = new TableCell();
            photoCell.CssClass = "CellInImageGrid";
            photoCell.Controls.Add(hlPhoto);

            photoGridRow.Cells.Add(photoCell);

            columnCount += 1;

            if (columnCount == numberOfColumns)
            {
                tableToDisplayIn.Rows.Add(photoGridRow);
                columnCount = 0;
                photoGridRow = new TableRow();
            }
        }

        if (photoGridRow.Cells.Count > 0)
        {
            tableToDisplayIn.Rows.Add(photoGridRow);
        }
    }

    private void DisplayPhotosAsDivs(FacebookAlbum album)
    {
        foreach (FacebookPhoto photo in album.Photos)
        {
            Panel pnlPhoto = new Panel();
            pnlPhoto.CssClass = "faderPhoto";

            HyperLink hlPhoto = new HyperLink();
            hlPhoto.ImageUrl = photo.Src;
            hlPhoto.ToolTip = photo.Caption;
            hlPhoto.NavigateUrl = "PhotoFaderTagger.aspx?" + Constants.PhotoIDQueryString + "=" + photo.ID.ToString();

            pnlPhoto.Controls.Add(hlPhoto);

            phPhotos.Controls.Add(pnlPhoto);
        }
    }
}
