# Fax

Send a document as a fax via the TNZ REST API. Unlike SMS/RCS/WhatsApp, Fax has no free-text message field — content comes entirely from an attachment.

→ [Common parameters & authentication](../README.md#common-parameters)

## Quick example

```csharp
using var builder = new FaxBuilder();

var model = builder
    .AddRecipient("+6491111111")
    .AddAttachment("My Document.pdf", "[base64-encoded-file-data]")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.Fax.SendMessage(model);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Success - MessageID: {response.MessageID}");
}
```

## Parameters (`FaxModel`)

| Parameter | Type | Required | Description |
|---|---|---|---|
| `Files` | `ICollection<Attachment>?` | Yes* | The document(s) to fax. |
| `TemplateID` | `string?` | Yes* | Pre-configured fax template ID (alternative to `Files`). |
| `Recipients` | `ICollection<Recipient>?` | Yes† | One or more recipients — see Destination fields (`Recipient`) below. |
| `Destinations` | `ICollection<Destination>?` | Yes† | Alternative to `Recipients` — sends the wire's primary `Recipient` field per destination; preferred for new code. See Destination fields (`Destination`) below. |
| `ContactID` | `ContactID?` | No | Single addressbook contact to send to (alternative/addition to `Recipients`). |
| `GroupID` | `GroupID?` | No | Single addressbook group to send to (alternative/addition to `Recipients`). |
| `Reference` | `string?` | No | Your internal reference, returned in reports and webhooks. |
| `CSID` | `string?` | No | Fax CSID (station identifier). |
| `Resolution` | `Enums.FaxResolution?` | No | Fax output resolution. |
| `WatermarkFolder` | `string?` | No | Folder containing watermark images. |
| `WatermarkFirstPage` | `string?` | No | Watermark image applied to the first page only. |
| `WatermarkAllPages` | `string?` | No | Watermark image applied to every page. |
| `RetryAttempts` | `int?` | No | Number of retry attempts on busy/no-answer. |
| `RetryPeriod` | `int?` | No | Minutes between retry attempts. |
| `SendTime` | `DateTime?` | No | Schedule delivery — combine with `Timezone`. |
| `Timezone` | `string?` | No | Windows Timezone name for `SendTime`. |
| `SubAccount` | `string?` | No | Sub-account code for billing separation. |
| `Department` | `string?` | No | Department code. |
| `MessageID` | `MessageID?` | No | Supply your own message ID (otherwise auto-generated). |
| `ReportTo` | `string?` | No | Email address to receive delivery reports. |
| `WebhookCallbackURL` | `string?` | No | URL for delivery status callbacks. |
| `WebhookCallbackFormat` | `Enums.WebhookCallbackType?` | No | Callback format. |
| `NotificationType` | `Enums.NotificationType?` | No | Notification delivery mode. |
| `SendMode` | `Enums.SendModeType` | No | Set `Test` to validate without sending. Default `Live`. |

\*Either `Files` or `TemplateID` must be provided.
†Set directly via `Recipients`/`Destinations`, via the named `SendMessage(...)` shortcut's `toNumber`/`contactID`/`groupID`/`contactIDs`/`groupIDs`/`destination`/`destinations`/`recipients` parameters, or via `FaxBuilder.AddRecipient(...)`/`AddRecipients(...)`/`AddDestination(...)`/`AddDestinations(...)`.

**Named-parameter shortcut**: `client.Messaging.Fax.SendMessage(...)` also accepts `destination`, `toNumber`, `contactID`, `groupID`, `contactIDs`, `groupIDs`, `recipients`, `destinations`, `file`, `attachments`, `sendMode` directly. For `CSID`, `Resolution`, watermarks, retry settings, etc. use `FaxBuilder` or construct a `FaxModel` directly.

## Destination fields (`Recipient`)

| Field | Description |
|---|---|
| `FaxNumber` | Destination fax number, e.g. `"+6491111111"`. |
| `ContactID` | Addressbook contact reference — sends to that contact instead of a raw number. |
| `GroupID` | Addressbook group reference — sends to all members of that group. |
| `GroupCode` | Alternative group lookup by code. |
| `CompanyName` | For reference/reporting — Fax has no message body to personalise. |
| `Attention` | For reference/reporting — Fax has no message body to personalise. |

## Destination fields (`Destination`)

Prefer `Destination` over `Recipient` for new code — it mirrors TNZ's wire "Destinations Object" 1:1, including the primary `Recipient` field (sent as-is, channel-agnostic) plus the same channel-specific alternate address fields other channels' wire types use. `AddDestination(...)`/`AddDestinations(...)` on the builder, or the `destination`/`destinations` named-parameter shortcuts, populate this collection instead of `Recipients`.

| Field | Description |
|---|---|
| `Recipient` | Primary destination address, e.g. `"+6491111111"` — sent as-is regardless of channel. |
| `ToNumber` / `MobilePhone` / `MainPhone` / `FaxNumber` / `EmailAddress` | Channel-specific alternate address fields — set instead of/alongside `Recipient` if you need to target a different wire field explicitly. |
| `ContactID` | Addressbook contact reference — sends to that contact instead of a raw number. |
| `GroupID` | Addressbook group reference — sends to all members of that group. |
| `GroupCode` | Alternative group lookup by code. |
| `Company` | For reference/reporting — Fax has no message body to personalise. |
| `Attention` | For reference/reporting — Fax has no message body to personalise. |

`new Destination("+6491111111")` sets only `Recipient`. Use the object initializer (`new Destination { Recipient = "...", Attention = "..." }`) when you need reference fields alongside the number.

## Code samples

### Single recipient with an attachment

```csharp
using var builder = new FaxBuilder();

var model = builder
    .AddRecipient("+6491111111")
    .AddAttachment("My Document.pdf", "[base64-encoded-file-data]")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.Fax.SendMessage(model);
```

### From a file path

```csharp
using var builder = new FaxBuilder();

var model = builder
    .AddRecipient("+6491111111")
    .AddAttachment(@"C:\Documents\MyDocument.pdf")   // reads and base64-encodes the file
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.Fax.SendMessage(model);
```

### Multiple recipients

See `FaxSamples.SendToMultipleRecipients()`.

```csharp
using var builder = new FaxBuilder();

var model = builder
    .AddRecipient("+6491111111")
    .AddRecipient("+6492222222")
    .AddAttachment("My Document.pdf", "[base64-encoded-file-data]")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.Fax.SendMessage(model);
```

### Scheduled send

See `FaxSamples.SendScheduled()`.

```csharp
using var builder = new FaxBuilder();

var model = builder
    .AddRecipient("+6491111111")
    .AddAttachment("My Document.pdf", "[base64-encoded-file-data]")
    .SetSendTime(DateTime.Now.AddDays(1))
    .SetTimezone("New Zealand")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.Fax.SendMessage(model);
```

### Poll for status

```csharp
var status = client.Messaging.Fax.Status(response.MessageID);

if (status.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"JobStatus: '{status.JobStatus}', JobNum: '{status.JobNum}'");

    foreach (var recipient in status.Recipients)
    {
        Console.WriteLine($" -> {recipient.Destination}: {recipient.Status} ({recipient.Result})");
    }
}
```

## Response

- `SendMessage(...)` → `FaxApiResult`: `Result`, `MessageID`, `ErrorMessage`.
- `Status(...)` → `FaxStatusApiResult`: `MessageID`, `JobStatus`, `JobNum`, `Recipients` (each with `Destination`, `Status`, `Result`).
- `Reschedule(...)`/`Abort(...)`/`Resubmit(...)` → `FaxActionApiResult`: `Action`, `Status`.

## See also

- [README — Fax](../README.md#fax)
- [Samples — FaxSamples.cs](../TNZAPI.NET.Samples/Messaging/Fax/FaxSamples.cs)