using System.Collections.Generic;
using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary>Encapsulates information about the organization (or company) of the
/// object the <see cref="VCard"/> represents.</summary>
public sealed class Organization
{
    /// <summary>Initializes a new <see cref="Organization" /> object.</summary>
    /// <param name="orgList">Organization name, optional followed by the name(s) 
    /// of the organizational units.</param>
    internal Organization(List<string> orgList)
    {
        Debug.Assert(orgList.Count != 0);
        string organizationName = orgList[0];
        orgList.RemoveAt(0);

        this.OrganizationName = string.IsNullOrWhiteSpace(organizationName) ? null : organizationName;
        this.OrganizationalUnits = ReadOnlyCollectionConverter.ToReadOnlyCollection(orgList);

        if (OrganizationalUnits.Count == 0)
        {
            OrganizationalUnits = null;
        }
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
           (OrganizationalUnits != null && OrganizationalUnits.Any(s => s.NeedsToBeQpEncoded()));
    
    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder();

        string orgName = OrganizationName is null ? "" : nameof(OrganizationName);
        string orgUnit = OrganizationalUnits is null ? "" : nameof(OrganizationalUnits);

        int padLength = Math.Max(orgName.Length, orgUnit.Length) + 2;

        if (OrganizationName != null)
        {
            _ = sb.Append($"{orgName}: ".PadRight(padLength)).Append(OrganizationName);
        }

        if (OrganizationalUnits != null)
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
        StringBuilder worker = serializer.Worker;

        _ = worker.Clear().Append(OrganizationName).Mask(serializer.Version);
        _ = builder.Append(worker);

        if (OrganizationalUnits != null)
        {
            for (int i = 0; i < OrganizationalUnits.Count; i++)
            {
                _ = worker.Clear().Append(OrganizationalUnits[i]).Mask(serializer.Version);

                _ = builder.Append(';');
                _ = builder.Append(worker);
            }
        }
    }
}
