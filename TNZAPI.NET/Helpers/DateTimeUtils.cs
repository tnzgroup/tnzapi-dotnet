namespace TNZAPI.NET.Helpers
{
    public static class DateTimeUtils
    {
        public static double GetLocalTimeOffset()
        {
            return TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalMinutes;
        }

        public static DateTime ChangeToUTCDateTime(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Local).ToUniversalTime();
        }

        public static DateTime ChangeToLocalDateTime(this DateTime dateTime)
        {
            return dateTime.AddMinutes(GetLocalTimeOffset());
        }
    }
}
