using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards.Models.PropertyParts
{
    public sealed partial class ParameterSection : ICloneable
    {
        /// <inheritdoc/>
        public object Clone() => new ParameterSection(this);
    }
}
