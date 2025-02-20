using System.Collections;
using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Formatters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Encapsulates the data of the vCard property <c>ADR</c> that contains 
/// information about the postal address.</summary>
/// <remarks>See <see cref="VCard.Addresses"/>.</remarks>
/// <seealso cref="VCard.Addresses"/>
public sealed class AddressProperty : VCardProperty, IEnumerable<AddressProperty>, ICompoundProperty
{
    /// <summary>
    /// Initializes a new <see cref="AddressProperty"/> instance with a 
    /// specified <see cref="Address"/>.
    /// </summary>
    /// 
    /// <param name="value">The <see cref="Address"/> instance used as <see cref="Value"/>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
    public AddressProperty(Address value, string? group = null)
        : base(new ParameterSection(), group)
    {
        _ArgumentNullException.ThrowIfNull(value, nameof(value));
        Value = value;
    }

    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="AddressProperty" /> object to clone.</param>
    private AddressProperty(AddressProperty prop) : base(prop) => Value = prop.Value;

    internal AddressProperty(VcfRow vcfRow, VCdVersion version)
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
            ? Address.Empty
            : new Address(in val, version);
    }

    /// <summary> The data provided by the <see cref="AddressProperty" />.</summary>
    public new Address Value { get; }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <remarks>
    /// <note type="important">
    /// If only the <see cref="ParameterSection.Label" /> parameter or the <see cref="ParameterSection.GeoPosition"/> or
    /// the <see cref="ParameterSection.TimeZone"/> is not equal
    /// to <c>null</c>, <see cref="IsEmpty"/> returns <c>false</c>.
    /// </note>
    /// </remarks>
    public override bool IsEmpty => Value.IsEmpty
                                 && Parameters.Label is null
                                 && Parameters.GeoPosition is null
                                 && Parameters.TimeZone is null;

    /// <inheritdoc />
    public override object Clone() => new AddressProperty(this);

    /// <inheritdoc/>
    IEnumerator<AddressProperty> IEnumerable<AddressProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable<AddressProperty>)this).GetEnumerator();

    /// <inheritdoc/>
    int ICompoundProperty.Count
        => ((IReadOnlyList<IReadOnlyList<string>>)Value).Count;

    /// <inheritdoc/>
    IReadOnlyList<string> ICompoundProperty.this[int index]
        => ((IReadOnlyList<IReadOnlyList<string>>)Value)[index];

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
