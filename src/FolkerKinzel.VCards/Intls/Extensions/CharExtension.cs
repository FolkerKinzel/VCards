#if NET40
namespace FolkerKinzel.VCards.Intls.Extensions
{
    internal static class CharExtension
    {
        internal static bool IsDecimalDigit(this char c) => 47u < c && 58u > c;
    }
}
#endif
