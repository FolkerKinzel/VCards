using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Intls.Extensions;

internal static class IntExtension
{
    /// <summary>Checks, wether <paramref name="id" /> is a positive <see cref="int"/>.
    /// </summary>
    /// <param name="id">The number to check.</param>
    /// <param name="paramName">Name of the checked method parameter.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> 
    /// is less than 1.</exception>
    internal static void ValidateID(this int id, string paramName)
    {
        if (id < 1)
        {
            throw new ArgumentOutOfRangeException(paramName, Res.PidValue);
        }
    }
}
