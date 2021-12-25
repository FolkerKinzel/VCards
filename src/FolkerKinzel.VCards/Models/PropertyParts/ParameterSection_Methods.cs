using System.Collections;
using System.Text;
using FolkerKinzel.VCards.Intls.Extensions;

#if !NET40
using FolkerKinzel.Strings.Polyfills;
#endif

namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed partial class ParameterSection
{
    /// <summary>
    /// Löscht alle Inhalte der aktuellen Instanz.
    /// </summary>
    public void Clear() => this._propDic.Clear();


    /// <summary>
    /// Weist der Instanz den Inhalt von <paramref name="other"/> zu.
    /// </summary>
    /// <param name="other">Fremde Instanz, deren Inhalt übernommen wird.</param>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> ist <c>null</c>.</exception>
    public void Assign(ParameterSection other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        if (object.ReferenceEquals(this, other))
        {
            return;
        }

        Clear();

        foreach (KeyValuePair<VCdParam, object> kvp in other._propDic)
        {
            this._propDic.Add(kvp.Key, kvp.Value);
        }
    }

    /// <summary>
    /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="ParameterSection"/>-Objekts. (Nur zum 
    /// Debuggen.)
    /// </summary>
    /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="ParameterSection"/>-Objekts.</returns>
    public override string ToString()
    {
        if (this._propDic.Count == 0)
        {
            return "";
        }

        var sb = new StringBuilder(64);

        foreach (KeyValuePair<VCdParam, object> para in this._propDic.OrderBy(x => x.Key))
        {
            if (para.Value is not string && para.Value is IEnumerable enumerable)
            {
                foreach (var value in enumerable)
                {
                    AppendValue(sb, new KeyValuePair<VCdParam, object>(para.Key, value));
                }
            }
            else
            {
                AppendValue(sb, para);
            }
        }

        sb.Length -= Environment.NewLine.Length;

        return sb.ToString();
    }

    private static void AppendValue(StringBuilder sb, KeyValuePair<VCdParam, object> para)
    {
        const string INDENT = "    ";

        _ = sb.Append('[').Append(para.Key).Append(": ");

        string? valStr = para.Value?.ToString();

        if (valStr != null &&
            valStr.Contains(Environment.NewLine, StringComparison.Ordinal))
        {
            string[] arr = valStr.Split(Environment.NewLine, StringSplitOptions.None);

            _ = sb.Append(Environment.NewLine);

            foreach (string str in arr)
            {
                _ = sb.Append(INDENT).AppendLine(str);
            }

            sb.Length -= Environment.NewLine.Length;
        }
        else
        {
            _ = sb.Append(para.Value);
        }
        _ = sb.AppendLine("]");
    }
}
