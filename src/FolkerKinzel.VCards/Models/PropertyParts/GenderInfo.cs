using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary>Encapsulates information to specify the components of the gender and 
/// gender identity of the object the <see cref="VCard"/> represents.</summary>
public sealed class GenderInfo
{
    /// <summary> Initializes a new <see cref="GenderInfo" /> object. </summary>
    /// <param name="gender">Standardized information about the gender of the object
    /// the <see cref="VCard"/> represents.</param>
    /// <param name="genderIdentity">Free text describing the gender identity.</param>
    internal GenderInfo(Gender? gender, string? genderIdentity)
    {
        Gender = gender;
        GenderIdentity = string.IsNullOrWhiteSpace(genderIdentity) ? null : genderIdentity;
    }

    /// <summary>Standardized information about the gender of the object the 
    /// <see cref="VCard"/> represents.</summary>
    public Gender? Gender { get; }

    /// <summary>Free text describing the gender identity.</summary>
    public string? GenderIdentity { get; }

    /// <summary> Returns <c>true</c> if the <see cref="GenderInfo" /> object does not 
    /// contain any usable data, otherwise <c>false</c>.</summary>
    public bool IsEmpty => !Gender.HasValue && GenderIdentity is null;

    /// <inheritdoc/>
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
}
