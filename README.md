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

## Messaging

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

### Send an SMS

```dotnet
from tnzapi import TNZAPI

client = TNZAPI(
    AuthToken="[Your Auth Token]"
)

request = client.Send.SMS(
    Reference="Test",
    MessageText = "Test SMS Message click [[Reply]] to opt out",
    Recipients = ["+64211231234"],
)

response = request.SendMessage()

print(repr(response))
```

### Send a Fax Document

```dotnet
from tnzapi import TNZAPI

client = TNZAPI(
    AuthToken="[Your Auth Token]"
)

request = client.Send.Fax(
    Recipients = "+6491232345",
    Attachments = ["C:\\Document.pdf"]
)

response = request.SendMessage()

print(repr(response))
```

### Make a Call - Text-to-Speech (TTS)

```dotnet
from tnzapi import TNZAPI

client = TNZAPI(
    AuthToken="[Your Auth Token]"
)

request = client.Send.TTS(
    Recipients = "+64211232345",
    Reference = "Voice Test - 64211232345",
    MessageToPeople = "Hi there!"
)

request.AddKeypad(Tone=1,RouteNumber="+6491232345",Play="You pressed 1")

response = request.SendMessage()

print(repr(response))
```

### Make a Call - Upload MP3 / Wav File

```dotnet
from tnzapi import TNZAPI

client = TNZAPI(
    AuthToken="[Your Auth Token]"
)

request = client.Send.Voice(
    Recipients = "+64211232345",
    Reference = "Voice Test - 64211232345",
)

request.AddMessageData("MessageToPeople","C:\\file1.wav")
request.AddMessageData("MessageToAnswerPhones","C:\\file2.wav")

request.AddKeypad(Tone=1,RouteNumber="+6491232345",PlayFile="C:\\file3.wav")

response = request.SendMessage()

print(repr(response))
```

### Getting help

If you need help installing or using the library, please check the [TNZ Contact](https://www.tnz.co.nz/About/Contact/) if you don't find an answer to your question.

[apidocs]: https://www.tnz.co.nz/Docs/dotNetLib/