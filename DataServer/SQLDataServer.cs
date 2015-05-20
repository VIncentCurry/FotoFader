using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data;

namespace DataServer
{
    /// <summary>
    /// This class provides a variety of static methods for accessing and updating data in a SQL 7.0 or higher database.
    /// </summary>
    public class SQLDataServer
    {
        public SQLDataServer() { }

        #region ExecuteSQLStringReturnDataReader
        public static SqlDataReader ExecuteSQLStringReturnDataReader(string sQLStatement, string connectionString)
        {
            SqlConnection dbConnection;

            dbConnection = new SqlConnection(connectionString);

            return ExecuteSQLStringReturnDataReader(sQLStatement, dbConnection);

        }

        /// <summary>
        /// This function executes a sql statement against a SQL 7.0 or higher database which returns a data reader against a database connect to by dbConnection. Once the datareader has been used it should be closed as this will also close the connection. The connection that is passed in should be closed as this function will open it.
        /// </summary>
        /// <param name="sQLStatement"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteSQLStringReturnDataReader(string sQLStatement, SqlConnection dbConnection)
        {
            SqlCommand commandObject;
            SqlDataReader dataReader;

            commandObject = new SqlCommand(sQLStatement, dbConnection);

            dbConnection.Open();

            dataReader = commandObject.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            return dataReader;
        }
        #endregion

        #region Execute SQL Count

        /// <summary>
        /// This function executes a count statement and returns the count from the calling function.
        /// </summary>
        /// <param name="sqlCountStatement"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static long ExecuteSQLReturnCount(string sqlCountStatement, string connectionString)
        {
            SqlConnection dbConnection;

            dbConnection = new SqlConnection(connectionString);

            return ExecuteSQLReturnCount(sqlCountStatement, dbConnection);

        }

        /// <summary>
        /// This function executes a sql statement against a SQL 7.0 or higher database which returns a data reader against a database connect to by dbConnection. Once the datareader has been used it should be closed as this will also close the connection. The connection that is passed in should be closed as this function will open it.
        /// </summary>
        /// <param name="sQLStatement"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public static long ExecuteSQLReturnCount(string sqlCountStatement, SqlConnection dbConnection)
        {
            SqlCommand commandObject;
            SqlDataReader dataReader;
            long count;

            commandObject = new SqlCommand(sqlCountStatement, dbConnection);

            dbConnection.Open();

            dataReader = commandObject.ExecuteReader();

            dataReader.Read();

            count = Convert.ToInt64(dataReader.GetInt32(0));

            return count;
        }
        #endregion

        #region ExecuteSQL
        public static void ExecuteSQL(string sqlString, string connectionString)
        {
            SqlConnection dbConnection;

            dbConnection = new SqlConnection(connectionString);

            ExecuteSQL(sqlString, dbConnection);

        }

        public static void ExecuteSQL(string sqlString, SqlConnection dbConnection)
        {
            SqlCommand commandObject;

            commandObject = new SqlCommand(sqlString, dbConnection);

            dbConnection.Open();

            commandObject.ExecuteNonQuery();
        }
        #endregion

        #region ExecuteSPReturnDataReader
        /// <summary>
        /// This function executes a SQL Stored procedure which returns a datareader. Even if there are no input parameters for the stored procedure an instantiated array list still needs to be passed in otherwise an exception will be raised.
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="connectionString"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteSPReturnDataReader(string storedProcedure, string connectionString, List<SqlParameter> parameters)
        {
            SqlCommand commandObject;
            SqlDataReader dataReader;
            SqlConnection dbConnection;

            dbConnection = new SqlConnection(connectionString);

            commandObject = SetUpStoredProcedureCommandAndParameters(storedProcedure, dbConnection, parameters);

            dbConnection.Open();

            dataReader = commandObject.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            return dataReader;

        }

        public static SqlDataReader ExecuteSPReturnDataReader(string storedProcedure, SqlConnection dbConnection, List<SqlParameter> parameters, SqlTransaction transaction)
        {
            SqlCommand commandObject;
            SqlDataReader dataReader;

            commandObject = SetUpStoredProcedureCommandAndParameters(storedProcedure, dbConnection, parameters, transaction);

            dataReader = commandObject.ExecuteReader();

            return dataReader;

        }
        #endregion

