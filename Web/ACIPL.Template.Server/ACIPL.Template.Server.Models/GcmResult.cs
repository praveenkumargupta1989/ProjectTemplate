using System.Collections.Generic;

namespace ACIPL.Template.Server.Models
{
    public class GcmResult
    {
        public long multicast_id { get; set; }
        public bool success { get; set; }
        public bool failure { get; set; }
        public int canonical_ids { get; set; }
        public List<GcmResultData> results { get; set; }
    }
    public class GcmResultData
    {
        public string error { get; set; }
    }
}
