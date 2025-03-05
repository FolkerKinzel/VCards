using FolkerKinzel.VCards.Models.Properties;
using FolkerKinzel.VCards.Models.Properties.Parameters;

namespace FolkerKinzel.VCards.Formatters;

/// <summary>
/// Interface implemented by <see cref="VCardProperty"/> instances containing a compound value.
/// </summary>
/// <seealso cref="AddressProperty"/>
/// <seealso cref="NameProperty"/>
public interface ICompoundProperty : ICompoundModel
{
    /// <summary>Gets the data of the parameter section of a vCard property.</summary>
    ParameterSection Parameters { get; }
}


