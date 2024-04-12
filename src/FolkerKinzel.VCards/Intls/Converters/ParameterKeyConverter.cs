using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class ParameterKeyConverter
{
    internal static string ParseParameterKey(ReadOnlySpan<char> key)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return key.StartsWith(
            ParameterSection.ParameterKey.TYPE, comp)
            ? ParameterSection.ParameterKey.TYPE
            : key.StartsWith(
                ParameterSection.ParameterKey.VALUE, comp)
            ? ParameterSection.ParameterKey.VALUE
            : key.StartsWith(
        ParameterSection.ParameterKey.PREF, comp)
            ? ParameterSection.ParameterKey.PREF
            : key.StartsWith(
        ParameterSection.ParameterKey.PID, comp)
            ? ParameterSection.ParameterKey.PID
            : key.StartsWith(
        ParameterSection.ParameterKey.MEDIATYPE, comp)
            ? ParameterSection.ParameterKey.MEDIATYPE
            : key.StartsWith(
        ParameterSection.ParameterKey.ENCODING, comp)
            ? ParameterSection.ParameterKey.ENCODING
            : key.StartsWith(
        ParameterSection.ParameterKey.CHARSET, comp)
            ? ParameterSection.ParameterKey.CHARSET
            : key.StartsWith(
        ParameterSection.ParameterKey.LABEL, comp)
            ? ParameterSection.ParameterKey.LABEL
            : key.StartsWith(
        ParameterSection.ParameterKey.LANGUAGE, comp)
            ? ParameterSection.ParameterKey.LANGUAGE
            : key.StartsWith(
        ParameterSection.ParameterKey.ALTID, comp)
            ? ParameterSection.ParameterKey.ALTID
            : key.StartsWith(
        ParameterSection.ParameterKey.SORT_AS, comp)
            ? ParameterSection.ParameterKey.SORT_AS
            : key.StartsWith(
        ParameterSection.ParameterKey.GEO, comp)
            ? ParameterSection.ParameterKey.GEO
            : key.StartsWith(
        ParameterSection.ParameterKey.TZ, comp)
            ? ParameterSection.ParameterKey.TZ
            : key.StartsWith(
        ParameterSection.ParameterKey.INDEX, comp)
            ? ParameterSection.ParameterKey.INDEX
        //    : key.StartsWith(
        //ParameterSection.ParameterKey.LEVEL, comp)
        //    ? ParameterSection.ParameterKey.LEVEL
        //    : key.StartsWith(
        //ParameterSection.ParameterKey.CALSCALE, comp)
        //    ? ParameterSection.ParameterKey.CALSCALE
        //    : key.StartsWith(
        //ParameterSection.ParameterKey.CONTEXT, comp)
        //    ? ParameterSection.ParameterKey.CONTEXT
            : key.ToString();
    }


}
