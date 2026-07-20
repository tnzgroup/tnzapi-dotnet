# Messaging

Send messages across eight channels via `client.Messaging.<Channel>.SendMessage(...)`.

| Channel | Doc | Description |
|---|---|---|
| SMS | [sms.md](sms.md) | Text messages with optional Voice/RCS/WhatsApp fallback |
| Email | [email.md](email.md) | Plain-text/HTML email with attachments |
| TTS | [tts.md](tts.md) | Text-to-speech voice calls with keypad routing |
| Voice | [voice.md](voice.md) | Pre-recorded audio calls with keypad routing |
| Fax | [fax.md](fax.md) | PDF/document fax with attachments |
| WhatsApp | [whatsapp.md](whatsapp.md) | WhatsApp template messages with fallback |
| RCS | [rcs.md](rcs.md) | Rich Communication Services messages |
| Workflow | [workflow.md](workflow.md) | Multi-channel Workflow Template triggers |

## Common parameters

Every channel's `<X>Model` shares most of these fields (see each channel's page for its full table and any channel-specific fields). Full descriptions: [README — Common Parameters](../README.md#common-parameters).

| Parameter | Type | Description |
|---|---|---|
| `Reference` | `string?` | Your internal reference, returned in reports and webhooks. |
| `SendTime` | `DateTime?` | Schedule delivery — combine with `Timezone`. |
| `Timezone` | `string?` | Windows Timezone name for `SendTime` (e.g. `"New Zealand"`, `"AUS Eastern"`). |
| `SubAccount` | `string?` | Sub-account code for billing separation. |
| `Department` | `string?` | Department code. |
| `MessageID` | `MessageID?` | Supply your own message ID (otherwise auto-generated). |
| `WebhookCallbackURL` | `string?` | URL for delivery status callbacks. |
| `WebhookCallbackFormat` | `Enums.WebhookCallbackType?` | Callback format. |
| `NotificationType` | `Enums.NotificationType?` | Notification delivery mode. |
| `ReportTo` | `string?` | Email address to receive delivery reports. |
| `SendMode` | `Enums.SendModeType` | Set `Enums.SendModeType.Test` to validate without sending. Default `Live`. |

> `ReportTo` is supported by SMS, Email, TTS, Voice, Fax, WhatsApp, and RCS — it is **not** available on Workflow.

## Common destination fields (`Recipient`)

All 8 channels share the same `Recipient` type (`TNZAPI.NET.Api.Messaging.Common.Components.Recipient`) for addressing — see [README — Common Parameters](../README.md#common-parameters) and each channel page's own Destination fields section for the full table.

## Builder pattern

Every channel supports a fluent `<X>Builder` as an alternative to the named-parameter `SendMessage(...)` shortcut or constructing the `<X>Model` directly:

```csharp
using var builder = new SMSBuilder();

var model = builder
    .SetMessageText("Test SMS")
    .SetReference("Test SMS - Builder sample")
    .AddRecipient("+64211111111")
    .AddRecipient("+64222222222")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.SMS.SendMessage(model);
```

TTS and Voice additionally support `.AddKeypad(...)`. Email, Fax, RCS, SMS, and WhatsApp additionally support `.AddAttachment(...)`.

`AddRecipient`/`AddRecipients` accept a `string`, a `Recipient`, a `ContactID`, a `GroupID`, or a collection of any of those — see each channel's Destination fields section.

## Response shape

Every `SendMessage(...)` call returns a type implementing `IApiResult`:

```csharp
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
```

`Enums.ResultCode` has four values: `Success`, `Failed`, `Unauthorized`, `RecordNotFound`. See each channel's page for the extra fields its `Status`/`Received`/Action results carry.

## Authentication

```csharp
using TNZAPI.NET.Core;

var client = new TNZApiClient("[Your Auth Token]");

// or via a TNZApiUser
var apiUser = new TNZApiUser() { AuthToken = "[Your Auth Token]" };
var client = new TNZApiClient(apiUser);
```

See [README — Authentication](../README.md#api-credentials) for details.