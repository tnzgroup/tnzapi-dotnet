# TNZAPI.NET

## Documentation

The documentation for the TNZ API can be found [here][apidocs].

## Versions

`TNZAPI.NET` uses a modified version of [Semantic Versioning](https://semver.org) for all changes. [See this document](VERSIONS.md) for details.

### Supported .NET Versions

This library supports the following .NET implementations:

* .NET6

## Getting Started

Getting started with the TNZ API couldn't be easier. Create a
`TNZApiClient` and you're ready to go.

### API Credentials

The `TNZAPI.NET` needs your TNZ API credentials (TNZ Auth Tokens). You can either pass these
directly to the constructor (see the code below) or via environment variables.

```dotnet
var apiUser = new TNZApiUser()
{
    AuthToken = "[Your Auth Token]"
};

var client = new TNZApiClient(apiUser);
```

## Messaging - Send SMS / Email / TTS / Voice / Fax

### Send an SMS

```dotnet
var response = client.Messaging.SMS.SendMessage(
    destinations: new List<string>()
    {
        "+64211111111",                         // Recipient
        "+64222222222"                          // Recipient
    },
    messageText: "Test SMS",                    // SMS Message
    sendMode: Enums.SendModeType.Test           // TEST Mode - Remove this to send live traffic
    );

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine("Success - " + response.MessageID);
}
else
{
    Console.WriteLine("Error occurred while processing...");
    foreach (var error in response.ErrorMessage)
    {
        Console.WriteLine($"- {error}");
    }
}
```

### Send an Email
```dotnet
var response = client.Messaging.Email.SendMessage(
    fromEmail: "from@test.com",             // Optional : Sets From Email Address - leave blank to use your api username as email sender
    emailSubject: "Test Email",             // Email Subject
    messagePlain: "Test Email Body",        // Email Body
    destination: "email.one@test.com",      // Recipient 1
    sendMode: Enums.SendModeType.Test       // TEST Mode - Remove this to send live traffic
);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine("Success - " + response.MessageID);
}
else
{
    Console.WriteLine("Error - " + response.ErrorMessage);
}
```

### Make a Call - Text-to-Speech (TTS)

```dotnet
var response = client.Messaging.TTS.SendMessage(
    messageToPeople: "Hello, this is a call from test. This is relevant information.", // Message to people
    destinations: new List<string>
    {
        "+64211111111",                     // Recipient
        "+64222222222",                     // Recipient
    },
    ttsVoiceType: TTSVoiceType.Emma,        // TTS Engine
    sendMode: Enums.SendModeType.Test       // TEST Mode - Remove this to send live traffic
);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine("Success - " + response.MessageID);
}
else
{
    Console.WriteLine("Error occurred while processing...");
    foreach (var error in response.ErrorMessage)
    {
        Console.WriteLine($"- {error}");
    }
}
```

### Make a Call - Upload MP3 / Wav File

```dotnet
var response = client.Messaging.Voice.SendMessage(
    destinations: new List<string>()
    {
        "+64211111111",                     // Recipient
        "+64222222222"                      // Recipient
    },
    messageToPeople: "D:\\File1.wav",       // WAV format, 16-bit, 8000hz
    sendMode: Enums.SendModeType.Test       // TEST Mode - Remove this to send live traffic
);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine("Success - " + response.MessageID);
}
else
{
    Console.WriteLine("Error occurred while processing...");
    foreach (var error in response.ErrorMessage)
    {
        Console.WriteLine($"- {error}");
    }
}
```

### Send a Fax Document

```dotnet
var client = new TNZApiClient(apiUser);

var response = client.Messaging.Fax.SendMessage(
    destinations: new List<string>()
    {
        "+6491111111",                      // Recipient 1
        "+6491111112"                       // Recipient 2
    },
    file: "D:\\File1.pdf",                  // Attach File
    sendMode: Enums.SendModeType.Test       // TEST Mode - Remove this to send live traffic
);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine("Success - " + response.MessageID);
}
else
{
    Console.WriteLine("Error occurred while processing...");
    foreach (var error in response.ErrorMessage)
    {
        Console.WriteLine($"- {error}");
    }
}
```

## Reporting

### Message Status

```dotnet
var response = client.Reports.Status.Poll("ID123456");

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Status of MessageID '{response.MessageID}':");
    Console.WriteLine($" => Status: '{response.Status}'");
    Console.WriteLine($" => JobNum: '{response.JobNum}'");
    Console.WriteLine($" => Account: '{response.Account}'");
    Console.WriteLine($" => SubAccount: '{response.SubAccount}'");
    Console.WriteLine($" => Department: '{response.Department}'");
    Console.WriteLine($" => Reference: '{response.Reference}'");
    Console.WriteLine($" => Created: '{response.Created}'");
    Console.WriteLine($" => CreatedUTC: '{response.CreatedUTC}'");
    Console.WriteLine($" => Delayed: '{response.Delayed}'");
    Console.WriteLine($" => DelayedUTC: '{response.DelayedUTC}'");
    Console.WriteLine($" => Count: {response.Count}");
    Console.WriteLine($" => Complete: {response.Complete}");
    Console.WriteLine($" => Success: {response.Success}");
    Console.WriteLine($" => Failed: {response.Failed}");
    Console.WriteLine($" => TotalRecords (Recipients): {response.TotalRecords}");
    Console.WriteLine($" => PageCount (Recipients): {response.PageCount}");
    Console.WriteLine($" => RecordsPerPage (Recipients): {response.RecordsPerPage}");
    Console.WriteLine($" => Page (Recipients): {response.Page}");

    foreach (var message in response.Recipients)
    {
        Console.WriteLine($"======================================");
        Console.WriteLine($" => Message Delivered");
        Console.WriteLine($"    -> Type: '{message.Type}'");
        Console.WriteLine($"    -> DestSeq: '{message.DestSeq}'");
        Console.WriteLine($"    -> Destination: '{message.Destination}'");
        Console.WriteLine($"    -> MessageText: '{message.MessageText}'");
        Console.WriteLine($"    -> Status: '{message.Status}'");
        Console.WriteLine($"    -> Result: '{message.Result}'");
        Console.WriteLine($"    -> SentDate: '{message.SentDate}'");
        Console.WriteLine($"    -> Attention: '{message.Attention}'");
        Console.WriteLine($"    -> Company: '{message.Company}'");
        Console.WriteLine($"    -> Custom1: '{message.Custom1}'");
        Console.WriteLine($"    -> Custom2: '{message.Custom2}'");
        Console.WriteLine($"    -> Custom3: '{message.Custom3}'");
        Console.WriteLine($"    -> Custom4: '{message.Custom4}'");
        Console.WriteLine($"    -> Custom5: '{message.Custom5}'");
        Console.WriteLine($"    -> Custom6: '{message.Custom6}'");
        Console.WriteLine($"    -> Custom7: '{message.Custom7}'");
        Console.WriteLine($"    -> Custom8: '{message.Custom8}'");
        Console.WriteLine($"    -> Custom9: '{message.Custom9}'");
        Console.WriteLine($"    -> RemoteID: '{message.RemoteID}'");
        Console.WriteLine($"    -> Price: '{message.Price}'");

        foreach (var reply in message.SMSReplies)
        {
            Console.WriteLine($"======================================");
            Console.WriteLine($" => SMS Reply");
            Console.WriteLine($"    -> Date: '{reply.Date}'");
            Console.WriteLine($"    -> DateUTC: '{reply.DateUTC}'");
            Console.WriteLine($"    -> From: '{reply.From}'");
            Console.WriteLine($"    -> MessageText: '{reply.MessageText}'");
        }
    }
}
```

### SMS Received

```dotnet
var client = new TNZApiClient(apiUser);

var response = client.Reports.SMSReceived.List(
    timePeriod: 1440,       // No. of minutes
    recordsPerPage: 100,    // x numbers of records to return per request
    page: 1                 // current location
);

if (response.Result == Enums.ResultCode.Success)
{
    foreach (var received in response.Messages)
    {
        Console.WriteLine("======================================");
        Console.WriteLine(" => MessageReceived");
        Console.WriteLine("    -> Date: '" + received.Date.ToString("yyyy-MM-dd hh:mm:ss") + "'");
        Console.WriteLine("    -> From: '" + received.From + "'");
        Console.WriteLine("    -> MessageText: '" + received.MessageText.Replace("'", "\'") + "'");
    }

}
```

## Actions - Resbumit / Reschedule / Abort / Pacing

### Resubmit

```csharp
var response = client.Actions.Resubmit.Submit("ID123456"); // Message ID

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine("Status of MessageID '" + response.MessageID + "':");
    Console.WriteLine(" => Status: '" + response.GetStatusString() + "'");
    Console.WriteLine(" => JobNum: '" + response.JobNum + "'");
    Console.WriteLine(" => Action: '" + response.Action + "'");
}
```

### Reschedule
```cs
var response = client.Actions.Reschedule.Submit(
    messageID: "ID123456",                              // MessageID
    sendTime: DateTime.Parse("2023-12-31T12:00:00")     // Set send time
);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine("Status of MessageID '" + response.MessageID + "':");
    Console.WriteLine(" => Status: '" + response.GetStatusString() + "'");
    Console.WriteLine(" => JobNum: '" + response.JobNum + "'");
    Console.WriteLine(" => Action: '" + response.Action + "'");
}
```

## Addressbook

###

## Support

### Getting help

If you need help installing or using the library, please check the [TNZ Contact](https://www.tnz.co.nz/About/Contact/) if you don't find an answer to your question.

[apidocs]: https://www.tnz.co.nz/Docs/dotNetLib/