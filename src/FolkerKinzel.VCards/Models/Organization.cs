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

        string[]? orgUnitsColl = null;

        if (orgUnits is not null)
        {
            orgUnitsColl = StringArrayConverter.ToStringArray(orgUnits);
            orgUnitsColl = orgUnitsColl.ContainsData() ? orgUnitsColl : null;
        }

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
           (_units is not null && _units.ContainsAnyThatNeedsQpEncoding());

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
