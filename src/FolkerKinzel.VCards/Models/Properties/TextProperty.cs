using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Represents vCard properties whose content consists of text.</summary>
public class TextProperty : VCardProperty, IEnumerable<TextProperty>
{
    /// <summary>Copy constructor.</summary>
    /// <param name="prop">The <see cref="TextProperty" /> instance to clone.</param>
    protected TextProperty(TextProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>Initializes a new <see cref="TextProperty" /> object.</summary>
    /// <param name="value">A <see cref="string" /> or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public TextProperty(string? value, string? group = null)
        : base(new ParameterSection(), group)
        => Value = string.IsNullOrWhiteSpace(value) ? null : value;

    internal TextProperty(string value, VcfRow vcfRow)
        : base(vcfRow.Parameters, vcfRow.Group)
        => Value = value.Length == 0 ? null : value;

    internal TextProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        string val = vcfRow.Parameters.Encoding == Enc.QuotedPrintable
                ? vcfRow.Value.Span.UnMaskAndDecodeValue(vcfRow.Parameters.CharSet)
                : vcfRow.Value.Span.UnMaskValue(version);

        Value = val.Length == 0 ? null : val;
    }

    /// <summary>The data provided by the <see cref="TextProperty" />.</summary>
    public new virtual string? Value
    {
        get;
    }

    /// <inheritdoc />
    [MemberNotNullWhen(false, nameof(Value))]
    public override bool IsEmpty => base.IsEmpty;

    /// <inheritdoc />
    public override object Clone() => new TextProperty(this);

    /// <inheritdoc />
    IEnumerator<TextProperty> IEnumerable<TextProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TextProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);

        base.PrepareForVcfSerialization(serializer);
        StringSerializer.Prepare(Value, this, serializer.Version);
    }

    internal override void AppendValue(VcfSerializer serializer)
        => StringSerializer.AppendVcf(serializer.Builder, Value, Parameters, serializer.Version);
}
