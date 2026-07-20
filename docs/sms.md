# SMS

Send text messages to one or more recipients via the TNZ REST API.

→ [Common parameters & authentication](../README.md#common-parameters)

## Quick example

```csharp
using TNZAPI.NET.Core;

var client = new TNZApiClient(apiUser);

var response = client.Messaging.SMS.SendMessage(
    toNumber: "+64211111111",
    messageText: "Test SMS",
    sendMode: Enums.SendModeType.Test
);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Success - MessageID: {response.MessageID}");
}
```

## Parameters (`SMSModel`)

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
| `Timezone` | `string?` | No | Windows Timezone name for `SendTime` (e.g. `"New Zealand"`, `"AUS Eastern"`). |
| `SubAccount` | `string?` | No | Sub-account code for billing separation. |
| `Department` | `string?` | No | Department code. |
| `MessageID` | `MessageID?` | No | Supply your own message ID (otherwise auto-generated). |
| `ReportTo` | `string?` | No | Email address to receive delivery reports. |
| `WebhookCallbackURL` | `string?` | No | URL for delivery status callbacks. |
| `WebhookCallbackFormat` | `Enums.WebhookCallbackType?` | No | Callback format. |
| `NotificationType` | `Enums.NotificationType?` | No | Notification delivery mode. |
| `FallbackMode` | `ICollection<Enums.SMSFallbackMode>?` | No | Fallback channel(s) if SMS fails, tried in the order given, e.g. `RCS` then `Voice`. |
| `SMSEmailReply` | `string?` | No | Email address to receive SMS replies. |
| `CharacterConversion` | `bool` | No | Convert characters outside the GSM character set automatically. Default `false`. |
| `Files` | `ICollection<Attachment>?` | No | MMS attachments. |
| `SendMode` | `Enums.SendModeType` | No | Set `Test` to validate without sending. Default `Live`. |

\*Either `Message` or `TemplateID` must be provided.
†Set directly via `Recipients`/`Destinations`, via the named `SendMessage(...)` shortcut's `toNumber`/`contactID`/`groupID`/`contactIDs`/`groupIDs`/`destination`/`destinations`/`recipients` parameters, or via `SMSBuilder.AddRecipient(...)`/`AddRecipients(...)`/`AddDestination(...)`/`AddDestinations(...)`.

**Named-parameter shortcut**: `client.Messaging.SMS.SendMessage(...)` also accepts a subset of these directly — `messageText`, `reference`, `destination`, `toNumber`, `contactID`, `groupID`, `contactIDs`, `groupIDs`, `recipients`, `destinations`, `file`, `attachments`, `sendMode`. For every other field (`SendTime`, `Timezone`, `SubAccount`, `FallbackMode`, etc.) use `SMSBuilder` or construct an `SMSModel` directly.

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

`new Recipient("+64211111111")` sets `MobileNumber`/`FaxNumber`/`PhoneNumber`/`EmailAddress` all to the same value — SMS's request-body transform reads `MobileNumber`. Use the object initializer (`new Recipient { MobileNumber = "...", FirstName = "..." }`) when you need personalisation fields alongside the number.

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

`new Destination("+64211111111")` sets only `Recipient`. Use the object initializer (`new Destination { Recipient = "...", FirstName = "..." }`) when you need personalisation fields alongside the address.

## Code samples

### Single recipient shorthand

```csharp
var response = client.Messaging.SMS.SendMessage(
    toNumber: "+64211111111",
    messageText: "Office closed today.",
    sendMode: Enums.SendModeType.Test
);
```

### Multiple recipients, via the builder

```csharp
var groupID = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD");
var contactID = new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

using var builder = new SMSBuilder();

var model = builder
    .SetMessageText("Test SMS")
    .SetReference("Test SMS - Builder sample")
    .AddRecipient("+64211111111")
    .AddRecipient("+64222222222")
    .AddRecipient(contactID)   // Recipient from TNZ Addressbook by ContactID
    .AddRecipients(groupID)    // Recipients from TNZ Addressbook by GroupID
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.SMS.SendMessage(model);
```

### Bulk send with per-recipient personalisation

See `SMSSamples.SendToMultipleRecipients()` — sends one personalised message to several recipients in a single call using `Recipient` object initializers for `FirstName`/`Custom1`.

```csharp
using var builder = new SMSBuilder();

var model = builder
    .SetMessageText("Hi [[FirstName]], your appointment is on [[Custom1]].")
    .AddRecipient(new Recipient { MobileNumber = "+64211111111", FirstName = "Alice", Custom1 = "Monday 3pm" })
    .AddRecipient(new Recipient { MobileNumber = "+64222222222", FirstName = "Bob", Custom1 = "Tuesday 10am" })
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.SMS.SendMessage(model);
```

### Scheduled send with webhook callback

See `SMSSamples.SendScheduledWithWebhook()`.

```csharp
using var builder = new SMSBuilder();

var model = builder
    .SetMessageText("Your reminder.")
    .AddRecipient("+64211111111")
    .SetSendTime(DateTime.Now.AddDays(1))
    .SetTimezone("New Zealand")
    .SetWebhookCallbackURL("https://yourapp.example.com/webhooks/sms")
    .SetWebhookCallbackFormat(Enums.WebhookCallbackType.JSON)
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.SMS.SendMessage(model);
```

### Poll for status

```csharp
var status = client.Messaging.SMS.Status(response.MessageID);

if (status.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"JobStatus: {status.JobStatus}");
    foreach (var recipient in status.Recipients)
    {
        Console.WriteLine($" -> {recipient.Destination}: {recipient.Status} ({recipient.Result})");

        foreach (var reply in recipient.SMSReplies)
        {
            Console.WriteLine($"    reply: {reply.MessageText}");
        }
    }
}
```

### Poll for inbound SMS

```csharp
var received = client.Messaging.SMS.Received(timePeriod: 1440); // minutes

if (received.Result == Enums.ResultCode.Success)
{
    foreach (var message in received.Messages)
    {
        Console.WriteLine($"From {message.From}: {message.MessageText}");
    }
}
```

## Response

- `SendMessage(...)` → `SMSApiResult`: `Result`, `MessageID`, `ErrorMessage`.
- `Status(...)` → `SMSStatusApiResult`: `MessageID`, `JobStatus`, `JobNum`, `Count`, `Complete`, `Success`, `Failed`, `Recipients` (each with `Destination`, `Status`, `Result`, `SMSReplies`).
- `Received(...)` → `SMSReceivedApiResult`: `Messages` (each with `From`, `MessageText`).
- `Reschedule(...)`/`Abort(...)` → `SMSActionApiResult`: `Action`, `Status`.

## See also

- [README — SMS](../README.md#sms)
- [Samples — SMSSamples.cs](../TNZAPI.NET.Samples/Messaging/SMS/SMSSamples.cs)