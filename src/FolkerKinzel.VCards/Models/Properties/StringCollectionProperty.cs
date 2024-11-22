using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Represents vCard properties that store a collection of <see cref="string" />s .</summary>
/// <seealso cref="VCard.NickNames"/>
/// <seealso cref="VCard.Categories"/>
public sealed class StringCollectionProperty : VCardProperty, IEnumerable<StringCollectionProperty>
{
    private readonly string[] _value;

    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="StringCollectionProperty"/>
    /// instance to clone</param>
    private StringCollectionProperty(StringCollectionProperty prop) : base(prop)
        => _value = prop._value;

    /// <summary>Initializes a new <see cref="StringCollectionProperty" /> object.</summary>
    /// <param name="value">A collection of <see cref="string" />s or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
    public StringCollectionProperty(IEnumerable<string?> value, string? group = null)
        : base(new ParameterSection(), group)
    {
        _value = StringArrayConverter.ToStringArray(value ?? throw new ArgumentNullException(nameof(value)));
        IsEmpty = !_value.ContainsData();
    }

    /// <summary>Initializes a new <see cref="StringCollectionProperty" /> object.</summary>
    /// <param name="value">A <see cref="string" />, or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public StringCollectionProperty(string? value, string? group = null)
        : base(new ParameterSection(), group)
    {
        _value = StringArrayConverter.ToStringArray(value);
        IsEmpty = !string.IsNullOrWhiteSpace(value);
    }

    internal StringCollectionProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        // StringCollectionProperty is used for NickNames and categories. Neither NickNames nor
        // Categories are known in VCard 2.1. That's why Quoted-Printable encoding can't occur.

        ReadOnlySpan<char> span = vcfRow.Value.Span;
        _value = span.Contains(',')
            ? Splitted(vcfRow, version).ToArray()
            : StringArrayConverter.ToStringArray(span.UnMaskValue(version));

        IsEmpty = !_value.ContainsData();

        /////////////////////////////////////////////////////////////////////////

        static IEnumerable<string> Splitted(VcfRow vcfRow, VCdVersion version)
            => PropertyValueSplitter.Split(vcfRow.Value,
                                           ',',
                                           version: version);
    }

    /// <summary>The data provided by the <see cref="StringCollectionProperty" />.</summary>
    public new IReadOnlyList<string> Value => _value;

    /// <inheritdoc />
    public override bool IsEmpty { get; }

    /// <inheritdoc />
    public override string ToString() => IsEmpty ? base.ToString() : string.Join(", ", _value);

    /// <inheritdoc />
    public override object Clone() => new StringCollectionProperty(this);

    /// <inheritdoc />
    IEnumerator<StringCollectionProperty> IEnumerable<StringCollectionProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable<StringCollectionProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object GetVCardPropertyValue() => Value;

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);

        if (IsEmpty)
        {
            return;
        }

        Debug.Assert(Value.Count != 0);

        StringBuilder builder = serializer.Builder;

        foreach (string s in _value.AsSpan())
        {
            Debug.Assert(!string.IsNullOrEmpty(s));
            _ = builder.AppendValueMasked(s, serializer.Version).Append(',');
        }

        --builder.Length;
    }
}
