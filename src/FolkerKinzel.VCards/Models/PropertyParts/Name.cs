using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Enums;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using StringExtension = FolkerKinzel.VCards.Intls.Extensions.StringExtension;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary>Encapsulates information about the name of the person the 
/// <see cref="VCard"/> represents.</summary>
public sealed class Name
{
    private const int STANDARD_COUNT = 5;
    private const int MAX_COUNT = 7;
    private readonly Dictionary<NameProp, ReadOnlyCollection<string>> _dic = [];
    private readonly ReadOnlyCollection<string> _familyNamesView;
    private readonly ReadOnlyCollection<string> _suffixesView;

    private ReadOnlyCollection<string> Get(NameProp prop)
        => _dic.TryGetValue(prop, out ReadOnlyCollection<string>? coll)
            ? coll
            : ReadOnlyStringCollection.Empty;

    #region Remove this code with version 8.0.0

    /// <summary>Formats the data encapsulated by the instance into a human-readable
    /// form.</summary>
    /// <returns>The data encapsulated by the instance in human-readable form or
    /// <c>null</c> if the instance <see cref="IsEmpty"/>.</returns>
    /// <remarks>
    /// The method takes only the properties defined in RFC 6350 into account:
    /// <list type="bullet">
    /// <item><see cref="Prefixes"/></item>
    /// <item><see cref="GivenNames"/></item>
    /// <item><see cref="AdditionalNames"/></item>
    /// <item><see cref="FamilyNames"/></item>
    /// <item><see cref="Suffixes"/></item>
    /// </list>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string? ToDisplayName() => NameFormatter.ToDisplayName(this);

    private void Add(NameProp prop, ReadOnlyCollection<string> coll)
    {
        if (coll.Count != 0)
        {
            _dic[prop] = coll;
        }
    }

    internal Name(
        ReadOnlyCollection<string> familyNames,
        ReadOnlyCollection<string> givenNames,
        ReadOnlyCollection<string> additionalNames,
        ReadOnlyCollection<string> prefixes,
        ReadOnlyCollection<string> suffixes)
    {
        Add(NameProp.FamilyNames, familyNames);
        Add(NameProp.GivenNames, givenNames);
        Add(NameProp.AdditionalNames, additionalNames);
        Add(NameProp.Prefixes, prefixes);
        Add(NameProp.Suffixes, suffixes);

        _familyNamesView = familyNames;
        _suffixesView = suffixes;
    }

    #endregion

    internal Name() => _familyNamesView = _suffixesView = ReadOnlyStringCollection.Empty;

    [SuppressMessage("Style", "IDE0305:Simplify collection initialization",
        Justification = "Performance: Collection initializer initializes a new List.")]
    internal Name(NameBuilder builder)
    {
        foreach (KeyValuePair<NameProp, List<string>> kvp in builder.Data)
        {
            if (kvp.Value.Count != 0)
            {
                _dic[kvp.Key] = new ReadOnlyCollection<string>(kvp.Value.ToArray());
            }
        }

        _familyNamesView = GetFamilyNamesView();
        _suffixesView = GetSuffixesView();
    }

    internal Name(in ReadOnlyMemory<char> vCardValue, VCdVersion version)
    {
        int index = -1;

        foreach (ReadOnlyMemory<char> mem in PropertyValueSplitter.SplitIntoMemories(vCardValue, ';'))
        {
            index++;

            if (index >= MAX_COUNT)
            {
                break;
            }

            if (mem.IsEmpty)
            {
                continue;
            }

            ReadOnlySpan<char> span = mem.Span;
            ReadOnlyCollection<string> coll = span.Contains(',')
                ? ReadOnlyCollectionConverter.ToReadOnlyCollection(ToArray(in mem, version))
                : ReadOnlyCollectionConverter.ToReadOnlyCollection(span.UnMaskValue(version));

            if (coll.Count == 0)
            {
                continue;
            }

            _dic[(NameProp)index] = coll;
        }//foreach

        _familyNamesView = GetFamilyNamesView();
        _suffixesView = GetSuffixesView();

        /////////////////////////////////////////

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string[] ToArray(in ReadOnlyMemory<char> mem, VCdVersion version)
            => PropertyValueSplitter.Split(mem,
                                    ',',
                                    StringSplitOptions.RemoveEmptyEntries,
                                    unMask: true,
                                    version).ToArray();
    }

