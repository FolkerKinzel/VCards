using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Syncs;
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards;

public sealed partial class VCard
{
    /// <summary>Indicates whether the <see cref="VCard" /> object doesn't contain
    /// any usable data.</summary>
    /// <returns> <c>true</c> if the <see cref="VCard" /> object doesn't contain
    /// any usable data, otherwise <c>false</c>.</returns>
    public bool IsEmpty() =>
        !_propDic
            .Where(static x => x.Key is not (Prop.TimeStamp or Prop.AppIDs or Prop.ID))
            .Select(static x => x.Value)
            .Any(static x => x switch
            {
                VCardProperty prop => !prop.IsEmpty,
                IEnumerable<VCardProperty?> numerable => numerable.Any(x => !(x?.IsEmpty ?? true)),
                _ => false
            });

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
        => EnumerateGroups().Distinct(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets a new <see cref="VCardProperty.Group"/> identifier that doesn't
    /// yet has been used in the <see cref="VCard"/> instance.
    /// </summary>
    /// <returns>A new <see cref="VCardProperty.Group"/> identifier that doesn't
    /// yet has been used in the <see cref="VCard"/> instance.</returns>
    public string NewGroup()
    {
        int i = -1;

        foreach (var group in GroupIDs)
        {
            if (int.TryParse(group, out int result) && result > i)
            {
                i = result;
            }
        }

        return (++i).ToString();
    }

    private IEnumerable<string> EnumerateGroups()
    {
        foreach (var kvp in _propDic)
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
