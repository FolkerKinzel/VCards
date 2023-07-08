using System.Text;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.PropertyParts;

#if !NET40
using FolkerKinzel.Strings;
#endif

namespace FolkerKinzel.VCards.Intls.Converters;

#pragma warning disable CS0618 // Typ oder Element ist veraltet

internal static class AddressToDinLabelConverter
{
    private const int BUILDER_CAPACITY = 256;
    private const int WORKER_CAPACITY = 64;

    internal static string ConvertToDinLabel(this Address address)
    {
        StringBuilder builder = new StringBuilder(BUILDER_CAPACITY);
        StringBuilder worker = new StringBuilder(WORKER_CAPACITY);

        if (address.PostOfficeBox.Count != 0)
        {
            builder.AppendReadableProperty(address.PostOfficeBox).Append(Environment.NewLine);
        }
        else
        {
            builder.AppendReadableProperty(address.Street);

            if(address.ExtendedAddress.Count != 0) 
            {
                if (builder.Length != 0) { builder.Append(' '); }
                builder.Append(@"//").AppendReadableProperty(address.ExtendedAddress).Append(Environment.NewLine);
            }
            else
            {
                builder.AppendNewLineIfNeeded();
            }
        }

        worker.AppendReadableProperty(address.PostalCode).AppendReadableProperty(address.Locality).AppendReadableProperty(address.Region);
        builder.Append(worker).AppendNewLineIfNeeded();

        worker.Clear().AppendReadableProperty(address.Country).ToUpperInvariant();
        return builder.Append(worker).TrimEnd().ToString();
    }


    private static StringBuilder AppendStreet(this StringBuilder builder, Address address)
    {
        return builder.AppendReadableProperty(address.Street);
    }

    private static StringBuilder AppendNewLineIfNeeded(this StringBuilder builder)
    {
        if(builder.Length != 0) { builder.Append(Environment.NewLine); }    
        return builder;
    }
}
#pragma warning restore CS0618 // Typ oder Element ist veraltet
