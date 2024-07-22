using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards.Intls.Enums;

namespace FolkerKinzel.VCards;

/// <summary>
/// Collects the data used to initialize a <see cref="Models.NameProperty"></see> instance.
/// </summary>
public class NamePropertyBuilder
{
    private readonly Dictionary<NameProp, List<string>> _dic = [];

    /// <summary>Adds a family name (also known as surname). (2,3,4)</summary>
    public void AddFamilyName(string? familyName) => throw new NotImplementedException();

    /// <summary>Adds a given name (first name). (2,3,4)</summary>
    public void AddGivenName(string? givenName) => throw new NotImplementedException();

    /// <summary>Adds an additional name (middle name). (2,3,4)</summary>
    public void AddAdditionalName(string? givenName2) => throw new NotImplementedException();

    /// <summary>Adds a honorific prefix. (2,3,4)</summary>
    public void AddPrefix(string? prefix) => throw new NotImplementedException();

    /// <summary>Adds a honorific suffix. (2,3,4)</summary>
    public void AddSuffix(string? suffix) => throw new NotImplementedException();

    /// <summary>Adds a secondary surname (used in some cultures), also known as "maternal surname". (4 - RFC 9554)</summary>
    /// <remarks>
    /// <note type="tip">
    /// For backwards compatibility use <see cref="AddFamilyName(string?)"/> to add a copy to 
    /// <see cref="Models.PropertyParts.Name.FamilyNames"/>.
    /// </note>
    /// </remarks>
    public void AddSurname2(string? surname2) => throw new NotImplementedException();

    /// <summary>A generation marker or qualifier, e.g., "Jr." or "III". (4 - RFC 9554)</summary>
    /// <note type="tip">
    /// For backwards compatibility use <see cref="AddSuffix(string?)"/> to add a copy to 
    /// <see cref="Models.PropertyParts.Name.Suffixes"/>.
    /// </note>
    public void AddGeneration(string? generation) => throw new NotImplementedException();

    internal IEnumerable<KeyValuePair<NameProp, List<string>>> Data => _dic;

    /// <summary>
    /// Clears every content of the <see cref="NamePropertyBuilder"/> instance but keeps the 
    /// allocated memory for reuse.
    /// </summary>
    public void Clear()
    {
        foreach (var pair in _dic)
        {
            pair.Value.Clear(); 
        }
    }

    private void Add(NameProp prop, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        if (_dic.TryGetValue(prop, out List<string>? list))
        {
            list.Add(value);
        }

        _dic[prop] = [value];
    }

    private void AddRange(NameProp prop, IEnumerable<string> collection)
    {
        IEnumerable<string> valsToAdd = collection?.Where(static x => !string.IsNullOrWhiteSpace(x)) 
            ?? throw new ArgumentOutOfRangeException(nameof(collection));

        if(!valsToAdd.Any())
        {
            return;
        }

        if (_dic.TryGetValue(prop, out List<string>? list))
        {
            list.AddRange(valsToAdd);
        }

        _dic[prop] = new List<string>(valsToAdd);
    }

}
