using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Samples.Addressbook.Contact.Groups;
using TNZAPI.NET.Samples.Addressbook.Contacts;
using TNZAPI.NET.Samples.Addressbook.Group.Contacts;
using TNZAPI.NET.Samples.Addressbook.Groups;
using TNZAPI.NET.Samples.Messaging.Actions;
using TNZAPI.NET.Samples.Messaging.Reports;
using TNZAPI.NET.Samples.Messaging.Send;

var apiUser = new TNZApiUser()
{
    AuthToken = "[TNZ Auth Token]"
};

const bool pauseOnEachTest = false;


//
// Test Send Messages
//

void TestSendMessages(ITNZAuth apiUser, bool pauseOnEachTest)
{
    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send Email'...");
    }
    else
    {
        Console.Write("Test 'Send Email' - Press any key to continue...");
        Console.ReadLine();
    }

    var emailTest = new SendEmail(apiUser);

    emailTest.Basic();
    emailTest.Simple();
    emailTest.Builder();
    emailTest.Advanced();

    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send SMS'...");
    }
    else
    {
        Console.Write("Test 'Send SMS' - Press any key to continue...");
        Console.ReadLine();
    }

    var smsTest = new SendSMS(apiUser);

    smsTest.Basic();
    smsTest.Simple();
    smsTest.Builder();
    smsTest.Advanced();

    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send Fax'...");
    }
    else
    {
        Console.Write("Test 'Send Fax' - Press any key to continue...");
        Console.ReadLine();
    }

    var faxTest = new SendFax(apiUser);

    faxTest.Basic();
    faxTest.Simple();
    faxTest.Builder();
    faxTest.Advanced();

    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send TTS'...");
    }
    else
    {
        Console.Write("Test 'Send TTS' - Press any key to continue...");
        Console.ReadLine();
    }

    var ttsTest = new SendTTS(apiUser);

    ttsTest.Basic();
    ttsTest.Simple();
    ttsTest.Builder();
    ttsTest.Advanced();

    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send Voice'...");
    }
    else
    {
        Console.Write("Test 'Send Voice' - Press any key to continue...");
        Console.ReadLine();
    }

    var voiceTest = new SendVoice(apiUser);

    voiceTest.Basic();
    voiceTest.Simple();
    voiceTest.Builder();
    voiceTest.Advanced();


}

void TestReports(ITNZAuth apiUser, bool pauseOnEachTest)
{
    ////
    //// Test Get Message Information
    ////

    if (!pauseOnEachTest)
    {
        Console.WriteLine("Test 'Get Status'...");
    }
    else
    {
        Console.Write("Test 'Get Status' - Press any key to continue...");
        Console.ReadLine();
    }

    var statusTest = new StatusReport(apiUser);

    statusTest.Basic();
    statusTest.Simple();
    statusTest.Builder();
    statusTest.Advanced();

    if (!pauseOnEachTest)
    {
        Console.WriteLine("Test 'Get SMS Received'...");
    }
    else
    {
        Console.Write("Test 'Get SMS Received' - Press any key to continue...");
        Console.ReadLine();
    }

    var smsReceivedTest = new SMSReceivedReport(apiUser);

    smsReceivedTest.Basic();
    smsReceivedTest.Simple();
    smsReceivedTest.Builder();
    smsReceivedTest.Advanced();
}

void TestActions(ITNZAuth apiUser, bool pauseOnEachTest)
{
    ////
    //// Test Edit Message
    ////

    if (!pauseOnEachTest)
    {
        Console.WriteLine("Test 'Set Resubmit'...");
    }
    else
    {
        Console.Write("Test 'Set Resubmit' - Press any key to continue...");
        Console.ReadLine();
    }

    var resubmitTest = new ResubmitAction(apiUser);

    resubmitTest.Basic();
    //resubmitTest.Simple();
    //resubmitTest.Builder();
    //resubmitTest.Advanced();

    if (!pauseOnEachTest)
    {
        Console.WriteLine("Test 'Set Reschedule'...");
    }
    else
    {
        Console.Write("Test 'Set Reschedule' - Press any key to continue...");
        Console.ReadLine();
    }

    var rescheduleTest = new RescheduleAction(apiUser);

    rescheduleTest.Basic();
    //rescheduleTest.Simple();
    //rescheduleTest.Builder();
    //rescheduleTest.Advanced();

    if (!pauseOnEachTest)
    {
        Console.WriteLine("Test 'Set Abort'...");
    }
    else
    {
        Console.Write("Test 'Set Abort' - Press any key to continue...");
        Console.ReadLine();
    }

    var abortTest = new AbortAction(apiUser);

    abortTest.Basic();
    //abortTest.Simple();
    //abortTest.Builder();
    //abortTest.Advanced();

    if (!pauseOnEachTest)
    {
        Console.WriteLine("Test 'Set NumberOfOperators'...");
    }
    else
    {
        Console.Write("Test 'Set NumberOfOperators' - Press any key to continue...");
        Console.ReadLine();
    }

    var numberOfOperatorsTest = new PacingAction(apiUser);

    numberOfOperatorsTest.Basic();
    //numberOfOperatorsTest.Simple();
    //numberOfOperatorsTest.Builder();
    //numberOfOperatorsTest.Advanced();
}

