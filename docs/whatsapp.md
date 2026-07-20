# WhatsApp

Send WhatsApp template messages, with optional fallback to another channel, via the TNZ REST API. WhatsApp requires **both** `Message` and `TemplateID` (not either/or like SMS) — find your Template ID in the Dashboard.

→ [Common parameters & authentication](../README.md#common-parameters)

## Quick example

```csharp
var response = client.Messaging.WhatsApp.SendMessage(
    messageText: "Hi [[FirstName]], your order has shipped!",
    templateId: "123e4567-e89b-12d3-a456-426614174000",
    toNumber: "+64211111111",
    sendMode: Enums.SendModeType.Test
);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Success - MessageID: {response.MessageID}");
}
```

## Parameters (`WhatsAppModel`)

| Parameter | Type | Required | Description |
|---|---|---|---|
| `Message` | `string?` | Yes | Message body. Supports personalisation tokens `[[FirstName]]`, `[[Custom1]]`, etc. |
| `TemplateID` | `string?` | Yes | Pre-approved WhatsApp template ID — required **alongside** `Message`, unlike SMS/RCS's either/or. |
| `Recipients` | `ICollection<Recipient>?` | Yes† | One or more recipients — see Destination fields (`Recipient`) below. |
| `Destinations` | `ICollection<Destination>?` | Yes† | Alternative to `Recipients` — sends the wire's primary `Recipient` field per destination; preferred for new code. See Destination fields (`Destination`) below. |
| `ContactID` | `ContactID?` | No | Single addressbook contact to send to (alternative/addition to `Recipients`). |
| `GroupID` | `GroupID?` | No | Single addressbook group to send to (alternative/addition to `Recipients`). |
| `Reference` | `string?` | No | Your internal reference, returned in reports and webhooks. |
| `FromNumber` | `string?` | No | Sender ID shown on the recipient's device. |
| `FallbackMode` | `ICollection<Enums.WhatsAppFallbackMode>?` | No | Fallback channel(s) if WhatsApp delivery fails, e.g. `SMS`. |
| `SendTime` | `DateTime?` | No | Schedule delivery — combine with `Timezone`. |
| `Timezone` | `string?` | No | Windows Timezone name for `SendTime`. |
| `SubAccount` | `string?` | No | Sub-account code for billing separation. |
| `Department` | `string?` | No | Department code. |
| `MessageID` | `MessageID?` | No | Supply your own message ID (otherwise auto-generated). |
| `ReportTo` | `string?` | No | Email address to receive delivery reports. |
| `WebhookCallbackURL` | `string?` | No | URL for delivery status callbacks. |
| `WebhookCallbackFormat` | `Enums.WebhookCallbackType?` | No | Callback format. |
| `NotificationType` | `Enums.NotificationType?` | No | Notification delivery mode. |
| `Files` | `ICollection<Attachment>?` | No | Media attachments. |
| `SendMode` | `Enums.SendModeType` | No | Set `Test` to validate without sending. Default `Live`. |

†Set directly via `Recipients`/`Destinations`, via the named `SendMessage(...)` shortcut's `toNumber`/`contactID`/`groupID`/`contactIDs`/`groupIDs`/`destination`/`destinations`/`recipients` parameters, or via `WhatsAppBuilder.AddRecipient(...)`/`AddRecipients(...)`/`AddDestination(...)`/`AddDestinations(...)`.

**Named-parameter shortcut**: `client.Messaging.WhatsApp.SendMessage(...)` also accepts `messageText`, `templateId`, `destination`, `toNumber`, `contactID`, `groupID`, `contactIDs`, `groupIDs`, `recipients`, `destinations`, `file`, `attachments`, `sendMode` directly. For `FallbackMode`, `SendTime`, `SubAccount`, etc. use `WhatsAppBuilder` or construct a `WhatsAppModel` directly.

## Destination fields (`Recipient`)

| Field | Description |
|---|---|
| `MobileNumber` | Destination phone number, e.g. `"+64211111111"`. |
| `ContactID` | Addressbook contact reference — sends to that contact instead of a raw number. |
| `GroupID` | Addressbook group reference — sends to all members of that group. |
| `GroupCode` | Alternative group lookup by code. |
| `CompanyName` | Personalisation token `[[Company]]`. |
| `Attention` | Personalisation token `[[Attention]]`. |
| `FirstName` | Personalisation token `[[FirstName]]`. |
| `LastName` | Personalisation token `[[LastName]]`. |
| `Custom1`–`Custom9` | Arbitrary per-recipient personalisation values, `[[Custom1]]` … `[[Custom9]]`. |

## Destination fields (`Destination`)

Prefer `Destination` over `Recipient` for new code — it mirrors TNZ's wire "Destinations Object" 1:1, including the primary `Recipient` field (sent as-is, channel-agnostic) plus the same channel-specific alternate address fields other channels' wire types use. `AddDestination(...)`/`AddDestinations(...)` on the builder, or the `destination`/`destinations` named-parameter shortcuts, populate this collection instead of `Recipients`.

| Field | Description |
|---|---|
| `Recipient` | Primary destination address, e.g. `"+64211111111"` — sent as-is regardless of channel. |
| `ToNumber` / `MobilePhone` / `MainPhone` / `FaxNumber` / `EmailAddress` | Channel-specific alternate address fields — set instead of/alongside `Recipient` if you need to target a different wire field explicitly. |
| `ContactID` | Addressbook contact reference — sends to that contact instead of a raw number. |
| `GroupID` | Addressbook group reference — sends to all members of that group. |
| `GroupCode` | Alternative group lookup by code. |
| `Company` | Personalisation token `[[Company]]`. |
| `Attention` | Personalisation token `[[Attention]]`. |
| `FirstName` | Personalisation token `[[FirstName]]`. |
| `LastName` | Personalisation token `[[LastName]]`. |
| `Custom1`–`Custom9` | Arbitrary per-recipient personalisation values, `[[Custom1]]` … `[[Custom9]]`. |

`new Destination("+64211111111")` sets only `Recipient`. Use the object initializer (`new Destination { Recipient = "...", FirstName = "..." }`) when you need personalisation fields alongside the number.

## Code samples

### Single recipient shorthand

```csharp
var response = client.Messaging.WhatsApp.SendMessage(
    messageText: "Hi [[FirstName]], your order has shipped!",
    templateId: "123e4567-e89b-12d3-a456-426614174000",
    toNumber: "+64211111111",
    sendMode: Enums.SendModeType.Test
);
```

### With SMS fallback, via the builder

```csharp
using var builder = new WhatsAppBuilder();

var model = builder
    .SetMessageText("Hi [[FirstName]], your order has shipped!")
    .SetTemplateID("123e4567-e89b-12d3-a456-426614174000")
    .AddRecipient("+64211111111")
    .AddFallbackMode(Enums.WhatsAppFallbackMode.SMS)
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.WhatsApp.SendMessage(model);
```

### Multiple recipients

See `WhatsAppSamples.SendToMultipleRecipients()`.

```csharp
using var builder = new WhatsAppBuilder();

var model = builder
    .SetMessageText("Hi [[FirstName]], your order has shipped!")
    .SetTemplateID("123e4567-e89b-12d3-a456-426614174000")
    .AddRecipient("+64211111111")
    .AddRecipient("+64222222222")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.WhatsApp.SendMessage(model);
```

### With an attachment

See `WhatsAppSamples.SendWithAttachment()`.

```csharp
using var builder = new WhatsAppBuilder();

var model = builder
    .SetMessageText("Here's your invoice.")
    .SetTemplateID("123e4567-e89b-12d3-a456-426614174000")
    .AddRecipient("+64211111111")
    .AddAttachment("Invoice.pdf", "[base64-encoded-file-data]")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.WhatsApp.SendMessage(model);
```

### Poll for status / inbound messages

```csharp
var status = client.Messaging.WhatsApp.Status(response.MessageID);

var received = client.Messaging.WhatsApp.Received(timePeriod: 1440);
if (received.Result == Enums.ResultCode.Success)
{
    foreach (var message in received.Messages)
    {
        Console.WriteLine($" => From: '{message.From}', MessageText: '{message.MessageText}'");
    }
}
```

## Response

- `SendMessage(...)` → `WhatsAppApiResult`: `Result`, `MessageID`, `ErrorMessage`.
- `Status(...)` → `WhatsAppStatusApiResult`: `MessageID`, `JobStatus`, `JobNum`, `Recipients` (each with `Destination`, `Status`, `Result`).
- `Received(...)` → `WhatsAppReceivedApiResult`: `Messages` (each with `From`, `MessageText`).
- `Reschedule(...)`/`Abort(...)` → `WhatsAppActionApiResult`: `Action`, `Status`.

## See also

- [README — WhatsApp](../README.md#whatsapp)
- [Samples — WhatsAppSamples.cs](../TNZAPI.NET.Samples/Messaging/WhatsApp/WhatsAppSamples.cs)