        #region ExecuteSPReturnLong
        public static long ExecuteSPReturnLong(string storedProcedure, string connectionString, List<SqlParameter> parameters, string returnParameterName)
        {
            SqlParameter parameter = new SqlParameter(returnParameterName, SqlDbType.Int, 4);
            parameter.Direction = ParameterDirection.Output;

            parameters.Add(parameter);

            SqlConnection dbConnection;
            long returnValue;

            dbConnection = new SqlConnection(connectionString);

            SqlCommand commandObject = SetUpStoredProcedureCommandAndParameters(storedProcedure, dbConnection, parameters);

            dbConnection.Open();

            commandObject.ExecuteNonQuery();

            returnValue = Convert.ToInt64(commandObject.Parameters[returnParameterName].Value);

            dbConnection.Close();

            return returnValue;
        }

        public static long ExecuteSPReturnLong(string storedProcedure, SqlConnection dbConnection, List<SqlParameter> parameters, string returnParameterName, SqlTransaction transaction)
        {
            SqlParameter parameter = new SqlParameter(returnParameterName, SqlDbType.Int, 4);
            parameter.Direction = ParameterDirection.Output;

            parameters.Add(parameter);

            SqlCommand commandObject = SetUpStoredProcedureCommandAndParameters(storedProcedure, dbConnection, parameters, transaction);

            commandObject.ExecuteNonQuery();

            return Convert.ToInt64(commandObject.Parameters[returnParameterName].Value);

        }
        #endregion

        #region ExecuteSP
        public static void ExecuteSP(string storedProcedure, string connectionString, List<SqlParameter> parameters)
        {
            bool bAlreadyOpen;
            SqlConnection dbConnection;

            dbConnection = new SqlConnection(connectionString);

            SqlCommand commandObject = SetUpStoredProcedureCommandAndParameters(storedProcedure, dbConnection, parameters);

            bAlreadyOpen = (dbConnection.State == ConnectionState.Open);
            if (!bAlreadyOpen)
                dbConnection.Open();
            commandObject.ExecuteNonQuery();
            if (!bAlreadyOpen)
                dbConnection.Close();

        }

        public static void ExecuteSP(string storedProcedure, SqlConnection dbConnection, List<SqlParameter> parameters, SqlTransaction transaction)
        {
            SqlCommand commandObject = SetUpStoredProcedureCommandAndParameters(storedProcedure, dbConnection, parameters, transaction);

            commandObject.ExecuteNonQuery();
        }

        public static void ExecuteSP(string storedProcedure, SqlConnection dbConnection, List<SqlParameter> parameters)
        {
            SqlCommand commandObject = SetUpStoredProcedureCommandAndParameters(storedProcedure, dbConnection, parameters);

            commandObject.ExecuteNonQuery();
        }
        #endregion

        #region ExecuteSPReturnString

        public static string ExecuteSPReturnString(string storedProcedure, string connectionString, List<SqlParameter> parameters, string returnParameterName, int size)
        {
            SqlParameter parameter = new SqlParameter(returnParameterName, SqlDbType.VarChar, size);
            parameter.Direction = ParameterDirection.Output;

            parameters.Add(parameter);

            SqlConnection dbConnection;
            string returnValue;

            dbConnection = new SqlConnection(connectionString);

            SqlCommand commandObject = SetUpStoredProcedureCommandAndParameters(storedProcedure, dbConnection, parameters);

            dbConnection.Open();

            commandObject.ExecuteNonQuery();

            returnValue = Convert.ToString(commandObject.Parameters[returnParameterName].Value);

            dbConnection.Close();

            return returnValue;
        }

        public static string ExecuteSPReturnString(string storedProcedure, SqlConnection dbConnection, List<SqlParameter> parameters, string returnParameterName, int size)
        {
            SqlParameter parameter = new SqlParameter(returnParameterName, SqlDbType.VarChar, size);
            parameter.Direction = ParameterDirection.Output;

            parameters.Add(parameter);

            SqlCommand commandObject = SetUpStoredProcedureCommandAndParameters(storedProcedure, dbConnection, parameters);

            commandObject.ExecuteNonQuery();

            return Convert.ToString(commandObject.Parameters[returnParameterName].Value);
        }


