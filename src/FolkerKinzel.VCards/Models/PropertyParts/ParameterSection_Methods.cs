using System.Collections;

namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed partial class ParameterSection
{
    /// <summary>Deletes all contents of the current instance.</summary>
    public void Clear() => this._propDic.Clear();


    /// <summary>Assigns the content of <paramref name="other" /> to the instance.</summary>
    /// <param name="other">Foreign instance, whose content is adopted.</param>
    /// <exception cref="ArgumentNullException"> <paramref name="other" /> is <c>null</c>.</exception>
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

        foreach (KeyValuePair<VCdParam, object> kvp in ((ParameterSection)other.Clone())._propDic)
        {
            this._propDic.Add(kvp.Key, kvp.Value);
        }
    }

    /// <summary>Creates a <see cref="string" /> representation of the <see cref="ParameterSection"
    /// /> object. (For debugging only.)</summary>
    /// <returns>A <see cref="string" /> representation of the <see cref="ParameterSection"
    /// /> object.</returns>
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
