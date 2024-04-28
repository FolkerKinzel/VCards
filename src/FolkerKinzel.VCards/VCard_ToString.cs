using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards;

public sealed partial class VCard
{
    ///<inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder();

        _ = sb.Append("Version: ").Append(GetVersionString(this.Version))
              .Append(Environment.NewLine);

        foreach (Group group in Groups)
        {
            foreach (var entity in group.OrderBy(x => x.Key))
            {
                AppendEntity(entity);
            }
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

        void AppendEntity(Entity entity)
        {
            const string INDENT = "    ";

            var key = entity.Key;
            var vcdProp = entity.Value;

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
