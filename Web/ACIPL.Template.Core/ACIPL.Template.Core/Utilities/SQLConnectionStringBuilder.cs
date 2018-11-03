using System.Data.SqlClient;

namespace ACIPL.Template.Core.Utilities
{
    public interface ISQLConnectionStringBuilder
    {
        string DatabaseName { get; }
        string ServerName { get; }
        string UserId { get; }
        string Password { get; }
        void Build(string connectionString);
    }

    public class SQLConnectionStringBuilder : ISQLConnectionStringBuilder
    {
        private SqlConnectionStringBuilder connBuilder;
        private readonly IConfigurationManager configurationManager;
        public SQLConnectionStringBuilder(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }
        public string DatabaseName
        {
            get
            {
                return connBuilder.InitialCatalog;
            }
        }

        public string Password
        {
            get
            {
                return connBuilder.Password;
            }
        }

        public string ServerName
        {
            get
            {
                return connBuilder.DataSource;
            }
        }

        public string UserId
        {
            get
            {
                return connBuilder.UserID;
            }
        }

        public void Build(string connectionString)
        {
            connBuilder = new SqlConnectionStringBuilder(CryptoEngine.Decrypt(connectionString, configurationManager.GetConfigurationValue("SymmetricKey")));
        }
    }
}
