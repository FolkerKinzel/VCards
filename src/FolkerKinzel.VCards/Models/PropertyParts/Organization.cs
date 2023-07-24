using System.Collections.ObjectModel;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary>
/// Kapselt Angaben zur Organisation (oder Firma) des Subjekts, das die vCard repräsentiert.
/// </summary>
public sealed class Organization
{
    internal Organization() { }
    

    /// <summary>
    /// Initialisiert ein neues <see cref="Organization"/>-Objekt.
    /// </summary>
    /// <param name="organizationName">Name der Organisation.</param>
    /// <param name="organizationalUnits">Name(n) der Unterorganisation(en).</param>
    internal Organization(string? organizationName, IEnumerable<string?>? organizationalUnits = null)
    {
        this.OrganizationName = string.IsNullOrWhiteSpace(organizationName) ? null : organizationName;

        if (organizationalUnits != null)
        {
            this.OrganizationalUnits = ReadOnlyCollectionConverter.ToReadOnlyCollection(organizationalUnits);

            if (OrganizationalUnits.Count == 0)
            {
                OrganizationalUnits = null;
            }
        }
    }

    /// <summary>
    /// Name der Organisation
    /// </summary>
    public string? OrganizationName { get; }

    /// <summary>
    /// Name(n) der Unterorganisation(en)
    /// </summary>
    public ReadOnlyCollection<string>? OrganizationalUnits { get; }


    /// <summary>
    /// <c>true</c>, wenn das <see cref="Organization"/>-Objekt keine verwertbaren Daten enthält.
    /// </summary>
    public bool IsEmpty => OrganizationName is null && OrganizationalUnits is null;



    internal bool NeedsToBeQpEncoded => OrganizationName.NeedsToBeQpEncoded() ||
            (OrganizationalUnits != null && OrganizationalUnits.Any(s => s.NeedsToBeQpEncoded()));


    internal void AppendVCardString(VcfSerializer serializer)
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

    /// <summary>
    /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="Organization"/>-Objekts. (Nur zum 
    /// Debuggen.)
    /// </summary>
    /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="Organization"/>-Objekts.</returns>
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

}
