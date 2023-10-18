using System.Collections;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

    /// <summary>Encapsulates information that is used to identify vCard properties
    /// across different versions of the same vCard.</summary>
public sealed class PropertyIDMappingProperty : VCardProperty, IEnumerable<PropertyIDMappingProperty>
{
    /// <summary />
    /// <param name="prop" />
    private PropertyIDMappingProperty(PropertyIDMappingProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary> Initialisiert ein neues <see cref="PropertyIDMappingProperty" />-Objekt.
    /// </summary>
    /// <param name="value">A <see cref="PropertyIDMapping" /> object or <c>null</c>.</param>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="value" /> is
    /// less than 1 or greater than 9.</exception>
    public PropertyIDMappingProperty(PropertyIDMapping? value) : base(new ParameterSection(), null)
        => Value = value;


    /// <summary>ctor</summary>
    /// <param name="vcfRow" />
    /// <exception cref="ArgumentException" />
    internal PropertyIDMappingProperty(VcfRow vcfRow)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        if (string.IsNullOrWhiteSpace(vcfRow.Value))
        {
            return;
        }

        Value = PropertyIDMapping.Parse(vcfRow.Value);
    }


    /// <summary> Die von der <see cref="PropertyIDMappingProperty" /> zur Verfügung
    /// gestellten Daten. </summary>
    public new PropertyIDMapping? Value
    {
        get;
    }


    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    internal override void AppendValue(VcfSerializer serializer) => Value?.AppendTo(serializer.Builder);

    IEnumerator<PropertyIDMappingProperty> IEnumerable<PropertyIDMappingProperty>.GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<PropertyIDMappingProperty>)this).GetEnumerator();

    /// <inheritdoc />
    public override object Clone() => new PropertyIDMappingProperty(this);
}
