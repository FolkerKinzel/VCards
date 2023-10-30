using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;

namespace FolkerKinzel.VCards.Models;

/// <summary>Represents the vCard&#160;3.0 property <c>PROFILE</c>, which specifies that
/// the vCard is a vCard.</summary>
/// <seealso cref="VCard.Profile"/>
public sealed class ProfileProperty : TextProperty
{
    private const string PROFILE_PROPERTY_VALUE = "VCARD";

    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="ProfileProperty"/> instance to clone.</param>
    private ProfileProperty(ProfileProperty prop) : base(prop) { }

    /// <summary>  Initializes a new <see cref="ProfileProperty" /> object. </summary>
    public ProfileProperty() 
        : base(PROFILE_PROPERTY_VALUE, null) { }

    internal ProfileProperty(VcfRow row, VCdVersion version) : base(row, version) { }

    /// <summary> The data provided by the <see cref="ProfileProperty" />. </summary>
    public override string Value => base.Value ?? PROFILE_PROPERTY_VALUE;

    /// <inheritdoc />
    public override object Clone() => new ProfileProperty(this);

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        this.Parameters.Encoding = null;
        this.Parameters.CharSet = null;
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);

        _ = serializer.Builder.Append(PROFILE_PROPERTY_VALUE);
    }
}
