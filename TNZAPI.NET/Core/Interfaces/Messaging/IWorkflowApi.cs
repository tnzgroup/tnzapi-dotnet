using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Workflow.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Core.Interfaces.Messaging
{
    public interface IWorkflowApi
    {
        WorkflowApiResult SendMessage(WorkflowModel entity);
        Task<WorkflowApiResult> SendMessageAsync(WorkflowModel entity);

        WorkflowApiResult SendMessage(
            string? workflowTemplateId = null,
            object? destination = null,
            string? toNumber = null,
            string? mainPhone = null,
            string? emailAddress = null,
            string? contactID = null,
            string? groupID = null,
            ICollection<ContactID>? contactIDs = null,
            ICollection<GroupID>? groupIDs = null,
            ICollection<Recipient>? recipients = null,
            ICollection<Destination>? destinations = null,
            Enums.NotificationType? notificationType = null,
            Enums.SendModeType? sendMode = null
        );
        Task<WorkflowApiResult> SendMessageAsync(
            string? workflowTemplateId = null,
            object? destination = null,
            string? toNumber = null,
            string? mainPhone = null,
            string? emailAddress = null,
            string? contactID = null,
            string? groupID = null,
            ICollection<ContactID>? contactIDs = null,
            ICollection<GroupID>? groupIDs = null,
            ICollection<Recipient>? recipients = null,
            ICollection<Destination>? destinations = null,
            Enums.NotificationType? notificationType = null,
            Enums.SendModeType? sendMode = null
        );
    }
}
