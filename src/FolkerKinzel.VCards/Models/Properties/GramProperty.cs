using System.Collections;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Represents the <c>GRAMGENDER</c> property, introduced in RFC&#160;9554, which
/// defines which grammatical gender to use in salutations and other grammatical constructs.</summary>
/// <seealso cref="VCard.GramGenders"/>
/// <seealso cref="Gram"/>
public sealed class GramProperty : VCardProperty, IEnumerable<GramProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="GramProperty"/> instance to clone.</param>
    private GramProperty(GramProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>  Initializes a new <see cref="GramProperty" /> object. </summary>
    /// <param name="value">A member of the <see cref="Gram" /> enum.</param>
    /// <param name="group">Identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group.</param>
    public GramProperty(Gram value, string? group = null)
        : base(new ParameterSection(), group) => Value = value;

    private GramProperty(Gram value, VcfRow vcfRow)
        : base(vcfRow.Parameters, vcfRow.Group)
        => Value = value;

    internal static bool TryParse(VcfRow vcfRow, [NotNullWhen(true)] out GramProperty? prop)
    {
        if (GramConverter.TryParse(vcfRow.Value.Span, out Gram gram))
        {
            prop = new GramProperty(gram, vcfRow);
            return true;
        }

        prop = null;
        return false;
    }

    /// <summary> The data provided by the <see cref="GramProperty" />.
    /// </summary>
    public new Gram Value { get; }

    /// <inheritdoc />
    public override bool IsEmpty => false;

    /// <inheritdoc />
    public override object Clone() => new GramProperty(this);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object GetVCardPropertyValue() => Value;

    internal override void AppendValue(VcfSerializer serializer)
    {
        Debug.Assert(serializer is not null);
        _ = serializer.Builder.Append(Value.ToVcfString());
    }

    /// <inheritdoc />
    IEnumerator<GramProperty> IEnumerable<GramProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<GramProperty>)this).GetEnumerator();
}
