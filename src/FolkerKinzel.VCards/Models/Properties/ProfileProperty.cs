using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;

namespace FolkerKinzel.VCards.Models.Properties;

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
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public ProfileProperty(string? group = null)
        : base(PROFILE_PROPERTY_VALUE, group) { }

    internal ProfileProperty(VcfRow row, VCdVersion version) : base(row, version) { }

    /// <summary> The data provided by the <see cref="ProfileProperty" />. </summary>
    public override string Value => base.Value ?? PROFILE_PROPERTY_VALUE;

    /// <inheritdoc />
    public override object Clone() => new ProfileProperty(this);

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);

        Parameters.Encoding = null;
        Parameters.CharSet = null;
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);

        _ = serializer.Builder.Append(PROFILE_PROPERTY_VALUE);
    }
}
