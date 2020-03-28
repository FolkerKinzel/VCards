using System.Resources;

namespace FolkerKinzel.VCards.Resources
{
    /// <summary>
    ///   Eine stark typisierte Ressourcenklasse zum Suchen von lokalisierten Zeichenfolgen.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1304:CultureInfo angeben", Justification = "<Ausstehend>")]
    internal static class Res
    {
        /// <summary>
        ///   Gibt die bei jedem Aufruf eine neue <see cref="ResourceManager"/>-Instanz zurück. Dies ist sinnvoll, weil
        ///   der <see cref="ResourceManager"/> nur für Ausnahmen verwendet wird.
        /// </summary>
        private static ResourceManager ResourceManager => new ResourceManager("FolkerKinzel.VCards.Resources.Res", typeof(Res).Assembly);


        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "The argument contains no data." ähnelt.
        /// </summary>
        internal static string NoData => ResourceManager.GetString("NoData");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "The XML element did not explicitly specify an own namespace." ähnelt.
        /// </summary>
        internal static string NoNameSpace => ResourceManager.GetString("NoNameSpace");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "The argument is not a valid X-Name." ähnelt.
        /// </summary>
        internal static string NoXName => ResourceManager.GetString("NoXName");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "The XML element is in the reserved vCard 4.0-namespace." ähnelt.
        /// </summary>
        internal static string ReservedNameSpace => ResourceManager.GetString("ReservedNameSpace");


        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "The value must be greater than zero." ähnelt.
        /// </summary>
        internal static string ValueMustBeGreaterThanZero => ResourceManager.GetString("ValueMustBeGreaterThanZero");
    }
}
