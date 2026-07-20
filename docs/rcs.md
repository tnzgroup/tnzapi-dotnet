# RCS

Send Rich Communication Services messages via the TNZ REST API. Like SMS, `Message`/`TemplateID` are either/or (not both required, unlike WhatsApp).

→ [Common parameters & authentication](../README.md#common-parameters)

## Quick example

```csharp
var response = client.Messaging.RCS.SendMessage(
    messageText: "Test RCS message",
    toNumber: "+64211111111",
    sendMode: Enums.SendModeType.Test
);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Success - MessageID: {response.MessageID}");
}
```

## Parameters (`RCSModel`)

| Parameter | Type | Required | Description |
|---|---|---|---|
| `Message` | `string?` | Yes* | Message body. Supports personalisation tokens `[[FirstName]]`, `[[Custom1]]`, etc. |
| `TemplateID` | `string?` | Yes* | Pre-configured message template ID (alternative to `Message`). |
| `Recipients` | `ICollection<Recipient>?` | Yes† | One or more recipients — see Destination fields (`Recipient`) below. |
| `Destinations` | `ICollection<Destination>?` | Yes† | Alternative to `Recipients` — sends the wire's primary `Recipient` field per destination; preferred for new code. See Destination fields (`Destination`) below. |
| `ContactID` | `ContactID?` | No | Single addressbook contact to send to (alternative/addition to `Recipients`). |
| `GroupID` | `GroupID?` | No | Single addressbook group to send to (alternative/addition to `Recipients`). |
| `Reference` | `string?` | No | Your internal reference, returned in reports and webhooks. |
| `FromNumber` | `string?` | No | Sender ID shown on the recipient's device. |
| `SendTime` | `DateTime?` | No | Schedule delivery — combine with `Timezone`. |
| `Timezone` | `string?` | No | Windows Timezone name for `SendTime`. |
| `SubAccount` | `string?` | No | Sub-account code for billing separation. |
| `Department` | `string?` | No | Department code. |
| `MessageID` | `MessageID?` | No | Supply your own message ID (otherwise auto-generated). |
| `ReportTo` | `string?` | No | Email address to receive delivery reports. |
| `WebhookCallbackURL` | `string?` | No | URL for delivery status callbacks. |
| `WebhookCallbackFormat` | `Enums.WebhookCallbackType?` | No | Callback format. |
| `NotificationType` | `Enums.NotificationType?` | No | Notification delivery mode. |
| `SMSEmailReply` | `string?` | No | Email address to receive replies. |
| `CharacterConversion` | `bool` | No | Convert characters outside the GSM character set automatically. Default `false`. |
| `Files` | `ICollection<Attachment>?` | No | Media attachments. |
| `SendMode` | `Enums.SendModeType` | No | Set `Test` to validate without sending. Default `Live`. |

\*Either `Message` or `TemplateID` must be provided.
†Set directly via `Recipients`/`Destinations`, via the named `SendMessage(...)` shortcut's `toNumber`/`contactID`/`groupID`/`contactIDs`/`groupIDs`/`destination`/`destinations`/`recipients` parameters, or via `RCSBuilder.AddRecipient(...)`/`AddRecipients(...)`/`AddDestination(...)`/`AddDestinations(...)`.

**Named-parameter shortcut**: `client.Messaging.RCS.SendMessage(...)` also accepts `messageText`, `destination`, `toNumber`, `contactID`, `groupID`, `contactIDs`, `groupIDs`, `recipients`, `destinations`, `file`, `attachments`, `sendMode` directly. For `SendTime`, `SubAccount`, `SMSEmailReply`, etc. use `RCSBuilder` or construct an `RCSModel` directly.

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
var response = client.Messaging.RCS.SendMessage(
    messageText: "Test RCS message",
    toNumber: "+64211111111",
    sendMode: Enums.SendModeType.Test
);
```

### Via the builder, with a custom sender ID

```csharp
using var builder = new RCSBuilder();

var model = builder
    .SetMessageText("Test RCS message")
    .SetFromNumber("61410023004")   // Sender ID, E.164 without leading '+'
    .AddRecipient("+64211111111")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.RCS.SendMessage(model);
```

### Multiple recipients

See `RCSSamples.SendToMultipleRecipients()`.

```csharp
using var builder = new RCSBuilder();

var model = builder
    .SetMessageText("Test RCS message")
    .AddRecipient("+64211111111")
    .AddRecipient("+64222222222")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.RCS.SendMessage(model);
```

### Scheduled send

See `RCSSamples.SendScheduled()`.

```csharp
using var builder = new RCSBuilder();

var model = builder
    .SetMessageText("Your reminder.")
    .AddRecipient("+64211111111")
    .SetSendTime(DateTime.Now.AddDays(1))
    .SetTimezone("New Zealand")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.RCS.SendMessage(model);
```

### Poll for status / inbound messages

```csharp
var status = client.Messaging.RCS.Status(response.MessageID);

var received = client.Messaging.RCS.Received(timePeriod: 1440);
if (received.Result == Enums.ResultCode.Success)
{
    foreach (var message in received.Messages)
    {
        Console.WriteLine($" => From: '{message.From}', MessageText: '{message.MessageText}'");
    }
}
```

## Response

- `SendMessage(...)` → `RCSApiResult`: `Result`, `MessageID`, `ErrorMessage`.
- `Status(...)` → `RCSStatusApiResult`: `MessageID`, `JobStatus`, `JobNum`, `Recipients` (each with `Destination`, `Status`, `Result`).
- `Received(...)` → `RCSReceivedApiResult`: `Messages` (each with `From`, `MessageText`).
- `Reschedule(...)`/`Abort(...)` → `RCSActionApiResult`: `Action`, `Status`.

## See also

- [README — RCS](../README.md#rcs)
- [Samples — RCSSamples.cs](../TNZAPI.NET.Samples/Messaging/RCS/RCSSamples.cs)