using FolkerKinzel.VCards.Intls.Deserializers;
using System.Collections.Generic;
using System.Collections;
using System;

namespace FolkerKinzel.VCards.Intls.Deserializers
{
    internal sealed partial class VcfRow
    {
        


        // key=value;key="value,value,va;lue";key="val;ue" wird zu
        // key=value | key="value,value,va;lue" | key="val;ue"
        private readonly struct ParameterSplitter
        {
            private readonly string _parameterSection;

            public ParameterSplitter(string parameterSection) => this._parameterSection = parameterSection;

            public IEnumerator<string> GetEnumerator()
            {
                bool isInDoubleQuotes = false;
                int nextStringStartIndex = 0;

                string subString;
                int i;

                for (i = 0; i < _parameterSection.Length; i++)
                {
                    char c = _parameterSection[i];

                    if (c == '"')
                    {
                        isInDoubleQuotes = !isInDoubleQuotes;
                    }
                    else if (c == ';' && !isInDoubleQuotes)
                    {
                        subString = _parameterSection.Substring(nextStringStartIndex, i - nextStringStartIndex);

                        yield return subString;

                        nextStringStartIndex = i + 1;
                    }
                }//for

                subString = _parameterSection.Substring(nextStringStartIndex, i - nextStringStartIndex);

                yield return subString;
            }


        }











    }
}
