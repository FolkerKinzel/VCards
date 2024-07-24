using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Enums;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class CompoundObjectConverter
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

    internal static void SerializeProperty(IList<string> strings, char joinChar, VcfSerializer serializer)
    {
        StringBuilder builder = serializer.Builder;

        if (strings.Count == 0)
        {
            builder.Append(';');
            return;
        }

        for (int i = 0; i < strings.Count; i++)
        {
            _ = builder.AppendValueMasked(strings[i], serializer.Version).Append(joinChar);
        }

        --builder.Length;
        builder.Append(';');
    }

    internal static void EncodeQuotedPrintable(StringBuilder builder, int startIdx)
    {
        int count = builder.Length - startIdx;
        using ArrayPoolHelper.SharedArray<char> tmp = ArrayPoolHelper.Rent<char>(count);
        builder.CopyTo(startIdx, tmp.Array, 0, count);
        builder.Length = startIdx;
        builder.AppendQuotedPrintable(tmp.Array.AsSpan(0, count), startIdx);
    }
}
