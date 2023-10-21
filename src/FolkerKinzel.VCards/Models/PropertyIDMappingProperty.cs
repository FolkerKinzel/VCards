using System.Collections;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates information that is used to identify vCard properties
/// across different versions of the same vCard.</summary>
/// <seealso cref="PropertyIDMapping"/>
/// <seealso cref="VCard.PropertyIDMappings"/>
public sealed class PropertyIDMappingProperty : VCardProperty, IEnumerable<PropertyIDMappingProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="PropertyIDMappingProperty"/> instance
    /// to clone.</param>
    private PropertyIDMappingProperty(PropertyIDMappingProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>  Initializes a new <see cref="PropertyIDMappingProperty" /> object. 
    /// </summary>
    /// <param name="value">A <see cref="PropertyIDMapping" /> object or <c>null</c>.</param>
    public PropertyIDMappingProperty(PropertyIDMapping? value)
        : base(new ParameterSection(), null)
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

    /// <summary> The data provided by the <see cref="PropertyIDMappingProperty" />. </summary>
    public new PropertyIDMapping? Value
    {
        get;
    }

    /// <inheritdoc />
    public override object Clone() => new PropertyIDMappingProperty(this);

    /// <inheritdoc />
    IEnumerator<PropertyIDMappingProperty> IEnumerable<PropertyIDMappingProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable<PropertyIDMappingProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;


    internal override void AppendValue(VcfSerializer serializer) => Value?.AppendTo(serializer.Builder);
}
