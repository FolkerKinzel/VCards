using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class UuidConverter
    {
        internal const string UUID_PROTOCOL = "urn:uuid:";
        private const int GUID_MIN_LENGTH = 32;

#if NET40
        internal static bool IsUuidUri(this string? uri)
        {
            if(uri is null || uri.Length < GUID_MIN_LENGTH) 
#else
        internal static bool IsUuidUri(this ReadOnlySpan<char> uri)
        {
            if (uri.Length < GUID_MIN_LENGTH) 
#endif
            {
                return false; 
            }

            int i = 0;

            while (i < uri.Length && char.IsWhiteSpace(uri[i]))
            {
                i++;
            }

            for (int j = i, k = 0; j < uri.Length; j++, k++)
            {
                if (k == UUID_PROTOCOL.Length)
                {
                    return true;
                }

                if (!char.ToLowerInvariant(uri[j]).Equals(UUID_PROTOCOL[k]))
                {
                    return false;
                }
            }

            return false;
        }

#if NET461 || NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Guid ToGuid(string? uuid)
            => ToGuid(uuid.AsSpan());
#endif

#if NET40
        internal static Guid ToGuid(string? uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid) || uuid.Length < GUID_MIN_LENGTH)
#else
        internal static Guid ToGuid(ReadOnlySpan<char> uuid)
        {
            if (uuid.IsWhiteSpace() || uuid.Length < GUID_MIN_LENGTH)
#endif
            {
                return Guid.Empty;
            }

            int startOfGuid = 0;

            for (int i = uuid.Length - GUID_MIN_LENGTH - 1; i >= 0; i--)
            {
                if (uuid[i].Equals(':'))
                {
                    startOfGuid = i + 1;
                    break;
                }
            }

#if NET40
            _ = Guid.TryParse(uuid.Substring(startOfGuid), out Guid guid);
#elif NET461 || NETSTANDARD2_0
            _ = Guid.TryParse(uuid.Slice(startOfGuid).ToString(), out Guid guid);
#else
            _ = Guid.TryParse(uuid.Slice(startOfGuid), out Guid guid);
#endif
            return guid;
        }


        internal static StringBuilder AppendUuid(this StringBuilder builder, Guid guid, VCdVersion version = VCdVersion.V4_0)
        {
            Debug.Assert(builder != null);

            if (version >= VCdVersion.V4_0)
            {
                _ = builder.Append(UUID_PROTOCOL);
            }
            _ = builder.Append(guid.ToString()); // FormatProvider ist reserviert

            return builder;
        }

    }
}
