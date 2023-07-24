using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary>
/// Kapselt Informationen über die Postanschrift in vCards.
/// </summary>
public sealed class Address
{
    private const int MAX_COUNT = 7;

    private const int POST_OFFICE_BOX = 0;
    private const int EXTENDED_ADDRESS = 1;
    private const int STREET = 2;
    private const int LOCALITY = 3;
    private const int REGION = 4;
    private const int POSTAL_CODE = 5;
    private const int COUNTRY = 6;

    /// <summary>
    /// Initialisiert ein neues <see cref="Address"/>-Objekt.
    /// </summary>
    /// <param name="street">Straße</param>
    /// <param name="locality">Ort</param>
    /// <param name="region">Bundesland</param>
    /// <param name="postalCode">Postleitzahl</param>
    /// <param name="country">Land (Staat)</param>
    /// <param name="postOfficeBox">Postfach. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
    /// <param name="extendedAddress">Adresszusatz. (Nicht verwenden: Sollte immer <c>null</c> sein.)</param>
    internal Address(ReadOnlyCollection<string> street,
                     ReadOnlyCollection<string> locality,
                     ReadOnlyCollection<string> region,
                     ReadOnlyCollection<string> postalCode,
                     ReadOnlyCollection<string> country,
                     ReadOnlyCollection<string> postOfficeBox,
                     ReadOnlyCollection<string> extendedAddress)
    {
#pragma warning disable CS0618 // Typ oder Element ist veraltet
        PostOfficeBox = postOfficeBox;
        ExtendedAddress = extendedAddress;
#pragma warning restore CS0618 // Typ oder Element ist veraltet
        Street = street;
        Locality = locality;
        Region = region;
        PostalCode = postalCode;
        Country = country;
    }


    internal Address()
    {
#pragma warning disable CS0618 // Typ oder Element ist veraltet
        PostOfficeBox =
        ExtendedAddress =
#pragma warning restore CS0618 // Typ oder Element ist veraltet
            Street =
        Locality =
        Region =
        PostalCode =
        Country = ReadOnlyCollectionString.Empty;
    }


    internal Address(string vCardValue, VcfDeserializationInfo info, VCdVersion version)
    {
        Debug.Assert(vCardValue != null);

        StringBuilder builder = info.Builder;
        ValueSplitter semicolonSplitter = info.SemiColonSplitter;
        ValueSplitter commaSplitter = info.CommaSplitter;

        int index = 0;
        semicolonSplitter.ValueString = vCardValue;
        foreach (var s in semicolonSplitter)
        {
            switch (index++)
            {
#pragma warning disable CS0618 // Typ oder Element ist veraltet
                case POST_OFFICE_BOX:
                    {
                        if (s.Length == 0)
                        {
                            PostOfficeBox = ReadOnlyCollectionString.Empty;
                        }
                        else
                        {
                            var list = new List<string>();
                            commaSplitter.ValueString = s;

                            foreach (var item in commaSplitter)
                            {
                                list.Add(item.UnMask(builder, version));
                            }

                            PostOfficeBox = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                        }

                        break;
                    }
                case EXTENDED_ADDRESS:
                    {
                        if (s.Length == 0)
                        {
                            ExtendedAddress = ReadOnlyCollectionString.Empty;
                        }
                        else
                        {
                            var list = new List<string>();
                            commaSplitter.ValueString = s;

                            foreach (var item in commaSplitter)
                            {
                                list.Add(item.UnMask(builder, version));
                            }

                            ExtendedAddress = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                        }

                        break;
                    }
#pragma warning restore CS0618 // Typ oder Element ist veraltet
                case STREET:
                    {
                        if (s.Length == 0)
                        {
                            Street = ReadOnlyCollectionString.Empty;
                        }
                        else
                        {
                            var list = new List<string>();
                            commaSplitter.ValueString = s;

                            foreach (var item in commaSplitter)
                            {
                                list.Add(item.UnMask(builder, version));
                            }

                            Street = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                        }

                        break;
                    }
                case LOCALITY:
                    {
                        if (s.Length == 0)
                        {
                            Locality = ReadOnlyCollectionString.Empty;
                        }
                        else
                        {
                            var list = new List<string>();
                            commaSplitter.ValueString = s;

                            foreach (var item in commaSplitter)
                            {
                                list.Add(item.UnMask(builder, version));
                            }

                            Locality = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                        }

                        break;
                    }
                case REGION:
                    {
                        if (s.Length == 0)
                        {
                            Region = ReadOnlyCollectionString.Empty;
                        }
                        else
                        {
                            var list = new List<string>();
                            commaSplitter.ValueString = s;

                            foreach (var item in commaSplitter)
                            {
                                list.Add(item.UnMask(builder, version));
                            }

                            Region = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                        }

                        break;
                    }
                case POSTAL_CODE:
                    {
                        if (s.Length == 0)
                        {
                            PostalCode = ReadOnlyCollectionString.Empty;
                        }
                        else
                        {
                            var list = new List<string>();
                            commaSplitter.ValueString = s;

                            foreach (var item in commaSplitter)
                            {
                                list.Add(item.UnMask(builder, version));
                            }

                            PostalCode = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                        }

                        break;
                    }
                case COUNTRY:
                    {
                        if (s.Length == 0)
                        {
                            Country = ReadOnlyCollectionString.Empty;
                        }
                        else
                        {
                            var list = new List<string>();
                            commaSplitter.ValueString = s;

                            foreach (var item in commaSplitter)
                            {
                                list.Add(item.UnMask(builder, version));
                            }

                            Country = ReadOnlyCollectionConverter.ToReadOnlyCollection(list);
                        }

                        break;
                    }
                default:
                    break;
            }//switch
        }//foreach


        // Wenn die VCF-Datei fehlerhaft ist, könnten Properties null sein:

#pragma warning disable CS0618 // Typ oder Element ist veraltet
        PostOfficeBox ??= ReadOnlyCollectionString.Empty;
        ExtendedAddress ??= ReadOnlyCollectionString.Empty;
#pragma warning restore CS0618 // Typ oder Element ist veraltet
        Street ??= ReadOnlyCollectionString.Empty;
        Locality ??= ReadOnlyCollectionString.Empty;
        Region ??= ReadOnlyCollectionString.Empty;
        PostalCode ??= ReadOnlyCollectionString.Empty;
        Country ??= ReadOnlyCollectionString.Empty;
    }

