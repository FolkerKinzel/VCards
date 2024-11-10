using System.Collections.ObjectModel;
using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;

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

        OrgName = orgNameParsed;
        OrgUnits = orgUnitsParsed;
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

        OrgName = orgNameParsed;
        OrgUnits = orgUnitsParsed;
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
    public string? OrgName { get; }

    [Obsolete("Use OrgName instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public string? OrganizationName => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


    /// <summary>Organizational unit name(s).</summary>
    public ReadOnlyCollection<string>? OrgUnits { get; }

    [Obsolete("Use OrganizationalUnits instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ReadOnlyCollection<string>? OrganizationalUnits => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


    /// <summary>Returns <c>true</c>, if the <see cref="Organization" /> object does
    /// not contain any usable data.</summary>
    public bool IsEmpty => OrgName is null && OrgUnits is null;

    internal bool NeedsToBeQpEncoded()
        => OrgName.NeedsToBeQpEncoded() ||
           (OrgUnits?.Any(s => s.NeedsToBeQpEncoded()) ?? false);

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder();

        string orgName = OrgName is null ? "" : nameof(OrgName);
        string orgUnit = OrgUnits is null ? "" : nameof(OrgUnits);

        int padLength = Math.Max(orgName.Length, orgUnit.Length) + 2;

        if (OrgName is not null)
        {
            _ = sb.Append($"{orgName}: ".PadRight(padLength)).Append(OrgName);
        }

        if (OrgUnits is not null)
        {
            if (sb.Length != 0)
            {
                _ = sb.Append(Environment.NewLine);
            }

            _ = sb.Append($"{orgUnit}: ".PadRight(padLength));

            for (int i = 0; i < OrgUnits.Count - 1; i++)
            {
                _ = sb.Append(OrgUnits[i]).Append("; ");
            }

            _ = sb.Append(OrgUnits[OrgUnits.Count - 1]);
        }

        return sb.ToString();
    }

    internal void AppendVCardStringTo(VcfSerializer serializer)
    {
        StringBuilder builder = serializer.Builder;
        int startIdx = builder.Length;

        _ = builder.AppendValueMasked(OrgName, serializer.Version);

        if (OrgUnits is not null)
        {
            for (int i = 0; i < OrgUnits.Count; i++)
            {
                _ = builder.Append(';').AppendValueMasked(OrgUnits[i], serializer.Version);
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
