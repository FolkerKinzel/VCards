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
    internal static bool IsAppRegistered { get; private set; }
    
    /// <summary>
    /// The global identifier of the executing application.
    /// </summary>
    /// <remarks>
    /// Call <see cref="VCard.RegisterApp(Uri?)"/> at application startup
    /// to set this property and to enable global data synchronization.
    /// </remarks>
    public static string? CurrentApp { get; private set; }

    /// <summary>
    /// Provides a <see cref="Sync"/> instance that allows to perform
    /// data synchronization with the <see cref="VCard"/> instance.
    /// </summary>
    /// <remarks>
    /// Call <see cref="VCard.RegisterApp(Uri?)"/> once before using any
    /// of the methods the <see cref="SyncOperation"/> object provides.
    /// </remarks>
    public SyncOperation Sync { get; }

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
    /// once at the startup of the application: It's not allowed to register
    /// different <see cref="Uri"/>s within a single application.
    /// </para>
    /// <para>
    /// The method sets the static property <see cref="CurrentApp"/>.
    /// </para>
    /// <para>
    /// Although it is allowed to call this method with the <c>null</c> argument, this is
    /// not recommended. (UUID-URNs are ideal for the task.) Call the method before any other
    /// method of the library and only once in the lifetime of your application. The URI
    /// should be the same everytime the application runs.
    /// </para>
    /// </remarks>
    /// <seealso cref="CurrentApp"/>
    /// <seealso cref="Sync"/>
    /// <seealso cref="SyncOperation"/>
    /// <exception cref="ArgumentException"><paramref name="globalID"/> is not an absolute
    /// <see cref="Uri"/>.</exception>
    /// <exception cref="InvalidOperationException"> An attempt was made to register
    /// different <see cref="Uri"/>s.</exception>
    public static void RegisterApp(Uri? globalID)
    {
        if (IsAppRegistered)
        {
            if (globalID is null)
            {
                if(CurrentApp != null)
                {
                    throw new InvalidOperationException(Res.MultipleCalls);
                }

                return;
            }

            if (!globalID.IsAbsoluteUri || globalID.AbsoluteUri != CurrentApp)
            {
                throw new InvalidOperationException(Res.MultipleCalls);
            }

            return;
        }

        if (globalID is null)
        {
            CurrentApp = null; // set this to have no problems with unit tests
            IsAppRegistered = true;
            return;
        }

        if (!globalID.IsAbsoluteUri)
        {
            throw new ArgumentException(string.Format(Res.RelativeUri, nameof(globalID)),
                                        nameof(globalID));
        }

        IsAppRegistered = true;
        CurrentApp = globalID.AbsoluteUri;
    }


    /// <summary>
    /// Resets <see cref="IsAppRegistered"/> to enable unit tests.
    /// </summary>
    internal static void SyncTestReset() => IsAppRegistered = false;
}
