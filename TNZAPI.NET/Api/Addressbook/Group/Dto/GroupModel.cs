using System.Xml.Serialization;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Addressbook.Group.Dto
{
    [XmlType(TypeName = "Group")]
    public class GroupModel
    {
        public GroupID GroupID { get; set; }

        public string GroupCode { get; set; }

        public string GroupName { get; set; }

        public string SubAccount { get; set; }

        public string Department { get; set; }

        public DateTime Created 
        {
            get
            {
                return CreatedUTC.ChangeToLocalDateTime();
            }
        }

        [XmlElement("CreatedTimeUTC_RFC3339")]
        public DateTime CreatedUTC { get; set; }

        public Enums.ViewEditByOptions? ViewEditBy { get; set; }

        public string Owner { get; set; }
    }
}
