using FolkerKinzel.VCards.Formatters;

namespace FolkerKinzel.VCards.Intls.Models;

internal static class CompoundModelHelper
{
    internal static bool Equals(ICompoundModel first, ICompoundModel second)
    {
        Debug.Assert(first.Count == second.Count);

        StringComparer comparer = StringComparer.Ordinal;

        for (int i = 0; i < first.Count; i++)
        {
            if (!Equals(first[i], second[i], comparer))
            {
                return false;
            }
        }

        return true;

        ////////////////////////////////////////////

        static bool Equals(IReadOnlyList<string> first, IReadOnlyList<string> second, StringComparer comparer)
        {
            if (first.Count != second.Count)
            {
                return false;
            }

            for (int i = 0; i < first.Count; i++)
            {
                if (!comparer.Equals(first[i], second[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }

    internal static int GetHashCode(ICompoundModel model)
    {
        HashCode hash = new();
        StringComparer comparer = StringComparer.Ordinal;

        for (int i = 0; i < model.Count; i++)
        {
            IReadOnlyList<string> coll = model[i];

            for (int j = 0; j < coll.Count; j++)
            {
                hash.Add(coll[j], comparer);
            }
        }

        return hash.ToHashCode();
    }
}
