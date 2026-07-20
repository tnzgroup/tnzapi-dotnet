using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Helpers
{
    // Shared defensive guard for the private static URL builders across every Api class. Every
    // public caller already validates its ID non-null/non-empty before reaching a URL builder, so
    // these throws only ever fire if a future call site forgets to validate first — they exist to
    // fail loudly with a clear exception type instead of silently producing a malformed URL or
    // crashing inside Uri.EscapeDataString with no context.
    internal static class IDGuard
    {
        internal static void EnsureProvided(MessageID? messageID)
        {
            if (messageID is null)
            {
                throw new ArgumentNullException(nameof(messageID));
            }
            if (string.IsNullOrEmpty(messageID.Value))
            {
                throw new ArgumentException("MessageID must be provided.", nameof(messageID));
            }
        }

        internal static void EnsureProvided(ContactID? contactID)
        {
            if (contactID is null)
            {
                throw new ArgumentNullException(nameof(contactID));
            }
            if (string.IsNullOrEmpty(contactID.Value))
            {
                throw new ArgumentException("ContactID must be provided.", nameof(contactID));
            }
        }

        internal static void EnsureProvided(GroupID? groupID)
        {
            if (groupID is null)
            {
                throw new ArgumentNullException(nameof(groupID));
            }
            if (string.IsNullOrEmpty(groupID.Value))
            {
                throw new ArgumentException("GroupID must be provided.", nameof(groupID));
            }
        }

        internal static void EnsureProvided(OptOutID? optOutID)
        {
            if (optOutID is null)
            {
                throw new ArgumentNullException(nameof(optOutID));
            }
            if (string.IsNullOrEmpty(optOutID.Value))
            {
                throw new ArgumentException("OptOutID must be provided.", nameof(optOutID));
            }
        }
    }
}
