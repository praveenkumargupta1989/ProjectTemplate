using System;

namespace ACIPL.Template.Core.Utilities
{
    public class CustomException : Exception
    {
        public CustomException(string customMessage) : base(customMessage)
        {

        }
    }
}
