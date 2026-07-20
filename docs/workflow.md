# Workflow

Trigger a pre-configured, no-code Workflow Template (built via the Dashboard) that can span multiple channels and automation steps. Workflow is the only messaging module with no `Message`/`TemplateID` text content, no `Status`, no `Received`, and no `client.Actions.Workflow` — just Send.

→ [Common parameters & authentication](../README.md#common-parameters)

## Quick example

Workflow is genuinely omni-channel: unlike every other module, `toNumber`, `mainPhone`, and `emailAddress` can all be set together for the *same* recipient — the Workflow Template decides which channel(s) actually get used.

```csharp
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
```

## Parameters (`WorkflowModel`)

| Parameter | Type | Required | Description |
|---|---|---|---|
| `WorkflowTemplateID` | `string?` | Yes | ID of the Workflow Template to trigger (built in the Dashboard). |
| `Recipients` | `ICollection<Recipient>?` | Yes† | One or more recipients — see Destination fields (`Recipient`) below. |
| `Destinations` | `ICollection<Destination>?` | Yes† | Alternative to `Recipients`; preferred for new code. See Destination fields (`Destination`) below. |
| `ContactID` | `ContactID?` | No | Single addressbook contact to send to (alternative/addition to `Recipients`). |
| `GroupID` | `GroupID?` | No | Single addressbook group to send to (alternative/addition to `Recipients`). |
| `Reference` | `string?` | No | Your internal reference, returned in reports and webhooks. |
| `SendTime` | `DateTime?` | No | Schedule delivery — combine with `Timezone`. |
| `Timezone` | `string?` | No | Windows Timezone name for `SendTime`. |
| `SubAccount` | `string?` | No | Sub-account code for billing separation. |
| `Department` | `string?` | No | Department code. |
| `MessageID` | `MessageID?` | No | Supply your own message ID (otherwise auto-generated). |
| `WebhookCallbackURL` | `string?` | No | URL for delivery status callbacks. |
| `WebhookCallbackFormat` | `Enums.WebhookCallbackType?` | No | Callback format. |
| `NotificationType` | `Enums.NotificationType?` | No | Notification delivery mode. |
| `SendMode` | `Enums.SendModeType` | No | Set `Test` to validate without sending. Default `Live`. |

†Set directly via `Recipients`/`Destinations`, via the named `SendMessage(...)` shortcut's `toNumber`/`mainPhone`/`emailAddress`/`contactID`/`groupID`/`contactIDs`/`groupIDs`/`destination`/`destinations`/`recipients` parameters, or via `WorkflowBuilder.AddRecipient(...)`/`AddRecipients(...)`/`AddDestination(...)`/`AddDestinations(...)`.

**Named-parameter shortcut**: `client.Messaging.Workflow.SendMessage(...)` also accepts `workflowTemplateId`, `destination`, `toNumber`, `mainPhone`, `emailAddress`, `contactID`, `groupID`, `contactIDs`, `groupIDs`, `recipients`, `destinations`, `sendMode` directly — this covers nearly the entire model, since Workflow has no message-content or attachment fields. For `SendTime`, `SubAccount`, etc. use `WorkflowBuilder` or construct a `WorkflowModel` directly.

## Destination fields (`Recipient`)

| Field | Description |
|---|---|
| `MobileNumber` | Phone destination, e.g. `"+64211111111"` — sent as the `toNumber` shortcut's destination. |
| `PhoneNumber` | Phone destination sent as the `mainPhone` shortcut's destination — a separate wire field from `MobileNumber`, letting a Workflow Template distinguish the two. |
| `EmailAddress` | Email destination — can be set **alongside** `MobileNumber`/`PhoneNumber` on the same `Recipient` for omni-channel Workflow Templates. |
| `ContactID` | Addressbook contact reference — sends to that contact instead of raw addresses. |
| `GroupID` | Addressbook group reference — sends to all members of that group. |
| `GroupCode` | Alternative group lookup by code. |
| `CompanyName` / `Attention` / `FirstName` / `LastName` / `Custom1`–`Custom9` | Personalisation values passed through to whichever channel(s) the Workflow Template actually uses. |

## Destination fields (`Destination`)

Prefer `Destination` over `Recipient` for new code — it mirrors TNZ's wire "Destinations Object" 1:1. Unlike other channels, Workflow's wire object reads every address field on `Destination` (not just the primary `Recipient` field), so — matching the omni-channel behaviour above — you can set `ToNumber`, `MainPhone`, and `EmailAddress` together on the **same** `Destination` for one recipient; the Workflow Template decides which channel(s) actually get used. `AddDestination(...)`/`AddDestinations(...)` on the builder, or the `destination`/`destinations` named-parameter shortcuts, populate this collection instead of `Recipients`. Converting from `Recipient` (e.g. via `AddRecipient(Recipient)` or the `recipients`/`toNumber`/`mainPhone`/`emailAddress` shortcuts) maps `Recipient.MobileNumber`/`PhoneNumber`/`EmailAddress` onto `Destination.ToNumber`/`MainPhone`/`EmailAddress` respectively — the primary `Recipient` field is left unset in that case, since there's no single "primary" channel to pick for an omni-channel recipient.

| Field | Description |
|---|---|
| `Recipient` | Primary destination address — set this directly if you want a single channel-agnostic value; leave unset and use the fields below for explicit multi-channel targeting. |
| `ToNumber` | Phone destination, e.g. `"+64211111111"` — sent as the `toNumber` shortcut's destination. |
| `MainPhone` | Phone destination sent as the `mainPhone` shortcut's destination — a separate wire field from `ToNumber`, letting a Workflow Template distinguish the two. |
| `EmailAddress` | Email destination — can be set **alongside** `ToNumber`/`MainPhone` on the same `Destination` for omni-channel Workflow Templates. |
| `MobilePhone` / `FaxNumber` | Additional channel-specific alternate address fields, shared with every other module's `Destination`. |
| `ContactID` | Addressbook contact reference — sends to that contact instead of raw addresses. |
| `GroupID` | Addressbook group reference — sends to all members of that group. |
| `GroupCode` | Alternative group lookup by code. |
| `Company` / `Attention` / `FirstName` / `LastName` / `Custom1`–`Custom9` | Personalisation values passed through to whichever channel(s) the Workflow Template actually uses. |

## Code samples

### Single recipient, multi-channel shorthand

```csharp
var response = client.Messaging.Workflow.SendMessage(
    workflowTemplateId: "a1b2c3d4-e5f6-7890-1234-567890abcdef",
    toNumber: "+64211111111",
    emailAddress: "test@example.com",
    sendMode: Enums.SendModeType.Test
);
```

### Via the builder

```csharp
using var builder = new WorkflowBuilder();

var model = builder
    .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
    .AddRecipient("+64211111111")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.Workflow.SendMessage(model);
```

### Multiple recipients

See `WorkflowSamples.SendToMultipleRecipients()`.

```csharp
using var builder = new WorkflowBuilder();

var model = builder
    .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
    .AddRecipient("+64211111111")
    .AddRecipient("+64222222222")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.Workflow.SendMessage(model);
```

### Scheduled send

See `WorkflowSamples.SendScheduled()`.

```csharp
using var builder = new WorkflowBuilder();

var model = builder
    .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
    .AddRecipient("+64211111111")
    .SetSendTime(DateTime.Now.AddDays(1))
    .SetTimezone("New Zealand")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.Workflow.SendMessage(model);
```

## Response

- `SendMessage(...)` → `WorkflowApiResult`: `Result`, `MessageID`, `ErrorMessage`. Workflow has no `Status`, `Received`, or Action methods.

## See also

- [README — Workflow](../README.md#workflow)
- [Samples — WorkflowSamples.cs](../TNZAPI.NET.Samples/Messaging/Workflow/WorkflowSamples.cs)