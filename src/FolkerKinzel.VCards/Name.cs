using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;

/* Unmerged change from project 'FolkerKinzel.VCards (net8.0)'
Before:
using FolkerKinzel.VCards.Enums;
After:
using FolkerKinzel;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
*/
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Enums;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Formatters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using StringExtension = FolkerKinzel.VCards.Intls.Extensions.StringExtension;

namespace FolkerKinzel.VCards;

/// <summary>Encapsulates information about the name of the person the 
/// <see cref="VCard"/> represents.</summary>
/// <remarks>
/// <note type="tip">Use <see cref="NameBuilder"/> to create an instance.</note>
/// </remarks>
/// <seealso cref="NameBuilder"/>
/// <seealso cref="NameProperty"/>
/// <seealso cref="VCard.NameViews"/>
public sealed class Name : IReadOnlyList<IReadOnlyList<string>>
{
    #region Remove this code with version 8.0.1

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use NameFormatter instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public string? ToDisplayName() => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    [Obsolete("Use Given instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ReadOnlyCollection<string> GivenNames => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    [Obsolete("Use Given instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ReadOnlyCollection<string> AdditionalNames => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    [Obsolete("Use Surnames instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public IReadOnlyCollection<string> FamilyNames => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    #endregion

    private const int STANDARD_COUNT = 5;
    private const int MAX_COUNT = 7;
    private readonly Dictionary<NameProp, string[]> _dic = [];

    private string[] Get(NameProp prop)
        => _dic.TryGetValue(prop, out string[]? coll) ? coll : [];

    internal Name() => Surnames = Suffixes = [];

    /// <summary>
    /// Initializes a new <see cref="Name"/> instance with the content of a 
    /// specified <see cref="NameBuilder"/>.
    /// </summary>
    /// 
    /// <param name="builder">The <see cref="NameBuilder"/> whose content is used.</param>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <c>null</c>.</exception>
    [SuppressMessage("Style", "IDE0305:Simplify collection initialization",
        Justification = "Performance: Collection initializer initializes a new List.")]
    internal Name(NameBuilder builder)
    {
        List<string> surnames, surname2, suffixes, generation;
        surnames = surname2 = suffixes = generation = [];

        foreach (KeyValuePair<NameProp, List<string>> kvp in builder.Data)
        {
            if (kvp.Value.Count != 0)
            {
                // Copy kvp.Value because it comes from NameBuilder and can be reused!
                _dic[kvp.Key] = kvp.Value.ToArray();

                switch (kvp.Key)
                {
                    case NameProp.Surnames:
                        surnames = kvp.Value;
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
            AddOldValueForCompatibility(NameProp.Surnames, surnames, surname2, builder.Worker);
        }

        if (generation.Count != 0)
        {
            AddOldValueForCompatibility(NameProp.Suffixes, suffixes, generation, builder.Worker);
        }

        Surnames = GetSurnamesView();
        Suffixes = GetSuffixesView();
    }

    [SuppressMessage("Style", "IDE0305:Simplify collection initialization",
        Justification = "Performance: Collection initializer allocate a new List<string>")]
    private void AddOldValueForCompatibility(NameProp oldPropKey, List<string> oldPropVals, IReadOnlyList<string> newPropVals, List<string> newValuesNotInOldProp)
    {
        GetNewValuesNotInOldProp(oldPropVals, newPropVals, newValuesNotInOldProp);

        if (newValuesNotInOldProp.Count != 0)
        {
            if (oldPropVals.Count != 0)
            {
                oldPropVals.AddRange(newValuesNotInOldProp);

                // Copy oldPropVals because it comes from NameBuilder and can be reused!
                _dic[oldPropKey] = oldPropVals.ToArray();
            }
            else
            {
                _dic[oldPropKey] = newValuesNotInOldProp.ToArray();
            }
        }

        static void GetNewValuesNotInOldProp(IReadOnlyList<string> oldPropVals, IReadOnlyList<string> newPropVals, List<string> newValuesNotInOldProp)
        {
            newValuesNotInOldProp.Clear();

            foreach (string newVal in newPropVals)
            {
                foreach (string oldVal in oldPropVals)
                {
                    if (oldVal.Contains(newVal, StringComparison.CurrentCultureIgnoreCase))
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
            string[]? coll = span.Contains(',')
                ? StringArrayConverter.AsNonEmptyStringArray(ToArray(in mem, version))
                : StringArrayConverter.AsNonEmptyStringArray(span.UnMaskValue(version));

            if (coll is null)
            {
                continue;
            }

            _dic[(NameProp)index] = coll;
        }//foreach

        Surnames = GetSurnamesView();
        Suffixes = GetSuffixesView();

        /////////////////////////////////////////

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string[] ToArray(in ReadOnlyMemory<char> mem, VCdVersion version)
            => PropertyValueSplitter.Split(mem,
                                    ',',
                                    StringSplitOptions.RemoveEmptyEntries,
                                    unMask: true,
                                    version).ToArray();
    }

    /// <summary>Surname(s) (also known as "family name(s)"). (2,3,4)</summary>
    /// <remarks>
    /// Returns a collection that omits any value if an equal value is set to
    /// <see cref="Surnames2"/> accordingly.
    /// </remarks>
    public IReadOnlyList<string> Surnames { get; }

    /// <summary>Given Name(s) (first name(s)). (2,3,4)</summary>
    public IReadOnlyList<string> Given => Get(NameProp.Given);

    /// <summary>Additional Name(s) (middle name(s)). (2,3,4)</summary>
    public IReadOnlyList<string> Given2 => Get(NameProp.Given2);

    /// <summary>Honorific Prefix(es). (2,3,4)</summary>
    public IReadOnlyList<string> Prefixes => Get(NameProp.Prefixes);

    /// <summary>Honorific Suffix(es). (2,3,4)</summary>
    /// 
    /// <remarks>
    /// Returns a collection that omits any value if an equal value is set to
    /// <see cref="Generations"/> accordingly.
    /// </remarks>
    public IReadOnlyList<string> Suffixes { get; }

    /// <summary>Secondary surnames (used in some cultures), also known as "maternal surnames". (4 - RFC 9554)</summary>
    public IReadOnlyList<string> Surnames2 => Get(NameProp.Surnames2);

    /// <summary>Generation markers or qualifiers, e.g., "Jr." or "III". (4 - RFC 9554)</summary>
    public IReadOnlyList<string> Generations => Get(NameProp.Generations);

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
                NameProp.Surnames => Surnames,
                NameProp.Suffixes => Suffixes,
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

    /// <inheritdoc/>
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
        foreach (KeyValuePair<NameProp, string[]> kvp in _dic)
        {
            if (kvp.Key <= NameProp.Suffixes
                && kvp.Value.ContainsAnyThatNeedsQpEncoding())
            {
                return true;
            }
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IReadOnlyList<string> GetSurnamesView() => GetDiffView(NameProp.Surnames, NameProp.Surnames2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IReadOnlyList<string> GetSuffixesView() => GetDiffView(NameProp.Suffixes, NameProp.Generations);

    private IReadOnlyList<string> GetDiffView(NameProp oldIdx, NameProp newIdx)
    {
        if (!_dic.TryGetValue(oldIdx, out string[]? oldProps))
        {
            return [];
        }

        if (!_dic.TryGetValue(newIdx, out string[]? newProps))
        {
            return oldProps;
        }

        List<string>? list = null;

        foreach (string old in oldProps.AsSpan())
        {
            if (!newProps.Contains(old, StringComparer.CurrentCultureIgnoreCase))
            {
                list ??= [];
                list.Add(old);
            }
        }

        return list is null ? []
                            : list.Count == oldProps.Length
                                 ? oldProps
                                 : list;
    }
}


