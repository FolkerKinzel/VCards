namespace FolkerKinzel.VCards.Intls;

internal static class DebugWriter
{
    [Conditional("DEBUG")]
    [ExcludeFromCodeCoverage]
    [DebuggerStepThrough]
    internal static void WriteMethodHeader(string? MethodName)
    {
        const int HEADER_LENGTH = 40;

        Debug.WriteLine("");
        Debug.WriteLine(new string('/', HEADER_LENGTH));
        Debug.WriteLine("");
        Debug.WriteLine("", "     " + MethodName);
        Debug.WriteLine("");
        Debug.WriteLine(new string('/', HEADER_LENGTH));
    }
}
