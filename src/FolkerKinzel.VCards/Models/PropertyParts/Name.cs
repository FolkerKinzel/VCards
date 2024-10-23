using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Enums;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Formatters;
using FolkerKinzel.VCards.Intls.Serializers;
using StringExtension = FolkerKinzel.VCards.Intls.Extensions.StringExtension;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary>Encapsulates information about the name of the person the 
/// <see cref="VCard"/> represents.</summary>
public sealed class Name : IReadOnlyList<IReadOnlyList<string>>
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
    [Obsolete("Use NameFormatter instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string? ToDisplayName() => DisplayNameFormatter.ToDisplayName(this);

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
        IList<string> familyNames, surname2, suffixes, generation;
        familyNames = surname2 = suffixes = generation = Array.Empty<string>();

        foreach (KeyValuePair<NameProp, List<string>> kvp in builder.Data)
        {
            if (kvp.Value.Count != 0)
            {
                // Copy kvp.Value because it comes from NameBuilder and can be reused!
                _dic[kvp.Key] = new ReadOnlyCollection<string>(kvp.Value.ToArray());

                switch (kvp.Key)
                {
                    case NameProp.FamilyNames:
                        familyNames = kvp.Value;
                        break;
                    case NameProp.Surnames2:
                        surname2 = kvp.Value;
                        break;
                    case NameProp.Suffixes:
                        suffixes = kvp.Value;
                        break;
                    case NameProp.Generations:
                        generation = kvp.Value;
                        break;
                    default:
                        break;
                }
            }
        }

        if (surname2.Count != 0)
        {
            AddOldValueForCompatibility(NameProp.FamilyNames, familyNames, surname2, builder.Worker);
        }

        if (generation.Count != 0)
        {
            AddOldValueForCompatibility(NameProp.Suffixes, suffixes, generation, builder.Worker);
        }

        _familyNamesView = GetFamilyNamesView();
        _suffixesView = GetSuffixesView();
    }

    [SuppressMessage("Style", "IDE0305:Simplify collection initialization",
        Justification = "Performance: Collection initializer allocate a new List<string>")]
    private void AddOldValueForCompatibility(NameProp oldPropKey, IList<string> oldPropVals, IList<string> newPropVals, List<string> newValuesNotInOldProp)
    {
        GetNewValuesNotInOldProp(oldPropVals, newPropVals, newValuesNotInOldProp);

        if (newValuesNotInOldProp.Count != 0)
        {
            if (oldPropVals.Count != 0)
            {
                Debug.Assert(oldPropVals is List<string>);
                ((List<string>)oldPropVals).AddRange(newValuesNotInOldProp);

                // Copy oldPropVals because it comes from NameBuilder and can be reused!
                oldPropVals = oldPropVals.ToArray();
            }
            else
            {
                oldPropVals = newValuesNotInOldProp;
            }

            _dic[oldPropKey] = new ReadOnlyCollection<string>(oldPropVals);
        }

        static void GetNewValuesNotInOldProp(IList<string> oldPropVals, IList<string> newPropVals, List<string> newValuesNotInOldProp)
        {
            newValuesNotInOldProp.Clear();

            foreach (string newVal in newPropVals)
            {
                foreach (string oldVal in oldPropVals)
                {
                    if(oldVal.Contains(newVal, StringComparison.CurrentCultureIgnoreCase))
                    {
                        goto repeat;
                    }
                }

                newValuesNotInOldProp.Add(newVal);

repeat:
                continue;
            }
        }
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

    /// <summary>Family Name(s) (also known as "surname(s)"). (2,3,4)</summary>
    /// <remarks>
    /// Returns a collection that omits any value if an equal value is set to
    /// <see cref="Surnames2"/> accordingly.
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
    /// <see cref="Generations"/> accordingly.
    /// </remarks>
    public ReadOnlyCollection<string> Suffixes => _suffixesView;

    /// <summary>Secondary surnames (used in some cultures), also known as "maternal surnames". (4 - RFC 9554)</summary>
    public ReadOnlyCollection<string> Surnames2 => Get(NameProp.Surnames2);

    /// <summary>Generation markers or qualifiers, e.g., "Jr." or "III". (4 - RFC 9554)</summary>
    public ReadOnlyCollection<string> Generations => Get(NameProp.Generations);

    /// <summary>Returns <c>true</c>, if the <see cref="Name" /> object does not contain
    /// any usable data, otherwise <c>false</c>.</summary>
    public bool IsEmpty => _dic.Count == 0;

    /// <inheritdoc/>
    int IReadOnlyCollection<IReadOnlyList<string>>.Count => MAX_COUNT;

    /// <inheritdoc/>
    /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is less than zero, or equal or greater than
    /// <see cref="IReadOnlyCollection{T}.Count"/></exception>
    IReadOnlyList<string> IReadOnlyList<IReadOnlyList<string>>.this[int index]
    {
        get
        {
            if ((uint)index >= MAX_COUNT)
            {
                throw new IndexOutOfRangeException();
            }

            var nameProp = (NameProp)index;

            return nameProp switch
            {
                NameProp.FamilyNames => _familyNamesView,
                NameProp.Suffixes => _suffixesView,
                _ => Get(nameProp)
            };
        }
    }

    /// <inheritdoc/>
    IEnumerator<IReadOnlyList<string>> IEnumerable<IReadOnlyList<string>>.GetEnumerator()
    {
        for (int i = 0; i < MAX_COUNT; i++)
        {
            yield return ((IReadOnlyList<IReadOnlyList<string>>)this)[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IReadOnlyList<IReadOnlyList<string>>)this).GetEnumerator();

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
            && _dic.Any(x => x.Key >= NameProp.Surnames2))
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
    private ReadOnlyCollection<string> GetFamilyNamesView() => GetDiffView(NameProp.FamilyNames, NameProp.Surnames2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ReadOnlyCollection<string> GetSuffixesView() => GetDiffView(NameProp.Suffixes, NameProp.Generations);

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

            if (!newProps.Contains(old, StringComparer.CurrentCultureIgnoreCase))
            {
                list ??= [];
                list.Add(old);
            }
        }

        return list is null ? ReadOnlyStringCollection.Empty
                            : list.Count == oldProps.Count
                                 ? oldProps
                                 : list.AsReadOnly();
    }
}


