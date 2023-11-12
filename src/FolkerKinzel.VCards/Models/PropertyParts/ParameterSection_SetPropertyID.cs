using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Syncs;

namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed partial class ParameterSection
{
    /// <summary>
    /// Sets the <see cref="PropertyID"/> for the <see cref="VCardProperty"/> 
    /// object this <see cref="ParameterSection"/> belongs to, depending on the 
    /// current value of <see cref="VCard.AppID"/>.
    /// </summary>
    /// <param name="props">The collection of <see cref="VCardProperty"/>
    /// objects the current instance belongs to.</param>
    /// <param name="vCard">The <see cref="VCard"/>&#160;<paramref name="props"/>
    /// belongs to.</param>
    /// <remarks>
    /// <note type="important">
    /// Call <see cref="VCard.RegisterAppInInstance(Uri)"/> before calling this method!
    /// </note>
    /// <para>
    /// <see cref="PropertyID"/>s (stored in <see cref="PropertyIDs"/>)
    /// enable the global data synchronization mechanism introduced with vCard&#160;4.0.
    /// The method can be called several times.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="props"/> or <paramref name="vCard"/> is 
    /// <c>null</c>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The current instance is not in <paramref name="props"/>
    /// or <paramref name="props"/> are not in <paramref name="vCard"/>.
    /// </exception>
    /// 
    public void SetPropertyID(IEnumerable<VCardProperty?> props, VCard vCard)
    {
        if (props == null)
        {
            throw new ArgumentNullException(nameof(props));
        }

        if (vCard is null)
        {
            throw new ArgumentNullException(nameof(vCard));
        }

        if (!props.Any(x => object.ReferenceEquals(this, x?.Parameters)))
        {
            throw new InvalidOperationException();
        }

        if (!vCard.AsProperties().Any(x => object.ReferenceEquals(props, x.Value)))
        {
            throw new InvalidOperationException();
        }

        SetPropertyIDIntl(props, vCard);
    }

    internal void SetPropertyIDIntl(IEnumerable<VCardProperty?> props, VCard vCard)
    {
        var propIDs = PropertyIDs ?? Enumerable.Empty<PropertyID>();
        int? clientLocalID = vCard.Sync.AppID?.LocalID;

        if (propIDs.Any(x => (x != null) && (x.App == clientLocalID)))
        {
            return;
        }

        var id = props.WhereNotNull()
            .Select(static x => x.Parameters.PropertyIDs)
            .SelectMany(static x => x ?? Enumerable.Empty<PropertyID>())
            .WhereNotNull()
            .Where(x => x.App == clientLocalID)
            .Select(static x => x.ID)
            .Append(0)
            .Max() + 1;

        var propID = new PropertyID(id, vCard.Sync.AppID);
        PropertyIDs = propIDs.Concat(propID);
    }
}
