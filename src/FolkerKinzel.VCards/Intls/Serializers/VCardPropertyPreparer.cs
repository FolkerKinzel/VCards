using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Intls.Serializers;

internal static class VCardPropertyPreparer
{

    internal static void PrepareForText(string text, (ParameterSection parameters, VcfSerializer serializer) tuple)
    {
        tuple.parameters.DataType = Data.Text;
        tuple.parameters.ContentLocation = Loc.Inline;

        if (tuple.serializer.Version == VCdVersion.V2_1 && text.NeedsToBeQpEncoded())
        {
            tuple.parameters.Encoding = Enc.QuotedPrintable;
            tuple.parameters.CharSet = VCard.DEFAULT_CHARSET;
        }
    }

    internal static void PrepareForUri(Uri uri, (ParameterSection parameters, VcfSerializer serializer) tuple)
    {
        if (tuple.serializer.Version == VCdVersion.V2_1)
        {
            if (uri.IsContentId())
            {
                tuple.parameters.ContentLocation = Loc.Cid;
            }
            else if (tuple.parameters.ContentLocation != Loc.Cid)
            {
                tuple.parameters.ContentLocation = Loc.Url;
            }
        }
        else
        {
            tuple.parameters.DataType = Data.Uri;
        }
    }
}