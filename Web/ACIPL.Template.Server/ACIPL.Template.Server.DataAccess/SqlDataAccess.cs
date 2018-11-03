using ACIPL.Template.Server.DataAccess.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ACIPL.Template.Server.DataAccess
{
    public class SqlDataAccess : IDataAccess
    {
        private readonly IDatabaseConnection databaseConnection;
        private readonly ICommandFactory commandFactory;

        public SqlDataAccess(IDatabaseConnection databaseConnection,
                             ICommandFactory commandFactory)
        {
            this.databaseConnection = databaseConnection;
            this.commandFactory = commandFactory;
        }

        public IEnumerable<IDataRecord> ExecuteReader(string command, List<Parameter> parameters,
                                                      CommandType commandType = CommandType.Text)
        {
            var sqlCeCommand = commandFactory.GetSqlCommand(command, parameters, commandType);
            using (var con = databaseConnection.SqlConnection)
            {
                sqlCeCommand.Connection = con;
                sqlCeCommand.CommandTimeout = 2400;
                con.Open();
                var rdr = sqlCeCommand.ExecuteReaderWithLog();
                while (rdr.Read())
                {
                    yield return rdr;
                }
                con.Close();
            }
        }

        public IEnumerable<IDataRecord> ExecuteReader(string command, string connectionStringName, List<Parameter> parameters,
                                                      CommandType commandType = CommandType.Text)
        {
            var sqlCeCommand = commandFactory.GetSqlCommand(command, parameters, commandType);
            using (var con = databaseConnection.GetConnectionString(connectionStringName))
            {
                sqlCeCommand.Connection = con;
                sqlCeCommand.CommandTimeout = 1800;
                con.Open();
                var rdr = sqlCeCommand.ExecuteReaderWithLog();
                while (rdr.Read())
                {
                    yield return rdr;
                }
                con.Close();
            }
        }

        public int ExecuteNonQuery(string command, List<Parameter> parameters,
                                   CommandType commandType = CommandType.Text)
        {
            var sqlCeCommand = commandFactory.GetSqlCommand(command, parameters, commandType);
            using (var con = databaseConnection.SqlConnection)
            {
                sqlCeCommand.Connection = con;
                con.Open();
                var result = sqlCeCommand.ExecuteNonQueryWithLog();
                con.Close();
                return result;
            }
        }

        public object ExecuteScalar(string command, List<Parameter> parameters = null,
                                    CommandType commandType = CommandType.Text)
        {
            var sqlCeCommand = commandFactory.GetSqlCommand(command, parameters, commandType);
            using (var con = databaseConnection.SqlConnection)
            {
                sqlCeCommand.Connection = con;
                con.Open();
                var result = sqlCeCommand.ExecuteScalarWithLog();
                con.Close();
                return result;
            }
        }

        public string DataSource
        {
            get { return databaseConnection.DataSource; }
        }

        public string HostingEnvironment
        {
            get { return databaseConnection.HostingEnvironment; }
        }


        public IEnumerable<IDataRecord> ExecuteReaderSync(string command, List<Parameter> parameters, CommandType commandType = CommandType.Text)
        {
            var sqlCeCommand = commandFactory.GetSqlCommand(command, parameters, commandType);
            using (var con = databaseConnection.SqlConnection)
            {
                sqlCeCommand.Connection = con;
                sqlCeCommand.CommandTimeout = 1800;
                con.Open();
                var rdr = sqlCeCommand.ExecuteReaderWithLog();
                while (rdr.Read())
                {
                    yield return rdr;
                }
                con.Close();
            }
        }


        public DataTable ExecuteCommand(string command, List<Parameter> parameters = null, CommandType commandType = CommandType.Text)
        {
            DataTable table = new DataTable();
            var sqlCommand = commandFactory.GetSqlCommand(command, parameters, commandType);

            using (var con = databaseConnection.SqlConnection)
            {
                sqlCommand.Connection = con;
                sqlCommand.CommandTimeout = 2000;
                con.Open();
                using (var da = new SqlDataAdapter(sqlCommand))
                {
                    da.Fill(table);
                }
                con.Close();
            }
            return table;
        }
    }
}