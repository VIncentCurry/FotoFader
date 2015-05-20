using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DataServer;

namespace PhotoTaggerOM
{
    public class PageVisit
    {
        public static void RecordPageVisit(long facebookUserID, string pageName)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            SQLDataServer.AddParameter(ref parameters, "@FacebookUserID", facebookUserID, SqlDbType.BigInt, 8);
            SQLDataServer.AddParameter(ref parameters, "@PageName", pageName, SqlDbType.NVarChar,100);

            SQLDataServer.ExecuteSP("apsp_InsertPageVisit", Constants.AdvancedPhotoTaggerConnectionString, parameters);
        }
    }
}