        #endregion

        #region ExecuteSPReturnDate

        public static DateTime ExecuteSPReturnDateTime(string storedProcedure, string connectionString, List<SqlParameter> parameters)
        {
            SqlConnection dbConnection;

            dbConnection = new SqlConnection(connectionString);

            return ExecuteSPReturnDateTime(storedProcedure, dbConnection, parameters);

        }

        public static DateTime ExecuteSPReturnDateTime(string storedProcedure, SqlConnection dbConnection, List<SqlParameter> parameters)
        {
            SqlDataReader dataReader;
            DateTime dateTime;

            SqlCommand commandObject = SetUpStoredProcedureCommandAndParameters(storedProcedure, dbConnection, parameters);

            dbConnection.Open();

            dataReader = commandObject.ExecuteReader();

            dataReader.Read();

            dateTime = Convert.ToDateTime(dataReader.GetDateTime(0));

            dbConnection.Close();

            return dateTime;
        }

        #endregion
        
        #region ExecuteSPReturnGuid
        public static Guid ExecuteSPReturnGuid(string storedProcedure, string connectionString, List<SqlParameter> parameters)
        {
            SqlConnection dbConnection = new SqlConnection(connectionString);

            SqlCommand cmdObject = SetUpStoredProcedureCommandAndParameters(storedProcedure, dbConnection, parameters);

            dbConnection.Open();

            SqlDataReader dataReader = cmdObject.ExecuteReader();

            dataReader.Read();

            Guid returnGuid = dataReader.GetGuid(0);

            dbConnection.Close();

            return returnGuid;
        }
        #endregion

        #region SetUpStoredProcedureCommandAndParameters

        private static SqlCommand SetUpStoredProcedureCommandAndParameters(string storedProcedure, SqlConnection dbConnection, List<SqlParameter> parameters)
        {
            SqlCommand commandObject;

            commandObject = new SqlCommand(storedProcedure, dbConnection);

            commandObject.CommandType = CommandType.StoredProcedure;

            foreach (SqlParameter spParameter in parameters)
            {
                commandObject.Parameters.Add(spParameter);
            }

            return commandObject;

        }

        private static SqlCommand SetUpStoredProcedureCommandAndParameters(string storedProcedure, SqlConnection dbConnection, List<SqlParameter> parameters, SqlTransaction transaction)
        {
            SqlCommand commandObject;

            commandObject = new SqlCommand(storedProcedure, dbConnection, transaction);

            commandObject.CommandType = CommandType.StoredProcedure;

            foreach (SqlParameter spParameter in parameters)
            {
                commandObject.Parameters.Add(spParameter);
            }

            return commandObject;
        }
        #endregion


        /// <summary>
        /// This function adds a parameter to the parameters arraylist. The array list should already be instantiated (if not an exception is thrown) and the consumer of this function defines the name, value, type and size of the parameter to be added to the collection. All parameters added will be input parameters.
        /// </summary>
        /// <param name="parameters">An array list passed in by reference to which the parameter is added.</param>
        /// <param name="parameterName">The name of the paramater to be added to the parameters array list.</param>
        /// <param name="parameterValue">The value of the parameter to be added to the parameters array list.</param>
        /// <param name="parameterType">The type (as a SQLDbType) of the parameter.</param>
        /// <param name="parameterSize">The size of the parameter.</param>
        public static void AddParameter(ref List<SqlParameter> parameters, string parameterName, object parameterValue, SqlDbType parameterType, int parameterSize)
        {
            SqlParameter parameter = new SqlParameter(parameterName, parameterType, parameterSize);
            parameter.Direction = ParameterDirection.Input;
            if (parameterValue == null)
                parameter.Value = System.DBNull.Value;
            else
                parameter.Value = parameterValue;

            parameters.Add(parameter);
        }

        public static void AddNullableParameter(bool nullIfPositive, ref List<SqlParameter> parameters, string parameterName, object parameterValue, SqlDbType parameterType, int parameterSize)
        {
            if (nullIfPositive)
                AddParameter(ref parameters, parameterName, DBNull.Value, parameterType, parameterSize);
            else
                AddParameter(ref parameters, parameterName, parameterValue, parameterType, parameterSize);
        }
    }
}
