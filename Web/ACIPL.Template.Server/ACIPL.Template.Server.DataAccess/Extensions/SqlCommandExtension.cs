using ACIPL.Template.Core.Logging;
using System.Data.SqlClient;
using System.Text;

namespace ACIPL.Template.Server.DataAccess.Extensions
{
    public static class SqlCommandExtension
    {
        private static readonly Logger Logger = LoggerFactory.GetLogger();

        public static SqlDataReader ExecuteReaderWithLog(this SqlCommand sqlCommand)
        {
            LogSqlCommand(sqlCommand);
            return sqlCommand.ExecuteReader();
        }

        public static int ExecuteNonQueryWithLog(this SqlCommand sqlCommand)
        {
            LogSqlCommand(sqlCommand);
            return sqlCommand.ExecuteNonQuery();
        }

        public static object ExecuteScalarWithLog(this SqlCommand sqlCommand)
        {
            LogSqlCommand(sqlCommand);
            return sqlCommand.ExecuteScalar();
        }

        private static void LogSqlCommand(SqlCommand sqlCommand)
        {
            var logText = new StringBuilder();
            logText.Append(string.Format("sql: {0} ", sqlCommand.CommandText));

            for (int i = 0; i < sqlCommand.Parameters.Count; i++)
            {
                logText.Append(string.Format("{0}:{1}|", sqlCommand.Parameters[i].ParameterName, sqlCommand.Parameters[i].Value));
            }

            Logger.Info(logText.ToString());
        }
    }
}