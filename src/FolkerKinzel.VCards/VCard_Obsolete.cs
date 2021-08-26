using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards
{
    public sealed partial class VCard
    {
        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="textEncoding"></param>
        /// <returns></returns>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use LoadVcf instead.", true)]
        public static List<VCard> Load(string fileName, Encoding? textEncoding = null)
            => (List<VCard>)LoadVcf(fileName, textEncoding);


        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use ParseVcf instead.", true)]
        public static List<VCard> Parse(string content) => (List<VCard>)ParseVcf(content);


        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use DeserializeVcf instead.", true)]
        public static List<VCard> Deserialize(TextReader reader) => (List<VCard>)DeserializeVcf(reader);


        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="vCardList"></param>
        /// <param name="version"></param>
        /// <param name="options"></param>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use SaveVcf instead.", true)]
        public static void Save(
            string fileName,
            List<
#nullable disable
                VCard
#nullable restore
                > vCardList,
            VCdVersion version = DEFAULT_VERSION,
            VcfOptions options = VcfOptions.Default) => SaveVcf(fileName, vCardList, version, options: options);


        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="vCardList"></param>
        /// <param name="version"></param>
        /// <param name="options"></param>
        /// <param name="leaveStreamOpen"></param>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use SerializeVcf instead.", true)]
        public static void Serialize(Stream stream,
                                     List<
#nullable disable
                                         VCard
#nullable restore
                                         > vCardList,
                                     VCdVersion version = DEFAULT_VERSION,
                                     VcfOptions options = VcfOptions.Default,
                                     bool leaveStreamOpen = false) => SerializeVcf(stream, vCardList, version, options: options, leaveStreamOpen: leaveStreamOpen);


        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="version"></param>
        /// <param name="options"></param>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use SaveVcf instead!", true)]
        public void Save(
            string fileName,
            VCdVersion version = DEFAULT_VERSION,
            VcfOptions options = VcfOptions.Default) => SaveVcf(fileName, version, options: options);


        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="version"></param>
        /// <param name="options"></param>
        /// <param name="leaveStreamOpen"></param>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use SerializeVcf instead.", true)]
        public void Serialize(Stream stream,
                              VCdVersion version = DEFAULT_VERSION,
                              VcfOptions options = VcfOptions.Default,
                              bool leaveStreamOpen = false) => SerializeVcf(stream, version, options: options, leaveStreamOpen: leaveStreamOpen);

    }
}
