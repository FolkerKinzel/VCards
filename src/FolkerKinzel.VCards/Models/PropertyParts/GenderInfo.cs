using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models.PropertyParts;

    /// <summary>Encapsulates information to specify the components of the sex and gender
    /// identity of the object the vCard represents</summary>
public sealed class GenderInfo
{
    /// <summary> Initialisiert ein neues <see cref="GenderInfo" />-Objekt. </summary>
    /// <param name="gender">Standardized information about the sex of a person.</param>
    /// <param name="genderIdentity">Free text describing the gender identity.</param>
    internal GenderInfo(Gender? gender, string? genderIdentity)
    {
        Gender = gender;
        GenderIdentity = string.IsNullOrWhiteSpace(genderIdentity) ? null : genderIdentity;
    }

    /// <summary>Standardized information about the sex of a person.</summary>
    public Gender? Gender { get; }

    /// <summary>Free text describing the gender identity.</summary>
    public string? GenderIdentity { get; }

    /// <summary> <c>true</c>, wenn das <see cref="GenderInfo" />-Objekt keine verwertbaren
    /// Daten enthält. </summary>
    public bool IsEmpty => !Gender.HasValue && GenderIdentity is null;


    internal void AppendVCardStringTo(VcfSerializer serializer)
    {
        StringBuilder builder = serializer.Builder;
        if (Gender.HasValue)
        {
            _ = builder.Append(Gender.ToVcfString());
        }


        if (GenderIdentity != null)
        {
            StringBuilder worker = serializer.Worker;
            _ = worker.Clear().Append(GenderIdentity).Mask(serializer.Version);

            _ = builder.Append(';').Append(worker);
        }
    }

    /// <summary> Erstellt eine <see cref="string" />-Repräsentation des <see cref="GenderInfo"
    /// />-Objekts. (Nur zum Debugging.) </summary>
    /// <returns>Eine <see cref="string" />-Repräsentation des <see cref="GenderInfo"
    /// />-Objekts.</returns>
    public override string ToString()
    {
        string s = "";

        if (Gender.HasValue)
        {
            s += Gender.ToString();
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
