using System;
using System.Collections.Generic;
using System.Text;

namespace FolkerKinzel.VCards.Intls.Deserializers
{
    internal static class Assigner
    {
        internal static IEnumerable<T> GetAssignment<T>(T newElement, IEnumerable<T?>? currentElement) where T : IEnumerable<T>
        {
            switch (currentElement)
            {
                case null:
                    return newElement;
                case T t:
                    return new List<T>() { t, newElement };
                case List<T> list:
                    list.Add(newElement);
                    return list;

                default:
                    throw new ArgumentOutOfRangeException(nameof(currentElement));
            };
        }
    }
}
