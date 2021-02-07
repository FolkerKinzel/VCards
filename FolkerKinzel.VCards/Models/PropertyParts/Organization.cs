using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace FolkerKinzel.VCards.Models.PropertyParts
{
    /// <summary>
    /// Kapselt Angaben zur Organisation (oder Firma) des Subjekts, das die vCard repräsentiert.
    /// </summary>
    public sealed class Organization
    {
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
                this.OrganizationalUnits =
                    new ReadOnlyCollection<string>(organizationalUnits
                                                        .Where(x => !string.IsNullOrWhiteSpace(x))
                                                        .ToArray()!);

                if (OrganizationalUnits.Count == 0)
                {
                    OrganizationalUnits = null;
                }
            }
        }

        internal Organization(string? propertyValue, StringBuilder builder, VCdVersion version)
        {
            List<string>? list = propertyValue.SplitValueString(';', StringSplitOptions.RemoveEmptyEntries);

            if (list.Count != 0)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    Debug.Assert(list[i] != null);

                    _ = builder.Clear();
                    _ = builder.Append(list[i]);
                    _ = builder.UnMask(version); //.Trim().RemoveQuotes();

                    if (builder.Length == 0)
                    {
                        list.RemoveAt(i);
                    }
                    else
                    {
                        list[i] = builder.ToString();
                    }
                }

                if (list.Count != 0)
                {
                    OrganizationName = list[0];
                }

                if (list.Count > 1)
                {
                    OrganizationalUnits = new ReadOnlyCollection<string>(list.Skip(1).ToArray());
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
}
