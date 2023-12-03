using System.Collections;
using System.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards;

public sealed partial class VCard
{
    ///<inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder();

        _ = sb.Append("Version: ").Append(GetVersionString(this.Version))
            .Append(Environment.NewLine);

        foreach (
            KeyValuePair<Prop, VCardProperty> kvp in this._propDic
            .OrderBy(static x => x.Key)
            .Select(
                  static x => x.Value is IEnumerable<VCardProperty?> prop
                                ? prop.WhereNotNull()
                                      .Select<VCardProperty, KeyValuePair<Prop, VCardProperty>>
                                      (
                                        v => new KeyValuePair<Prop, VCardProperty>(x.Key, v)
                                      )
                                : Enumerable.Repeat(new KeyValuePair<Prop, VCardProperty>(x.Key, (VCardProperty)x.Value), 1))
            .SelectMany(static x => x.OrderBy(static z => z.Value.Parameters.Preference))
            .GroupBy(static x => x.Value.Group, StringComparer.OrdinalIgnoreCase)
            .OrderBy(static x => x.Key)
            .SelectMany(static x => x)
            )
        {
            AppendProperty(kvp.Key, kvp.Value);
        }

        sb.Length -= Environment.NewLine.Length;

        return sb.ToString();

        ////////////////////////////////////////////

        static string GetVersionString(VCdVersion version)
        {
            return version switch
            {
                VCdVersion.V2_1 => "2.1",
                VCdVersion.V3_0 => "3.0",
                VCdVersion.V4_0 => "4.0",
                _ => "2.1"
            };
        }

        // ////////////////////////////////

        void AppendProperty(Prop key, VCardProperty vcdProp)
        {
            const string INDENT = "    ";
            string s = vcdProp.Parameters.ToString();

            _ = sb.AppendLine(); //Leerzeile

            if (s.Length != 0)
            {
                _ = sb.AppendLine(s);
            }

            if (vcdProp.Group is not null)
            {
                _ = sb.Append('[').Append("Group: ").Append(vcdProp.Group).AppendLine("]");
            }

            string propStr = vcdProp.IsEmpty ? "<EMPTY>" : vcdProp.ToString();

            if (propStr is not null &&
                propStr.Contains(Environment.NewLine, StringComparison.Ordinal))
            {
                string?[] arr = propStr.Split(Environment.NewLine, StringSplitOptions.None);

                _ = sb.Append(key).AppendLine(":");

                foreach (string? str in arr)
                {
                    _ = sb.Append(INDENT).AppendLine(str);
                }
            }
            else
            {
                _ = sb.Append(key).Append(": ").AppendLine(propStr);
            }
        }
    }
}
