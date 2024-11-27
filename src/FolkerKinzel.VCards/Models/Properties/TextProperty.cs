using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Represents vCard properties whose content consists of text.</summary>
public sealed class TextProperty : VCardProperty, IEnumerable<TextProperty>
{
    /// <summary>Copy constructor.</summary>
    /// <param name="prop">The <see cref="TextProperty" /> instance to clone.</param>
    private TextProperty(TextProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>Initializes a new <see cref="TextProperty" /> object.</summary>
    /// <param name="value">A <see cref="string" /> or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public TextProperty(string? value, string? group = null)
        : base(new ParameterSection(), group)
    {
        Value = string.IsNullOrWhiteSpace(value) ? "" : value;
        IsEmpty = Value.Length == 0;
    }

    internal TextProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        Value = StringDeserializer.Deserialize(vcfRow, version);
        IsEmpty = string.IsNullOrWhiteSpace(Value);
    }

    /// <summary>The data provided by the <see cref="TextProperty" />.</summary>
    public new string Value { get;}

    /// <inheritdoc />
    public override bool IsEmpty { get; }

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
    protected override object GetVCardPropertyValue() => Value;

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);

        base.PrepareForVcfSerialization(serializer);
        StringSerializer.Prepare(Value, this, serializer.Version);
    }

    internal override void AppendValue(VcfSerializer serializer)
        => StringSerializer.AppendVcf(serializer.Builder, Value, Parameters, serializer.Version);
}
