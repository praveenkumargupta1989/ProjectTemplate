using System;
using System.Data.SqlClient;

namespace ACIPL.Template.Server.DataAccess.Extensions
{
    public static class AddSqlParameterExtension
    {
        public static string NullableDatetime = "1/1/0001 12:00:00 AM";
        public static SqlParameter AddParameter(this SqlParameterCollection parms, SqlParameter param)
        {
            if (param.Value == null || param.Value.ToString() == NullableDatetime)
            {
                param.Value = DBNull.Value;
                return parms.Add(param);
            }

            return parms.Add(param);
        }
    }
}