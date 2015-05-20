using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DataServer
{
    public abstract class DataObject
    {
        abstract protected static void PopulateObject(List<object> itemsToPopulate, SqlDataReader dataLine) { }

        protected static void PopulateObjects(ref List<object> itemsToPopulate, string storedProcedure, List<SqlParameter> parameters, string connectionString)
        {
            SqlDataReader data = SQLDataServer.ExecuteSPReturnDataReader(storedProcedure, connectionString, parameters);

            while (data.Read())
            {
                PopulateObject(itemsToPopulate, data);
            }

            data.Close();
        }
    }
}
