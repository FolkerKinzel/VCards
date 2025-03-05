using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Represents the vCard property <c>N</c>, which stores the name of the
/// vCard subject.</summary>
/// <seealso cref="VCard.NameViews"/>
/// <seealso cref="Name"/>
public sealed class NameProperty : VCardProperty, IEnumerable<NameProperty>, ICompoundProperty
{
    /// <summary>
    /// Initializes a new <see cref="NameProperty"/> instance with a 
    /// specified <see cref="Name"/>.
    /// </summary>
    /// <param name="value">The <see cref="Name"/> instance used as <see cref="Value"/>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
    public NameProperty(Name value, string? group = null)
        : base(new ParameterSection(), group)
    {
        _ArgumentNullException.ThrowIfNull(value, nameof(value));
        Value = value;
    }

    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="NameProperty"/> instance to clone.</param>
    private NameProperty(NameProperty prop) : base(prop)
        => Value = prop.Value;

    internal NameProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        ReadOnlyMemory<char> val = vcfRow.Value;

        if (Parameters.Encoding == Enc.QuotedPrintable)
        {
            val = QuotedPrintable.Decode(
                    val.Span,
                    TextEncodingConverter.GetEncoding(Parameters.CharSet)).AsMemory(); // null-check not needed
        }

        Value = val.Span.IsWhiteSpace()
            ? Name.Empty
            : new Name(in val, version);
    }

    /// <summary> The data provided by the <see cref="NameProperty" />.
    /// </summary>
    public new Name Value { get; }

    /// <inheritdoc />
    public override bool IsEmpty => Value.IsEmpty;

    /// <inheritdoc/>
    int ICompoundModel.Count => Name.MAX_COUNT;

    /// <inheritdoc/>
    IReadOnlyList<string> ICompoundModel.this[int index] => ((ICompoundModel)Value)[index];

    /// <inheritdoc />
    public override object Clone() => new NameProperty(this);

    /// <inheritdoc />
    IEnumerator<NameProperty> IEnumerable<NameProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable<NameProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object GetVCardPropertyValue() => Value;

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        base.PrepareForVcfSerialization(serializer);

        Debug.Assert(serializer is not null);
        Debug.Assert(Value is not null); // value ist nie null

        if (serializer.Version == VCdVersion.V2_1 && Value.NeedsToBeQpEncoded())
        {
            Parameters.Encoding = Enc.QuotedPrintable;
            Parameters.CharSet = VCard.DEFAULT_CHARSET;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal override void AppendValue(VcfSerializer serializer)
        => Value.AppendVcfString(serializer);
}
