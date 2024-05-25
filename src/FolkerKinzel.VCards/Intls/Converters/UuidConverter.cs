using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class UuidConverter
{
    internal const string UUID_PROTOCOL = "urn:uuid:";
    private const int GUID_MIN_LENGTH = 32;

    internal static bool IsUuidUri(this string? uri)
    {
        if (uri is null || uri.Length < GUID_MIN_LENGTH)
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

            if (char.ToLowerInvariant(uri[j]) != UUID_PROTOCOL[k])
            {
                return false;
            }
        }

        return false;
    }

    internal static Guid ToGuid(ReadOnlySpan<char> uuid)
    {
        if (uuid.IsWhiteSpace() || uuid.Length < GUID_MIN_LENGTH)
        {
            return Guid.Empty;
        }

        // e.g., urn:uuid:53e374d9-337e-4727-8803-a1e9c14e0556
        uuid = uuid.Slice(uuid.LastIndexOf(':') + 1);
        
        _ = _Guid.TryParse(uuid, out Guid guid);
        return guid;
    }

    internal static StringBuilder AppendUuid(this StringBuilder builder, Guid guid, VCdVersion version = VCdVersion.V4_0)
    {
        Debug.Assert(builder is not null);

        if (version >= VCdVersion.V4_0)
        {
            _ = builder.Append(UUID_PROTOCOL);
        }
        _ = builder.Append(guid.ToString()); // FormatProvider ist reserviert

        return builder;
    }
}
