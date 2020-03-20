using FK.VCards.Intls.Encodings.QuotedPrintable;
using FK.VCards.Model.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FK.VCards.Intls.Serializers
{
    internal class VcfSerializationInfo
    {
        //private QuotedPrintableConverter _qpConverter;

        internal VcfSerializationInfo(VCdVersion version, VcfOptions options)
        {
            this.Options = options;
            this.Version = version;

            Debug.Assert(Builder != null);
        }

        public StringBuilder Builder { get; } = new StringBuilder();
        public StringBuilder Worker { get; } = new StringBuilder();
        public VCdVersion Version { get; }
        public VcfOptions Options { get; }
        public string PropertyKey { get; internal set; }
        public int PropertyStartIndex { get; internal set; }

        //internal QuotedPrintableConverter QuotedPrintableConverter
        //{
        //    get
        //    {
        //        this._qpConverter ??= new QuotedPrintableConverter();
        //        return this._qpConverter;
        //    }
        //}
    }
}
