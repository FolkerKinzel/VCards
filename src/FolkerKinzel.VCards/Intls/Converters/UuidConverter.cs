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


    internal static Guid ToGuid(string? uuid)
    {
        if (string.IsNullOrWhiteSpace(uuid) || uuid.Length < GUID_MIN_LENGTH)

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

        _ = _Guid.TryParse(uuid.AsSpan().Slice(startOfGuid), out Guid guid);
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
