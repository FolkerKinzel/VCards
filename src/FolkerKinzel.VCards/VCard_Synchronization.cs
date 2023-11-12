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
    private static bool _isAppRegistered;
    
    public static string? App { get; private set; }


    /// <summary>
    /// Registers the executing application with its global identifier.
    /// </summary>
    /// <param name="globalID">An absolute <see cref="Uri"/> that serves as global 
    /// identifier of the executing application, or <c>null</c> to disable
    /// global synchronization. (The <see cref="Uri"/> should be
    /// unique: UUID-URNs, e.g., are well suited.)</param>
    /// <remarks>
    /// <para>
    /// The registration of the executing application is needed for the global data 
    /// synchronization mechanism introduced with vCard&#160;4.0. Call this method
    /// only once at the startup of the application.
    /// </para>
    /// <para>
    /// The method sets the static property <see cref="App"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="App"/>
    /// <seealso cref="Sync"/>
    /// <exception cref="ArgumentException"><paramref name="globalID"/> is not an absolute <see cref="Uri"/>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called more than once.</exception>
    public static void RegisterApp(Uri? globalID)
    {
        if (_isAppRegistered)
        {
            throw new InvalidOperationException();
        }

        if (globalID is null)
        {
            _isAppRegistered = true;
            return;
        }

        if (!globalID.IsAbsoluteUri)
        {
            throw new ArgumentException(string.Format(Res.RelativeUri, nameof(globalID)),
                                        nameof(globalID));
        }

        _isAppRegistered = true;
        App = globalID.AbsoluteUri;
    }
}
