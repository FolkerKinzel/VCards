using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Syncs;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed class Sync
{
    private readonly VCard _vCard;

    internal Sync(VCard vCard)
    {
        _vCard = vCard;
        RegisterAppInVCard(VCard.App);
    }

    /// <summary>
    /// Gets the identifier of the executing app that is used in the current
    /// <see cref="VCard"/> instance.
    /// </summary>
    public App? AppID { get; private set; }
    

    /// <summary>
    /// Registers the executing application with its global identifier.
    /// </summary>
    /// <param name="globalID">An absolute <see cref="Uri"/> that serves as global 
    /// identifier of the executing application, or <c>null</c> to disable
    /// global synchronization. (The <see cref="Uri"/> should be
    /// unique for this task: UUID-URNs, e.g., are well suited.)</param>
    /// <remarks>
    /// <para>
    /// The registration of the executing application is needed for the global data 
    /// synchronization mechanism introduced with vCard&#160;4.0.
    /// </para>
    /// <para>
    /// The method sets the property <see cref="AppID"/> and adds an item
    /// to <see cref="VCardClients"/> if the applícation has not yet been registered
    /// there before. The method can be called several times without causing any damage.
    /// </para>
    /// </remarks>
    /// <seealso cref="AppID"/>
    /// <seealso cref="VCardClients"/>
    /// <seealso cref="SetPropertyIDs"/>
    /// <exception cref="ArgumentException"><paramref name="globalID"/> is not an absolute <see cref="Uri"/>.</exception>
    private void RegisterAppInVCard(string? globalID)
    {
        if (globalID is null)
        {
            return;
        }

        if (_vCard.VCardApps is null)
        {
            var appID = new VCardClientProperty(1, globalID);
            _vCard.VCardApps = appID;
            AppID = appID.Value;
        }

        var resident = _vCard.VCardApps.FirstOrDefault(x => StringComparer.Ordinal.Equals(globalID, x?.Value.GlobalID));

        if (resident is null)
        {
            var appID = new VCardClientProperty
                (
                _vCard.VCardApps.WhereNotNull().Select(static x => x.Value.LocalID).Append(0).Max() + 1,
                globalID
                );

            _vCard.VCardApps = _vCard.VCardApps.ConcatWith(appID);
            AppID = appID.Value;
        }
        else
        {
            AppID = resident.Value;
        }
    }

    /// <summary>
    /// Sets the <see cref="PropertyID"/>s to all <see cref="VCardProperty"/> objects, which
    /// can have more than one instance inside the <see cref="VCard"/>, depending on the 
    /// current value of <see cref="VCard.AppID"/>.
    /// </summary>
    /// <remarks>
    /// <note type="important">
    /// Call <see cref="RegisterAppInInstance(Uri)"/> before calling this method!
    /// </note>
    /// <para>
    /// <see cref="PropertyID"/>s (stored in <see cref="ParameterSection.PropertyIDs"/>)
    /// enable the global data synchronization mechanism introduced with vCard&#160;4.0.
    /// The method can be called several times.
    /// </para>
    /// </remarks>
    public void SetPropertyIDs()
    {
        foreach (IEnumerable<VCardProperty?> coll in _vCard.AsProperties()
            .Where(x => (x.Key != Prop.VCardClients) && x.Value is IEnumerable<VCardProperty?>)
            .Select(x => x.Value)
            .Cast<IEnumerable<VCardProperty?>>())
        {
            foreach (VCardProperty? prop in coll)
            {
                prop?.Parameters.SetPropertyIDIntl(coll, _vCard);
            }
        }
    }

    public void Reset()
    {
        _vCard.VCardApps = null;
        RegisterAppInVCard(VCard.App);

        foreach (IEnumerable<VCardProperty?> coll in _vCard.AsProperties()
            .Where(x => x.Value is IEnumerable<VCardProperty?>)
            .Select(x => x.Value)
            .Cast<IEnumerable<VCardProperty?>>())
        {
            foreach (VCardProperty? prop in coll)
            {
                if (prop != null)
                {
                    prop.Parameters.PropertyIDs = null;
                }
            }
        }
    }

    public void Simplify()
    {
        foreach (IEnumerable<VCardProperty?> coll in _vCard.AsProperties()
            .Where(x => x.Value is IEnumerable<VCardProperty?>)
            .Select(x => x.Value)
            .Cast<IEnumerable<VCardProperty?>>())
        {
            int? localAppID = AppID?.LocalID;

            foreach (VCardProperty? prop in coll)
            {
                if (prop != null)
                {
                    prop.Parameters.PropertyIDs = 
                        prop.Parameters.PropertyIDs?
                        .Where(x => x?.App == localAppID).ToArray();
                }
            }

            SetPropertyIDs();
        }
    }
}
