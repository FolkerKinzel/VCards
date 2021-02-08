using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class ReadOnlyCollectionConverter
    {
        private static readonly ReadOnlyCollection<string> _emptyColl = new ReadOnlyCollection<string>(
#if NET40
            VCard.EmptyStringArray);
#else
            Array.Empty<string>());
#endif

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal static ReadOnlyCollection<string> Empty() => _emptyColl;

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        internal static ReadOnlyCollection<string> ToReadOnlyCollection(string? s)
            => string.IsNullOrWhiteSpace(s) ? _emptyColl :  new ReadOnlyCollection<string>(new string[] { s });

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:In bedingten Ausdruck konvertieren", Justification = "<Ausstehend>")]
        internal static ReadOnlyCollection<string> ToReadOnlyCollection(IEnumerable<string?>? coll)
        {
            if (coll is null || !coll.Any(x => !string.IsNullOrWhiteSpace(x)))
            {
                return _emptyColl;
            }

            if (coll.All(x => !string.IsNullOrWhiteSpace(x)))
            {
                if (coll is ReadOnlyCollection<string> roc)
                {
                    return roc;
                }
                else if (coll is IList<string> list)
                {
                    return new ReadOnlyCollection<string>(list);
                }
                else
                {
                    return new ReadOnlyCollection<string>(coll.ToArray()!);
                }
            }
            else
            {
                return new ReadOnlyCollection<string>(coll.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray()!);
            }
        }

        //////////////////////////////////////////////

        //        private readonly struct SingleStringList : IList<string>
        //        {
        //            private readonly string _s;

        //            internal SingleStringList(string s) => _s = s;

        //            public string this[int index]
        //            {
        //#if !NET40
        //                [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //#endif
        //                get => index == 0 ? _s : OutOfBounds();
        //                set => throw new NotImplementedException();
        //            }

        //            private static string OutOfBounds() => throw new IndexOutOfRangeException();


        //            public int Count => 1;

        //            public bool IsReadOnly => true;


        //            public bool Contains(string item) => StringComparer.Ordinal.Equals(item, _s);

        //            public void CopyTo(string[] array, int arrayIndex)
        //            {
        //                if (array is null)
        //                {
        //                    throw new ArgumentNullException(nameof(array));
        //                }

        //                array[arrayIndex] = _s;
        //            }

        //            public int IndexOf(string item) => StringComparer.Ordinal.Equals(item, _s) ? 0 : -1;



        //            public IEnumerator<string> GetEnumerator()
        //            {
        //                yield return _s;
        //            }

        //            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //            public void Add(string item) => throw new NotImplementedException();
        //            public void Clear() => throw new NotImplementedException();
        //            public void Insert(int index, string item) => throw new NotImplementedException();
        //            public bool Remove(string item) => throw new NotImplementedException();
        //            public void RemoveAt(int index) => throw new NotImplementedException();
        //        }
    }
}
