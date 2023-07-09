using System.Globalization;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class AddressOrderConverter
{
    internal static AddressOrder ToAddressOrder(this CultureInfo cultureInfo)
    {
        Debug.Assert(cultureInfo != null);
        string name = cultureInfo.Name;
        int separatorIndex = name.IndexOf('-');
        if(separatorIndex == -1 || name.Length < separatorIndex + 3) { return AddressOrder.Din; }

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
                return AddressOrder.Venzuela;
            default:
                return AddressOrder.Din;
        }
    }

}
