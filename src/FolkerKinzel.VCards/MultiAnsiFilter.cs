using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards
{
    /// <summary>
    /// Hilfsklasse, die dem Ausfiltern und korrekten Einlesen vCard-Dateien dient, die in ANSI-Codepages gespeichert wurden.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Die Klasse ist ein Wrapper um die <see cref="VCard"/>.<see cref="VCard.LoadVcf(string, Encoding?)"/>-Methode, der zunächst prüft, ob die zu
    /// ladende Datei korrektes UTF-8 darstellt. Schlägt die Prüfung fehl, wird die VCF-Datei erneut von dem als Konstruktorparameter übergebenen 
    /// <see cref="AnsiFilter"/>-Objekt mit dessen <see cref="AnsiFilter.FallbackEncoding"/> geladen. Anschließend wird der Inhalt der geladenen VCF-Datei
    /// nach <c>CHARSET</c>-Parametern durchsucht, die einen Anhaltspunkt über die verwendete Kodierung geben. Erweist sich bei dieser Untersuchung, dass diese
    /// Kodierung von der Kodierung abweicht, mit der das VCF-Objekt geladen wurde, wird es mit der ermittelten Kodierung erneut geladen.
    /// </para>
    /// <para>
    /// Die Verwendung der Klasse eignet sich nicht für Code, bei dem es auf Performance ankommt, denn zur Auswertung werden <see cref="DecoderFallbackException"/>s 
    /// abgefangen und Dateien müssen ggf. dreimal geladen werden.
    /// </para>
    /// <para>
    /// Die Verwendung der Klasse ist nur dann sinnvoll, wenn Sie es mit vCard 2.1 - Dateien zu tun haben, denn nur in diesem Standard existieren 
    /// <c>CHARSET</c>-Parameter. Die Wirkungsweise der Klasse beruht auf der Annahme, dass innerhalb einer VCF-Datei nur eine ANSI-Kodierung Verwendung fand.
    /// </para>
    /// </remarks>
    public sealed class MultiAnsiFilter
    {
    }
}
