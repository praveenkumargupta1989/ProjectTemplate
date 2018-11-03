using System;
using System.Collections;

namespace ACIPL.Template.Client.Web.Models
{
    public class RestApiRequest
    {
        public Uri WebApiUri { get; set; }
        public string ActionName { get; set; }
        public string RequestBody { get; set; }
        public IDictionary Headers { get; set; }
        public Object RequestObject { get; set; }
    }
}