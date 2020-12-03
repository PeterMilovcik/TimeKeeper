using System;

namespace TimeKeeper
{
    static class TimeSpanExtensions
    {
        public static string ToFormattedTimeSpan(this TimeSpan timeSpan)
        {
            var result = string.Empty;
            if (timeSpan >= TimeSpan.Zero)
            {
                if (timeSpan.Days > 0) result += $"{timeSpan.Days}.";
                if (timeSpan.Hours > 0) result += $"{timeSpan.Hours}:";
                if (timeSpan.Minutes > 0) result += $"{timeSpan.Minutes}:";
                result += $"{timeSpan.Seconds:00}";
            }
            else
            {
                result += "-";
                if (timeSpan.Days < 0) result += $"{-1 * timeSpan.Days}.";
                if (timeSpan.Hours < 0) result += $"{-1 * timeSpan.Hours}:";
                if (timeSpan.Minutes < 0) result += $"{-1 * timeSpan.Minutes}:";
                result += $"{-1 * timeSpan.Seconds:00}";
            }

            return result;
        }
    }
}
