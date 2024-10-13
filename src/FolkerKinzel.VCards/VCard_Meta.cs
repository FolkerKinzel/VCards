using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards;

public sealed partial class VCard
{
    /// <summary> <c>VERSION</c>: Version of the vCard standard. <c>(2,3,4)</c></summary>
    public VCdVersion Version
    {
        get; internal set;
    }

    /// <summary>Indicates whether the <see cref="VCard" /> object doesn't contain
    /// any usable data.</summary>
    /// <returns> <c>true</c> if the <see cref="VCard" /> object doesn't contain
    /// any usable data, otherwise <c>false</c>.</returns>
    public bool IsEmpty()
    {
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
        return !_propDic
            .Where(static x => !IsNonContentProperty(x.Key))
            .Select(static x => x.Value)
            .Any(static x => x switch
            {
                VCardProperty prop => !prop.IsEmpty,
                IEnumerable<VCardProperty?> numerable => numerable.Any(x => !(x?.IsEmpty ?? true)),
#if DEBUG
                _ => throw new NotImplementedException()
#endif
            });
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).

        static bool IsNonContentProperty(Prop prop)
        {
            return prop switch
            {
                Prop.Created 
                    or Prop.ID
                    or Prop.TimeStamp
                    or Prop.AppIDs
                    or Prop.Kind
                    or Prop.Language
                    or Prop.Pronouns
                    or Prop.GramGenders
                    or Prop.Access 
                    or Prop.Mailer 
                    or Prop.ProductID 
                    or Prop.Profile => true,
                _ => false,
            };
        }
    }

    /// <summary>
    /// Gets an <see cref="IEnumerable{T}"/> of <see cref="string"/>s that can be used
    /// to iterate over the
    /// <see cref="VCardProperty.Group"/> identifiers of the <see cref="VCard"/>.
    /// </summary>
    /// <remarks>
    /// <note type="tip">
    /// Iterating over the <see cref="VCardProperty.Group"/> identifiers is an expensive
    /// operation. Store the results if they are needed several times.
    /// </note>
    /// <para>
    /// The method returns each <see cref="VCardProperty.Group"/> identifier only
    /// once. <c>null</c> is not a <see cref="VCardProperty.Group"/> identifier.
    /// The comparison of <see cref="VCardProperty.Group"/> identifiers is
    /// case-insensitive.
    /// </para>
    /// </remarks>
    public IEnumerable<string> GroupIDs
        => EnumerateGroupIDs().Distinct(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets a new <see cref="VCardProperty.Group"/> identifier that doesn't
    /// yet has been used in the <see cref="VCard"/> instance.
    /// </summary>
    /// <returns>A new <see cref="VCardProperty.Group"/> identifier that doesn't
    /// yet has been used in the <see cref="VCard"/> instance.</returns>
    public string NewGroup()
    {
        int i = -1;

        foreach (string group in GroupIDs)
        {
            if (int.TryParse(group, out int result) && result > i)
            {
                i = result;
            }
        }

        return (++i).ToString();
    }

    private IEnumerable<string> EnumerateGroupIDs()
    {
        foreach (KeyValuePair<Prop, object> kvp in _propDic)
        {
            if (kvp.Value is VCardProperty prop && prop.Group is not null)
            {
                yield return prop.Group;
                continue;
            }

            if (kvp.Value is IEnumerable<VCardProperty?> numerable)
            {
                foreach (VCardProperty? vcProp in numerable)
                {
                    string? group = vcProp?.Group;

                    if (group is not null)
                    {
                        yield return group;
                    }
                }
            }
        }
    }
}
