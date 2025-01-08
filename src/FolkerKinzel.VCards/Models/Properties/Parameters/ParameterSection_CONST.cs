namespace FolkerKinzel.VCards.Models.Properties.Parameters;

public sealed partial class ParameterSection
{
    internal const int PREF_MIN_VALUE = 1;
    internal const int PREF_MAX_VALUE = 100;
    internal const string TRIM_CHARS = " \"";

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

        internal static class Rfc9554
        {
            internal const string AUTHOR = "AUTHOR";
            internal const string AUTHOR_NAME = "AUTHOR-NAME";
            internal const string CREATED = "CREATED";
            internal const string DERIVED = "DERIVED";
            internal const string PHONETIC = "PHONETIC";
            internal const string PROP_ID = "PROP-ID";
            internal const string SCRIPT = "SCRIPT";
            internal const string SERVICE_TYPE = "SERVICE-TYPE";
            internal const string USERNAME = "USERNAME";
        }

        internal static class Rfc9555
        {
            internal const string JSCOMPS = "JSCOMPS";
            internal const string JSPTR = "JSPTR";
        }

        internal static class NonStandard
        {
            internal const string X_SERVICE_TYPE = "X-SERVICE-TYPE";
        }
    }

    internal static class TypeValue
    {
        internal const string PREF = "PREF";
        internal const string WORK = "WORK";
        internal const string HOME = "HOME";

    }//class TypeValue
}
