using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataServer
{
    public class DBNullReturnValues
    {

        public static int Return0(object dataReaderValue)
        {
            if (dataReaderValue == DBNull.Value)
                return 0;
            else
                return Convert.ToInt32(dataReaderValue);
        }

        public static long LongReturn0(object dataReaderValue)
        {
            if (dataReaderValue == DBNull.Value)
                return 0;
            else
                return Convert.ToInt32(dataReaderValue);
        }

        public static decimal DecimalReturn0(object dataReaderValue)
        {
            if (dataReaderValue == DBNull.Value)
                return 0;
            else
                return Convert.ToDecimal(dataReaderValue);
        }

        public static string ReturnZeroLength(object dataReaderValue)
        {
            if (dataReaderValue == DBNull.Value)
                return "";
            else
                return dataReaderValue.ToString();
        }

        public static DateTime ReturnMinDate(object dataReaderValue)
        {
            if (dataReaderValue == DBNull.Value)
                return DateTime.MinValue;
            else
                return Convert.ToDateTime(dataReaderValue);
        }

        public static bool ReturnFalse(object dataReaderValue)
        {
            if (dataReaderValue == DBNull.Value)
                return false;
            else
                return Convert.ToBoolean(dataReaderValue);
        }

        public static bool? ReturnBooleanNull(object dataReaderValue)
        {
            if (dataReaderValue == DBNull.Value)
                return null;
            else
                return Convert.ToBoolean(dataReaderValue);
        }

        public static long? ReturnLongNull(object dataReaderValue)
        {
            if (dataReaderValue == DBNull.Value)
                return null;
            else
                return Convert.ToInt32(dataReaderValue);
        }

        public static int? ReturnIntNull(object dataReaderValue)
        {
            if (dataReaderValue == DBNull.Value)
                return null;
            else
                return Convert.ToInt32(dataReaderValue);
        }

        public static DateTime? ReturnDateTimeNull(object dataReaderValue)
        {
            if (dataReaderValue == DBNull.Value)
                return null;
            else
                return Convert.ToDateTime(dataReaderValue);
        }

        public static decimal? ReturnDecimalNull(object dataReaderValue)
        {
            if (dataReaderValue == DBNull.Value)
                return null;
            else
                return Convert.ToDecimal(dataReaderValue);
        }
    }

    public class DBBooleanValues
    {
        public static bool ReturnBooleanFromYesOrNo(object dataReaderValue)
        {
            if (dataReaderValue.ToString().ToLower() == "yes")
                return true;
            else
                return false;
        }
    }
}
