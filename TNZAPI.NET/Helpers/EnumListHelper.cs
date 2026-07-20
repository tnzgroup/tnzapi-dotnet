namespace TNZAPI.NET.Helpers
{
    internal static class EnumListHelper
    {
        // Shared by SMS/RCS/WhatsApp FallbackMode wire serialization — TNZ's API accepts a comma-separated
        // list of mode names for all three channels. An optional tokenMapper lets a caller translate an
        // enum value to its wire token when it doesn't match the enum member's own name (see
        // ToFallbackModeWireToken below) — default is the member's own ToString().
        internal static string? ToCommaSeparatedString<T>(ICollection<T>? values, Func<T, string>? tokenMapper = null) where T : struct, Enum
        {
            if (values is null || values.Count == 0)
            {
                return null;
            }

            tokenMapper ??= value => value.ToString();

            return string.Join(", ", values.Select(tokenMapper));
        }

        // Confirmed against the live API: the FallbackMode wire token for WhatsApp is "WAPP", not
        // "WhatsApp" — every other FallbackMode value's wire token matches its C# member name
        // exactly, so this is the only enum-to-wire-token translation FallbackMode needs.
        internal static string ToFallbackModeWireToken<T>(T mode) where T : struct, Enum
        {
            var name = mode.ToString();

            return name == "WhatsApp" ? "WAPP" : name;
        }
    }
}