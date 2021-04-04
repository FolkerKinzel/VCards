using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FolkerKinzel.VCards.Models.PropertyParts
{
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

            if(object.ReferenceEquals(this, other))
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
            const string INDENT = "    ";

            foreach (KeyValuePair<VCdParam, object> para in this._propDic.OrderBy(x => x.Key))
            {
                sb.Append('[').Append(para.Key).Append(": ");

                string? valStr = para.Value?.ToString();

                if (valStr != null &&
#if NET40
                    valStr.Contains(Environment.NewLine))
#else
                    valStr.Contains(Environment.NewLine, StringComparison.Ordinal))
#endif
                {
#if NET40
                    string?[] arr = valStr.Split(
                        new string[] { Environment.NewLine }, StringSplitOptions.None);
#else
                    string?[] arr = valStr.Split(Environment.NewLine, StringSplitOptions.None);
#endif

                    sb.Append(Environment.NewLine);

                    foreach (string? str in arr)
                    {
                        sb.Append(INDENT).AppendLine(str);
                    }

                    sb.Length -= Environment.NewLine.Length;
                }
                else
                {
                    sb.Append(para.Value);
                }
                sb.AppendLine("]");

            }

            sb.Length -= Environment.NewLine.Length;

            return sb.ToString();
        }


    }
}
