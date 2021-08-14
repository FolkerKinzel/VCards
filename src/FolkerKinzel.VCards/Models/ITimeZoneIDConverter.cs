using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Models
{
    public interface ITimeZoneIDConverter
    {
        bool TryGetUtcOffset(string timeZoneID, out TimeSpan utcOffset);
    }
}
