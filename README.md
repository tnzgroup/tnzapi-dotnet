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

```csharp
var apiUser = new TNZApiUser()
{
    AuthToken = "[Your Auth Token]"
};

var client = new TNZApiClient(apiUser);
```

## Messaging - Send SMS / Email / TTS / Voice / Fax

### Send an SMS

```csharp
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
```csharp
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

```csharp
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

```csharp
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

```csharp
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

```csharp
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

```csharp
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

```csharp
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

### Abort Job

```csharp
var response = client.Actions.Abort.Submit("ID123456"); // MessageID

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine("Status of MessageID '" + response.MessageID + "':");
    Console.WriteLine(" => Status: '" + response.GetStatusString() + "'");
    Console.WriteLine(" => JobNum: '" + response.JobNum + "'");
    Console.WriteLine(" => Action: '" + response.Action + "'");
}
```

### Change pacing (TTS/Voice message only)

```csharp
var response = client.Actions.Pacing.Submit(
    messageID: "ID123456",      // MessageID
    numberOfOperators: 1        // No. of operators
);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine("Status of MessageID '" + response.MessageID + "':");
    Console.WriteLine(" => Status: '" + response.GetStatusString() + "'");
    Console.WriteLine(" => JobNum: '" + response.JobNum + "'");
    Console.WriteLine(" => Action: '" + response.Action + "'");
}
```

## Addressbook - Contact

### Create Contact

```csharp
var requestResult = request.Addressbook.Contact.Create(
  new ContactBuilder()
    .SetAttention("API Test")
    .SetFirstName("API")
    .SetLastName("Test")
    .SetMobileNumber("+64211231234")
    .SetEmailAddress("test@example.com")
    .SetMainPhone("+6491112222")
    .Build()
);
```

### Get Contact Detail

```csharp
var response = client.Addressbook.Contact.ReadById(ContactID);

if (response.Result == ResultCode.Success)
{
    Console.WriteLine($"Contact details for ContactID={response.Contact.ID}");
    Console.WriteLine($"    -> Owner: '{response.Contact.Owner}'");
    Console.WriteLine($"    -> Created: '{response.Contact.Created}'");
    Console.WriteLine($"    -> Updated: '{response.Contact.Updated}'");
    Console.WriteLine($"    -> Attention: '{response.Contact.Attention}'");
    Console.WriteLine($"    -> Company: '{response.Contact.Company}'");
    Console.WriteLine($"    -> RecipDepartment: '{response.Contact.CompanyDepartment}'");
    Console.WriteLine($"    -> FirstName: '{response.Contact.FirstName}'");
    Console.WriteLine($"    -> LastName: '{response.Contact.LastName}'");
    Console.WriteLine($"    -> Position: '{response.Contact.Position}'");
    Console.WriteLine($"    -> StreetAddress: '{response.Contact.StreetAddress}'");
    Console.WriteLine($"    -> Suburb: '{response.Contact.Suburb}'");
    Console.WriteLine($"    -> City: '{response.Contact.City}'");
    Console.WriteLine($"    -> State: '{response.Contact.State}'");
    Console.WriteLine($"    -> Country: '{response.Contact.Country}'");
    Console.WriteLine($"    -> Postcode: '{response.Contact.Postcode}'");
    Console.WriteLine($"    -> MainPhone: '{response.Contact.MainPhone}'");
    Console.WriteLine($"    -> AltPhone1: '{response.Contact.AltPhone1}'");
    Console.WriteLine($"    -> AltPhone2: '{response.Contact.AltPhone2}'");
    Console.WriteLine($"    -> DirectPhone: '{response.Contact.DirectPhone}'");
    Console.WriteLine($"    -> MobilePhone: '{response.Contact.MobilePhone}'");
    Console.WriteLine($"    -> FaxNumber: '{response.Contact.FaxNumber}'");
    Console.WriteLine($"    -> EmailAddress: '{response.Contact.EmailAddress}'");
    Console.WriteLine($"    -> WebAddress: '{response.Contact.WebAddress}'");
    Console.WriteLine($"    -> Custom1: '{response.Contact.Custom1}'");
    Console.WriteLine($"    -> Custom2: '{response.Contact.Custom2}'");
    Console.WriteLine($"    -> Custom3: '{response.Contact.Custom3}'");
    Console.WriteLine($"    -> Custom4: '{response.Contact.Custom4}'");
    Console.WriteLine($"-------------------------");
}
```

