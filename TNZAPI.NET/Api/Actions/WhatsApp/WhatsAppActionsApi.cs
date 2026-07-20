using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.WhatsApp;
using TNZAPI.NET.Api.Messaging.WhatsApp.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Actions;

namespace TNZAPI.NET.Api.Actions.WhatsApp
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class WhatsAppActionsApi : IWhatsAppActionsApi
    {
        private readonly WhatsAppApi _whatsApp;

        public WhatsAppActionsApi(ITNZAuth auth)
        {
            _whatsApp = new WhatsAppApi(auth);
        }

        public WhatsAppActionApiResult Reschedule(MessageID messageID, DateTime sendTime)
        {
            return _whatsApp.Reschedule(messageID, sendTime);
        }

        [ComVisible(false)]
        public Task<WhatsAppActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime)
        {
            return _whatsApp.RescheduleAsync(messageID, sendTime);
        }

        public WhatsAppActionApiResult Abort(MessageID messageID)
        {
            return _whatsApp.Abort(messageID);
        }

        [ComVisible(false)]
        public Task<WhatsAppActionApiResult> AbortAsync(MessageID messageID)
        {
            return _whatsApp.AbortAsync(messageID);
        }
    }
}