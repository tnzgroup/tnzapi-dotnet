using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Addressbook.Group.Dto
{
    public class GroupDetail
    {
        public GroupID? GroupID { get; set; }
        public string? GroupCode { get; set; }
        public string? GroupName { get; set; }
        public string? SubAccount { get; set; }
        public string? Department { get; set; }
        public Enums.ViewEditByOptions? ViewEditBy { get; set; }
        public Enums.AccessControlLevel? AccessControl { get; set; }
        public string? Owner { get; set; }
        public DateTime? CreatedTimeLocal { get; set; }
        public DateTime? CreatedTimeUTC { get; set; }
        public DateTime? CreatedTimeUTC_RFC3339 { get; set; }
        public string? Timezone { get; set; }
    }
}