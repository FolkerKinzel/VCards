using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;

// Organization is a PropertyPart, but it could be useful to reuse a single
// Organization object for more than one VCard (e.g., the vCards of a team).
// That's why Organization has a public constructor and why it is in the Models
// namespace rather than in the PropertyParts namespace.

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates information about the organization (or company) of the
/// object the <see cref="VCard"/> represents.</summary>
public sealed class Organization
{
    /// <summary>Initializes a new <see cref="Organization" /> object.</summary>
    /// <param name="orgName">Organization name or <c>null</c>.</param>
    /// <param name="orgUnits">Organization unit(s) or <c>null</c>.</param>
    public Organization(string? orgName, IEnumerable<string?>? orgUnits = null)
    {
        (string? orgNameParsed, ReadOnlyCollection<string>? orgUnitsParsed) = ParseProperties(orgName, orgUnits);

        OrganizationName = orgNameParsed;
        OrganizationalUnits = orgUnitsParsed;
    }

    internal Organization(List<string> orgList)
    {
        if (orgList.Count == 0)
        {
            return;
        }

        string orgName = orgList[0];
        orgList.RemoveAt(0);

        (string? orgNameParsed, ReadOnlyCollection<string>? orgUnitsParsed) = ParseProperties(orgName, orgList);

        OrganizationName = orgNameParsed;
        OrganizationalUnits = orgUnitsParsed;
    }

    private static (string?, ReadOnlyCollection<string>?) ParseProperties(string? orgName,
                                                                          IEnumerable<string?>? orgUnits)
    {
        orgName = string.IsNullOrWhiteSpace(orgName) ? null : orgName;
        var orgUnitsColl = ReadOnlyCollectionConverter.ToReadOnlyCollection(orgUnits);

        if (orgUnitsColl.Count == 0)
        {
            orgUnitsColl = null;
        }

        return (orgName, orgUnitsColl);
    }

    /// <summary>Organization name.</summary>
    public string? OrganizationName { get; }

    /// <summary>Organizational unit name(s).</summary>
    public ReadOnlyCollection<string>? OrganizationalUnits { get; }

    /// <summary>Returns <c>true</c>, if the <see cref="Organization" /> object does
    /// not contain any usable data.</summary>
    public bool IsEmpty => OrganizationName is null && OrganizationalUnits is null;

    internal bool NeedsToBeQpEncoded()
        => OrganizationName.NeedsToBeQpEncoded() ||
           (OrganizationalUnits?.Any(s => s.NeedsToBeQpEncoded()) ?? false);

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder();

        string orgName = OrganizationName is null ? "" : nameof(OrganizationName);
        string orgUnit = OrganizationalUnits is null ? "" : nameof(OrganizationalUnits);

        int padLength = Math.Max(orgName.Length, orgUnit.Length) + 2;

        if (OrganizationName is not null)
        {
            _ = sb.Append($"{orgName}: ".PadRight(padLength)).Append(OrganizationName);
        }

        if (OrganizationalUnits is not null)
        {
            if (sb.Length != 0)
            {
                _ = sb.Append(Environment.NewLine);
            }

            _ = sb.Append($"{orgUnit}: ".PadRight(padLength));

            for (int i = 0; i < OrganizationalUnits.Count - 1; i++)
            {
                _ = sb.Append(OrganizationalUnits[i]).Append("; ");
            }

            _ = sb.Append(OrganizationalUnits[OrganizationalUnits.Count - 1]);
        }

        return sb.ToString();
    }

    internal void AppendVCardStringTo(VcfSerializer serializer)
    {
        StringBuilder builder = serializer.Builder;
        int startIdx = builder.Length;

        _ = builder.AppendValueMasked(OrganizationName, serializer.Version);

        if (OrganizationalUnits is not null)
        {
            for (int i = 0; i < OrganizationalUnits.Count; i++)
            {
                _ = builder.Append(';').AppendValueMasked(OrganizationalUnits[i], serializer.Version);
            }
        }

        if (serializer.ParameterSerializer.ParaSection.Encoding == Enc.QuotedPrintable)
        {
            int count = builder.Length - startIdx;
            using ArrayPoolHelper.SharedArray<char> tmp = ArrayPoolHelper.Rent<char>(count);
            builder.CopyTo(startIdx, tmp.Array, 0, count);
            builder.Length = startIdx;
            builder.AppendQuotedPrintable(tmp.Array.AsSpan(0, count), startIdx);
        }
    }
}
