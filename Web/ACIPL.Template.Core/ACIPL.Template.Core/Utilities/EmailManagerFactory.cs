namespace ACIPL.Template.Core.Utilities
{
    public static class EmailManagerFactory
    {
        public static IEmailManager GetEmailManager()
        {
            return new EmailManager(new ConfigurationManager());
        }
    }
}
