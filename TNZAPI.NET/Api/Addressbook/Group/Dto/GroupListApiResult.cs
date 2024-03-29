﻿using System.Xml.Serialization;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Addressbook.Group.Dto
{
    [XmlType(TypeName = "root")]
    public class GroupListApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }

        public int TotalRecords { get; set; } = 0;

        public int RecordsPerPage { get; set; } = 100;

        public int PageCount { get; set; } = 0;

        public int Page { get; set; } = 1;

        public List<GroupModel> Groups { get; set; }

        [XmlElement("ErrorMessage")]
        public List<string> ErrorMessage { get; set; }
    }
}
