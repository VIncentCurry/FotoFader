﻿using System;
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
using System.Drawing;
using System.Drawing.Drawing2D;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "facebookApplicationKey", "facebookApplicationKey = '" + Constants.FaceBookApplicationKey + "';", true);
    }

    protected void btnSaveTag_Click(object sender, EventArgs e)
    {
        string coordinates = txtCoordinates.Text;

        GraphicsPath graphicsPath = new GraphicsPath();


        
        string maparea;
        maparea = @"<map name='maparea'>";
        maparea += @"<area shape=poly coords=" + coordinates + @" onmouseover=""javascript:window.status='Celine'"" />";
        maparea += @"</maparea>";

        imgPhoto.Attributes.Add("usemap", "maparea");

        ltlMapArea.Text = maparea;
    }
}
