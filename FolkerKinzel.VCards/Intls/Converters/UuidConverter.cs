using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Text;

namespace FolkerKinzel.VCards.Intls.Converters
{
    static class UuidConverter
    {
        internal const string UUID_PROTOCOL = "urn:uuid:";


        internal static bool IsUuid(this string? uri)
        {
            return uri?.StartsWith(UUID_PROTOCOL, StringComparison.OrdinalIgnoreCase) ?? false;
        }

        internal static Guid ToGuid(string? uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid)) return Guid.Empty;

#if NET40
            var arr = uuid.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
#else
            var arr = uuid.Split(':', StringSplitOptions.RemoveEmptyEntries);
#endif

            _ = Guid.TryParse(arr[arr.Length - 1], out Guid guid);

            return guid;
        }


        internal static StringBuilder AppendUuid(this StringBuilder builder, Guid guid, VCdVersion version = VCdVersion.V4_0)
        {
            Debug.Assert(builder != null);

            if (version >= VCdVersion.V4_0)
            {
                builder.Append(UUID_PROTOCOL);
            }
            builder.Append(guid.ToString()); // FormatProvider ist reserviert

            return builder;
        }

    }
}
