using TNZAPI.NET.Core.Interfaces.Messaging;

namespace TNZAPI.NET.Core.Interfaces
{
    public interface IMessagingApi
    {
        public IEmailApi Email { get; set; }
        public IFaxApi Fax { get; set; }
        public ISMSApi SMS { get; set; }
        public ITTSApi TTS { get; set; }
        public IVoiceApi Voice { get; set; }
    }
}
