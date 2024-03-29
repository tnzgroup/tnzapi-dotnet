﻿using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Core.Interfaces
{
    public interface IApiResult
    {
        public ResultCode Result { get; set; }

        public List<string> ErrorMessage { get; set; }
    }
}
