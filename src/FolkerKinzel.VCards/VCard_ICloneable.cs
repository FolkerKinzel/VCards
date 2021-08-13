using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkerKinzel.VCards
{
    public sealed partial class VCard : ICloneable
    {
        object ICloneable.Clone() => Clone();

        public VCard Clone() => new VCard(this);
    }
}
