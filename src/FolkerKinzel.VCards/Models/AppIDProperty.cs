using System.Collections;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Syncs;

namespace FolkerKinzel.VCards.Models;

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

    ///// <summary>  Initializes a new <see cref="AppIDProperty" /> object. 
    ///// </summary>
    ///// <param name="localID">Local ID that identifies the <see cref="AppID"/>
    ///// in the <see cref="ParameterSection.PropertyIDs"/>. (A positive <see cref="int"/>, not zero.)</param>
    ///// <param name="globalID">A URI that identifies the <see cref="AppID"/> globally.</param>
    ///// <exception cref="ArgumentOutOfRangeException"> <paramref name="localID" /> is less
    ///// than 1.</exception>
    ///// <exception cref="ArgumentNullException"> <paramref name="globalID" /> is 
    ///// <c>null</c>.</exception>
    ///// <exception cref="ArgumentException"> <paramref name="globalID" /> is 
    ///// not a valid URI.</exception>
    ///// <remarks>
    ///// <note type="caution">
    ///// Using this constructor in own code endangers the referential integrity. Prefer using
    ///// <see cref="VCard.RegisterAppInInstance(Uri)"/> instead.
    ///// </note>
    ///// </remarks>
    //public AppIDProperty(int localID, string globalID)
    //    : this(new AppID(localID, globalID)) { }

    /// <summary>
    /// Initializes a new <see cref="AppIDProperty" /> object. 
    /// </summary>
    /// <param name="appID">The <see cref="AppID"/> object that will
    /// be the encapsulated <see cref="AppIDProperty.Value"/>.</param>
    internal AppIDProperty(AppID appID)
        : base(new ParameterSection(), null)
    {
        Debug.Assert(appID != null);
        Value = appID;
    }

    private AppIDProperty(AppID appID, ParameterSection parameters, string? group)
        : base(parameters, group) => Value = appID;

    internal static bool TryParse(VcfRow vcfRow, [NotNullWhen(true)] out AppIDProperty? prop)
    {
        prop = null;

        if(AppID.TryParse(vcfRow.Value, out AppID? client))
        {
            prop = new AppIDProperty(client, vcfRow.Parameters, vcfRow.Group);
            return true;
        }

        return false;
    }

    /// <summary> The data provided by the <see cref="AppIDProperty" />. </summary>
    public new AppID Value
    {
        get;
    }

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
