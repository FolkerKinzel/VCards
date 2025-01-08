using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards;

/// <summary>
/// Provides methods that perform data synchronization operations on
/// the <see cref="VCard"/> instance.
/// </summary>
/// <note type="important">
/// Call <see cref="VCard.RegisterApp(Uri?)"/> before calling any of 
/// these methods.
/// </note>
/// <seealso cref="VCard.Sync"/>
public sealed class SyncOperation
{
    private readonly VCard _vCard;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="vCard">The <see cref="VCard"/> instance the <see cref="SyncOperation"/>
    /// object will work with.</param>
    /// <exception cref="InvalidOperationException">The executing application is
    /// not yet registered with the <see cref="VCard"/> class.</exception>
    internal SyncOperation(VCard vCard)
    {
        _vCard = vCard;
        RegisterAppInVCardInstance();
    }

    /// <summary>
    /// Gets the identifier of the executing application
    /// that is currently used within the <see cref="VCard"/>
    /// instance.
    /// </summary>
    /// <remarks>
    /// The value of this property may change when calling
    /// the methods of the <see cref="SyncOperation"/> object.
    /// </remarks>
    /// <seealso cref="AppID"/>
    public AppID? CurrentAppID { get; private set; }

    /// <summary>
    /// Marks the <see cref="VCardProperty"/> objects with <see cref="PropertyID"/>s.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The method sets the <see cref="PropertyID"/>s to the <see cref="VCardProperty"/> objects that
    /// doesn't yet have one depending on the 
    /// value of <see cref="CurrentAppID"/>, and adds <see cref="CurrentAppID"/> to <see cref="VCard.AppIDs"/>
    /// if it's not yet there.
    /// </para>
    /// <para>
    /// <see cref="PropertyID"/>s (stored in <see cref="ParameterSection.PropertyIDs"/>)
    /// enable the global data synchronization mechanism introduced with vCard&#160;4.0.
    /// </para>
    /// <para>
    /// The method is called automatically when serializing vCard&#160;4.0 using <see cref="VcfOpts.Default"/>
    /// but it may be called several times without causing any damage.
    /// </para>
    /// </remarks>
    public void SetPropertyIDs()
    {
        bool any = false;

        foreach (IEnumerable<VCardProperty?> coll in _vCard.Properties
            .Where(x => x.Value is IEnumerable<VCardProperty?> && x.Key != Prop.AppIDs)
            .Select(x => (IEnumerable<VCardProperty?>)x.Value))
        {
            foreach (VCardProperty? prop in coll)
            {
                if (prop is not null)
                {
                    any = true;
                    SetPropertyID(prop.Parameters, coll);
                }
            }
        }

        if (any && CurrentAppID is not null)
        {
            if (!(_vCard.AppIDs?.Any(x => ReferenceEquals(x.Value, CurrentAppID)) ?? false))
            {
                var newAppIDProp = new AppIDProperty(CurrentAppID);
                _vCard.AppIDs = _vCard.AppIDs?.Concat(newAppIDProp) ?? newAppIDProp;
            }
        }
    }

    /// <summary>
    /// Resets the data synchronization mechanism.
    /// </summary>
    /// <remarks>
    /// The method removes all <see cref="PropertyID"/> and <see cref="AppIDProperty"/> 
    /// objects from the <see cref="VCard"/> instance. Call this method when the 
    /// data synchronization has been completed (see RFC&#160;6350, 7.2.5. "Global 
    /// Context Simplification").
    /// </remarks>
    /// <example>
    /// <code language="cs" source="..\Examples\NoPidExample.cs"/>
    /// </example>
    public void Reset()
    {
        _vCard.AppIDs = null;
        RegisterAppInVCardInstance();

        foreach (Entity kvp in _vCard.Entities)
        {
            kvp.Value.Parameters.PropertyIDs = null;
        }
    }

    /// <summary>
    /// Registers the executing application in the VCard instance.
    /// </summary>
    /// <exception cref="InvalidOperationException">The executing application is
    /// not yet registered with the <see cref="VCard"/> class.</exception>
    private void RegisterAppInVCardInstance()
    {
        if (!VCard.IsAppRegistered)
        {
            VCard.RegisterApp(null);
        }

        if (VCard.App is null)
        {
            return;
        }

        if (_vCard.AppIDs is null)
        {
            CurrentAppID = new AppID(1, VCard.App);
            return;
        }

        AppIDProperty? resident = _vCard.AppIDs.FirstOrDefault(x => StringComparer.Ordinal.Equals(VCard.App, x.Value.GlobalID));

        CurrentAppID = resident is null
            ? new AppID
                (
                _vCard.AppIDs.Select(static x => x.Value.LocalID).Append(0).Max() + 1,
                VCard.App
                )
            : resident.Value;
    }

    private void SetPropertyID(ParameterSection parameters, IEnumerable<VCardProperty?> props)
    {
        IEnumerable<PropertyID> propIDs = parameters.PropertyIDs ?? [];
        int? appLocalID = CurrentAppID?.LocalID;

        if (propIDs.Any(x => x.App == appLocalID))
        {
            return;
        }

        int id = props
            .OfType<VCardProperty>()
            .Select(static x => x.Parameters.PropertyIDs)
            .SelectMany(static x => x ?? [])
            .Where(x => x.App == appLocalID)
            .Select(static x => x.ID)
            .Append(0)
            .Max() + 1;

        var propID = new PropertyID(id, CurrentAppID);
        parameters.PropertyIDs = propIDs.Concat(propID);
    }


    // Overriding Equals, GetHashCode and ToString to hide these methods in IntelliSense:

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString()!;
}
