# TNZAPI.NET

## Documentation

The documentation for the TNZ API can be found [here][apidocs].

## Table of Contents

- [Versions](#versions)
- [Getting Started](#getting-started)
  - [Environment Variables](#environment-variables)
- [Messaging](#messaging)
  - [Common Parameters](#common-parameters)
  - [SMS](#sms)
  - [Email](#email)
  - [TTS (Text-to-Speech)](#tts-text-to-speech)
  - [Voice (pre-recorded audio)](#voice-pre-recorded-audio)
  - [Fax](#fax)
  - [WhatsApp](#whatsapp)
  - [RCS](#rcs)
  - [Workflow](#workflow)
- [Actions](#actions)
- [Addressbook](#addressbook)
- [Configuration — OptOut](#configuration--optout)
- [Inbound Webhooks](#inbound-webhooks)
- [Support](#support)

## Versions

`TNZAPI.NET` uses a modified version of [Semantic Versioning](https://semver.org) for all changes. [See this document](VERSIONS.md) for details.

As of v3.00, this library wraps TNZ's JSON REST API (previously an XML API) — see `VERSIONS.md` for the migration history if you're upgrading from an earlier version.

### Supported .NET Versions

This library supports the following .NET implementations:

* .NET Standard 2.0 (covers .NET Framework 4.6.1+, Mono, Xamarin, Unity, etc.)
* .NET6
* .NET7
* .NET8
* .NET9
* .NET10

## Getting Started

Getting started with the TNZ API couldn't be easier. Create a
`TNZApiClient` and you're ready to go.

### API Credentials

`TNZAPI.NET` authenticates every request with a JWT **Auth Token** — find yours in the TNZ Dashboard (Users > API). Pass it directly to the constructor:

```csharp
var client = new TNZApiClient("[Your Auth Token]");
```

or via a `TNZApiUser`:

```csharp
var apiUser = new TNZApiUser()
{
    AuthToken = "[Your Auth Token]"
};

var client = new TNZApiClient(apiUser);
```

Every response implements `IApiResult` — check `response.Result == Enums.ResultCode.Success` before reading response fields; on failure, `response.ErrorMessage` is a `List<string>` of human-readable error messages. `Enums.ResultCode` has four values: `Success`, `Failed`, `Unauthorized`, `RecordNotFound`.

### Environment Variables

`TNZAPI.NET` can also be configured via environment variables — useful for CI, containers, or keeping credentials out of source control:

| Variable | Purpose | Default |
|---|---|---|
| `TNZ_AUTH_TOKEN` | Fallback Auth Token used whenever a `TNZApiUser` is constructed without one set explicitly. An explicit `AuthToken` always takes precedence. | *(none)* |
| `TNZ_API_URL` | Overrides the API host `TNZAPI.NET` sends requests to. Read fresh on every request, so it can be changed at runtime (e.g. between test cases). | `https://api.tnz.co.nz` |
| `TNZ_ALLOW_INSECURE_HTTP` | Set to `true` to allow requests over plain HTTP — useful when pointing `TNZ_API_URL` at a local/staging server without TLS. By default `TNZAPI.NET` refuses to send the Auth Token over anything but HTTPS. | *(unset — HTTPS enforced)* |

```csharp
// Picks up TNZ_AUTH_TOKEN automatically since no AuthToken is set explicitly
var client = new TNZApiClient();
```

**Setting them:**

<details>
<summary>Windows</summary>

Current PowerShell session only:

```powershell
$env:TNZ_AUTH_TOKEN = "your-auth-token"
```

Current Command Prompt (cmd.exe) session only:

```cmd
set TNZ_AUTH_TOKEN=your-auth-token
```

Permanently, for your user account (visible in new terminals/processes, including Visual Studio, after you open one):

```powershell
[System.Environment]::SetEnvironmentVariable("TNZ_AUTH_TOKEN", "your-auth-token", "User")
```

Or via the GUI: Windows Settings → search "Environment Variables" → *Edit environment variables for your account*.

</details>

<details>
<summary>Linux</summary>

Current shell session only:

```bash
export TNZ_AUTH_TOKEN="your-auth-token"
```

Permanently, for your user account — add the `export` line above to `~/.bashrc` (bash), `~/.zshrc` (zsh), or `~/.profile`, then start a new shell (or `source` the file).

For a systemd service, set it in the unit file instead:

```ini
[Service]
Environment="TNZ_AUTH_TOKEN=your-auth-token"
```

</details>

<details>
<summary>macOS</summary>

Current shell session only:

```bash
export TNZ_AUTH_TOKEN="your-auth-token"
```

Permanently, for your user account — add the `export` line above to `~/.zshrc` (the default shell on modern macOS) or `~/.bash_profile`, then start a new terminal (or `source` the file).

</details>

Same pattern applies to `TNZ_API_URL` and `TNZ_ALLOW_INSECURE_HTTP` — just swap the variable name. Note that `TNZAPI.NET` reads OS-level environment variables only; it does not auto-load a `.env` file. If you want `.env` support, use a package like [`DotNetEnv`](https://www.nuget.org/packages/DotNetEnv) in your own application and call it before constructing `TNZApiClient`.

## Messaging

Every messaging module supports both a fluent `Builder` and a named-parameter `SendMessage(...)` overload; pick whichever reads better for your use case. Set `sendMode: Enums.SendModeType.Test` while testing — no charges are incurred and no message actually sends.

### Common Parameters

Each channel's named-parameter `SendMessage(...)` overload accepts a `destination` parameter typed `object?` — pass either a `string` (one address, or several comma-separated — sends the wire's primary `Recipient` field) or a `Destination` object (for personalisation fields or alternate address fields alongside it). A channel-specific alias is also available — `toNumber` for phone-based channels, `emailAddress` for Email — which behaves the same as a string `destination` but reads more naturally for that channel. Use `destination`/`destinations` with a `Destination` object when you need personalisation fields, or when combining multiple address types for one recipient (see [Workflow](#workflow)).

```csharp
var response = client.Messaging.SMS.SendMessage(
    messageText: "Test SMS",
    destination: new Destination { Recipient = "+64211111111", FirstName = "Alice" },
    sendMode: Enums.SendModeType.Test
);
```

For multiple recipients in one call, pass `destinations` (`ICollection<Destination>`) or use `<X>Builder.AddDestination(...)`/`AddDestinations(...)`.

You can also target existing TNZ Addressbook entries directly: `contactID`/`groupID` (single `string` ID) for one recipient, or `contactIDs`/`groupIDs` (`ICollection<ContactID>`/`ICollection<GroupID>`) for several:

```csharp
var response = client.Messaging.SMS.SendMessage(
    messageText: "Test SMS",
    groupID: "GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD",
    sendMode: Enums.SendModeType.Test
);
```

Every channel's underlying `<X>Model` (reachable via the `<X>Builder` or by constructing the model directly — not all of these are exposed on the named-parameter `SendMessage(...)` shortcut) shares most of this parameter set:

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

`ReportTo` is supported by SMS, Email, TTS, Voice, Fax, WhatsApp, and RCS — it is **not** available on Workflow. See the [docs/ folder](docs/messaging.md) — one reference page per channel — for each channel's full parameter table, including channel-specific fields.

### SMS

Send text messages to one or more recipients, with optional Voice/RCS/WhatsApp fallback.

```csharp
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

**Full reference:** [docs/sms.md](docs/sms.md)

### Email

Send plain-text or HTML email, with optional attachments.

```csharp
var response = client.Messaging.Email.SendMessage(
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

**Full reference:** [docs/email.md](docs/email.md)

### TTS (Text-to-Speech)

Place automated text-to-speech voice calls, with optional keypad-menu routing.

```csharp
var response = client.Messaging.TTS.SendMessage(
    messageToPeople: "Hello, this is a call from test. This is relevant information.",
    toNumber: "+64211111111",
    sendMode: Enums.SendModeType.Test
);
```

TTS and Voice are the only two channels with `client.Actions.TTS.Pacing(...)` (adjust the number of simultaneous operators on an in-progress job).

**Full reference:** [docs/tts.md](docs/tts.md)

### Voice (pre-recorded audio)

Place automated pre-recorded-audio calls, with optional keypad-menu routing. Voice shares its Keypad model/enums with TTS (`KeypadModel`, `Enums.AnswerPhoneMode`, `Enums.KeypadPlaySection`) — the builder API is identical.

```csharp
var response = client.Messaging.Voice.SendMessage(
    toNumber: "+64211111111",
    sendMode: Enums.SendModeType.Test
);
```

**Full reference:** [docs/voice.md](docs/voice.md)

### Fax

Send a document as a fax — content comes entirely from an attachment.

```csharp
using var builder = new FaxBuilder();

var model = builder
    .AddDestination("+6491111111")
    .AddAttachment("My Document.pdf", base64FileContent)
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.Fax.SendMessage(model);
```

**Full reference:** [docs/fax.md](docs/fax.md)

### WhatsApp

Send WhatsApp template messages, with optional fallback to another channel. WhatsApp requires **both** `Message` and `TemplateID` (not either/or like SMS) — find your Template ID in the Dashboard.

```csharp
var response = client.Messaging.WhatsApp.SendMessage(
    messageText: "Hi [[FirstName]], your order has shipped!",
    templateId: "123e4567-e89b-12d3-a456-426614174000",
    toNumber: "+64211111111",
    sendMode: Enums.SendModeType.Test
);
```

**Full reference:** [docs/whatsapp.md](docs/whatsapp.md)

### RCS

Send Rich Communication Services messages. Like SMS, `Message`/`TemplateID` are either/or (not both required, unlike WhatsApp).

```csharp
var response = client.Messaging.RCS.SendMessage(
    messageText: "Test RCS message",
    toNumber: "+64211111111",
    sendMode: Enums.SendModeType.Test
);
```

**Full reference:** [docs/rcs.md](docs/rcs.md)

### Workflow

Triggers a pre-configured, no-code Workflow Template (built via the Dashboard) that can span multiple channels and automation steps. This is the only messaging module with no `Message`/`TemplateID` text content, no Status, no Received, and no `client.Actions.Workflow` — just Send.

Workflow is genuinely omni-channel: unlike every other module, `toNumber`, `mainPhone`, and `emailAddress` can all be set together for the *same* recipient — the Workflow Template decides which channel(s) actually get used:

```csharp
var response = client.Messaging.Workflow.SendMessage(
    workflowTemplateId: "a1b2c3d4-e5f6-7890-1234-567890abcdef",
    toNumber: "+64211111111",
    emailAddress: "test@example.com",
    sendMode: Enums.SendModeType.Test
);
```

**Full reference:** [docs/workflow.md](docs/workflow.md)

## Actions

`client.Actions` is **per-messaging-channel**, not a single flat facade — each channel exposes only the actions its real API supports:

| Channel | Reschedule | Abort | Resubmit | Pacing |
|---|:---:|:---:|:---:|:---:|
| SMS | ✓ | ✓ | | |
| Email | ✓ | ✓ | ✓ | |
| TTS | ✓ | ✓ | ✓ | ✓ |
| Voice | ✓ | ✓ | ✓ | ✓ |
| Fax | ✓ | ✓ | ✓ | |
| WhatsApp | ✓ | ✓ | | |
| RCS | ✓ | ✓ | | |
| Workflow | | | | |

```csharp
var response = client.Actions.SMS.Reschedule(
    messageID: response.MessageID,
    sendTime: DateTime.Parse("2026-12-31T12:00:00")
);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Action: {response.Action}, Status: {response.Status}");
}
```

`Abort` takes just the `messageID`:

```csharp
client.Actions.SMS.Abort(response.MessageID);
```

## Addressbook

### Contact

```csharp
using var builder = new ContactBuilder();

var contact = builder
    .SetAttention("API Test")
    .SetFirstName("API")
    .SetLastName("Test")
    .SetMobilePhone("+64211231234")
    .SetEmailAddress("test@example.com")
    .SetMainPhone("+6491112222")
    .Build();

var response = client.Addressbook.Contact.Create(contact);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Created ContactID={response.ContactID}");
}
```

```csharp
var details = client.Addressbook.Contact.Details(response.ContactID);

if (details.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"{details.FirstName} {details.LastName} — {details.EmailAddress}");
}
```

```csharp
var updated = client.Addressbook.Contact.Update(response.ContactID, new ContactModel
{
    Company = "Example Company"
});

client.Addressbook.Contact.Delete(response.ContactID);
```

Search and list:

```csharp
var results = client.Addressbook.Contact.Search(emailAddress: "test@example.com");

var page = client.Addressbook.Contact.List(page: 1, recordsPerPage: 100);

if (page.Result == Enums.ResultCode.Success)
{
    foreach (var contact in page.Contacts)
    {
        Console.WriteLine($"{contact.ContactID}: {contact.FirstName} {contact.LastName}");
    }
}
```

### Group

```csharp
using var builder = new GroupBuilder();

var group = builder
    .SetGroupName("API Test Group")
    .Build();

var response = client.Addressbook.Group.Create(group);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Created GroupID={response.GroupID}, GroupCode={response.GroupCode}");
}
```

```csharp
var details = client.Addressbook.Group.Details(response.GroupID);
client.Addressbook.Group.Update(response.GroupID, new GroupModel { GroupName = "Renamed Group" });
client.Addressbook.Group.Delete(response.GroupID);

var page = client.Addressbook.Group.List(page: 1, recordsPerPage: 100);
```

### Contact ↔ Group relationships

`Contact.Group` manages one contact's group memberships (list/add/remove); `Group.Contact` is the fully symmetric counterpart from the group's side (list/add/remove/detail).

```csharp
// Groups a contact belongs to
var groups = client.Addressbook.Contact.Group.List(contactID);

// Add a contact to a group
var addResult = client.Addressbook.Contact.Group.Add(contactID, groupID);

if (addResult.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Added to group: {addResult.Group.GroupName}");
}

// Remove a contact from a group
client.Addressbook.Contact.Group.Remove(contactID, groupID);

// Contacts belonging to a group
var contacts = client.Addressbook.Group.Contact.List(groupID);

if (contacts.Result == Enums.ResultCode.Success)
{
    foreach (var contact in contacts.Contacts)
    {
        Console.WriteLine($"{contact.FirstName} {contact.LastName}");
    }
}

// Add/remove a contact from a group, from the group's side
client.Addressbook.Group.Contact.Add(groupID, contactID);
client.Addressbook.Group.Contact.Remove(groupID, contactID);
```

## Configuration — OptOut

```csharp
using var builder = new OptOutBuilder();

var entry = builder
    .SetDestType("SMS")
    .SetDestination("+6421003004")
    .SetNotes("Requested via support call")
    .Build();

var response = client.Configuration.OptOut.Create(entry);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Created OptOut ID={response.ID}");
}
```

Opt out multiple destinations at once:

```csharp
var batchResponse = client.Configuration.OptOut.CreateBatch(new OptOutBatchModel
{
    DestType = "SMS",
    Destinations = new List<string> { "+6421003004", "+6421003005" }
});

if (batchResponse.Result == Enums.ResultCode.Success)
{
    foreach (var entry in batchResponse.OptOuts)
    {
        Console.WriteLine($"Opted out: {entry.Destination} ({entry.ID})");
    }
}
```

```csharp
var details = client.Configuration.OptOut.Details(response.ID);
client.Configuration.OptOut.Delete(response.ID);

var list = client.Configuration.OptOut.List(timePeriod: 30, destType: "SMS");

if (list.Result == Enums.ResultCode.Success)
{
    foreach (var entry in list.OptOuts)
    {
        Console.WriteLine($"{entry.Destination} — {entry.DestType}");
    }
}
```

## Inbound Webhooks

Webhooks are **inbound** — TNZ's servers POST these payloads *to your own server* (configure `WebhookCallbackURL` on a Send call, or your Sender's Dashboard settings for SMS-reply/result reporting). The SDK doesn't call anything for this; instead, `TNZAPI.NET.Webhooks` provides typed payload models you can deserialize an incoming request body into from your own webhook receiver endpoint:

```csharp
// Inside your own ASP.NET (or similar) webhook receiver action:
var payload = System.Text.Json.JsonSerializer.Deserialize<TNZAPI.NET.Webhooks.ResultWebhookPayload>(requestBody);

Console.WriteLine($"{payload.Type} to {payload.Destination}: {payload.Status} ({payload.Result})");
```

```csharp
var inboundSms = System.Text.Json.JsonSerializer.Deserialize<TNZAPI.NET.Webhooks.InboundSMSWebhookPayload>(requestBody);

Console.WriteLine($"Inbound SMS from {inboundSms.Destination}: {inboundSms.Message}");
```

Both payload types deserialize correctly with your own default `JsonSerializerOptions` — no need to configure anything on your end, including TNZ's non-standard timestamp format.

## Support

### Getting help

If you need help installing or using the library, please check the [TNZ Contact](https://www.tnz.co.nz/About/Contact/) if you don't find an answer to your question.

[apidocs]: https://www.tnz.co.nz/Docs/dotNetLib/