using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Definiert eine Methode, die einen <see cref="string"/>, der eine benannte Zeitzone
    /// (IANA time zone ID) darstellt, in einen entsprechenden UTC-Offset umwandelt.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    /// Der Standard empfiehlt für vCard 4.0, Zeitzonen als IANA Zeitzonen-IDs anzugeben. - Ältere Standards (vCard 2.1, vCard 3.0)
    /// bevorzugen hingegen UTC-Offsets.
    /// </para>
    /// <para>Es ist möglich, IANA Zeitzonennamen in UTC-Offsets umzuwandeln, aber es ist nicht möglich, UTC-Offsets in IANA-Zeitzonennamen
    /// umzuwandeln, da keine eindeutige Zuordnung besteht. Deshalb wird empfohlen <see cref="TimeZoneID"/>-Objekte mit IANA-Zeitzonennamen
    /// zu initialisieren und diese dann mit einer Implementierung von <see cref="ITimeZoneIDConverter"/> in UTC-Offsets
    /// umzuwandeln, wenn eine vCard 2.1 oder eine vCard 3.0 serialisiert wird.</para>
    /// <para>Da .NET die Umwandlung bis .NET 5.0 nicht selbst vornehmen kann, ermöglicht dieses Interface eine 3rd-Party-Library
    /// einzubinden.</para>
    /// </remarks>
    /// 
    /// <example>
    /// <note type="note">Der leichteren Lesbarkeit wegen, wird in den Beispielen auf Ausnahmebehandlung verzichtet.</note>
    /// <para>Beispiel-Implementierung für <see cref="ITimeZoneIDConverter"/>:</para>
    /// <code language="cs" source="..\Examples\TimeZoneIDConverter.cs"/>
    /// </example>
    public interface ITimeZoneIDConverter
    {
        /// <summary>
        /// Versucht, einen <see cref="string"/>, der einen Zeitzonennamen (IANA time zone ID) darstellt, in einen 
        /// zu dieser Zeitzone gehörenden UTC-Offset umzuwandeln.
        /// </summary>
        /// <param name="timeZoneID">Ein <see cref="string"/>, der den Namen einer Zeitzone darstellt.</param>
        /// <param name="utcOffset">Enthält nach erfolgreicher Beendigung der Methode den UTC-Offset. Das Argument
        /// wird uninitialisiert übergeben.</param>
        /// <returns><c>true</c>, wenn <paramref name="timeZoneID"/> in einen UTC-Offset umgewandelt werden konnte,
        /// andernfalls <c>false</c>.</returns>
        /// 
        /// <remarks>
        /// <note type="implement">
        /// <para>
        /// Die Methode <see cref="TryConvertToUtcOffset(string, out TimeSpan)"/> sollte mindestens in der Lage sein
        /// IANA Zeitzonen-IDs in UTC-Offsets umzuwandeln. (Sie könnte darüberhinaus z.B. auch POSIX-Zeitzonenbezeichner
        /// umwandeln.) Es bietet sich an, für die Aufgabe ein 3rd party nuget-Package einzusetzen, da .NET derzeit 
        /// nicht selbst dazu in der Lage ist.
        /// </para>
        /// <para>
        /// Es ist nicht notwendig, dass die Methode <see cref="string"/>s umwandelt, die bereits einen UTC-Offset darstellen,
        /// da das von <see cref="FolkerKinzel.VCards"/> bereits selbst erledigt wird.
        /// </para>
        /// </note>
        /// </remarks>
        bool TryConvertToUtcOffset(string timeZoneID, out TimeSpan utcOffset);
    }
}
