using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class IntExtension
{
    /// <summary />
    /// <param name="id" />
    /// <param name="idName" />
    /// <exception cref="ArgumentOutOfRangeException" />
    internal static void ValidateID(this int id, string idName)
    {
        if (id < 1 || id > 9)
        {
            throw new ArgumentOutOfRangeException(idName, Res.PidValue);
        }
    }
}
