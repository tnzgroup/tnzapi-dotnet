# Email

Send plain-text or HTML email, with optional attachments, via the TNZ REST API.

→ [Common parameters & authentication](../README.md#common-parameters)

## Quick example

```csharp
var response = client.Messaging.Email.SendMessage(
    fromEmail: "from@test.com",       // Optional - leave blank to use your API username as sender
    emailSubject: "Test Email",
    messagePlain: "Test Email Body",
    emailAddress: "email.one@test.com",
    sendMode: Enums.SendModeType.Test
);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Success - MessageID: {response.MessageID}");
}
```

## Parameters (`EmailModel`)

| Parameter | Type | Required | Description |
|---|---|---|---|
| `MessagePlain` | `string?` | Yes* | Plain-text body. |
| `MessageHTML` | `string?` | Yes* | HTML body. |
| `TemplateID` | `string?` | Yes* | Pre-configured message template ID (alternative to `MessagePlain`/`MessageHTML`). |
| `EmailSubject` | `string?` | Yes | Email subject line. |
| `Recipients` | `ICollection<Recipient>?` | Yes† | One or more recipients — see Destination fields (`Recipient`) below. |
| `Destinations` | `ICollection<Destination>?` | Yes† | Alternative to `Recipients` — sends the wire's primary `Recipient` field per destination; preferred for new code. See Destination fields (`Destination`) below. |
| `ContactID` | `ContactID?` | No | Single addressbook contact to send to (alternative/addition to `Recipients`). |
| `GroupID` | `GroupID?` | No | Single addressbook group to send to (alternative/addition to `Recipients`). |
| `Reference` | `string?` | No | Your internal reference, returned in reports and webhooks. |
| `FromEmail` | `string?` | No | Sender address. Leave blank to use your API username. |
| `From` | `string?` | No | Sender display name. |
| `SMTPFrom` | `string?` | No | SMTP envelope-from override. |
| `CCEmail` | `string?` | No | CC address. |
| `ReplyTo` | `string?` | No | Reply-To address. |
| `SendTime` | `DateTime?` | No | Schedule delivery — combine with `Timezone`. |
| `Timezone` | `string?` | No | Windows Timezone name for `SendTime`. |
| `SubAccount` | `string?` | No | Sub-account code for billing separation. |
| `Department` | `string?` | No | Department code. |
| `MessageID` | `MessageID?` | No | Supply your own message ID (otherwise auto-generated). |
| `ReportTo` | `string?` | No | Email address to receive delivery reports. |
| `WebhookCallbackURL` | `string?` | No | URL for delivery status callbacks. |
| `WebhookCallbackFormat` | `Enums.WebhookCallbackType?` | No | Callback format. |
| `NotificationType` | `Enums.NotificationType?` | No | Notification delivery mode. |
| `Files` | `ICollection<Attachment>?` | No | Email attachments. |
| `SendMode` | `Enums.SendModeType` | No | Set `Test` to validate without sending. Default `Live`. |

\*Either `MessagePlain`, `MessageHTML`, or `TemplateID` must be provided; HTML and plain-text may be combined for multi-part email.
†Set directly via `Recipients`/`Destinations`, via the named `SendMessage(...)` shortcut's `emailAddress`/`contactID`/`groupID`/`contactIDs`/`groupIDs`/`destination`/`destinations`/`recipients` parameters, or via `EmailBuilder.AddRecipient(...)`/`AddRecipients(...)`/`AddDestination(...)`/`AddDestinations(...)`.

**Named-parameter shortcut**: `client.Messaging.Email.SendMessage(...)` also accepts `messagePlain`, `messageHTML`, `destination`, `emailAddress`, `contactID`, `groupID`, `contactIDs`, `groupIDs`, `recipients`, `destinations`, `emailSubject`, `fromEmail`, `file`, `attachments`, `sendMode` directly. For `CCEmail`, `ReplyTo`, `SendTime`, `SubAccount`, etc. use `EmailBuilder` or construct an `EmailModel` directly.

## Destination fields (`Recipient`)

| Field | Description |
|---|---|
| `EmailAddress` | Destination email address, e.g. `"email.one@test.com"`. |
| `ContactID` | Addressbook contact reference — sends to that contact instead of a raw address. |
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
| `Recipient` | Primary destination address, e.g. `"email.one@test.com"` — sent as-is regardless of channel. |
| `ToNumber` / `MobilePhone` / `MainPhone` / `FaxNumber` / `EmailAddress` | Channel-specific alternate address fields — set instead of/alongside `Recipient` if you need to target a different wire field explicitly. |
| `ContactID` | Addressbook contact reference — sends to that contact instead of a raw address. |
| `GroupID` | Addressbook group reference — sends to all members of that group. |
| `GroupCode` | Alternative group lookup by code. |
| `Company` | Personalisation token `[[Company]]`. |
| `Attention` | Personalisation token `[[Attention]]`. |
| `FirstName` | Personalisation token `[[FirstName]]`. |
| `LastName` | Personalisation token `[[LastName]]`. |
| `Custom1`–`Custom9` | Arbitrary per-recipient personalisation values, `[[Custom1]]` … `[[Custom9]]`. |

`new Destination("email.one@test.com")` sets only `Recipient`. Use the object initializer (`new Destination { Recipient = "...", FirstName = "..." }`) when you need personalisation fields alongside the address.

## Code samples

### Single recipient, plain text

```csharp
var response = client.Messaging.Email.SendMessage(
    fromEmail: "from@test.com",
    emailSubject: "Test Email",
    messagePlain: "Test Email Body",
    emailAddress: "email.one@test.com",
    sendMode: Enums.SendModeType.Test
);
```

### Via the builder

```csharp
using var builder = new EmailBuilder();

var model = builder
    .SetEmailSubject("Test Email")
    .SetMessagePlain("Test Email Body")
    .AddRecipient("email.one@test.com")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.Email.SendMessage(model);
```

### HTML email

See `EmailSamples.SendHtmlEmail()`.

```csharp
using var builder = new EmailBuilder();

var model = builder
    .SetEmailSubject("Test Email")
    .SetMessageHTML("<p>Test <strong>Email</strong> Body</p>")
    .AddRecipient("email.one@test.com")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.Email.SendMessage(model);
```

### With an attachment

See `EmailSamples.SendWithAttachment()`.

```csharp
using var builder = new EmailBuilder();

var model = builder
    .SetEmailSubject("Test Email")
    .SetMessagePlain("See attached.")
    .AddRecipient("email.one@test.com")
    .AddAttachment("My Document.pdf", "[base64-encoded-file-data]")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.Email.SendMessage(model);
```

### Poll for status

```csharp
var status = client.Messaging.Email.Status(response.MessageID);

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

- `SendMessage(...)` → `EmailApiResult`: `Result`, `MessageID`, `ErrorMessage`.
- `Status(...)` → `EmailStatusApiResult`: `MessageID`, `JobStatus`, `JobNum`, `Recipients` (each with `Destination`, `Status`, `Result`).
- `Reschedule(...)`/`Abort(...)`/`Resubmit(...)` → `EmailActionApiResult`: `Action`, `Status`.

## See also

- [README — Email](../README.md#email)
- [Samples — EmailSamples.cs](../TNZAPI.NET.Samples/Messaging/Email/EmailSamples.cs)