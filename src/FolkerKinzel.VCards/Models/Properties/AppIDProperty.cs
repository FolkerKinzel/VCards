using System.Collections;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Models.Properties;

/// <summary>Encapsulates information that is used to identify a vCard client globally, 
/// and locally inside the <see cref="VCard"/></summary>
/// <seealso cref="AppID"/>
/// <seealso cref="VCard.AppIDs"/>
public sealed class AppIDProperty : VCardProperty, IEnumerable<AppIDProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="AppIDProperty"/> instance
    /// to clone.</param>
    private AppIDProperty(AppIDProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>
    /// Initializes a new <see cref="AppIDProperty" /> object. 
    /// </summary>
    /// <param name="value">The <see cref="AppID"/> object that will
    /// be the encapsulated <see cref="Value"/>.</param>
    internal AppIDProperty(AppID value)
        : base(new ParameterSection(), null)
    {
        Debug.Assert(value is not null);
        Value = value;
    }

    private AppIDProperty(AppID value, ParameterSection parameters, string? group)
        : base(parameters, group) => Value = value;

    internal static bool TryParse(VcfRow vcfRow, [NotNullWhen(true)] out AppIDProperty? prop)
    {
        prop = null;

        if (AppID.TryParse(vcfRow.Value.Span, out AppID? client))
        {
            prop = new AppIDProperty(client, vcfRow.Parameters, vcfRow.Group);
            return true;
        }

        return false;
    }

    /// <summary> The data provided by the <see cref="AppIDProperty" />. </summary>
    public new AppID Value { get; }

    /// <inheritdoc />
    public override bool IsEmpty => false;

    /// <inheritdoc />
    public override object Clone() => new AppIDProperty(this);

    /// <inheritdoc />
    IEnumerator<AppIDProperty> IEnumerable<AppIDProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable<AppIDProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;

    internal override void AppendValue(VcfSerializer serializer) => Value.AppendTo(serializer.Builder);
}
