using System.Collections;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

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

    internal TextProperty(VcfRow vcfRow, VCdVersion version) 
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        vcfRow.UnMask(version);
        Value = vcfRow.Value.Length == 0 ? null : vcfRow.Value;
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
        Debug.Assert(serializer != null);

        base.PrepareForVcfSerialization(serializer);

        if (serializer.Version == VCdVersion.V2_1 && Value.NeedsToBeQpEncoded())
        {
            this.Parameters.Encoding = ValueEncoding.QuotedPrintable;
            this.Parameters.CharSet = VCard.DEFAULT_CHARSET;
        }
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        StringBuilder builder = serializer.Builder;


        if (serializer.Version == VCdVersion.V2_1)
        {
            _ = this.Parameters.Encoding == ValueEncoding.QuotedPrintable
                ? builder.Append(QuotedPrintable.Encode(Value, builder.Length))
                : builder.Append(Value);
        }
        else
        {
            StringBuilder worker = serializer.Worker;

            _ = worker.Clear().Append(Value).Mask(serializer.Version);
            _ = builder.Append(worker);
        }
    }
}
