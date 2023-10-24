using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class IntExtension
{
    /// <summary>Checks, wether <paramref name="id" /> is a number between 
    /// 1 and 9 (including the borders). </summary>
    /// <param name="id">The number to check.</param>
    /// <param name="paramName">Name of the checked method parameter.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> 
    /// is smaller than 1 or greater than 9.</exception>
    internal static void ValidateID(this int id, string paramName)
    {
        if (id < 1 || id > 9)
        {
            throw new ArgumentOutOfRangeException(paramName, Res.PidValue);
        }
    }
}
