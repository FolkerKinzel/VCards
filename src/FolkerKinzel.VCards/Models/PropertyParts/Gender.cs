using System.Text;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary>
/// Kapselt Informationen zur Angabe des Geschlechts und der Geschlechtsidentität.
/// </summary>
public sealed class Gender
{
    /// <summary>
    /// Initialisiert ein neues <see cref="Gender"/>-Objekt.
    /// </summary>
    /// <param name="sex">Standardisierte Geschlechtsangabe.</param>
    /// <param name="genderIdentity">Freie Beschreibung des Geschlechts.</param>
    internal Gender(VCdSex? sex, string? genderIdentity)
    {
        Sex = sex;
        GenderIdentity = string.IsNullOrWhiteSpace(genderIdentity) ? null : genderIdentity;
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
    /// <c>true</c>, wenn das <see cref="Gender"/>-Objekt keine verwertbaren Daten enthält.
    /// </summary>
    public bool IsEmpty => !Sex.HasValue && GenderIdentity is null;


    internal void AppendVCardStringTo(VcfSerializer serializer)
    {
        StringBuilder builder = serializer.Builder;
        if (Sex.HasValue)
        {
            _ = builder.Append(Sex.ToVcfString());
        }


        if (GenderIdentity != null)
        {
            StringBuilder worker = serializer.Worker;
            _ = worker.Clear().Append(GenderIdentity).Mask(serializer.Version);

            _ = builder.Append(';').Append(worker);
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
