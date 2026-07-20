using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Api.Configuration.OptOut.Dto
{
    public class OptOutBatchModel
    {
        public string DestType { get; set; } = "";
        public string Destination { get; set; } = "";
        public ICollection<string>? Destinations { get; set; } = new List<string>();
        public ContactID? ContactID { get; set; }
        public ICollection<ContactID>? ContactIDs { get; set; } = new List<ContactID>();
        public string SubAccount { get; set; } = "";
        public string Department { get; set; } = "";
    }
}