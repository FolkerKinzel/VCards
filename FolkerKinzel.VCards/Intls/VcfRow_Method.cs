using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Text;

namespace FolkerKinzel.VCards.Intls
{
    internal partial class VcfRow
    {
        internal void DecodeQuotedPrintable()
        {
            if (this.Parameters.Encoding == VCdEncoding.QuotedPrintable)
            {
                this.Value = QuotedPrintableConverter.Decode(this.Value, // null-Prüfung nicht erforderlich
                    TextEncodingConverter.GetEncoding(this.Parameters.Charset)); // null-Prüfung nicht erforderlich
            }
        }

        internal void UnMask(VCardDeserializationInfo info, VCdVersion version)
        {
            StringBuilder builder = info.Builder;

            builder.Clear();
            builder.Append(this.Value);
            builder.UnMask(version);
            this.Value = builder.ToString();
            if (this.Value.Length == 0)
            {
                this.Value = null;
            }
        }

        internal void UnMaskAndTrim(VCardDeserializationInfo info, VCdVersion version)
        {
            StringBuilder builder = info.Builder;

            builder.Clear();
            builder.Append(this.Value);
            builder.UnMask(version).Trim().RemoveQuotes();
            this.Value = builder.ToString();
            if (this.Value.Length == 0)
            {
                this.Value = null;
            }
        }

        internal void DecodeQuotedPrintableData()
        {
            Debug.Assert(this.Value != null);

            // vCard 2.1 ermöglicht noch andere Kodierungsarten als BASE64
            // wandle diese in BASE64 um: (vCard 2.1 kennt keine DataUri)
            if (this.Parameters.Encoding == VCdEncoding.QuotedPrintable)
            {
                var bytes = QuotedPrintableConverter.DecodeData(this.Value);
                this.Value = Convert.ToBase64String(bytes);
                this.Parameters.Encoding = VCdEncoding.Base64;
            }
            //else if(vcfRow.Parameters.Encoding == VCardEncoding.Ansi)
            //{
            //    // ?? TODO
            //}
        }

    }
}