### Update Contact

```csharp
var response = client.Addressbook.Contact.Update(contactModel);

if (response.Result == ResultCode.Success)
{
    Console.WriteLine($"Contact details for ContactID={response.Contact.ID}");
    Console.WriteLine($"    -> Owner: '{response.Contact.Owner}'");
    Console.WriteLine($"    -> Created: '{response.Contact.Created}'");
    Console.WriteLine($"    -> Updated: '{response.Contact.Updated}'");
    Console.WriteLine($"    -> Attention: '{response.Contact.Attention}'");
    Console.WriteLine($"    -> Company: '{response.Contact.Company}'");
    Console.WriteLine($"    -> RecipDepartment: '{response.Contact.CompanyDepartment}'");
    Console.WriteLine($"    -> FirstName: '{response.Contact.FirstName}'");
    Console.WriteLine($"    -> LastName: '{response.Contact.LastName}'");
    Console.WriteLine($"    -> Position: '{response.Contact.Position}'");
    Console.WriteLine($"    -> StreetAddress: '{response.Contact.StreetAddress}'");
    Console.WriteLine($"    -> Suburb: '{response.Contact.Suburb}'");
    Console.WriteLine($"    -> City: '{response.Contact.City}'");
    Console.WriteLine($"    -> State: '{response.Contact.State}'");
    Console.WriteLine($"    -> Country: '{response.Contact.Country}'");
    Console.WriteLine($"    -> Postcode: '{response.Contact.Postcode}'");
    Console.WriteLine($"    -> MainPhone: '{response.Contact.MainPhone}'");
    Console.WriteLine($"    -> AltPhone1: '{response.Contact.AltPhone1}'");
    Console.WriteLine($"    -> AltPhone2: '{response.Contact.AltPhone2}'");
    Console.WriteLine($"    -> DirectPhone: '{response.Contact.DirectPhone}'");
    Console.WriteLine($"    -> MobilePhone: '{response.Contact.MobilePhone}'");
    Console.WriteLine($"    -> FaxNumber: '{response.Contact.FaxNumber}'");
    Console.WriteLine($"    -> EmailAddress: '{response.Contact.EmailAddress}'");
    Console.WriteLine($"    -> WebAddress: '{response.Contact.WebAddress}'");
    Console.WriteLine($"    -> Custom1: '{response.Contact.Custom1}'");
    Console.WriteLine($"    -> Custom2: '{response.Contact.Custom2}'");
    Console.WriteLine($"    -> Custom3: '{response.Contact.Custom3}'");
    Console.WriteLine($"    -> Custom4: '{response.Contact.Custom4}'");
    Console.WriteLine($"-------------------------");
}
```

### Delete Contact