    /// <summary>
    /// Postfach (nie <c>null</c>) (nicht verwenden)
    /// </summary>
    [Obsolete("Don't use this property.", false)]
    public ReadOnlyCollection<string> PostOfficeBox { get; }

    /// <summary>
    /// Adresszusatz (nie <c>null</c>) (nicht verwenden)
    /// </summary>
    [Obsolete("Don't use this property.", false)]
    public ReadOnlyCollection<string> ExtendedAddress { get; }

    /// <summary>
    /// Straße (nie <c>null</c>)
    /// </summary>
    public ReadOnlyCollection<string> Street { get; }

    /// <summary>
    /// Ort (nie <c>null</c>)
    /// </summary>
    public ReadOnlyCollection<string> Locality { get; }

    /// <summary>
    /// Bundesland (nie <c>null</c>)
    /// </summary>
    public ReadOnlyCollection<string> Region { get; }

    /// <summary>
    /// Postleitzahl (nie <c>null</c>)
    /// </summary>
    public ReadOnlyCollection<string> PostalCode { get; }

    /// <summary>
    /// Land (Staat) (nie <c>null</c>)
    /// </summary>
    public ReadOnlyCollection<string> Country { get; }


    /// <summary>
    /// <c>true</c>, wenn das <see cref="Address"/>-Objekt keine verwertbaren Daten enthält.
    /// </summary>
    public bool IsEmpty
    {
        get
        {
            if (Locality.Count != 0)
            {
                return false;
            }

            if (Street.Count != 0)
            {
                return false;
            }

            if (Country.Count != 0)
            {
                return false;
            }

            if (Region.Count != 0)
            {
                return false;
            }

            if (PostalCode.Count != 0)
            {
                return false;
            }

#pragma warning disable CS0618 // Typ oder Element ist veraltet
            if (PostOfficeBox.Count != 0)
            {
                return false;
            }

            if (ExtendedAddress.Count != 0)
            {
                return false;
            }
#pragma warning restore CS0618 // Typ oder Element ist veraltet

            return true;
        }
    }

    /// <summary>
    /// Wandelt die in der Instanz gekapselten Daten in formatierten Text für ein Adressetikett um.
    /// </summary>
    /// <returns>Die in der Instanz gekapselten Daten, umgewandelt in formatierten Text für ein Adressetikett.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToLabel() => this.ConvertToLabel();


