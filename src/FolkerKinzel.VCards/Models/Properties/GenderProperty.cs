using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Represents the vCard property <c>GENDER</c>, introduced in vCard&#160;4.0,
/// which stores information to specify the components of gender and gender identity
/// of the object the <see cref="VCard"/> represents.
/// </summary>
/// <seealso cref="Gender"/>
/// <seealso cref="VCard.GenderViews"/>
public sealed class GenderProperty : VCardProperty, IEnumerable<GenderProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="GenderProperty"/> instance
    /// to clone.</param>
    private GenderProperty(GenderProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary> Initializes a new <see cref="GenderProperty" /> object with a specified
    /// <see cref="Gender"/> instance. </summary>
    /// <param name="value">The <see cref="Gender"/> instance to use as <see cref="Value"/>.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    /// 
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
    public GenderProperty(Gender value, string? group = null) : base(new ParameterSection(), group)
        => Value = value ?? throw new ArgumentNullException(nameof(value));

    internal GenderProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters, vcfRow.Group)
    {
        Sex? sex = null;
        string? genderIdentity = null;

        ReadOnlySpan<char> span = vcfRow.Value.Span;

        if (!span.IsEmpty)
        {
            sex = SexConverter.Parse(span[0]);

            if (span.Length > 2)
            {
                genderIdentity = span.Slice(2).UnMaskValue(version);
            }
        }

        Value = Gender.Create(sex, genderIdentity);
    }

    /// <summary>The data provided by the <see cref="GenderProperty" />. </summary>
    public new Gender Value { get; }

    /// <inheritdoc />
    public override bool IsEmpty => Value.IsEmpty;

    /// <inheritdoc />
    public override object Clone() => new GenderProperty(this);

    /// <inheritdoc />
    IEnumerator<GenderProperty> IEnumerable<GenderProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<GenderProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object GetVCardPropertyValue() => Value;

    internal override void AppendValue(VcfSerializer serializer)
    {
        if (Value.Sex.HasValue)
        {
            _ = serializer.Builder.Append(Value.Sex.ToVcfString());
        }

        if (Value.Identity is not null)
        {
            _ = serializer.Builder.Append(';').AppendValueMasked(Value.Identity, serializer.Version);
        }
    }
}
