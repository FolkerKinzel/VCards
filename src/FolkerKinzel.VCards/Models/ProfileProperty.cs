using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models;

/// <summary>
/// Repräsentiert die vCard-3.0-Property <c>PROFILE</c>, die festlegt, dass die vCard eine vCard ist.
/// </summary>
public sealed class ProfileProperty : TextProperty
{
    private const string PROFILE_PROPERTY_VALUE = "VCARD";

    /// <summary>
    /// Copy ctor.
    /// </summary>
    /// <param name="prop"></param>
    private ProfileProperty(ProfileProperty prop) : base(prop) { }

    /// <summary>
    /// Initialisiert ein neues <see cref="ProfileProperty"/>-Objekt.
    /// </summary>
    /// <param name="propertyGroup">Bezeichner der Gruppe,
    /// der die <see cref="VCardProperty"/> zugehören soll, oder <c>null</c>,
    /// um anzuzeigen, dass die <see cref="VCardProperty"/> keiner Gruppe angehört.</param>
    public ProfileProperty(string? propertyGroup = null) : base(PROFILE_PROPERTY_VALUE, propertyGroup) { }


    internal ProfileProperty(VcfRow row, VCdVersion version) : base(row, version) { }


    /// <summary>
    /// Die von der <see cref="ProfileProperty"/> zur Verfügung gestellten Daten.
    /// </summary>
    public override string Value => base.Value ?? PROFILE_PROPERTY_VALUE;


    [InternalProtected]
    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        InternalProtectedAttribute.Run();
        Debug.Assert(serializer != null);

        this.Parameters.Encoding = null;
        this.Parameters.Charset = null;
    }


    [InternalProtected]
    internal override void AppendValue(VcfSerializer serializer)
    {
        InternalProtectedAttribute.Run();
        Debug.Assert(serializer != null);

        _ = serializer.Builder.Append(PROFILE_PROPERTY_VALUE);
    }

    /// <inheritdoc/>
    public override object Clone() => new ProfileProperty(this);
}
