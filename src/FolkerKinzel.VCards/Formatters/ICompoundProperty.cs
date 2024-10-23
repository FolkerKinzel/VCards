using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Formatters;

public interface ICompoundProperty
{
    ParameterSection Parameters {  get; }

    IReadOnlyList<string> this[int index] { get; }

     int Count { get; }
}


