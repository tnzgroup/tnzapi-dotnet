namespace TNZAPI.NET.Api.Messaging.Common
{
    public static class Enums
    {
        public enum SendModeType { Live, Test };

        public enum MessageType { Email, Text, Voice }

        public enum WebhookCallbackType { JSON, XML };

        public enum ResultCode { Failed, Success };

        public enum StatusCode { Unknown, Received, Pending, Delayed, Remote, Error, CreditHold, Completed, Transmit };
    }
}
