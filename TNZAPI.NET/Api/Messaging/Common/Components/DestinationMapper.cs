namespace TNZAPI.NET.Api.Messaging.Common.Components
{
    // Converts a Recipient (which only ever carries channel-specific alternate address fields) into
    // a Destination sending the wire's primary "Recipient" field instead. Shared by every messaging
    // module's Builder/Api ToDestination(Recipient) helper, since the field mapping is identical
    // everywhere except which Recipient field supplies the channel value. Kept separate from
    // Destination itself (rather than a static factory on the entity) so Destination stays a pure
    // entity with no knowledge of Recipient. Distinct from the generic, reflection-based
    // TNZAPI.NET.Helpers.Mapper — this is a hand-written, typed conversion between these two specific
    // component types.
    public static class DestinationMapper
    {
        public static Destination FromRecipient(Recipient recipient, string? recipientValue)
        {
            return new Destination
            {
                Recipient = recipientValue,
                Attention = recipient.Attention,
                Company = recipient.CompanyName,
                FirstName = recipient.FirstName,
                LastName = recipient.LastName,
                Custom1 = recipient.Custom1,
                Custom2 = recipient.Custom2,
                Custom3 = recipient.Custom3,
                Custom4 = recipient.Custom4,
                Custom5 = recipient.Custom5,
                Custom6 = recipient.Custom6,
                Custom7 = recipient.Custom7,
                Custom8 = recipient.Custom8,
                Custom9 = recipient.Custom9,
                ContactID = recipient.ContactID,
                GroupID = recipient.GroupID,
                GroupCode = recipient.GroupCode
            };
        }

        // Workflow-only overload: Workflow is omni-channel, so its Recipient can carry three
        // independent channel values simultaneously — these map onto Destination's alternate fields
        // rather than the single primary Recipient field the overload above targets.
        public static Destination FromRecipient(Recipient recipient, string? toNumber, string? mainPhone, string? emailAddress)
        {
            return new Destination
            {
                ToNumber = toNumber,
                MainPhone = mainPhone,
                EmailAddress = emailAddress,
                Attention = recipient.Attention,
                Company = recipient.CompanyName,
                FirstName = recipient.FirstName,
                LastName = recipient.LastName,
                Custom1 = recipient.Custom1,
                Custom2 = recipient.Custom2,
                Custom3 = recipient.Custom3,
                Custom4 = recipient.Custom4,
                Custom5 = recipient.Custom5,
                Custom6 = recipient.Custom6,
                Custom7 = recipient.Custom7,
                Custom8 = recipient.Custom8,
                Custom9 = recipient.Custom9,
                ContactID = recipient.ContactID,
                GroupID = recipient.GroupID,
                GroupCode = recipient.GroupCode
            };
        }
    }
}
