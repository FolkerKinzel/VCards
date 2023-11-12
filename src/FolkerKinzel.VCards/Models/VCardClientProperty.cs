using System.Collections;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Syncs;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates information that is used to identify a vCard client globally, 
/// and locally inside the <see cref="VCard"/></summary>
/// <seealso cref="App"/>
/// <seealso cref="VCard.VCardApps"/>
public sealed class VCardClientProperty : VCardProperty, IEnumerable<VCardClientProperty>
{
    /// <summary>Copy ctor.</summary>
    /// <param name="prop">The <see cref="VCardClientProperty"/> instance
    /// to clone.</param>
    private VCardClientProperty(VCardClientProperty prop) : base(prop)
        => Value = prop.Value;

    /// <summary>  Initializes a new <see cref="VCardClientProperty" /> object. 
    /// </summary>
    /// <param name="localID">Local ID that identifies the <see cref="App"/>
    /// in the <see cref="ParameterSection.PropertyIDs"/>. (A positive <see cref="int"/>, not zero.)</param>
    /// <param name="globalID">A URI that identifies the <see cref="App"/> globally.</param>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="localID" /> is less
    /// than 1.</exception>
    /// <exception cref="ArgumentNullException"> <paramref name="globalID" /> is 
    /// <c>null</c>.</exception>
    /// <exception cref="ArgumentException"> <paramref name="globalID" /> is 
    /// not a valid URI.</exception>
    /// <remarks>
    /// <note type="caution">
    /// Using this constructor in own code endangers the referential integrity. Prefer using
    /// <see cref="VCard.RegisterAppInInstance(Uri)"/> instead.
    /// </note>
    /// </remarks>
    public VCardClientProperty(int localID, string globalID)
        : this(new App(localID, globalID)) { }

    /// <summary>
    /// Initializes a new <see cref="VCardClientProperty" /> object. 
    /// </summary>
    /// <param name="client">The <see cref="App"/> object that will
    /// be the encapsulated <see cref="VCardClientProperty.Value"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="client"/> is <c>null</c>.</exception>
    public VCardClientProperty(App client) 
        : base(new ParameterSection(), null)
        => Value = client ?? throw new ArgumentNullException(nameof(client));

    private VCardClientProperty(App client, ParameterSection parameters, string? group)
        : base(parameters, group) => Value = client;

    internal static bool TryParse(VcfRow vcfRow, [NotNullWhen(true)] out VCardClientProperty? prop)
    {
        prop = null;

        if(App.TryParse(vcfRow.Value, out App? client))
        {
            prop = new VCardClientProperty(client, vcfRow.Parameters, vcfRow.Group);
            return true;
        }

        return false;
    }

    /// <summary> The data provided by the <see cref="VCardClientProperty" />. </summary>
    public new App Value
    {
        get;
    }

    /// <inheritdoc />
    public override object Clone() => new VCardClientProperty(this);

    /// <inheritdoc />
    IEnumerator<VCardClientProperty> IEnumerable<VCardClientProperty>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable<VCardClientProperty>)this).GetEnumerator();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override object? GetVCardPropertyValue() => Value;

    internal override void AppendValue(VcfSerializer serializer) => Value.AppendTo(serializer.Builder);
}
