using System.Buffers;

namespace FolkerKinzel.VCards.Intls;

internal static class ArrayPoolHelper
{
    internal static SharedArray<T> Rent<T>(int minimumLength) => new(minimumLength);

    internal readonly struct SharedArray<T> : IDisposable
    {
        internal SharedArray(int minimumLength)
            => Array = ArrayPool<T>.Shared.Rent(minimumLength);

        internal T[] Array { get; }

        public void Dispose() => ArrayPool<T>.Shared.Return(Array, Confidentiality.IsConfidential);
    }
}
