using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Tests.Helpers;

public class RequestResultHelperTests
{
    private class FakeApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }
        public List<string> ErrorMessage { get; set; } = new();
    }

    [Fact]
    public void RespondError_SetsFailedResultAndMessage()
    {
        var result = ResultHelper.RespondError<FakeApiResult>("Empty sender: Please specify Sender");

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Single(result.ErrorMessage);
        Assert.Equal("Empty sender: Please specify Sender", result.ErrorMessage[0]);
    }
}