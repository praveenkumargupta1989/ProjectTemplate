using ACIPL.Template.Server.DataAccess.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ACIPL.Template.Server.DataAccess
{
    public interface ICommandFactory
    {
        SqlCommand GetSqlCommand(string commandText, List<Parameter> parameters = null,
                                 CommandType commandType = CommandType.Text);
    }

    public class CommandFactory : ICommandFactory
    {
        public SqlCommand GetSqlCommand(string commandText, List<Parameter> parameters = null,
                                        CommandType commandType = CommandType.Text)
        {
            var sqlCommand = new SqlCommand { CommandText = commandText, CommandType = commandType };
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    sqlCommand.Parameters.AddParameter(new SqlParameter(parameter.Name, parameter.Value));
                }
            }
            return sqlCommand;
        }
    }
}