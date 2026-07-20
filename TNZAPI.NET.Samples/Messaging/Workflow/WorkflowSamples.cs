using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Workflow;
using TNZAPI.NET.Api.Messaging.Workflow.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Messaging.Workflow
{
    /// <summary>
    /// Reference code demonstrating client.Messaging.Workflow. Workflow has no Status, Received,
    /// or client.Actions.Workflow — just Send.
    /// This class is not a runnable program — call these methods from your own application.
    /// Full parameter reference: docs/workflow.md.
    /// </summary>
    public class WorkflowSamples
    {
        private readonly ITNZAuth apiUser;

        public WorkflowSamples()
        {
            apiUser = new TNZApiUser();
        }

        public WorkflowSamples(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public WorkflowApiResult Send()
        {
            // Workflow triggers a pre-configured, no-code Workflow Template (built via the Dashboard).
            // It's the only messaging module with no Message/TemplateID content, no Status, no Received,
            // and no client.Actions.Workflow — just Send.
            var client = new TNZApiClient(apiUser);

            // toNumber/mainPhone/emailAddress can all be set together for the same recipient -
            // Workflow is genuinely omni-channel, unlike every other messaging module.
            var response = client.Messaging.Workflow.SendMessage(
                workflowTemplateId: "a1b2c3d4-e5f6-7890-1234-567890abcdef",
                toNumber: "+64211111111",
                emailAddress: "test@example.com",
                sendMode: Enums.SendModeType.Test
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }
            else
            {
                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- Error={error}");
                }
            }

            return response;
        }

        public WorkflowApiResult SendUsingBuilder()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new WorkflowBuilder();

            var model = builder
                .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
                .AddDestination("+64211111111")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            return client.Messaging.Workflow.SendMessage(model);
        }

        public WorkflowApiResult SendUsingDestination()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new WorkflowBuilder();

            // Unlike other modules, Workflow's Destination reads ToNumber/MainPhone/EmailAddress
            // directly (not the primary Recipient field) — set several together on the same
            // Destination for one omni-channel recipient.
            var model = builder
                .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
                .AddDestination(new Destination { ToNumber = "+64211111111", EmailAddress = "test@example.com" })
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            return client.Messaging.Workflow.SendMessage(model);
        }

        public WorkflowApiResult SendToMultipleRecipients()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new WorkflowBuilder();

            var model = builder
                .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
                .AddDestination("+64211111111")
                .AddDestination("+64222222222")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            return client.Messaging.Workflow.SendMessage(model);
        }

        public WorkflowApiResult SendScheduled()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new WorkflowBuilder();

            var model = builder
                .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
                .AddDestination("+64211111111")
                .SetSendTime(DateTime.Now.AddDays(1))
                .SetTimezone("New Zealand")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            return client.Messaging.Workflow.SendMessage(model);
        }
    }
}