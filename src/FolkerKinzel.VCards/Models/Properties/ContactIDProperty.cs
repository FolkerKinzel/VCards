using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Represents the vCard property <c>UID</c>, which stores a unique identifier
/// for the vCard subject.</summary>
/// <seealso cref="VCard.ContactID"/>
/// <seealso cref="ContactID"/>
/// <seealso cref="RelationProperty"/>
public sealed class ContactIDProperty : VCardProperty
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="ContactIDProperty"/> instance
    /// to clone.</param>
    private ContactIDProperty(ContactIDProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary> Initializes a new <see cref="ContactIDProperty" /> object with a 
    /// specified <see cref="ContactID"/>. </summary>
    /// <param name="value">A <see cref="Guid" /> value.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
    public ContactIDProperty(ContactID value, string? group = null)
        : base(new ParameterSection(), group)
        => Value = value ?? throw new ArgumentNullException(nameof(value));

    internal ContactIDProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        if (Parameters.DataType == Data.Text)
        {
            string? val = StringDeserializer.Deserialize(vcfRow, version);

            Value = string.IsNullOrWhiteSpace(val)
                        ? ContactID.Empty
                        : ContactID.Create(val);
            return;
        }

        if (UuidConverter.TryAsGuid(vcfRow.Value.Span, out Guid uuid))
        {
            Value = ContactID.Create(uuid);
        }

        string uriString = vcfRow.Value.Span.Trim().UnMaskValue(version);

        if (Uri.TryCreate(uriString, UriKind.Absolute, out Uri? uri))
        {
            Value = ContactID.Create(uri);
        }

        Value = string.IsNullOrWhiteSpace(uriString)
                    ? ContactID.Empty
                    : ContactID.Create(uriString);
    }

    /// <summary> The <see cref="ContactID"/> provided by the <see cref="ContactIDProperty" />.
    /// </summary>
    public new ContactID Value { get; }

    /// <inheritdoc />
    public override bool IsEmpty => Value.IsEmpty;

    /// <inheritdoc />
    public override object Clone() => new ContactIDProperty(this);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object GetVCardPropertyValue() => Value;

    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        base.PrepareForVcfSerialization(serializer);

        if (Value.String is string str)
        {
            Parameters.DataType = Data.Text;
            StringSerializer.Prepare(str, this, serializer.Version);
        }
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);
        StringBuilder builder = serializer.Builder;

        if (Value.Guid.HasValue)
        {
            _ = builder.AppendUuid(Value.Guid.Value, serializer.Version);
        }
        else if (Value.Uri is Uri uri)
        {
            // URIs are not masked according to the "Verifier notes" in
            // https://www.rfc-editor.org/errata/eid3845
            // It says that "the ABNF does not support escaping for URIs."
            _ = builder.Append(uri.AbsoluteUri);
        }
        else
        {
            StringSerializer.AppendVcf(builder, Value.String, Parameters, serializer.Version);
        }
    }
}