```csharp
var response = request.Addressbook.Contact.DeleteById(contactID);

if (response.Result == ResultCode.Success)
{
    Console.WriteLine($"Contact details for ContactID={response.Contact.ID}");
    Console.WriteLine($"    -> Owner: '{response.Contact.Owner}'");
    Console.WriteLine($"    -> Created: '{response.Contact.Created}'");
    Console.WriteLine($"    -> Updated: '{response.Contact.Updated}'");
    Console.WriteLine($"    -> Attention: '{response.Contact.Attention}'");
    Console.WriteLine($"    -> Company: '{response.Contact.Company}'");
    Console.WriteLine($"    -> RecipDepartment: '{response.Contact.CompanyDepartment}'");
    Console.WriteLine($"    -> FirstName: '{response.Contact.FirstName}'");
    Console.WriteLine($"    -> LastName: '{response.Contact.LastName}'");
    Console.WriteLine($"    -> Position: '{response.Contact.Position}'");
    Console.WriteLine($"    -> StreetAddress: '{response.Contact.StreetAddress}'");
    Console.WriteLine($"    -> Suburb: '{response.Contact.Suburb}'");
    Console.WriteLine($"    -> City: '{response.Contact.City}'");
    Console.WriteLine($"    -> State: '{response.Contact.State}'");
    Console.WriteLine($"    -> Country: '{response.Contact.Country}'");
    Console.WriteLine($"    -> Postcode: '{response.Contact.Postcode}'");
    Console.WriteLine($"    -> MainPhone: '{response.Contact.MainPhone}'");
    Console.WriteLine($"    -> AltPhone1: '{response.Contact.AltPhone1}'");
    Console.WriteLine($"    -> AltPhone2: '{response.Contact.AltPhone2}'");
    Console.WriteLine($"    -> DirectPhone: '{response.Contact.DirectPhone}'");
    Console.WriteLine($"    -> MobilePhone: '{response.Contact.MobilePhone}'");
    Console.WriteLine($"    -> FaxNumber: '{response.Contact.FaxNumber}'");
    Console.WriteLine($"    -> EmailAddress: '{response.Contact.EmailAddress}'");
    Console.WriteLine($"    -> WebAddress: '{response.Contact.WebAddress}'");
    Console.WriteLine($"    -> Custom1: '{response.Contact.Custom1}'");
    Console.WriteLine($"    -> Custom2: '{response.Contact.Custom2}'");
    Console.WriteLine($"    -> Custom3: '{response.Contact.Custom3}'");
    Console.WriteLine($"    -> Custom4: '{response.Contact.Custom4}'");
    Console.WriteLine($"-------------------------");
}
```

## Addressbook - Group

### Create Group

```csharp
var response = client.Addressbook.Group.Create(
  new GroupModel()
  {
      GroupName = "API Test Group"
  }
);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Group details for GroupCode={response.Group.GroupCode}");
    Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
    Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
    Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
    Console.WriteLine($"    -> Department: '{response.Group.Department}'");
    Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
    Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
    Console.WriteLine($"-------------------------");

    return response.Group;
}
```

### Get Group Detail

```csharp
var response = client.Addressbook.Group.GetByGroupCode(groupCode);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Group details for GroupCode={response.Group.GroupCode}");
    Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
    Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
    Console.WriteLine($"    -> Department: '{response.Group.Department}'");
    Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
    Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
    Console.WriteLine($"-------------------------");

    return response.Group;
}
```

### Update Group
```csharp
var response = client.Addressbook.Group.Update(groupModel);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Group details for GroupCode={response.Group.GroupCode}");
    Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
    Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
    Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
    Console.WriteLine($"    -> Department: '{response.Group.Department}'");
    Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
    Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
    Console.WriteLine($"-------------------------");
}
```

### Delete Group
```csharp
var response = client.Addressbook.Group.Delete(groupModel);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Group details for GroupCode={response.Group.GroupCode}");
    Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
    Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
    Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
    Console.WriteLine($"    -> Department: '{response.Group.Department}'");
    Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
    Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
    Console.WriteLine($"-------------------------");
}
```

## Addressbook - Contact/Group Relationships

### List ContactGroup

```csharp
var response = client.Addressbook.ContactGroupList.List(
    contactModel,           // ContactModel
    recordsPerPage: 10,     // Record per page
    page: 1                 // Page number
);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Contact list details");
    Console.WriteLine($"    -> Contact ID: {response.Contact.ID}");
    Console.WriteLine($"    -> Total Records: {response.TotalRecords}");
    Console.WriteLine($"    -> Records Per Page: {response.RecordsPerPage}");
    Console.WriteLine($"    -> Page Count: {response.PageCount}");
    Console.WriteLine($"    -> Page: {response.Page}");

    if (response.Groups is not null)
    {
        foreach (var group in response.Groups)
        {
            Console.WriteLine($"Group details for GroupCode={group.GroupCode}");
            Console.WriteLine($"    -> GroupCode: '{group.GroupCode}'");
            Console.WriteLine($"    -> GroupName: '{group.GroupName}'");
            Console.WriteLine($"    -> SubAccount: '{group.SubAccount}'");
            Console.WriteLine($"    -> Department: '{group.Department}'");
            Console.WriteLine($"    -> ViewEditBy: '{group.ViewEditBy}'");
            Console.WriteLine($"    -> Owner: '{group.Owner}'");
            Console.WriteLine($"-------------------------");
        }
    }
}
```

