using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using StringExtension = FolkerKinzel.VCards.Intls.Extensions.StringExtension;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary>Encapsulates information about the name of the person the 
/// <see cref="VCard"/> represents.</summary>
public sealed class Name
{
    private const int MAX_COUNT = 5;

    private const int FAMILY_NAMES = 0;
    private const int GIVEN_NAMES = 1;
    private const int ADDITIONAL_NAMES = 2;
    private const int PREFIXES = 3;
    private const int SUFFIXES = 4;

    internal Name(
        ReadOnlyCollection<string> familyNames,
        ReadOnlyCollection<string> givenNames,
        ReadOnlyCollection<string> additionalNames,
        ReadOnlyCollection<string> prefixes,
        ReadOnlyCollection<string> suffixes)
    {
        FamilyNames = familyNames;
        GivenNames = givenNames;
        AdditionalNames = additionalNames;
        Prefixes = prefixes;
        Suffixes = suffixes;
    }

    internal Name()
    {
        FamilyNames =
        GivenNames =
        AdditionalNames =
        Prefixes =
        Suffixes = ReadOnlyStringCollection.Empty;
    }

    internal Name(ReadOnlyMemory<char> vCardValue, VcfDeserializationInfo info, VCdVersion version)
    {
        int index = 0;

        foreach (ReadOnlyMemory<char> mem in ValueSplitter2.Split(vCardValue, ';'))
        {
            switch (index++)
            {
                case FAMILY_NAMES:
                    {
                        FamilyNames = mem.Length == 0
                            ? ReadOnlyStringCollection.Empty
                            : ReadOnlyCollectionConverter.ToReadOnlyCollection(ToArray(mem, version));

                        break;
                    }
                case GIVEN_NAMES:
                    {
                        GivenNames = mem.Length == 0
                            ? ReadOnlyStringCollection.Empty
                            : ReadOnlyCollectionConverter.ToReadOnlyCollection(ToArray(mem, version));

                        break;
                    }
                case ADDITIONAL_NAMES:
                    {
                        AdditionalNames = mem.Length == 0
                            ? ReadOnlyStringCollection.Empty
                            : ReadOnlyCollectionConverter.ToReadOnlyCollection(ToArray(mem, version));

                        break;
                    }
                case PREFIXES:
                    {
                        Prefixes = mem.Length == 0
                            ? ReadOnlyStringCollection.Empty
                            : ReadOnlyCollectionConverter.ToReadOnlyCollection(ToArray(mem, version));

                        break;
                    }
                case SUFFIXES:
                    {
                        Suffixes = mem.Length == 0
                            ? ReadOnlyStringCollection.Empty
                            : ReadOnlyCollectionConverter.ToReadOnlyCollection(ToArray(mem, version));

                        break;
                    }
                default:
                    break;
            }//switch
        }//foreach

        // If the VCF file is invalid, properties could be null:
        FamilyNames ??= ReadOnlyStringCollection.Empty;
        GivenNames ??= ReadOnlyStringCollection.Empty;
        AdditionalNames ??= ReadOnlyStringCollection.Empty;
        Prefixes ??= ReadOnlyStringCollection.Empty;
        Suffixes ??= ReadOnlyStringCollection.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string[] ToArray(ReadOnlyMemory<char> mem, VCdVersion version)
            => ValueSplitter2.Split(mem,
                                    ',',
                                    StringSplitOptions.RemoveEmptyEntries,
                                    unMask: true,
                                    version).ToArray();
    }

    /// <summary>Family Name(s) (also known as surname(s)).</summary>
    public ReadOnlyCollection<string> FamilyNames { get; }

    /// <summary>Given Name(s) (first name(s)).</summary>
    public ReadOnlyCollection<string> GivenNames { get; }

    /// <summary>Additional Name(s) (middle name(s)).</summary>
    public ReadOnlyCollection<string> AdditionalNames { get; }

    /// <summary>Honorific Prefix(es).</summary>
    public ReadOnlyCollection<string> Prefixes { get; }

    /// <summary>Honorific Suffix(es).</summary>
    public ReadOnlyCollection<string> Suffixes { get; }

