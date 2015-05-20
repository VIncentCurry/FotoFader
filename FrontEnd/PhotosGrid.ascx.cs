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
using PhotoTaggerOM;
using System.Collections.Generic;

public partial class PhotosGrid : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void DisplayNPhotosIntable(FacebookPhotos photos, int startIndex, int endIndex, int numberOfColumns)
    {
        int columnCount = 0;

        TableRow photoGridRow = new TableRow();

        for (int i = startIndex; i <= endIndex; i++)
        {

            FacebookPhoto photo = photos[i];

            Image imgPhoto = new Image();
            imgPhoto.ImageUrl = photo.Src;
            imgPhoto.ToolTip = photo.Caption;
            imgPhoto.CssClass = "PhotoInImageGrid";

            HyperLink hlPhoto = new HyperLink();
            hlPhoto.NavigateUrl = "PhotoFaderTagger.aspx?" + Constants.PhotoIDQueryString + "=" + photo.ID.ToString() + "&"
                + Constants.PhotoListIDQueryString + "=" + photos.FacebookPhotoIDsAsString(Constants.idListSeperator);
            hlPhoto.Controls.Add(imgPhoto);

            TableCell photoCell = new TableCell();
            photoCell.CssClass = "CellInImageGrid";
            photoCell.Controls.Add(hlPhoto);

            photoGridRow.Cells.Add(photoCell);

            columnCount += 1;

            if (columnCount == numberOfColumns)
            {
                tblPhotos.Rows.Add(photoGridRow);
                columnCount = 0;
                photoGridRow = new TableRow();
            }
        }

        if (photoGridRow.Cells.Count > 0)
        {
            tblPhotos.Rows.Add(photoGridRow);
        }
    }
}
