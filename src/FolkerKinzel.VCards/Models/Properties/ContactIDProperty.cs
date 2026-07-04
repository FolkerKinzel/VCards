using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Represents the vCard property <c>UID</c>, which stores a unique identifier
/// for the vCard subject.</summary>
/// <remarks>
/// <para>
/// If the <see cref="OriginalString"/> is not <c>null</c>, it is copied unchanged into the vCard being written.
/// This behavior can be undesirable when converting from one vCard version to another.
/// </para>
/// <para>
/// In this case, assign a new <see cref="ContactIDProperty"/> instance to the <see cref="VCard"/> being converted — 
/// initialized with the <see cref="Value"/> of the current one — in order to adapt the format (e.g., of <see cref="Guid"/>s)
/// to the new vCard version.
/// </para>
/// </remarks>
/// <seealso cref="VCard.ContactID"/>
/// <seealso cref="ContactID"/>
/// <seealso cref="RelationProperty"/>
public sealed class ContactIDProperty : VCardProperty
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="ContactIDProperty"/> instance
    /// to clone.</param>
    private ContactIDProperty(ContactIDProperty prop) : base(prop)
    {
        Value = prop.Value;
        OriginalString = prop.OriginalString;
    }

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

    /// <summary>
    /// Gets the original string found in the vCard file, or <c>null</c> if the <see cref="ContactIDProperty"/>
    /// was created programmatically.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the <see cref="OriginalString"/> is not <c>null</c>, it is copied unchanged into the vCard being written.
    /// This behavior can be undesirable when converting from one vCard version to another.
    /// </para>
    /// <para>
    /// In this case, assign a new <see cref="ContactIDProperty"/> instance to the <see cref="VCard"/> being converted — 
    /// initialized with the <see cref="Value"/> of the current one — in order to adapt the format (e.g., of <see cref="Guid"/>s)
    /// to the new vCard version.
    /// </para>
    /// </remarks>
    public string? OriginalString { get; }

    internal ContactIDProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        OriginalString = StringDeserializer.Deserialize(vcfRow, version);

        if (Parameters.DataType == Data.Text)
        {
            Value = string.IsNullOrWhiteSpace(OriginalString)
                        ? ContactID.Empty
                        : ContactID.Create(OriginalString);
            return;
        }

        if (UuidConverter.TryAsGuid(vcfRow.Value.Span, out Guid uuid))
        {
            Value = ContactID.Create(uuid);
        }

        string uriString = OriginalString.Trim();

        if (Uri.TryCreate(uriString, UriKind.Absolute, out Uri? uri))
        {
            Value = ContactID.Create(uri);
        }

        Value = uriString.Length == 0
                    ? ContactID.Empty
                    : ContactID.Create(uriString); // leading and trailing whitespace is not used for comparison.
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

        if (OriginalString is null)
        {
            if (Value.String is string str)
            {
                Parameters.DataType = Data.Text;
                StringSerializer.Prepare(str, this, serializer.Version);
            }
        }
        else
        {
            if (serializer.Version < VCdVersion.V4_0)
            {
                StringSerializer.Prepare(OriginalString, this, serializer.Version);
            }
            else if (Value.String != null || (Value.Guid.HasValue && !UuidConverter.IsUuidUri(OriginalString)))
            {
                Parameters.DataType = Data.Text;
            }
        }
    }

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);

        if (OriginalString is null)
        {
            if (Value.Guid.HasValue)
            {
                _ = serializer.Builder.AppendUuid(Value.Guid.Value, serializer.Version);
            }
            else if (Value.Uri is Uri uri)
            {
                // URIs are not masked according to the "Verifier notes" in
                // https://www.rfc-editor.org/errata/eid3845
                // It says that "the ABNF does not support escaping for URIs."
                _ = serializer.Builder.Append(uri.AbsoluteUri);
            }
            else
            {
                StringSerializer.AppendVcf(serializer.Builder, Value.String, Parameters, serializer.Version);
            }
        }
        else
        {
            if (Value.String is null)
            {
                // Valid Guids need no masking.
                // URIs are not masked according to the "Verifier notes" in
                // https://www.rfc-editor.org/errata/eid3845
                // It says that "the ABNF does not support escaping for URIs."
                _ = serializer.Builder.Append(OriginalString);
            }
            else
            {
                StringSerializer.AppendVcf(serializer.Builder, OriginalString, Parameters, serializer.Version);
            }
        }
    }
}
