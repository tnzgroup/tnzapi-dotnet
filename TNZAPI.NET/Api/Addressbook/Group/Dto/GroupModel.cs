using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Addressbook.Group.Dto
{
    public class GroupModel
    {
        public string GroupCode { get; set; }

        public string GroupName { get; set; }

        public string SubAccount { get; set; }

        public string Department { get; set; }

        public Enums.ViewEditByOptions? ViewEditBy { get; set; }

        public string Owner { get; set; }
    }
}