### Add ContactGroup

```csharp
var response = request.Addressbook.ContactGroup.Add(contactModel, groupModel);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Contact details for ContactID={response.Contact.ID}");
    Console.WriteLine($"    -> Owner: '{response.Contact.Owner}'");
    Console.WriteLine($"    -> Created: '{response.Contact.Created}'");
    Console.WriteLine($"    -> Updated: '{response.Contact.Updated}'");
    Console.WriteLine($"    -> Attention: '{response.Contact.Attention}'");
    Console.WriteLine($"    -> Company: '{response.Contact.Company}'");
    Console.WriteLine($"    -> RecipDepartment: '{response.Contact.CompanyDepartment}'");
    Console.WriteLine($"    -> FirstName: '{response.Contact.FirstName}'");
    Console.WriteLine($"    -> LastName: '{response.Contact.LastName}'");
    Console.WriteLine($"    -> Position: '{response.Contact.Position}'");
    Console.WriteLine($"    -> StreetAddress: '{response.Contact.StreetAddress}'");
    Console.WriteLine($"    -> Suburb: '{response.Contact.Suburb}'");
    Console.WriteLine($"    -> City: '{response.Contact.City}'");
    Console.WriteLine($"    -> State: '{response.Contact.State}'");
    Console.WriteLine($"    -> Country: '{response.Contact.Country}'");
    Console.WriteLine($"    -> Postcode: '{response.Contact.Postcode}'");
    Console.WriteLine($"    -> MainPhone: '{response.Contact.MainPhone}'");
    Console.WriteLine($"    -> AltPhone1: '{response.Contact.AltPhone1}'");
    Console.WriteLine($"    -> AltPhone2: '{response.Contact.AltPhone2}'");
    Console.WriteLine($"    -> DirectPhone: '{response.Contact.DirectPhone}'");
    Console.WriteLine($"    -> MobilePhone: '{response.Contact.MobilePhone}'");
    Console.WriteLine($"    -> FaxNumber: '{response.Contact.FaxNumber}'");
    Console.WriteLine($"    -> EmailAddress: '{response.Contact.EmailAddress}'");
    Console.WriteLine($"    -> WebAddress: '{response.Contact.WebAddress}'");
    Console.WriteLine($"    -> Custom1: '{response.Contact.Custom1}'");
    Console.WriteLine($"    -> Custom2: '{response.Contact.Custom2}'");
    Console.WriteLine($"    -> Custom3: '{response.Contact.Custom3}'");
    Console.WriteLine($"    -> Custom4: '{response.Contact.Custom4}'");
    Console.WriteLine($"-------------------------");

    if (response.Group is not null)
    {
        Console.WriteLine($"Group details for GroupCode={response.Group.GroupCode}");
        Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
        Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
        Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
        Console.WriteLine($"    -> Department: '{response.Group.Department}'");
        Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
        Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
        Console.WriteLine($"-------------------------");
    }

    return response;
}
```

### Remove ContactGroup

