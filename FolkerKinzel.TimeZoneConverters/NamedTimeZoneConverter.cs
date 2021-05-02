using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using FolkerKinzel.VCards;
using TZ = TimeZoneConverter;

namespace FolkerKinzel.TimeZoneConverters
{
    public class NamedTimeZoneConverter : INamedTimeZoneConverter
    {
        public bool TryGetUtcOffsetFromTimeZoneName(string tzName, VCard vcard, out TimeSpan offset)
        {
            offset = default;

            if (TZ::TZConvert.TryGetTimeZoneInfo(tzName, out TimeZoneInfo tzInfo))
            {
                offset = tzInfo.BaseUtcOffset;
                return true;
            }

            return false;
        }



        //    public static TimeZoneInfo ConvertPosixToTimeZoneInfo(string posix)
        //{
        //    string[] tokens = posix.Split(',');
        //    tokens[0] = tokens[0].Replace("/", ".");
        //    var match = Regex.Match(tokens[0], @"[-+]?[0-9]*\.?[0-9]+").Value;
        //    var ticks = (long)(decimal.Parse(match, CultureInfo.InvariantCulture) * 60) * 600000000;
        //    var baseOffset = new TimeSpan(ticks);

        //    var systemTimeZones = TimeZoneInfo.GetSystemTimeZones().Where(t => t.BaseUtcOffset == baseOffset).ToList();

        //    var startRuleTokens = tokens[1].TrimStart('M').Split('/');
        //    var startDateRuleTokens = startRuleTokens[0].Split('.');
        //    var startTimeRuleTokens = startRuleTokens[1].Split(':');

        //    var endRuleTokens = tokens[2].TrimStart('M').Split('/');
        //    var endDateRuleTokens = endRuleTokens[0].Split('.');
        //    var endTimeRuleTokens = endRuleTokens[1].Split(':');

        //    int? targetIndex = null;
        //    for (int i = 0; i < systemTimeZones.Count; i++)
        //    {
        //        var adjustmentRules = systemTimeZones[i].GetAdjustmentRules();
        //        foreach (var rule in adjustmentRules)
        //        {
        //            if (rule.DaylightTransitionStart.Month == int.Parse(startDateRuleTokens[0]) &&
        //                rule.DaylightTransitionStart.Week == int.Parse(startDateRuleTokens[1]) &&
        //                rule.DaylightTransitionStart.DayOfWeek == (DayOfWeek)int.Parse(startDateRuleTokens[2]) &&
        //                rule.DaylightTransitionStart.TimeOfDay.Hour == int.Parse(startTimeRuleTokens[0]) &&
        //                rule.DaylightTransitionStart.TimeOfDay.Minute == int.Parse(startTimeRuleTokens[1]) &&
        //                rule.DaylightTransitionStart.TimeOfDay.Second == int.Parse(startTimeRuleTokens[2]) &&

        //                rule.DaylightTransitionEnd.Month == int.Parse(endDateRuleTokens[0]) &&
        //                rule.DaylightTransitionEnd.Week == int.Parse(endDateRuleTokens[1]) &&
        //                rule.DaylightTransitionEnd.DayOfWeek == (DayOfWeek)int.Parse(endDateRuleTokens[2]) &&
        //                rule.DaylightTransitionEnd.TimeOfDay.Hour == int.Parse(endTimeRuleTokens[0]) &&
        //                rule.DaylightTransitionEnd.TimeOfDay.Minute == int.Parse(endTimeRuleTokens[1]) &&
        //                rule.DaylightTransitionEnd.TimeOfDay.Second == int.Parse(endTimeRuleTokens[2]))
        //            {
        //                targetIndex = i;
        //                break;
        //            }
        //        }
        //    }

        //    if (targetIndex.HasValue)
        //        return systemTimeZones[targetIndex.Value];
        //    return null;
        //}
    }
}
