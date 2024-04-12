using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Intls.Converters;
internal static class PropertyKeyConverter
{
    internal static string Parse(ReadOnlySpan<char> span)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return
            span.Equals(VCard.PropKeys.TEL, comp)
            ? VCard.PropKeys.TEL
            : span.Equals(VCard.PropKeys.EMAIL, comp)
            ? VCard.PropKeys.EMAIL
            : span.Equals(VCard.PropKeys.FN, comp)
            ? VCard.PropKeys.FN
            : span.Equals(VCard.PropKeys.VERSION, comp)
            ? VCard.PropKeys.VERSION
            : span.Equals(VCard.PropKeys.REV, comp)
            ? VCard.PropKeys.REV
            : span.Equals(VCard.PropKeys.UID, comp)
            ? VCard.PropKeys.UID
            : span.Equals(VCard.PropKeys.N, comp)
            ? VCard.PropKeys.N
            : span.Equals(VCard.PropKeys.ADR, comp)
            ? VCard.PropKeys.ADR
            : span.Equals(VCard.PropKeys.CLIENTPIDMAP, comp)
            ? VCard.PropKeys.CLIENTPIDMAP
            : span.ToString().ToUpperInvariant();
    }
}
