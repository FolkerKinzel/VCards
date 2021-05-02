using System;
using FolkerKinzel.VCards;

namespace FolkerKinzel.TimeZoneConverters
{
    public interface INamedTimeZoneConverter
    {
        bool TryGetUtcOffsetFromTimeZoneName(string tzName, VCard vcard, out TimeSpan offset);
    }
}