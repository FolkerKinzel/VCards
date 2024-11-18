using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Represents the vCard&#160;3.0 property <c>PROFILE</c>, which specifies that
/// the vCard is a vCard.</summary>
/// <seealso cref="VCard.Profile"/>
public sealed class ProfileProperty : VCardProperty
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
        : base(new ParameterSection(), group) { }

    internal ProfileProperty(VcfRow row) : base(row.Parameters, row.Group) { }

    /// <summary> The data provided by the <see cref="ProfileProperty" />. </summary>
    [SuppressMessage("Performance", "CA1822:Mark members as static",
        Justification = "Architectural design decision")]
    public new string Value => PROFILE_PROPERTY_VALUE;

    /// <inheritdoc />
    public override bool IsEmpty => false;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;

    /// <inheritdoc />
    public override object Clone() => new ProfileProperty(this);

    internal override void AppendValue(VcfSerializer serializer)
        => _ = serializer.Builder.Append(PROFILE_PROPERTY_VALUE);
}