    /// <summary>Returns <c>true</c>, if the <see cref="Name" /> object does not contain
    /// any usable data, otherwise <c>false</c>.</summary>
    public bool IsEmpty => FamilyNames.Count == 0 &&
                           GivenNames.Count == 0 &&
                           AdditionalNames.Count == 0 &&
                           Prefixes.Count == 0 &&
                           Suffixes.Count == 0;

    /// <inheritdoc/>
    public override string ToString()
    {
        var worker = new StringBuilder();
        var dic = new List<Tuple<string, string>>();

        for (int i = 0; i < MAX_COUNT; i++)
        {
            switch (i)
            {
                case FAMILY_NAMES:
                    {
                        string? s = BuildProperty(FamilyNames);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(FamilyNames), s));
                        break;
                    }
                case GIVEN_NAMES:
                    {
                        string? s = BuildProperty(GivenNames);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(GivenNames), s));
                        break;
                    }
                case ADDITIONAL_NAMES:
                    {
                        string? s = BuildProperty(AdditionalNames);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(AdditionalNames), s));
                        break;
                    }
                case PREFIXES:
                    {
                        string? s = BuildProperty(Prefixes);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(Prefixes), s));
                        break;
                    }
                case SUFFIXES:
                    {
                        string? s = BuildProperty(Suffixes);

                        if (s is null)
                        {
                            continue;
                        }

                        dic.Add(new Tuple<string, string>(nameof(Suffixes), s));
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

        //////////////////////////////////////////////////

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

    /// <summary>Formats the data encapsulated by the instance into a human-readable
    /// form.</summary>
    /// <returns>The data encapsulated by the instance in human-readable form or
    /// <c>null</c> if the instance <see cref="IsEmpty"/>.</returns>
    public string? ToDisplayName()
    {
        const int stringBuilderInitialCapacity = 32;
        return IsEmpty
            ? null
            : new StringBuilder(stringBuilderInitialCapacity)
            .AppendReadableProperty(Prefixes)
            .AppendReadableProperty(GivenNames)
            .AppendReadableProperty(AdditionalNames)
            .AppendReadableProperty(FamilyNames)
            .AppendReadableProperty(Suffixes)
            .ToString();
    }


    internal void AppendVCardString(VcfSerializer serializer)
    {
        StringBuilder builder = serializer.Builder;
        int startIdx = builder.Length;

        char joinChar = serializer.Version < VCdVersion.V4_0 ? ' ' : ',';

        AppendProperty(FamilyNames);
        _ = builder.Append(';');

        AppendProperty(GivenNames);
        _ = builder.Append(';');

        AppendProperty(AdditionalNames);
        _ = builder.Append(';');

        AppendProperty(Prefixes);
        _ = builder.Append(';');

        AppendProperty(Suffixes);

        if (serializer.ParameterSerializer.ParaSection.Encoding == Enc.QuotedPrintable)
        {
            int count = builder.Length - startIdx;
            using ArrayPoolHelper.SharedArray<char> tmp = ArrayPoolHelper.Rent<char>(count);
            builder.CopyTo(startIdx, tmp.Array, 0, count);
            builder.Length = startIdx;
            builder.AppendQuotedPrintable(tmp.Array.AsSpan(0, count), startIdx);
        }

        ///////////////////////////////////

        void AppendProperty(IList<string> strings)
        {
            if (strings.Count == 0)
            {
                return;
            }

            for (int i = 0; i < strings.Count; i++)
            {
                _ = builder.AppendMasked(strings[i], serializer.Version).Append(joinChar);
            }

            --builder.Length;
        }
    }

    internal bool NeedsToBeQpEncoded()
        => FamilyNames.Any(StringExtension.NeedsToBeQpEncoded) ||
           GivenNames.Any(StringExtension.NeedsToBeQpEncoded) ||
           AdditionalNames.Any(StringExtension.NeedsToBeQpEncoded) ||
           Prefixes.Any(StringExtension.NeedsToBeQpEncoded) ||
           Suffixes.Any(StringExtension.NeedsToBeQpEncoded);
}


