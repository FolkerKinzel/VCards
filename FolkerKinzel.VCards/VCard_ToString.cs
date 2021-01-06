using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FolkerKinzel.VCards
{
    public partial class VCard : IEnumerable<KeyValuePair<VCdProp, object>>
    {
        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="VCard"/>-Objekts. 
        /// (Nur zum Debugging.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="VCard"/>-Objekts.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("Version: ").Append(GetVersionString(this.Version))
                .Append(Environment.NewLine);


            foreach (KeyValuePair<VCdProp, object> prop in this._propDic.OrderBy(x => x.Key))
            {
                switch (prop.Value)
                {
                    case IEnumerable numerable:
                        Debug.Assert(numerable != null);

                        foreach (object? o in numerable)
                        {
                            if (o is null)
                            {
                                continue;
                            }

                            var vcdProp = (VCardProperty)o;
                            AppendProperty(prop.Key, vcdProp);
                        }
                        break;
                    case VCardProperty vcdProp:
                        AppendProperty(prop.Key, vcdProp);
                        break;
                    default:
                        break;
                }
            }

            sb.Length -= Environment.NewLine.Length;

            return sb.ToString();

            ////////////////////////////////////////////

            static string GetVersionString(VCdVersion version)
            {
                return (version) switch
                {
                    VCdVersion.V2_1 => "2.1",
                    VCdVersion.V3_0 => "3.0",
                    VCdVersion.V4_0 => "4.0",
                    _ => "2.1"
                };
            }


            // ////////////////////////////////

            void AppendProperty(VCdProp key, VCardProperty vcdProp)
            {
                const string INDENT = "    ";
                string s = vcdProp.Parameters.ToString();


                sb.AppendLine(); //Leerzeile

                if (s.Length != 0)
                {
                    sb.AppendLine(s);
                }


                if (vcdProp.Group != null)
                {
                    sb.Append('[').Append("Group: ").Append(vcdProp.Group).AppendLine("]");
                }

                string propStr = vcdProp.IsEmpty ? "<EMPTY>" : vcdProp.ToString();

                if (propStr != null &&
#if NET40
                    propStr.Contains(Environment.NewLine))
#else
                    propStr.Contains(Environment.NewLine, StringComparison.Ordinal))
#endif
                {
#if NET40
                    string?[] arr = propStr.Split(
                        new string[] { Environment.NewLine }, StringSplitOptions.None);
#else
                    string?[] arr = propStr.Split(Environment.NewLine, StringSplitOptions.None);
#endif

                    sb.Append(key).AppendLine(":");

                    foreach (string? str in arr)
                    {
                        sb.Append(INDENT).AppendLine(str);
                    }
                }
                else
                {
                    sb.Append(key).Append(": ").AppendLine(propStr);
                }
            }
        }


    }
}
