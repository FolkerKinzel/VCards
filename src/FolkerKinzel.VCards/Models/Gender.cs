using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Models;

/// <summary>Encapsulates information to specify the components of the gender and 
/// gender identity of the object the <see cref="VCard"/> represents.</summary>
/// <seealso cref="VCard.GenderViews"/>
/// <seealso cref="GenderProperty"/>
/// <remarks> Initializes a new <see cref="Gender" /> object. </remarks>
public sealed class Gender
{
    private Gender(Sex? sex, string? identity)
    {
        Sex = sex;
        Identity = identity;
    }

    /// <summary>
    /// Creates a <see cref="Gender"/> instance with the specified <paramref name="sex"/>
    /// and gender-<paramref name="identity"/>.
    /// </summary>
    /// <param name="sex">Standardized information about the sex of the object
    /// the <see cref="VCard"/> represents.</param>
    /// <param name="identity">Free text describing the gender identity.</param>
    /// <returns>A gender instance that corresponds to the specified arguments.</returns>
    public static Gender Create(Sex? sex, string? identity = null)
    {
        identity = string.IsNullOrWhiteSpace(identity) ? null : identity;

        if (identity is null)
        {
            if (sex == Enums.Sex.Male)
            {
                return Male;
            }

            if (sex == Enums.Sex.Female)
            {
                return Female;
            }
        }

        return new(sex, identity);
    }

    /// <summary>Standardized information about the gender of the object the 
    /// <see cref="VCard"/> represents.</summary>
    public Sex? Sex { get; }

    /// <summary>Free text describing the gender identity.</summary>
    public string? Identity { get; }

    /// <summary> Returns <c>true</c> if the <see cref="Gender" /> object does not 
    /// contain any usable data, otherwise <c>false</c>.</summary>
    public bool IsEmpty => !Sex.HasValue && Identity is null;

    /// <summary>
    /// A singleton that encapsulates <see cref="Sex.Male"/>
    /// </summary>
    public static Gender Male { get; } = new(Enums.Sex.Male, null);

    /// <summary>
    /// A singleton that encapsulates <see cref="Sex.Female"/>
    /// </summary>
    public static Gender Female { get; } = new(Enums.Sex.Female, null);

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
}
