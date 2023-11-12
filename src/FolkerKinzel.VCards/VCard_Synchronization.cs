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
