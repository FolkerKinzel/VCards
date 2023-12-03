using System.Globalization;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class AddressOrderConverter
{
    internal static AddressOrder ToAddressOrder(this CultureInfo cultureInfo)
    {
        Debug.Assert(cultureInfo is not null);

        string name = cultureInfo.Name;
        int separatorIndex = name.IndexOf('-');

        if (separatorIndex == -1 || name.Length < separatorIndex + 3)
        {
            return AddressOrder.Din;
        }

        string countryName = name.Substring(separatorIndex + 1, 2);

        switch (countryName)
        {
            case "AU": // AUSTRALIA												
            case "BH": // BAHRAIN												
            case "BD": // BANGLADESH												
            case "BM": // BERMUDA												
            case "BT": // BHUTAN													
            case "BR": // BRAZIL													
            case "BN": // BRUNEI DARUSSALAM										
            case "KH": // CAMBODIA												
            case "CA": // CANADA													
            case "KY": // CAYMAN ISLANDS											
            case "CN": // CHINA											
            case "CX": // Christmas Island										
            case "CC": // Cocos Islands											
            case "CO": // COLOMBIA												
            case "DO": // DOMINICAN REPUBLIC										
            case "EG": // EGYPT													
            case "FK": // Falkland Islands										
            case "GU": // Guam													
            case "HM": // Heard Island and McDonald Islands						
            case "IN": // INDIA													
            case "ID": // INDONESIA												
            case "IE": // IRELAND												
            case "IM": // Isle of Man											
            case "JP": // JAPAN													
            case "JO": // JORDAN													
            case "KZ": // KAZAKHSTAN												
            case "KE": // KENYA													
            case "KR": // REPUBLIC OF KOREA										
            case "LV": // LATVIA													
            case "LS": // LESOTHO												
            case "MV": // MALDIVES (State before Locality)						
            case "MT": // MALTA													
            case "MH": // Marshall Islands										
            case "MU": // MAURITIUS												
            case "FM": // Micronesia, Federated States of						
            case "MM": // MYANMAR												
            case "NR": // REPUBLIC OF NAURU										
            case "NP": // NEPAL													
            case "NZ": // NEW ZEALAND											
            case "NG": // NIGERIA												
            case "NF": // NORFOLK ISLAND											
            case "MP": // Northern Mariana Islands								
            case "PK": // PAKISTAN												
            case "PW": // Palau													
            case "PN": // PITCAIRN												
            case "PR": // Puerto Rico											
            case "RU": // RUSSIAN FEDERATION										
            case "SH": // SAINT HELENA											
            case "SA": // SAUDI ARABIA											
            case "SG": // SINGAPORE												
            case "SO": // SOMALIA												
            case "ZA": // SOUTH AFRICA											
            case "GS": // South Georgia and the South Sandwich Islands			
            case "LK": // SRI LANKA												
            case "SZ": // SWAZILAND, ESWATINI
            case "TW": // TAIWAN													
            case "TH": // THAILAND												
            case "TC": // TURKS AND CAICOS ISLANDS								
            case "UA": // UKRAINE												
            case "GB": // UNITED KINGDOM											
            case "US": // UNITED STATES											
            case "UM": // United States Minor Outlying Islands (UNITED STATES)	
            case "UZ": // Uzbekistan												
            case "VN": // VIET NAM												
            case "VG": // Virgin Islands (U.K.)									
            case "VI": // Virgin Islands (U.S.)	
                return AddressOrder.Usa;
            case "VE": // VENEZUELA (LOCALITY POSTAL_CODE, PROVINCE)				
            case "PG": // PAPUA NEW GUINEA (LOCALITY POSTAL_CODE PROVINCE)		
                return AddressOrder.Venezuela;
            default:
                return AddressOrder.Din;
        }
    }

    internal static AddressOrder? GetAddressOrder(this Address address)
    {
        var arr = address.Country.SelectMany(x => x)
                       .Where(x => char.IsLetter(x))
                       .Select(x => char.ToUpperInvariant(x)).ToArray();

        if (arr.Length == 0)
        {
            return null;
        }

        var span = new ReadOnlySpan<char>(arr);

        return span.Equals("USA", StringComparison.Ordinal) ||
               span.StartsWith("UNITEDSTATES") ||
               span.StartsWith("AUSTRAL") ||
               span.EndsWith("ANADA") || // Canada, Kanada
               span.EndsWith("BAHRAIN") ||
               span.EndsWith("BANGLADESH") ||
               span.Equals("BERMUDA", StringComparison.Ordinal) ||
               span.Equals("BHUTAN", StringComparison.Ordinal) ||
               span.Equals("BRAZIL", StringComparison.Ordinal) ||
               span.StartsWith("BRASIL") ||
               span.StartsWith("BRUNEI") ||
               span.StartsWith("CAYMAN") || // Cayman Islands
               span.StartsWith("CHRISTMAS") ||
               span.StartsWith("COCOS") ||
               span.EndsWith("KOLUMBIEN") ||
               span.StartsWith("DOMINICAN") || // Dominican Republic
               span.StartsWith("DOMINIKAN") || // Dominikanische Republik
               span.Equals("EGYPT", StringComparison.Ordinal) ||
               span.EndsWith("INDIA") ||
               span.StartsWith("INDONESI") ||
               span.EndsWith("IRELAND") ||
               span.Equals("JAPAN", StringComparison.Ordinal) ||
               span.EndsWith("JORDAN") ||
               span.Equals("KAZAKHSTAN", StringComparison.Ordinal) ||
               span.Equals("KENYA", StringComparison.Ordinal) ||
               span.Equals("KENIA", StringComparison.Ordinal) ||
               span.EndsWith("KOREA") ||
               span.Equals("LATVIA", StringComparison.Ordinal) ||
               span.Equals("LESOTHO", StringComparison.Ordinal) ||
               span.Equals("MALDIVES", StringComparison.Ordinal) ||
               span.Equals("MALEDIVEN", StringComparison.Ordinal) ||
               span.Equals("MALTA", StringComparison.Ordinal) ||
               span.Equals("MAURITIUS", StringComparison.Ordinal) ||
               span.Equals("MYANMAR", StringComparison.Ordinal) ||
               span.EndsWith("NAURU") ||
               span.Equals("NEPAL", StringComparison.Ordinal) ||
               span.EndsWith("ZEALAND") ||
               span.EndsWith("SEELAND") ||
               span.Equals("NIGERIA", StringComparison.Ordinal) ||
               span.StartsWith("NORFOLK") ||
               span.Equals("PAKISTAN", StringComparison.Ordinal) ||
               span.Equals("PITCAIRN", StringComparison.Ordinal) ||
               span.StartsWith("RUSS") ||
               span.EndsWith("HELENA") ||
               span.StartsWith("SAUDI") ||
               span.Equals("SINGAPORE", StringComparison.Ordinal) ||
               span.Equals("SOMALIA", StringComparison.Ordinal) ||
               span.EndsWith("SOUTHAFRICA") ||
               span.StartsWith("SRI") || // Sri Lanka
               span.Equals("SWAZILAND", StringComparison.Ordinal) ||
               span.Equals("TAIWAN", StringComparison.Ordinal) ||
               span.Equals("THAILAND", StringComparison.Ordinal) ||
               span.StartsWith("TURKS") || // Turks and Caicos Islands
               span.StartsWith("UKRAIN") ||
               span.Equals("UNITEDKINGDOM", StringComparison.Ordinal) ||
               span.Equals("UZBEKISTAN", StringComparison.Ordinal) ||
               span.Contains("CHINA", StringComparison.Ordinal) ||
               span.Contains("BRIT", StringComparison.Ordinal) || // Great Britain, Großbritannien
               span.Contains("AMBOD", StringComparison.Ordinal) || // Cambodia, Kambodscha
               span.Contains("COLOMBI", StringComparison.Ordinal) || // Colombia
               span.Contains("VIET", StringComparison.Ordinal)  // Viet Nam
               ? AddressOrder.Usa
               : span.StartsWith("PAPUA") || span.EndsWith("VENEZUELA") ? AddressOrder.Venezuela
                                                                        : AddressOrder.Din;
    }
}