    internal void AppendVCardString(VcfSerializer serializer)
    {
        StringBuilder builder = serializer.Builder;
        StringBuilder worker = serializer.Worker;

        char joinChar = serializer.Version < VCdVersion.V4_0 ? ' ' : ',';

#pragma warning disable CS0618 // Typ oder Element ist veraltet
        AppendProperty(PostOfficeBox);
        _ = builder.Append(';');

        AppendProperty(ExtendedAddress);
        _ = builder.Append(';');
#pragma warning restore CS0618 // Typ oder Element ist veraltet

        AppendProperty(Street);
        _ = builder.Append(';');

        AppendProperty(Locality);
        _ = builder.Append(';');

        AppendProperty(Region);
        _ = builder.Append(';');

        AppendProperty(PostalCode);
        _ = builder.Append(';');

        AppendProperty(Country);


        void AppendProperty(IList<string> strings)
        {
            if (strings.Count == 0)
            {
                return;
            }

            for (int i = 0; i < strings.Count - 1; i++)
            {
                _ = worker.Clear().Append(strings[i]).Mask(serializer.Version);
                _ = builder.Append(worker).Append(joinChar);
            }

            _ = worker.Clear().Append(strings[strings.Count - 1]).Mask(serializer.Version);
            _ = builder.Append(worker);
        }
    }

    internal bool NeedsToBeQpEncoded()
    {
        if (Locality.Any(x => x.NeedsToBeQpEncoded()))
        {
            return true;
        }

        if (Street.Any(x => x.NeedsToBeQpEncoded()))
        {
            return true;
        }

        if (Country.Any(x => x.NeedsToBeQpEncoded()))
        {
            return true;
        }

        if (Region.Any(x => x.NeedsToBeQpEncoded()))
        {
            return true;
        }

        if (PostalCode.Any(x => x.NeedsToBeQpEncoded()))
        {
            return true;
        }

#pragma warning disable CS0618 // Typ oder Element ist veraltet
        if (PostOfficeBox.Any(x => x.NeedsToBeQpEncoded()))
        {
            return true;
        }

        if (ExtendedAddress.Any(x => x.NeedsToBeQpEncoded()))
        {
            return true;
        }
#pragma warning restore CS0618 // Typ oder Element ist veraltet

        return false;
    }


    /// <summary>
    /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="Address"/>-Objekts. 
    /// (Nur zum Debugging.)
    /// </summary>
    /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="Address"/>-Objekts.</returns>
    public override string ToString()
    {
        var worker = new StringBuilder();
        var dic = new List<Tuple<string, string>>();

        for (int i = 0; i < MAX_COUNT; i++)
        {
            switch (i)
            {
#pragma warning disable CS0618 // Typ oder Element ist veraltet
                case POST_OFFICE_BOX:
                    {
                        string? s = BuildProperty(PostOfficeBox);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(PostOfficeBox), s));
                        break;
                    }
                case EXTENDED_ADDRESS:
                    {
                        string? s = BuildProperty(ExtendedAddress);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(ExtendedAddress), s));
                        break;
                    }
#pragma warning restore CS0618 // Typ oder Element ist veraltet
                case STREET:
                    {
                        string? s = BuildProperty(Street);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(Street), s));
                        break;
                    }
                case LOCALITY:
                    {
                        string? s = BuildProperty(Locality);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(Locality), s));
                        break;
                    }
                case REGION:
                    {
                        string? s = BuildProperty(Region);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(Region), s));
                        break;
                    }
                case POSTAL_CODE:
                    {
                        string? s = BuildProperty(PostalCode);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(PostalCode), s));
                        break;
                    }
                case COUNTRY:
                    {
                        string? s = BuildProperty(Country);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(Country), s));
                        break;
                    }
            }
        }

        if (dic.Count == 0)
        {
            return string.Empty;
        }

        int maxLength = dic.Select(x => x.Item1.Length).Max();
        maxLength += 2;

        _ = worker.Clear();

        for (int i = 0; i < dic.Count; i++)
        {
            Tuple<string, string>? tpl = dic[i];
            string s = tpl.Item1 + ": ";
            _ = worker.Append(s.PadRight(maxLength)).Append(tpl.Item2).Append(Environment.NewLine);
        }

        worker.Length -= Environment.NewLine.Length;
        return worker.ToString();

        ////////////////////////////////////////////

        string? BuildProperty(IList<string> strings)
        {
            _ = worker.Clear();

            if (strings.Count == 0)
            {
                return null;
            }

            for (int i = 0; i < strings.Count - 1; i++)
            {
                _ = worker.Append(strings[i]).Append(", ");
            }

            _ = worker.Append(strings[strings.Count - 1]);

            return worker.ToString();
        }
    }
}
