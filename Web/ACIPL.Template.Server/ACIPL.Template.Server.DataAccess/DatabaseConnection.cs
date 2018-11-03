using ACIPL.Template.Core.Utilities;
using System.Data.SqlClient;

namespace ACIPL.Template.Server.DataAccess
{
    public interface IDatabaseConnection
    {
        SqlConnection SqlConnection { get; }
        string DataSource { get; }
        string HostingEnvironment { get; }
        SqlConnection GetConnectionString(string connectionStringName);
    }

    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly IConfigurationManager configurationManager;
        private readonly string dbName = string.Empty;

        public DatabaseConnection(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }

        public SqlConnection SqlConnection
        {
            get
            {
                var connectionString = configurationManager.GetConnectionString("ConnectionString");
                var conn = CryptoEngine.Decrypt(connectionString, configurationManager.GetConfigurationValue("SymmetricKey"));
                return new SqlConnection(conn);
            }
        }

        public string DataSource
        {
            get { return dbName; }
        }

        public string HostingEnvironment
        {
            get { return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); }
        }

        public SqlConnection GetConnectionString(string connectionStringName)
        {
            return new SqlConnection(configurationManager.GetConnectionString(connectionStringName));
        }
    }
}