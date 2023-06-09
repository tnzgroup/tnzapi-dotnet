# TNZAPI.NET

## Documentation

The documentation for the TNZ API can be found [here][apidocs].

## Versions

`tnzapi` uses a modified version of [Semantic Versioning](https://semver.org) for all changes. [See this document](VERSIONS.md) for details.

### Supported .NET Versions

This library supports the following .NET implementations:

* .NET6

## Getting Started

Getting started with the TNZ API couldn't be easier. Create a
`Client` and you're ready to go.

### API Credentials

The `TNZAPI` needs your TNZ API credentials (TNZ Auth Tokens). You can either pass these
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

### Getting help

If you need help installing or using the library, please check the [TNZ Contact](https://www.tnz.co.nz/About/Contact/) if you don't find an answer to your question.

[apidocs]: https://www.tnz.co.nz/Docs/dotNetLib/