    /// <summary>Family Name(s) (also known as surname(s)). (2,3,4)</summary>
    /// <remarks>
    /// Returns a collection that omits any value if an equal value is set to
    /// <see cref="Surname2"/> accordingly.
    /// </remarks>
    public ReadOnlyCollection<string> FamilyNames => _familyNamesView;

    /// <summary>Given Name(s) (first name(s)). (2,3,4)</summary>
    public ReadOnlyCollection<string> GivenNames => Get(NameProp.GivenNames);

    /// <summary>Additional Name(s) (middle name(s)). (2,3,4)</summary>
    public ReadOnlyCollection<string> AdditionalNames => Get(NameProp.AdditionalNames);

    /// <summary>Honorific Prefix(es). (2,3,4)</summary>
    public ReadOnlyCollection<string> Prefixes => Get(NameProp.Prefixes);

    /// <summary>Honorific Suffix(es). (2,3,4)</summary>
    /// 
    /// <remarks>
    /// Returns a collection that omits any value if an equal value is set to
    /// <see cref="Generation"/> accordingly.
    /// </remarks>
    public ReadOnlyCollection<string> Suffixes => _suffixesView;

    /// <summary>A secondary surname (used in some cultures), also known as "maternal surname". (4 - RFC 9554)</summary>
    public ReadOnlyCollection<string> Surname2 => Get(NameProp.Surname2);

    /// <summary>A generation marker or qualifier, e.g., "Jr." or "III". (4 - RFC 9554)</summary>
    public ReadOnlyCollection<string> Generation => Get(NameProp.Generation);

    /// <summary>Returns <c>true</c>, if the <see cref="Name" /> object does not contain
    /// any usable data, otherwise <c>false</c>.</summary>
    public bool IsEmpty => _dic.Count == 0;

    /// <inheritdoc/>
    public override string ToString() => CompoundObjectConverter.ToString(_dic);

    internal void AppendVcfString(VcfSerializer serializer)
    {
        StringBuilder builder = serializer.Builder;
        int startIdx = builder.Length;

        char joinChar = serializer.Version < VCdVersion.V4_0 ? ' ' : ',';

        for (int i = 0; i < STANDARD_COUNT; i++)
        {
            CompoundObjectConverter.SerializeProperty(Get((NameProp)i), joinChar, serializer);
        }

        if (serializer.Version >= VCdVersion.V4_0
            && serializer.Options.HasFlag(Opts.WriteRfc9554Extensions)
            && _dic.Any(x => x.Key >= NameProp.Surname2))
        {
            for (int i = STANDARD_COUNT; i < MAX_COUNT; i++)
            {
                CompoundObjectConverter.SerializeProperty(Get((NameProp)i), joinChar, serializer);
            }
        }

        --builder.Length;

        if (serializer.ParameterSerializer.ParaSection.Encoding == Enc.QuotedPrintable)
        {
            CompoundObjectConverter.EncodeQuotedPrintable(builder, startIdx);
        }
    }

    internal bool NeedsToBeQpEncoded()
    {
        foreach (KeyValuePair<NameProp, ReadOnlyCollection<string>> kvp in _dic)
        {
            if (kvp.Key <= NameProp.Suffixes
                && kvp.Value.Any(StringExtension.NeedsToBeQpEncoded))
            {
                return true;
            }
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ReadOnlyCollection<string> GetFamilyNamesView() => GetDiffView(NameProp.FamilyNames, NameProp.Surname2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ReadOnlyCollection<string> GetSuffixesView() => GetDiffView(NameProp.Suffixes, NameProp.Generation);

    private ReadOnlyCollection<string> GetDiffView(NameProp oldIdx, NameProp newIdx)
    {
        if (!_dic.TryGetValue(oldIdx, out ReadOnlyCollection<string>? oldProps))
        {
            return ReadOnlyStringCollection.Empty;
        }

        if (!_dic.TryGetValue(newIdx, out ReadOnlyCollection<string>? newProps))
        {
            return oldProps;
        }

        List<string>? list = null;

        for (int i = 0; i < oldProps.Count; i++)
        {
            string old = oldProps[i];

            if (!newProps.Contains(old))
            {
                list ??= [];
                list.Add(old);
            }
        }

        return list is null ? ReadOnlyStringCollection.Empty
                            : list.SequenceEqual(oldProps)
                                 ? oldProps
                                 : list.AsReadOnly();
    }
}


