using ACIPL.Template.Core.Logging;
using System;

namespace ACIPL.Template.Core.Utilities
{
    public interface IConfigurationManager
    {
        string GetConfigurationValue(string key);
        string GetConnectionString(string key);
        string GetDecryptedConfigurationValue(string key);
    }

    /// <summary>
    /// Provides access to configuration data used by Application.
    /// </summary>
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly ILogger Logger = LoggerFactory.GetLogger();

        /// <summary>
        /// Gets the value of an optional key from config file.
        /// </summary>
        public string GetConfigurationValue(string key)
        {
            try
            {
                return ReadConfigValue(key);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the value of an optional key from config file.
        /// </summary>
        public string GetConnectionString(string key)
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetDecryptedConfigurationValue(string key)
        {
            try
            {
                var value = ReadConfigValue(key);
                if (value != string.Empty)
                    return CryptoEngine.Decrypt(value, ReadConfigValue("SymmetricKey"));
                return string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                throw;
            }
        }

        private string ReadConfigValue(string key)
        {
            if (System.Configuration.ConfigurationManager.AppSettings[key] == null)
            {
                throw new Exception(key);
            }

            var value = System.Configuration.ConfigurationManager.AppSettings[key];
            return value;
        }
    }
}