```csharp
var response = client.Addressbook.ContactGroup.Remove(contactModel, groupModel);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Contact details for ContactID={response.Contact.ID}");
    Console.WriteLine($"    -> Owner: '{response.Contact.Owner}'");
    Console.WriteLine($"    -> Created: '{response.Contact.Created}'");
    Console.WriteLine($"    -> Updated: '{response.Contact.Updated}'");
    Console.WriteLine($"    -> Attention: '{response.Contact.Attention}'");
    Console.WriteLine($"    -> Company: '{response.Contact.Company}'");
    Console.WriteLine($"    -> RecipDepartment: '{response.Contact.CompanyDepartment}'");
    Console.WriteLine($"    -> FirstName: '{response.Contact.FirstName}'");
    Console.WriteLine($"    -> LastName: '{response.Contact.LastName}'");
    Console.WriteLine($"    -> Position: '{response.Contact.Position}'");
    Console.WriteLine($"    -> StreetAddress: '{response.Contact.StreetAddress}'");
    Console.WriteLine($"    -> Suburb: '{response.Contact.Suburb}'");
    Console.WriteLine($"    -> City: '{response.Contact.City}'");
    Console.WriteLine($"    -> State: '{response.Contact.State}'");
    Console.WriteLine($"    -> Country: '{response.Contact.Country}'");
    Console.WriteLine($"    -> Postcode: '{response.Contact.Postcode}'");
    Console.WriteLine($"    -> MainPhone: '{response.Contact.MainPhone}'");
    Console.WriteLine($"    -> AltPhone1: '{response.Contact.AltPhone1}'");
    Console.WriteLine($"    -> AltPhone2: '{response.Contact.AltPhone2}'");
    Console.WriteLine($"    -> DirectPhone: '{response.Contact.DirectPhone}'");
    Console.WriteLine($"    -> MobilePhone: '{response.Contact.MobilePhone}'");
    Console.WriteLine($"    -> FaxNumber: '{response.Contact.FaxNumber}'");
    Console.WriteLine($"    -> EmailAddress: '{response.Contact.EmailAddress}'");
    Console.WriteLine($"    -> WebAddress: '{response.Contact.WebAddress}'");
    Console.WriteLine($"    -> Custom1: '{response.Contact.Custom1}'");
    Console.WriteLine($"    -> Custom2: '{response.Contact.Custom2}'");
    Console.WriteLine($"    -> Custom3: '{response.Contact.Custom3}'");
    Console.WriteLine($"    -> Custom4: '{response.Contact.Custom4}'");
    Console.WriteLine($"-------------------------");

    if (response.Group is not null)
    {
        Console.WriteLine($"Group details for GroupCode={response.Group.GroupCode}");
        Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
        Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
        Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
        Console.WriteLine($"    -> Department: '{response.Group.Department}'");
        Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
        Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
        Console.WriteLine($"-------------------------");
    }
}
```

### List GroupContact

