using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
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
/// <seealso cref="VCard.NonStandards"/>
public sealed class NonStandardProperty : VCardProperty, IEnumerable<NonStandardProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="NonStandardProperty"/> instance to clone.</param>
    private NonStandardProperty(NonStandardProperty prop) : base(prop)
    {
        XName = prop.XName;
        Value = prop.Value;
    }

    /// <summary>Initializes a new <see cref="NonStandardProperty" /> object.</summary>
    /// <param name="xName">The key ("name") of the non-standard vCard property
    /// (format: <c>X-NAME</c>).</param>
    /// <param name="value">The value of the vCard property: any data encoded as <see
    /// cref="string" /> or <c>null</c>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"> <paramref name="xName" /> is
    /// <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="xName" /> is not
    /// a valid X-NAME.</exception>
    public NonStandardProperty(string xName, string? value, string? group = null)
        : base(new ParameterSection(), group)
    {
        _ArgumentNullException.ThrowIfNull(xName, nameof(xName));

        if (xName.Length < 3 ||
            !xName.StartsWith("X-", StringComparison.OrdinalIgnoreCase) ||
             xName.Contains(' ', StringComparison.Ordinal))
        {
            throw new ArgumentException(
                Res.NoXName, nameof(xName));
        }

        XName = xName;
        Value = value;
    }

    internal NonStandardProperty(VcfRow vcfRow)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        XName = vcfRow.Key;
        Value = string.IsNullOrEmpty(vcfRow.Value) ? null : vcfRow.Value;
    }

    /// <summary>The key ("name") of the vCard property.</summary>
    public string XName { get; }

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

        _ = sb.Append($"{nameof(XName)}: ").AppendLine(XName);
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
