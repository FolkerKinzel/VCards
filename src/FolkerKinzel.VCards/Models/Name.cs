using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Enums;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Models;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates information about the name of the person the 
/// <see cref="VCard"/> represents.</summary>
/// <remarks>
/// <note type="tip">Use <see cref="NameBuilder"/> to create an instance.</note>
/// </remarks>
/// <seealso cref="NameBuilder"/>
/// <seealso cref="NameProperty"/>
/// <seealso cref="VCard.NameViews"/>
public sealed class Name : ICompoundModel, IEquatable<Name>
{
    private const int STANDARD_COUNT = 5;
    internal const int MAX_COUNT = 7;
    private readonly Dictionary<NameProp, string[]> _dic = [];

    private string[] Get(NameProp prop)
        => _dic.TryGetValue(prop, out string[]? coll) ? coll : [];

    private Name()
    {
        Surnames = Suffixes = [];
        IsEmpty = true;
    }

    /// <summary>
    /// Initializes a new <see cref="Name"/> instance with the content of a 
    /// specified <see cref="NameBuilder"/>.
    /// </summary>
    /// 
    /// <param name="builder">The <see cref="NameBuilder"/> whose content is used.</param>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <c>null</c>.</exception>
    internal Name(NameBuilder builder)
    {
        string[] surnames, surnames2, suffixes, generation;
        surnames = surnames2 = suffixes = generation = [];

        foreach (KeyValuePair<NameProp, List<string>> kvp in builder.Data)
        {
            if (kvp.Value.Count != 0)
            {
                // Copy kvp.Value because it comes from NameBuilder and can be reused!
                string[] val = [.. kvp.Value];
                _dic[kvp.Key] = val;

                switch (kvp.Key)
                {
                    case NameProp.Surnames:
                        surnames = val;
                        break;
                    case NameProp.Surnames2:
                        surnames2 = val;
                        break;
                    case NameProp.Suffixes:
                        suffixes = val;
                        break;
                    case NameProp.Generations:
                        generation = val;
                        break;
                    default:
                        break;
                }
            }
        }

        Surnames = GetSurnamesView();
        Suffixes = GetSuffixesView();

        if (surnames2.Length != 0)
        {
            AddToOldValuesForCompatibility(NameProp.Surnames, surnames, surnames2, builder.Worker);
        }

        if (generation.Length != 0)
        {
            AddToOldValuesForCompatibility(NameProp.Suffixes, suffixes, generation, builder.Worker);
        }

        IsEmpty = _dic.Count == 0;
    }

    [SuppressMessage("Style", "IDE0305:Simplify collection initialization",
        Justification = "Performance: Collection initializer allocate a new List<string>")]
    private void AddToOldValuesForCompatibility(NameProp oldPropKey, string[] oldPropVals, string[] newPropVals, List<string> list)
    {
        Debug.Assert(newPropVals.Length != 0);

        if (oldPropVals.Length == 0)
        {
            _dic[oldPropKey] = newPropVals;
            return;
        }

        list.Clear();
        list.AddRange(oldPropVals);

        AddNewValuesNotInOldProp(oldPropVals, newPropVals, list);

        if (list.Count != oldPropVals.Length)
        {
            _dic[oldPropKey] = list.ToArray();
        }

        static void AddNewValuesNotInOldProp(ReadOnlySpan<string> oldPropVals, ReadOnlySpan<string> newPropVals, List<string> list)
        {
            foreach (string newVal in newPropVals)
            {
                foreach (string oldPropVal in oldPropVals)
                {
                    if (oldPropVal.Contains(newVal, StringComparison.CurrentCultureIgnoreCase))
                    {
                        goto repeat;
                    }
                }

                list.Add(newVal);
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
            string[] coll = span.Contains(',')
                ? [.. Splitted(in mem, version)]
                : StringArrayConverter.ToStringArray(span.UnMaskValue(version));

            if (coll.ContainsData())
            {
                _dic[(NameProp)index] = coll;
            }
        }//foreach

        Surnames = GetSurnamesView();
        Suffixes = GetSuffixesView();

        IsEmpty = _dic.Count == 0;

        /////////////////////////////////////////

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static IEnumerable<string> Splitted(in ReadOnlyMemory<char> mem, VCdVersion version)
            => PropertyValueSplitter.Split(mem,
                                    ',',
                                    version: version);
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
    public bool IsEmpty { get; }

    internal static Name Empty => new(); // Not a singleton

    /// <inheritdoc/>
    int ICompoundModel.Count => MAX_COUNT;

    /// <inheritdoc/>
    IReadOnlyList<string> ICompoundModel.this[int index]
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
            && serializer.Options.HasFlag(VcfOpts.WriteRfc9554Extensions)
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
    private IReadOnlyList<string> GetSurnamesView() => GetOldValuesNotInNewValues(NameProp.Surnames, NameProp.Surnames2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IReadOnlyList<string> GetSuffixesView() => GetOldValuesNotInNewValues(NameProp.Suffixes, NameProp.Generations);

    private IReadOnlyList<string> GetOldValuesNotInNewValues(NameProp oldIdx, NameProp newIdx)
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

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] Name? other) => other is not null && CompoundModelHelper.Equals(this, other);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => Equals(obj as Name);

    /// <inheritdoc/>
    public override int GetHashCode() => CompoundModelHelper.GetHashCode(this);

    /// <summary>
    /// Overloads the equality operator for <see cref="Name"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="Name"/> object or <c>null</c>.</param>
    /// <param name="right">The right <see cref="Name"/> object or <c>null</c>.</param>
    /// <returns><c>true</c> if the contents of <paramref name="left"/> and 
    /// <paramref name="right"/> are equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(Name? left, Name? right)
        => ReferenceEquals(left, right) || (left?.Equals(right) ?? false);

    /// <summary>
    /// Overloads the not-equal-to operator for <see cref="Name"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="Name"/> object or <c>null</c>.</param>
    /// <param name="right">The right <see cref="Name"/> object or <c>null</c>.</param>
    /// <returns><c>true</c> if the contents of <paramref name="left"/> and 
    /// <paramref name="right"/> are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(Name? left, Name? right)
        => !(left == right);
}