```csharp
var response = client.Addressbook.GroupContactList.List(
    groupModel,             // GroupModel
    recordsPerPage: 10,     // Number of records for this request
    page: 1                 // Current location
);

if (response.Result == Enums.ResultCode.Success)
{
    Console.WriteLine($"Group Contact list details");
    Console.WriteLine($"    -> GroupCode: {response.Group.GroupCode}");
    Console.WriteLine($"    -> Total Records: {response.TotalRecords}");
    Console.WriteLine($"    -> Records Per Page: {response.RecordsPerPage}");
    Console.WriteLine($"    -> Page Count: {response.PageCount}");
    Console.WriteLine($"    -> Page: {response.Page}");

    foreach (var contact in response.Contacts)
    {
        Console.WriteLine($"Contact details for ContactID={contact.ID}");
        Console.WriteLine($"    -> Owner: '{contact.Owner}'");
        Console.WriteLine($"    -> Created: '{contact.Created}'");
        Console.WriteLine($"    -> Updated: '{contact.Updated}'");
        Console.WriteLine($"    -> Attention: '{contact.Attention}'");
        Console.WriteLine($"    -> Company: '{contact.Company}'");
        Console.WriteLine($"    -> RecipDepartment: '{contact.CompanyDepartment}'");
        Console.WriteLine($"    -> FirstName: '{contact.FirstName}'");
        Console.WriteLine($"    -> LastName: '{contact.LastName}'");
        Console.WriteLine($"    -> Position: '{contact.Position}'");
        Console.WriteLine($"    -> StreetAddress: '{contact.StreetAddress}'");
        Console.WriteLine($"    -> Suburb: '{contact.Suburb}'");
        Console.WriteLine($"    -> City: '{contact.City}'");
        Console.WriteLine($"    -> State: '{contact.State}'");
        Console.WriteLine($"    -> Country: '{contact.Country}'");
        Console.WriteLine($"    -> Postcode: '{contact.Postcode}'");
        Console.WriteLine($"    -> MainPhone: '{contact.MainPhone}'");
        Console.WriteLine($"    -> AltPhone1: '{contact.AltPhone1}'");
        Console.WriteLine($"    -> AltPhone2: '{contact.AltPhone2}'");
        Console.WriteLine($"    -> DirectPhone: '{contact.DirectPhone}'");
        Console.WriteLine($"    -> MobilePhone: '{contact.MobilePhone}'");
        Console.WriteLine($"    -> FaxNumber: '{contact.FaxNumber}'");
        Console.WriteLine($"    -> EmailAddress: '{contact.EmailAddress}'");
        Console.WriteLine($"    -> WebAddress: '{contact.WebAddress}'");
        Console.WriteLine($"    -> Custom1: '{contact.Custom1}'");
        Console.WriteLine($"    -> Custom2: '{contact.Custom2}'");
        Console.WriteLine($"    -> Custom3: '{contact.Custom3}'");
        Console.WriteLine($"    -> Custom4: '{contact.Custom4}'");
        Console.WriteLine($"-------------------------");
    }
}
```

### Add GroupContact

```csharp
var response = client.Addressbook.GroupContact.Add(groupModel, contactModel);

if (response.Result == Enums.ResultCode.Success)
{
    if (response.Group is not null)
    {
        Console.WriteLine($"Group details for GroupCode={response.Group.GroupCode}");
        Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
        Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
        Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
        Console.WriteLine($"    -> Department: '{response.Group.Department}'");
        Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
        Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
        Console.WriteLine($"-------------------------");
    }
    if (response.Contact is not null)
    {
        Console.WriteLine($"Contact details for ContactID={response.Contact.ID}");
        Console.WriteLine($"    -> Owner: '{response.Contact.Owner}'");
        Console.WriteLine($"    -> Created: '{response.Contact.Created}'");
        Console.WriteLine($"    -> Updated: '{response.Contact.Updated}'");
        Console.WriteLine($"    -> Attention: '{response.Contact.Attention}'");
        Console.WriteLine($"    -> Company: '{response.Contact.Company}'");
        Console.WriteLine($"    -> RecipDepartment: '{response.Contact.CompanyDepartment}'");
        Console.WriteLine($"    -> FirstName: '{response.Contact.FirstName}'");
        Console.WriteLine($"    -> LastName: '{response.Contact.LastName}'");
        Console.WriteLine($"    -> Position: '{response.Contact.Position}'");
        Console.WriteLine($"    -> StreetAddress: '{response.Contact.StreetAddress}'");
        Console.WriteLine($"    -> Suburb: '{response.Contact.Suburb}'");
        Console.WriteLine($"    -> City: '{response.Contact.City}'");
        Console.WriteLine($"    -> State: '{response.Contact.State}'");
        Console.WriteLine($"    -> Country: '{response.Contact.Country}'");
        Console.WriteLine($"    -> Postcode: '{response.Contact.Postcode}'");
        Console.WriteLine($"    -> MainPhone: '{response.Contact.MainPhone}'");
        Console.WriteLine($"    -> AltPhone1: '{response.Contact.AltPhone1}'");
        Console.WriteLine($"    -> AltPhone2: '{response.Contact.AltPhone2}'");
        Console.WriteLine($"    -> DirectPhone: '{response.Contact.DirectPhone}'");
        Console.WriteLine($"    -> MobilePhone: '{response.Contact.MobilePhone}'");
        Console.WriteLine($"    -> FaxNumber: '{response.Contact.FaxNumber}'");
        Console.WriteLine($"    -> EmailAddress: '{response.Contact.EmailAddress}'");
        Console.WriteLine($"    -> WebAddress: '{response.Contact.WebAddress}'");
        Console.WriteLine($"    -> Custom1: '{response.Contact.Custom1}'");
        Console.WriteLine($"    -> Custom2: '{response.Contact.Custom2}'");
        Console.WriteLine($"    -> Custom3: '{response.Contact.Custom3}'");
        Console.WriteLine($"    -> Custom4: '{response.Contact.Custom4}'");
        Console.WriteLine($"-------------------------");
    }
}
```

