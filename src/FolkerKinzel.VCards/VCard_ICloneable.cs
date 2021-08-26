using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards
{
    public sealed partial class VCard : ICloneable
    {
        /// <inheritdoc/>
        public object Clone()
            => new VCard(this);
    }
}
