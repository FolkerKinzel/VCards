using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.Syncs;

/// <summary> 
/// Identifies a vCard client globally, and locally inside the <see cref="VCard"/>.
/// </summary>
/// <seealso cref="AppIDProperty"/>
/// <seealso cref="VCard.AppIDs"/>
public sealed class AppID
{
    /// <summary>Initializes a new <see cref="AppID" /> object.</summary>
    /// <param name="localID">Local ID that identifies the <see cref="AppID"/>
    /// in the <see cref="ParameterSection.PropertyIDs"/>. (A positive <see cref="int"/>, not zero.)</param>
    /// <param name="globalID">A URI that identifies the <see cref="AppID"/> globally.</param>
    internal AppID(int localID, string globalID)
    {
        Debug.Assert(localID.ValidateID());
        Debug.Assert(!string.IsNullOrWhiteSpace(globalID));

        LocalID = localID;
        GlobalID = globalID; 
    }

    /// <summary>Gets the Local ID.</summary>
    public int LocalID
    {
        get;
    }

    /// <summary>Gets the URI that serves as global identifier.</summary>
    public string GlobalID
    {
        get;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var sb = new StringBuilder(48);
        AppendTo(sb);
        return sb.ToString();
    }

    /// <summary>Parses a <see cref="string" /> that represents a vCard&#160;4.0 Property-ID Mapping. </summary>
    /// <param name="s">The <see cref="string"/> to parse.</param>
    /// <param name="client">The parsed <see cref="AppID"/> if the method returns <c>true</c>,
    /// otherwise <c>null</c>.</param>
    /// <returns><c>true</c> if <see cref="s"/> could be parsed, otherwise <c>false</c>.</returns>
    internal static bool TryParse(string s, [NotNullWhen(true)] out AppID? client)
    {
        Debug.Assert(s != null);
        client = null;

        var span = s.AsSpan();
        int separatorIdx = span.IndexOf(';');

        if (separatorIdx < 1)
        {
            return false;
        }

        if (!_Int.TryParse(span.Slice(0, separatorIdx), out int localID))
        {
            return false;
        }

        string globalID = span.Slice(separatorIdx + 1).Trim().ToString();

        if(Validate(localID, globalID))
        {
            client = new AppID(localID, globalID);
            return true;
        }
        
        return false;

        static bool Validate(int localID, string globalID) => 
            localID.ValidateID() && !string.IsNullOrWhiteSpace(globalID);
    }


    internal void AppendTo(StringBuilder builder)
    {
        Debug.Assert(builder != null);
        _ = builder.Append(LocalID).Append(';').Append(GlobalID);
    }
}