### Remove GroupContact

```csharp
var response = client.Addressbook.GroupContact.Remove(groupModel, contactModel);

if (response.Result == Enums.ResultCode.Success)
{
    if (response.Group is not null)
    {
        Console.WriteLine($"Group details for GroupCode={response.Group.GroupCode}");
        Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
        Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
        Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
        Console.WriteLine($"    -> Department: '{response.Group.Department}'");
        Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
        Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
        Console.WriteLine($"-------------------------");
    }
    if (response.Contact is not null)
    {
        Console.WriteLine($"Contact details for ContactID={response.Contact.ID}");
        Console.WriteLine($"    -> Owner: '{response.Contact.Owner}'");
        Console.WriteLine($"    -> Created: '{response.Contact.Created}'");
        Console.WriteLine($"    -> Updated: '{response.Contact.Updated}'");
        Console.WriteLine($"    -> Attention: '{response.Contact.Attention}'");
        Console.WriteLine($"    -> Company: '{response.Contact.Company}'");
        Console.WriteLine($"    -> RecipDepartment: '{response.Contact.CompanyDepartment}'");
        Console.WriteLine($"    -> FirstName: '{response.Contact.FirstName}'");
        Console.WriteLine($"    -> LastName: '{response.Contact.LastName}'");
        Console.WriteLine($"    -> Position: '{response.Contact.Position}'");
        Console.WriteLine($"    -> StreetAddress: '{response.Contact.StreetAddress}'");
        Console.WriteLine($"    -> Suburb: '{response.Contact.Suburb}'");
        Console.WriteLine($"    -> City: '{response.Contact.City}'");
        Console.WriteLine($"    -> State: '{response.Contact.State}'");
        Console.WriteLine($"    -> Country: '{response.Contact.Country}'");
        Console.WriteLine($"    -> Postcode: '{response.Contact.Postcode}'");
        Console.WriteLine($"    -> MainPhone: '{response.Contact.MainPhone}'");
        Console.WriteLine($"    -> AltPhone1: '{response.Contact.AltPhone1}'");
        Console.WriteLine($"    -> AltPhone2: '{response.Contact.AltPhone2}'");
        Console.WriteLine($"    -> DirectPhone: '{response.Contact.DirectPhone}'");
        Console.WriteLine($"    -> MobilePhone: '{response.Contact.MobilePhone}'");
        Console.WriteLine($"    -> FaxNumber: '{response.Contact.FaxNumber}'");
        Console.WriteLine($"    -> EmailAddress: '{response.Contact.EmailAddress}'");
        Console.WriteLine($"    -> WebAddress: '{response.Contact.WebAddress}'");
        Console.WriteLine($"    -> Custom1: '{response.Contact.Custom1}'");
        Console.WriteLine($"    -> Custom2: '{response.Contact.Custom2}'");
        Console.WriteLine($"    -> Custom3: '{response.Contact.Custom3}'");
        Console.WriteLine($"    -> Custom4: '{response.Contact.Custom4}'");
        Console.WriteLine($"-------------------------");
    }
}
```

## Support

### Getting help

If you need help installing or using the library, please check the [TNZ Contact](https://www.tnz.co.nz/About/Contact/) if you don't find an answer to your question.

[apidocs]: https://www.tnz.co.nz/Docs/dotNetLib/