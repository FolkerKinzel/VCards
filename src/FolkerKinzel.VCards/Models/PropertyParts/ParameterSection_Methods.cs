using System.Collections;
using FolkerKinzel.VCards.Intls;

namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed partial class ParameterSection
{
    /// <summary>Deletes all content of the current instance.</summary>
    public void Clear() => this._propDic.Clear();

    /// <summary>Assigns the content of <paramref name="other" /> to the instance.</summary>
    /// <param name="other">Foreign instance, whose content is adopted.</param>
    /// <exception cref="ArgumentNullException"> <paramref name="other" /> is <c>null</c>.</exception>
    public void Assign(ParameterSection other)
    {
        _ArgumentNullException.ThrowIfNull(other, nameof(other));

        if (object.ReferenceEquals(this, other))
        {
            return;
        }

        Clear();

        foreach (KeyValuePair<VCdParam, object> kvp in ((ParameterSection)other.Clone())._propDic)
        {
            this._propDic.Add(kvp.Key, kvp.Value);
        }
    }

    /// <inheritdoc />
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

        if (sb.Length >= Environment.NewLine.Length)
        {
            sb.Length -= Environment.NewLine.Length;
        }

        return sb.ToString();
    }


    private static void AppendValue(StringBuilder sb, KeyValuePair<VCdParam, object> para)
    {
        Debug.Assert(para.Value is not null);
        Debug.Assert(para.Value.ToString() is not null);

        const string INDENT = "    ";

        _ = sb.Append('[').Append(para.Key).Append(": ");

        string valStr = para.Value.ToString()!;

        if (valStr.Contains(Environment.NewLine, StringComparison.Ordinal))
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
