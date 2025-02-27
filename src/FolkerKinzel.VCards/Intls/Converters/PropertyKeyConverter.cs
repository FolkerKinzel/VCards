﻿namespace FolkerKinzel.VCards.Intls.Converters;
internal static class PropertyKeyConverter
{
    internal static string Parse(ReadOnlySpan<char> span)
    {
        const StringComparison comp = StringComparison.OrdinalIgnoreCase;

        return
            span.Equals(VCard.PropKeys.TEL, comp) ? VCard.PropKeys.TEL
            : span.Equals(VCard.PropKeys.EMAIL, comp) ? VCard.PropKeys.EMAIL
            : span.Equals(VCard.PropKeys.FN, comp) ? VCard.PropKeys.FN
            : span.Equals(VCard.PropKeys.VERSION, comp) ? VCard.PropKeys.VERSION
            : span.Equals(VCard.PropKeys.REV, comp) ? VCard.PropKeys.REV
            : span.Equals(VCard.PropKeys.UID, comp) ? VCard.PropKeys.UID
            : span.Equals(VCard.PropKeys.N, comp) ? VCard.PropKeys.N
            : span.Equals(VCard.PropKeys.ADR, comp) ? VCard.PropKeys.ADR
            : span.Equals(VCard.PropKeys.CLIENTPIDMAP, comp) ? VCard.PropKeys.CLIENTPIDMAP
            : span.Equals(VCard.PropKeys.NonStandard.X_AB_LABEL, comp) ? VCard.PropKeys.NonStandard.X_AB_LABEL
            : span.Equals(VCard.PropKeys.URL, comp) ? VCard.PropKeys.URL
            : span.Equals(VCard.PropKeys.BDAY, comp) ? VCard.PropKeys.BDAY
            : span.Equals(VCard.PropKeys.ANNIVERSARY, comp) ? VCard.PropKeys.ANNIVERSARY
            : span.Equals(VCard.PropKeys.LABEL, comp) ? VCard.PropKeys.LABEL
            : span.Equals(VCard.PropKeys.RELATED, comp) ? VCard.PropKeys.RELATED
            : span.Equals(VCard.PropKeys.GENDER, comp) ? VCard.PropKeys.GENDER
            : span.Equals(VCard.PropKeys.Rfc8605.CONTACT_URI, comp) ? VCard.PropKeys.Rfc8605.CONTACT_URI
            : span.Equals(VCard.PropKeys.Rfc9554.CREATED, comp) ? VCard.PropKeys.Rfc9554.CREATED
            : span.Equals(VCard.PropKeys.Rfc9554.GRAMGENDER, comp) ? VCard.PropKeys.Rfc9554.GRAMGENDER
            : span.Equals(VCard.PropKeys.Rfc9554.SOCIALPROFILE, comp) ? VCard.PropKeys.Rfc9554.SOCIALPROFILE
            : span.Equals(VCard.PropKeys.NonStandard.X_SOCIALPROFILE, comp) ? VCard.PropKeys.Rfc9554.SOCIALPROFILE
            : span.Equals(VCard.PropKeys.Rfc9555.JSPROP, comp) ? VCard.PropKeys.Rfc9555.JSPROP
            : span.ToString().ToUpperInvariant();
    }
}
