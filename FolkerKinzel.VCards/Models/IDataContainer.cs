namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Interface, das von Klassen implementiert wird, die Daten zur Verfügung stellen.
    /// </summary>
    public interface IDataContainer
    {
        /// <summary>
        /// Die vom implementierenden Objekt bereitgestellten Daten.
        /// </summary>
        public object? Value
        {
            get;
        }

        /// <summary>
        /// <c>true</c>, wenn das Objekt keine verwertbaren Daten enthält.
        /// </summary>
        public bool IsEmpty
        {
            get;
        }
    }
}