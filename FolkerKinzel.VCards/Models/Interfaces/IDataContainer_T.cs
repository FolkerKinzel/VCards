using System;
using System.Collections.Generic;
using System.Text;

namespace FolkerKinzel.VCards.Models.Interfaces
{
    /// <summary>
    /// Typsichere Spezialisierung des <see cref="IDataContainer"/>-Interfaces.
    /// </summary>
    /// <typeparam name="T">Beliebiger Datentyp, der den Inhalt von <see cref="IDataContainer{T}"/> darstellt.</typeparam>
    public interface IDataContainer<T> : IDataContainer
    {
        /// <summary>
        /// Die vom Objekt bereitgestellten Daten.
        /// </summary>
        new T Value
        {
            get;
        }

    }
}
