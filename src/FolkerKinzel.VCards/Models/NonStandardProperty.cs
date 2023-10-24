using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;
using System.Collections;
using System.Numerics;

namespace FolkerKinzel.VCards.Models;

/// <summary>Represents a vCard property that is not defined by the official 
/// standards.</summary>
/// <remarks>
/// <note type="important">
/// <para>
/// To write <see cref="NonStandardProperty" /> objects into a vCard, the flag 
/// <see cref="VcfOptions.WriteNonStandardProperties" /> must be set. 
/// </para>
/// <para>
/// Please note that when using the class, yourself is responsible for the 
/// standard-compliant masking, unmasking, encoding and decoding of the data.
/// </para>
/// </note>
/// </remarks>
/// <seealso cref="VCard.NonStandard"/>
public sealed class NonStandardProperty : VCardProperty, IEnumerable<NonStandardProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="NonStandardProperty"/> instance to clone.</param>
    private NonStandardProperty(NonStandardProperty prop) : base(prop)
    {
        PropertyKey = prop.PropertyKey;
        Value = prop.Value;
    }

    /// <summary>Initializes a new <see cref="NonStandardProperty" /> object.</summary>
    /// <param name="propertyKey">The key ("name") of the non-standard vCard property
    /// (format: <c>X-NAME</c>).</param>
    /// <param name="value">The value of the vCard property: any data encoded as <see
    /// cref="string" /> or <c>null</c>.</param>
    /// <param name="propertyGroup">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"> <paramref name="propertyKey" /> is
    /// <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="propertyKey" /> is not
    /// a valid X-NAME.</exception>
    public NonStandardProperty(string propertyKey, string? value, string? propertyGroup = null)
        : base(new ParameterSection(), propertyGroup)
    {
        if (propertyKey == null)
        {
            throw new ArgumentNullException(nameof(propertyKey));
        }

        if (propertyKey.Length < 3 ||
            !propertyKey.StartsWith("X-", StringComparison.OrdinalIgnoreCase) ||
             propertyKey.Contains(' ', StringComparison.Ordinal))
        {
            throw new ArgumentException(
                Res.NoXName, nameof(propertyKey));
        }

        PropertyKey = propertyKey;
        Value = value;
    }


    internal NonStandardProperty(VcfRow vcfRow)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        PropertyKey = vcfRow.Key;
        Value = string.IsNullOrEmpty(vcfRow.Value) ? null : vcfRow.Value;
    }

    /// <summary>The key ("name") of the vCard property.</summary>
    public string PropertyKey { get; }

    /// <summary>The data provided by the <see cref="NonStandardProperty" /> (raw <see
    /// cref="string" /> data).</summary>
    public new string? Value
    {
        get;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder();

        _ = sb.Append("Key:   ").AppendLine(PropertyKey);
        _ = sb.Append("Value: ").Append(Value);

        return sb.ToString();
    }

    /// <inheritdoc />
    public override object Clone() => new NonStandardProperty(this);

    /// <inheritdoc />
    IEnumerator<NonStandardProperty> IEnumerable<NonStandardProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable<NonStandardProperty>)this).GetEnumerator();


    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        // MUST not call the base class implementation!
    }


    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        _ = serializer.Builder.Append(Value);
    }
}
