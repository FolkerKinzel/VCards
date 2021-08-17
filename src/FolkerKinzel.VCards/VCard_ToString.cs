using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FolkerKinzel.VCards.Intls.Extensions;

#if !NET40
using FolkerKinzel.Strings.Polyfills;
#endif

namespace FolkerKinzel.VCards
{
    public sealed partial class VCard : IEnumerable<KeyValuePair<VCdProp, object>>
    {
        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="VCard"/>-Objekts. 
        /// (Nur zum Debugging.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="VCard"/>-Objekts.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            _ = sb.Append("Version: ").Append(GetVersionString(this.Version))
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
                return version switch
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


                _ = sb.AppendLine(); //Leerzeile

                if (s.Length != 0)
                {
                    _ = sb.AppendLine(s);
                }


                if (vcdProp.Group != null)
                {
                    _ = sb.Append('[').Append("Group: ").Append(vcdProp.Group).AppendLine("]");
                }

                string propStr = vcdProp.IsEmpty ? "<EMPTY>" : vcdProp.ToString();

                if (propStr != null &&
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
}
