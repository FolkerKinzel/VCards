using System.Collections.ObjectModel;
using System.ComponentModel;


/* Unmerged change from project 'FolkerKinzel.VCards (net8.0)'
Before:
using FolkerKinzel.VCards.Enums;
After:
using FolkerKinzel;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
*/
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Encodings;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using StringExtension = FolkerKinzel.VCards.Intls.Extensions.StringExtension;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates information about the organization (or company) of the
/// object the <see cref="VCard"/> represents.</summary>
public sealed class Organization
{
    #region Remove with version 8.0.1

    [Obsolete("Use OrgName instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public string? OrganizationName => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    [Obsolete("Use OrganizationalUnits instead.", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ExcludeFromCodeCoverage]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ReadOnlyCollection<string>? OrganizationalUnits => throw new NotImplementedException();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    #endregion

    private readonly string[]? _units;

    /// <summary>Initializes a new <see cref="Organization" /> object.</summary>
    /// <param name="orgName">Organization name or <c>null</c>.</param>
    /// <param name="orgUnits">Organization unit(s) or <c>null</c>.</param>
    public Organization(string? orgName, IEnumerable<string?>? orgUnits = null)
    {
        (string? orgNameParsed, string[]? orgUnitsParsed) = ParseProperties(orgName, orgUnits);

        Name = orgNameParsed;
        _units = orgUnitsParsed;
    }

    private static (string?, string[]?) ParseProperties(string? orgName,
                                                                     IEnumerable<string?>? orgUnits)
    {
        orgName = string.IsNullOrWhiteSpace(orgName) ? null : orgName;
        string[]? orgUnitsColl = StringArrayConverter.AsNonEmptyStringArray(orgUnits?.ToArray());

        return (orgName, orgUnitsColl);
    }

    /// <summary>Organization name.</summary>
    public string? Name { get; }

    /// <summary>Organizational unit name(s).</summary>
    public IReadOnlyList<string>? Units => _units;

    /// <summary>Returns <c>true</c>, if the <see cref="Organization" /> object does
    /// not contain any usable data.</summary>
    public bool IsEmpty => Name is null && Units is null;

    internal bool NeedsToBeQpEncoded()
        => Name.NeedsToBeQpEncoded() ||
           _units is not null && _units.ContainsAnyThatNeedsQpEncoding();

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder();

        string orgName = Name is null ? "" : nameof(Name);
        string orgUnit = Units is null ? "" : nameof(Units);

        int padLength = Math.Max(orgName.Length, orgUnit.Length) + 2;

        if (Name is not null)
        {
            _ = sb.Append($"{orgName}: ".PadRight(padLength)).Append(Name);
        }

        if (Units is not null)
        {
            if (sb.Length != 0)
            {
                _ = sb.Append(Environment.NewLine);
            }

            _ = sb.Append($"{orgUnit}: ".PadRight(padLength));

            for (int i = 0; i < Units.Count - 1; i++)
            {
                _ = sb.Append(Units[i]).Append("; ");
            }

            _ = sb.Append(Units[Units.Count - 1]);
        }

        return sb.ToString();
    }

    internal void AppendVCardStringTo(VcfSerializer serializer)
    {
        StringBuilder builder = serializer.Builder;
        int startIdx = builder.Length;

        _ = builder.AppendValueMasked(Name, serializer.Version);

        if (Units is not null)
        {
            for (int i = 0; i < Units.Count; i++)
            {
                _ = builder.Append(';').AppendValueMasked(Units[i], serializer.Version);
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
