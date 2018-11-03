using System.Collections.Generic;
using System.Data;

namespace ACIPL.Template.Server.DataAccess
{
    public interface IDataAccess
    {
        IEnumerable<IDataRecord> ExecuteReader(string command, List<Parameter> parameters = null,
                                               CommandType commandType = CommandType.Text);

        IEnumerable<IDataRecord> ExecuteReaderSync(string command, List<Parameter> parameters = null,
                                               CommandType commandType = CommandType.Text);

        IEnumerable<IDataRecord> ExecuteReader(string command, string connectionString, List<Parameter> parameters = null,
                                               CommandType commandType = CommandType.Text);

        int ExecuteNonQuery(string command, List<Parameter> parameters = null,
                            CommandType commandType = CommandType.Text);

        object ExecuteScalar(string command, List<Parameter> parameters = null,
                             CommandType commandType = CommandType.Text);

        DataTable ExecuteCommand(string command, List<Parameter> parameters = null,
                            CommandType commandType = CommandType.Text);

        string DataSource { get; }
        string HostingEnvironment { get; }
    }
}