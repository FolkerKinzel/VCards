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
    }
}
