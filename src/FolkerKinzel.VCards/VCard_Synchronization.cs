using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Resources;
using FolkerKinzel.VCards.Syncs;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCard
{
    /// <summary>
    /// Registers the executing application with its global identifier.
    /// </summary>
    /// <param name="globalID">An absolute <see cref="Uri"/> that serves as global 
    /// identifier of the executing application. (The <see cref="Uri"/> should be
    /// unique for this task: UUID-URNs, e.g., are well suited.)</param>
    /// <remarks>
    /// <para>
    /// The registration of the executing application is needed for the global data 
    /// synchronization mechanism introduced with vCard&#160;4.0.
    /// </para>
    /// <para>
    /// The method sets the property <see cref="ExecutingApp"/> and adds an item
    /// to <see cref="VCardClients"/> if the applícation has not yet been registered
    /// there before. The method can be called several times without causing any damage.
    /// </para>
    /// </remarks>
    /// <seealso cref="ExecutingApp"/>
    /// <seealso cref="VCardClients"/>
    /// <seealso cref="SetPropertyIDs"/>
    /// <exception cref="ArgumentNullException"><paramref name="globalID"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="globalID"/> is not an absolute <see cref="Uri"/>.</exception>
    public void RegisterApp(Uri globalID)
    {
        if (!(globalID?.IsAbsoluteUri ?? throw new ArgumentNullException(nameof(globalID))))
        {
            throw new ArgumentException(string.Format(Res.RelativeUri, nameof(globalID)), 
                                        nameof(globalID));
        }

        string globalIDString = globalID.AbsoluteUri;

        if (VCardClients is null)
        {
            _currentApp = new VCardClientProperty(1, globalIDString);
            VCardClients = _currentApp;
        }

        var resident = VCardClients.FirstOrDefault(x => StringComparer.Ordinal.Equals(globalIDString, x?.Value.GlobalID));

        if (resident is null)
        {
            _currentApp = new VCardClientProperty
                (
                VCardClients.WhereNotNull().Select(static x => x.Value.LocalID).Append(0).Max() + 1,
                globalIDString
                );

            VCardClients = VCardClients.ConcatWith(_currentApp);
        }
        else
        {
            _currentApp = resident;
        }
    }

    /// <summary>
    /// Sets the <see cref="PropertyID"/>s to all <see cref="VCardProperty"/> objects, which
    /// can have more than one instance inside the <see cref="VCard"/>, depending on the 
    /// current value of <see cref="VCard.ExecutingApp"/>.
    /// </summary>
    /// <remarks>
    /// <note type="important">
    /// Call <see cref="RegisterApp(Uri)"/> before calling this method!
    /// </note>
    /// <para>
    /// <see cref="PropertyID"/>s (stored in <see cref="ParameterSection.PropertyIDs"/>)
    /// enable the global data synchronization mechanism introduced with vCard&#160;4.0.
    /// The method can be called several times.
    /// </para>
    /// </remarks>
    public void SetPropertyIDs()
    {
        foreach (IEnumerable<VCardProperty?> coll in AsProperties()
            .Where(x => (x.Key != Prop.VCardClients) && x.Value is IEnumerable<VCardProperty?>)
            .Select(x => x.Value)
            .Cast<IEnumerable<VCardProperty?>>()) 
        {
            foreach (VCardProperty? prop in coll)
            {
                prop?.Parameters.SetPropertyIDIntl(coll, this);
            }
        }
    }
}
