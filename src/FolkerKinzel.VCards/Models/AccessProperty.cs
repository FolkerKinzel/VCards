using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Kapselt die Daten der vCard-Property <c>CLASS</c>, die in vCard 3.0 die Geheimhaltungsstufe der 
/// vCard definiert.
/// </summary>
public sealed class AccessProperty : VCardProperty
{
    /// <summary>
    /// Copy ctor
    /// </summary>
    /// <param name="prop">The <see cref="AccessProperty"/> object to clone.</param>
    private AccessProperty(AccessProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>
    /// Initialisiert ein neues <see cref="AccessProperty"/>-Objekt.
    /// </summary>
    /// <param name="value">Ein Member der <see cref="Access"/>-Enumeration.</param>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    public AccessProperty(Access value, string? propertyGroup = null) : base(new ParameterSection(), propertyGroup)
        => Value = value;

    internal AccessProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group) => Value = VCdAccessConverter.Parse(vcfRow.Value);


    [InternalProtected]
    internal override void AppendValue(VcfSerializer serializer)
    {
        InternalProtectedAttribute.Run();
        Debug.Assert(serializer != null);

        _ = serializer.Builder.Append(Value.ToVCardString());
    }


    /// <inheritdoc/>
    public override bool IsEmpty => false;

    /// <summary>
    /// Die von der <see cref="AccessProperty"/> zur Verfügung gestellten Daten.
    /// </summary>
    public new Access Value
    {
        get;
    }


    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object GetVCardPropertyValue() => Value;


    /// <inheritdoc/>
    public override object Clone() => new AccessProperty(this);
}
