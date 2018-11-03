namespace ACIPL.Template.Core.Utilities
{
    public class BCPResponse
    {
        public string StandardOutput { get; set; }
        public string StandardError { get; set; }
        public bool IsSuccessful
        {
            get
            {
                return !StandardOutput.Contains("Error");
            }
        }
    }
}