#region Addressbook Combined
void TestAddressbook(ITNZAuth apiUser, bool pauseOnEachTest)
{
    //
    // 1. Create contact
    //
    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send TTS'...");
    }
    Console.Write("Testing 'Create Contact' - Press any key to continue...");
    if (pauseOnEachTest)
        Console.ReadLine();

    var createContactTest = new CreateContact(apiUser, new ContactModel()
    {
        Attention = "Test Person"
    });

    var contact = createContactTest.Basic();

    if (contact is null)
    {
        throw new Exception("Contact was NOT created");
    }

    //
    // 2. Check created contact
    //
    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send TTS'...");
    }
    Console.Write("Testing 'Get Contact' - Press any key to continue...");
    if (pauseOnEachTest)
        Console.ReadLine();

    var getContactTest = new GetContact(apiUser, contact.ID);

    getContactTest.Basic();

    //
    // 3. Create Group
    //
    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send TTS'...");
    }
    Console.Write("Testing 'Create Group' - Press any key to continue...");
    if (pauseOnEachTest)
        Console.ReadLine();

    var createGroupTest = new CreateGroup(apiUser, new GroupModel()
    {
        GroupName = "Test Group"
    });

    var group = createGroupTest.Basic();


    if (group is null)
    {
        throw new Exception("Group was NOT created");
    }

    //
    // 4. Check Created Group
    //
    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send TTS'...");
    }
    Console.Write("Testing 'Get Group' - Press any key to continue...");
    if (pauseOnEachTest)
        Console.ReadLine();

    var getGroupTest = new GetGroup(apiUser, group.GroupCode);

    getGroupTest.Basic();

    //
    // 5. Update Contact
    //
    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send TTS'...");
    }
    Console.Write("Testing 'Update Contact' - Press any key to continue...");
    if (pauseOnEachTest)
        Console.ReadLine();

    contact.Attention = "Test Person Updated";

    var updateContactTest = new UpdateContact(apiUser, contact);

    updateContactTest.Basic();

    //
    // 6. Update Group
    //
    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send TTS'...");
    }
    Console.Write("Testing 'Update Group' - Press any key to continue...");
    if (pauseOnEachTest)
        Console.ReadLine();

    group.GroupName = "Test Group Updated";

    var updateGroupTest = new UpdateGroup(apiUser, group);

    updateGroupTest.Basic();

    //
    // 7. Add Contact Group
    //
    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send TTS'...");
    }
    Console.Write("Testing 'Add Contact Group' - Press any key to continue...");
    if (pauseOnEachTest)
        Console.ReadLine();

    var addContactGroupTest = new AddContactGroup(apiUser, contact, group);

    addContactGroupTest.Basic();

    //
    // 8. Get Contact Groups
    //
    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send TTS'...");
    }
    Console.Write("Testing 'Get Contact Group List' - Press any key to continue...");
    if (pauseOnEachTest)
        Console.ReadLine();

    var contactGroupListTest = new GetContactGroupList(apiUser, contact);

    contactGroupListTest.Basic();


    //
    // 9. Delete Contact Group
    //
    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send TTS'...");
    }
    Console.Write("Testing 'Delete Contact Group List' - Press any key to continue...");
    if (pauseOnEachTest)
        Console.ReadLine();

    var deleteContactGroupTest = new DeleteContactGroup(apiUser, contact, group);

    deleteContactGroupTest.Basic();

    //
    // 10. Add Group Contact
    //
    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send TTS'...");
    }
    Console.Write("Testing 'Add Group Contact' - Press any key to continue...");
    if (pauseOnEachTest)
        Console.ReadLine();

    var addGroupContactTest = new AddGroupContact(apiUser, group, contact);

    addGroupContactTest.Basic();

    //
    // 11. Get Group Contacts
    //
    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send TTS'...");
    }
    Console.Write("Testing 'Get Contact Group List' - Press any key to continue...");
    if (pauseOnEachTest)
        Console.ReadLine();

    var groupContactListTest = new GetGroupContactList(apiUser, group);

    groupContactListTest.Basic();

    //
    // 12. Delete Group Contact
    //
    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send TTS'...");
    }
    Console.Write("Testing 'Delete Group Contact' - Press any key to continue...");
    if (pauseOnEachTest)
        Console.ReadLine();

    var deleteGroupContactTest = new DeleteGroupContact(apiUser, group, contact);

    deleteGroupContactTest.Basic();

    //
    // 13. Delete Contact
    //
    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send TTS'...");
    }
    Console.Write("Testing 'Delete Contact' - Press any key to continue...");
    if (pauseOnEachTest)
        Console.ReadLine();

    var deleteContactTest = new DeleteContact(apiUser, contact);

    deleteContactTest.Basic();

    //
    // 14. Delete4 Group
    //
    if (!pauseOnEachTest)
    {
        Console.WriteLine("Testing 'Send TTS'...");
    }
    Console.Write("Testing 'Delete Group' - Press any key to continue...");
    if (pauseOnEachTest)
        Console.ReadLine();

    var deleteGroupTest = new DeleteGroup(apiUser, group);

    deleteGroupTest.Basic();
}
#endregion

//TestSendMessages(apiUser, pauseOnEachTest);
TestReports(apiUser, pauseOnEachTest);
//TestActions(apiUser, pauseOnEachTest);
//TestAddressbook(apiUser, pauseOnEachTest);

Console.Write("Finished!");
Console.ReadLine();
