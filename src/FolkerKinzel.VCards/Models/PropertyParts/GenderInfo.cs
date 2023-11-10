using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;

namespace FolkerKinzel.VCards.Models.PropertyParts;

/// <summary>Encapsulates information to specify the components of the gender and 
/// gender identity of the object the <see cref="VCard"/> represents.</summary>
public sealed class GenderInfo
{
    /// <summary> Initializes a new <see cref="GenderInfo" /> object. </summary>
    /// <param name="sex">Standardized information about the sex of the object
    /// the <see cref="VCard"/> represents.</param>
    /// <param name="identity">Free text describing the gender identity.</param>
    internal GenderInfo(Sex? sex, string? identity)
    {
        Sex = sex;
        Identity = string.IsNullOrWhiteSpace(identity) ? null : identity;
    }

    /// <summary>Standardized information about the gender of the object the 
    /// <see cref="VCard"/> represents.</summary>
    public Sex? Sex { get; }

    /// <summary>Free text describing the gender identity.</summary>
    public string? Identity { get; }

    /// <summary> Returns <c>true</c> if the <see cref="GenderInfo" /> object does not 
    /// contain any usable data, otherwise <c>false</c>.</summary>
    public bool IsEmpty => !Sex.HasValue && Identity is null;

    /// <inheritdoc/>
    public override string ToString()
    {
        string s = "";

        if (Sex.HasValue)
        {
            s += Sex.ToString();
        }

        if (Identity != null)
        {
            if (s.Length != 0)
            {
                s += "; ";
            }

            s += Identity;
        }

        return s;
    }

    internal void AppendVCardStringTo(VcfSerializer serializer)
    {
        StringBuilder builder = serializer.Builder;
        if (Sex.HasValue)
        {
            _ = builder.Append(Sex.ToVcfString());
        }


        if (Identity != null)
        {
            StringBuilder worker = serializer.Worker;
            _ = worker.Clear().Append(Identity).Mask(serializer.Version);

            _ = builder.Append(';').Append(worker);
        }
    }
}
