# TTS (Text-to-Speech)

Place automated text-to-speech voice calls, with optional keypad-menu routing, via the TNZ REST API.

→ [Common parameters & authentication](../README.md#common-parameters)

## Quick example

```csharp
var response = client.Messaging.TTS.SendMessage(
    messageToPeople: "Hello, this is a call from test. This is relevant information.",
    toNumber: "+64211111111",
    sendMode: Enums.SendModeType.Test
);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Success - MessageID: {response.MessageID}");
}
```

## Parameters (`TTSModel`)

| Parameter | Type | Required | Description |
|---|---|---|---|
| `MessageToPeople` | `string?` | Yes* | Text read aloud to a human answering the call. |
| `TemplateID` | `string?` | Yes* | Pre-configured message template ID (alternative to `MessageToPeople`). |
| `Recipients` | `ICollection<Recipient>?` | Yes† | One or more recipients — see Destination fields (`Recipient`) below. |
| `Destinations` | `ICollection<Destination>?` | Yes† | Alternative to `Recipients` — sends the wire's primary `Recipient` field per destination; preferred for new code. See Destination fields (`Destination`) below. |
| `ContactID` | `ContactID?` | No | Single addressbook contact to send to (alternative/addition to `Recipients`). |
| `GroupID` | `GroupID?` | No | Single addressbook group to send to (alternative/addition to `Recipients`). |
| `Reference` | `string?` | No | Your internal reference, returned in reports and webhooks. |
| `MessageToAnswerPhones` | `string?` | No | Alternative text read when an answering machine picks up. |
| `AnswerPhoneMode` | `Enums.AnswerPhoneMode?` | No | How to handle an answering machine. |
| `Keypads` | `ICollection<KeypadModel>?` | No | Keypad menu options — see Keypad fields below. |
| `KeypadOptionRequired` | `bool` | No | Force the caller to press a key before the call proceeds. Default `false`. |
| `CallRouteMessageOnWrongKey` | `string?` | No | Message played if an invalid key is pressed. |
| `CallRouteMessageToPeople` | `string?` | No | Message played before routing to an operator. |
| `CallRouteMessageToOperators` | `string?` | No | Message played to the operator receiving the routed call. |
| `NumberOfOperators` | `int?` | No | Number of simultaneous operators for keypad-routed calls. |
| `RetryAttempts` | `int?` | No | Number of retry attempts on no-answer/busy. |
| `RetryPeriod` | `int?` | No | Minutes between retry attempts. |
| `CallerID` | `string?` | No | Caller ID shown to the recipient. |
| `Voice` | `string?` | No | TTS voice to use. |
| `Options` | `string?` | No | Additional provider-specific options. |
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

\*Either `MessageToPeople` or `TemplateID` must be provided.
†Set directly via `Recipients`/`Destinations`, via the named `SendMessage(...)` shortcut's `toNumber`/`contactID`/`groupID`/`contactIDs`/`groupIDs`/`destination`/`destinations`/`recipients` parameters, or via `TTSBuilder.AddRecipient(...)`/`AddRecipients(...)`/`AddDestination(...)`/`AddDestinations(...)`.

**Named-parameter shortcut**: `client.Messaging.TTS.SendMessage(...)` also accepts `messageToPeople`, `destination`, `toNumber`, `contactID`, `groupID`, `contactIDs`, `groupIDs`, `recipients`, `destinations`, `sendMode` directly. TTS has no `SendMessage(...)` shortcut for keypads, attachments, or any other field above — use `TTSBuilder` or construct a `TTSModel` directly.

## Keypad fields (`KeypadModel`)

| Field | Type | Description |
|---|---|---|
| `Tone` | `int` | The DTMF digit this entry responds to. |
| `Play` | `string?` | Message played when this key is pressed. |
| `RouteNumber` | `string?` | Phone number to route the call to when this key is pressed. |
| `PlaySection` | `Enums.KeypadPlaySection?` | Where in the call flow this keypad applies (e.g. `Main`). |

## Destination fields (`Recipient`)

| Field | Description |
|---|---|
| `PhoneNumber` | Destination phone number, e.g. `"+64211111111"`. TTS reads `PhoneNumber` (not `MobileNumber`) for its destination. |
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
var response = client.Messaging.TTS.SendMessage(
    messageToPeople: "Hello, this is a call from test. This is relevant information.",
    toNumber: "+64211111111",
    sendMode: Enums.SendModeType.Test
);
```

### With a keypad menu, via the builder

```csharp
using var builder = new TTSBuilder();

var model = builder
    .SetMessageToPeople("Press 1 for sales, press 2 for support.")
    .AddRecipient("+64211111111")
    .AddKeypad(new KeypadModel
    {
        Tone = 1,
        Play = "Connecting you to sales now.",
        PlaySection = Enums.KeypadPlaySection.Main,
        RouteNumber = "+64211112222"
    })
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.TTS.SendMessage(model);
```

### Multiple recipients

See `TTSSamples.SendToMultipleRecipients()`.

```csharp
using var builder = new TTSBuilder();

var model = builder
    .SetMessageToPeople("Hello, this is a call from test.")
    .AddRecipient("+64211111111")
    .AddRecipient("+64222222222")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.TTS.SendMessage(model);
```

### Scheduled send

See `TTSSamples.SendScheduled()`.

```csharp
using var builder = new TTSBuilder();

var model = builder
    .SetMessageToPeople("Your reminder call.")
    .AddRecipient("+64211111111")
    .SetSendTime(DateTime.Now.AddDays(1))
    .SetTimezone("New Zealand")
    .SetSendMode(Enums.SendModeType.Test)
    .Build();

var response = client.Messaging.TTS.SendMessage(model);
```

### Poll for status

```csharp
var status = client.Messaging.TTS.Status(response.MessageID);

if (status.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"JobStatus: '{status.JobStatus}', JobNum: '{status.JobNum}'");

    foreach (var recipient in status.Recipients)
    {
        Console.WriteLine($" -> {recipient.Destination}: {recipient.Status} ({recipient.Result})");
    }
}
```

### Pacing (adjust simultaneous operators)

```csharp
client.Actions.TTS.Pacing(response.MessageID, numberOfOperators: 1);
```

## Response

- `SendMessage(...)` → `TTSApiResult`: `Result`, `MessageID`, `ErrorMessage`.
- `Status(...)` → `TTSStatusApiResult`: `MessageID`, `JobStatus`, `JobNum`, `Recipients` (each with `Destination`, `Status`, `Result`).
- `Reschedule(...)`/`Abort(...)`/`Resubmit(...)`/`Pacing(...)` → `TTSActionApiResult`: `Action`, `Status`.

## See also

- [README — TTS](../README.md#tts-text-to-speech)
- [Samples — TTSSamples.cs](../TNZAPI.NET.Samples/Messaging/TTS/TTSSamples.cs)