using FolkerKinzel.VCards.Intls.Enums;
using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class CompoundPropertyConverter
{

    public static string ToString<TKey>(IReadOnlyDictionary<TKey, ReadOnlyCollection<string>> sourceDic) where TKey : struct, Enum
    {
        if (sourceDic.Count == 0)
        {
            return string.Empty;
        }

        var worker = new StringBuilder();
        var dic = new List<Tuple<string, string>>();

        foreach (KeyValuePair<TKey, ReadOnlyCollection<string>> pair in sourceDic.OrderBy(x => x.Key))
        {
            string s = BuildProperty(pair.Value);
            dic.Add(new Tuple<string, string>(pair.Key.ToString(), s));
        }

        int maxLength = dic.Select(x => x.Item1.Length).Max();
        maxLength += 2;

        _ = worker.Clear();

        for (int i = 0; i < dic.Count; i++)
        {
            Tuple<string, string>? tpl = dic[i];
            string s = tpl.Item1 + ": ";
            _ = worker.Append(s.PadRight(maxLength)).Append(tpl.Item2).Append(Environment.NewLine);
        }

        worker.Length -= Environment.NewLine.Length;
        return worker.ToString();

        ////////////////////////////////////////////

        string BuildProperty(IList<string> strings)
        {
            _ = worker.Clear();

            Debug.Assert(strings.Count >= 1);

            for (int i = 0; i < strings.Count - 1; i++)
            {
                _ = worker.Append(strings[i]).Append(", ");
            }

            _ = worker.Append(strings[strings.Count - 1]);

            return worker.ToString();
        }
    }

}
