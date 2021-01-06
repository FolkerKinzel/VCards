using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System.Text;

namespace FolkerKinzel.VCards.Models.PropertyParts
{
    /// <summary>
    /// Kapselt Informationen zur Angabe des Geschlechts und der Geschlechtsidentität.
    /// </summary>
    public class Gender
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="Gender"/>-Objekt.
        /// </summary>
        /// <param name="sex">Standardisierte Geschlechtsangabe.</param>
        /// <param name="genderIdentity">Freie Beschreibung des Geschlechts.</param>
        internal Gender(VCdSex? sex, string? genderIdentity)
        {
            Sex = sex;

            if (genderIdentity != null)
            {
                GenderIdentity = genderIdentity.Trim();
                GenderIdentity = genderIdentity.Length != 0 ? genderIdentity : null;
            }
        }

        /// <summary>
        /// Standardisierte Geschlechtsangabe.
        /// </summary>
        public VCdSex? Sex { get; }

        /// <summary>
        /// Freie Beschreibung der Geschlechtsidentität.
        /// </summary>
        public string? GenderIdentity { get; }

        /// <summary>
        /// True, wenn das <see cref="Gender"/>-Objekt keine verwertbaren Daten enthält.
        /// </summary>
        public bool IsEmpty => !Sex.HasValue && GenderIdentity is null;


        internal void AppendVCardStringTo(VcfSerializer serializer)
        {
            StringBuilder builder = serializer.Builder;
            if (Sex.HasValue)
            {
                builder.Append(Sex.ToVCardString());
            }


            if (GenderIdentity != null)
            {
                StringBuilder worker = serializer.Worker;
                worker.Clear().Append(GenderIdentity).Trim().Mask(serializer.Version);

                builder.Append(';');
                builder.Append(worker);
            }
        }

        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="Gender"/>-Objekts. 
        /// (Nur zum Debugging.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="Gender"/>-Objekts.</returns>
        public override string ToString()
        {
            string s = "";

            if (Sex.HasValue)
            {
                s += Sex.ToString();
            }

            if (GenderIdentity != null)
            {
                if (s.Length != 0)
                {
                    s += "; ";
                }

                s += GenderIdentity;
            }

            return s;
        }

    }
}
