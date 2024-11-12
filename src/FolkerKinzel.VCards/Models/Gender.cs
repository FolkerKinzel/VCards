using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates information to specify the components of the gender and 
/// gender identity of the object the <see cref="VCard"/> represents.</summary>
/// <seealso cref="VCard.GenderViews"/>
/// <seealso cref="GenderProperty"/>
/// <remarks> Initializes a new <see cref="Gender" /> object. </remarks>
/// <param name="sex">Standardized information about the sex of the object
/// the <see cref="VCard"/> represents.</param>
/// <param name="identity">Free text describing the gender identity.</param>
public sealed class Gender(Sex? sex, string? identity = null)
{
    /// <summary>Standardized information about the gender of the object the 
    /// <see cref="VCard"/> represents.</summary>
    public Sex? Sex { get; } = sex;

    /// <summary>Free text describing the gender identity.</summary>
    public string? Identity { get; } = string.IsNullOrWhiteSpace(identity) ? null : identity;

    /// <summary> Returns <c>true</c> if the <see cref="Gender" /> object does not 
    /// contain any usable data, otherwise <c>false</c>.</summary>
    public bool IsEmpty => !Sex.HasValue && Identity is null;

    /// <summary>
    /// A singleton that encapsulates <see cref="Enums.Sex.Male"/>
    /// </summary>
    public static Gender Male { get; } = new(Enums.Sex.Male);

    /// <summary>
    /// A singleton that encapsulates <see cref="Enums.Sex.Female"/>
    /// </summary>
    public static Gender Female { get; } = new(Enums.Sex.Female);

    internal static Gender Empty => new(null, null); // Don't use a singleton here: It's probably not often needed.

    /// <inheritdoc/>
    public override string ToString()
    {
        string s = "";

        if (Sex.HasValue)
        {
            s += Sex.ToString();
        }

        if (Identity is not null)
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
        if (Sex.HasValue)
        {
            _ = serializer.Builder.Append(Sex.ToVcfString());
        }

        if (Identity is not null)
        {
            _ = serializer.Builder.Append(';').AppendValueMasked(Identity, serializer.Version);
        }
    }
}
