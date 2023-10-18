using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;
using System.Collections;

namespace FolkerKinzel.VCards.Models;

    /// <summary>Represents a vCard property that is not defined by the official standards.</summary>
    /// <remarks>
    /// <note type="important">
    /// <para>
    /// Um Objekte der Klasse <see cref="NonStandardProperty" /> in eine vCard zu schreiben,
    /// muss beim Schreibvorgang das Flag <see cref="VcfOptions.WriteNonStandardProperties">VcfOptions.WriteNonStandardProperties</see>
    /// explizit gesetzt werden.
    /// </para>
    /// <para>
    /// Bedenken Sie, dass Sie sich bei Verwendung der Klasse um die standardgerechte
    /// Maskierung, Demaskierung, Enkodierung und Dekodierung der Daten selbst kümmern
    /// müssen.
    /// </para>
    /// </note>
    /// </remarks>
public sealed class NonStandardProperty : VCardProperty, IEnumerable<NonStandardProperty>
{
    /// <summary />
    /// <param name="prop" />
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
        Value = vcfRow.Value;
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


    /// <summary>Creates a <see cref="string" /> representation of the <see cref="NonStandardProperty"
    /// /> object. (For debugging only.)</summary>
    /// <returns>A <see cref="string" /> representation of the <see cref="NonStandardProperty"
    /// /> object.</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();

        _ = sb.Append("Key:   ").AppendLine(PropertyKey);
        _ = sb.Append("Value: ").Append(Value);

        return sb.ToString();
    }

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        // MUST not call the base class implementation!
    }


    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        _ = serializer.Builder.Append(Value);
    }

    IEnumerator<NonStandardProperty> IEnumerable<NonStandardProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<NonStandardProperty>)this).GetEnumerator();

    /// <inheritdoc />
    public override object Clone() => new NonStandardProperty(this);
}
