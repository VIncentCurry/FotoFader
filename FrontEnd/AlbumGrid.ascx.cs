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

public partial class AlbumGrid : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void DisplayNAlbumsIntable(List<FacebookAlbum> albums, int startIndex, int endIndex, int numberOfColumns)
    {
        int columnCount = 0;

        TableRow albumGridRow = new TableRow();

        for (int i = startIndex; i <= endIndex; i++)
        {

            FacebookAlbum album  = albums[i];
            
            Panel pnlAlbum = new Panel();
            pnlAlbum.CssClass = "fbgreybox";

            HyperLink hlalbumPicture = new HyperLink();

            if (album.CoverPhoto == null)
                hlalbumPicture.ImageUrl = Constants.FacebookQuestionImage;
            else
                hlalbumPicture.ImageUrl = album.CoverPhoto.Src;

            hlalbumPicture.NavigateUrl = "albumpage.aspx?" + Constants.AlbumIDQueryString + "=" + album.ID.ToString();
            hlalbumPicture.ToolTip = album.Name;
            pnlAlbum.Controls.Add(hlalbumPicture);

            HyperLink albumTextHyperLink = new HyperLink();
            albumTextHyperLink.Text = album.Name;
            albumTextHyperLink.NavigateUrl = "albumpage.aspx?" + Constants.AlbumIDQueryString + "=" + album.ID.ToString();
            pnlAlbum.Controls.Add(albumTextHyperLink);

            TableCell albumCell = new TableCell();
            albumCell.CssClass = "CellInImageGrid";
            albumCell.Controls.Add(pnlAlbum);
            //albumCell.Controls.Add(albumTextHyperLink);

            albumGridRow.Cells.Add(albumCell);

            columnCount += 1;

            if (columnCount == numberOfColumns)
            {
                tblAlbums.Rows.Add(albumGridRow);
                columnCount = 0;
                albumGridRow = new TableRow();
            }
        }

        if (albumGridRow.Cells.Count > 0)
        {
            tblAlbums.Rows.Add(albumGridRow);
        }
    }
}
