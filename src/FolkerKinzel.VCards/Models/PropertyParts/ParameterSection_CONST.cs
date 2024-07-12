namespace FolkerKinzel.VCards.Models.PropertyParts;

public sealed partial class ParameterSection
{
    internal const int PREF_MIN_VALUE = 1;
    internal const int PREF_MAX_VALUE = 100;

    internal const string TRIM_CHARS = " \"";

    /// <summary>
    /// Defines the default value for the <see cref="Calendar"/> property,
    /// which stands for the Gregorian calendar system.
    /// </summary>
    public const string DefaultCalendar = "gregorian";


    internal static class ParameterKey
    {
        // CAUTION: If the class is expanded,
        // ParameterSerializer.AppendNonStandardParameters() must be adjusted!

        internal const string LANGUAGE = "LANGUAGE";
        internal const string VALUE = "VALUE";
        internal const string PREF = "PREF";
        internal const string PID = "PID";
        internal const string TYPE = "TYPE";
        internal const string GEO = "GEO";
        internal const string TZ = "TZ";
        internal const string SORT_AS = "SORT-AS";
        internal const string CALSCALE = "CALSCALE";
        internal const string ENCODING = "ENCODING";
        internal const string CHARSET = "CHARSET";
        internal const string ALTID = "ALTID";
        internal const string MEDIATYPE = "MEDIATYPE";
        internal const string LABEL = "LABEL";
        internal const string CONTEXT = "CONTEXT";
        internal const string INDEX = "INDEX";
        internal const string LEVEL = "LEVEL";
        internal const string CC = "CC";
    }

    internal static class TypeValue
    {
        internal const string PREF = "PREF";
        internal const string WORK = "WORK";
        internal const string HOME = "HOME";

    }//class TypeValue
}
