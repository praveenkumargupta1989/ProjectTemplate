namespace ACIPL.Template.Core.Utilities
{
    public class BCPReqest
    {
        public string SqlObject { get; set; }
        public string FilePath { get; set; }
        public string ConnectionString { get; set; }
        public string DataType { get; set; }
        public string OverrideDelimiter { get; set; }
        public string Seperator { get; set; }
        public string BCPUtilityPath { get; set; }
        public string ResponseFilePath { get; set; }
    }
}