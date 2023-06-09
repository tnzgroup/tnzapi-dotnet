namespace TNZAPI.NET.Core
{
    public abstract class _Result
    {
        public enum ResultCode { Failed, Success };

        public ResultCode Result { get; set; } = ResultCode.Failed;

        public string ErrorMessage { get; set; } = "";

    }
}
