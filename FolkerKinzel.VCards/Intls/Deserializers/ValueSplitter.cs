using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FolkerKinzel.VCards.Intls.Deserializers
{
    internal readonly struct ValueSplitter : IEnumerable<string>
    {
        private readonly string? _valueString;
        private readonly char _splitChar;
        private readonly StringSplitOptions _options;

        public ValueSplitter(string? valueString, char splitChar, StringSplitOptions options = StringSplitOptions.None)
        {
            this._valueString = valueString;
            this._splitChar = splitChar;
            this._options = options;
        }


        public IEnumerator<string> GetEnumerator()
        {
            if (_valueString is null)
            {
                yield break;
            }

            int i = 0;

            while (i <= _valueString.Length)
            {
                int splitIndex = GetNextSplitIndex(i);

                if (splitIndex == i)
                {
                    if (_options == StringSplitOptions.None)
                    {
                        yield return string.Empty;
                    }
                }
                else if (_options != StringSplitOptions.RemoveEmptyEntries || ContainsData(0, _valueString.Length))
                {
                    int length = splitIndex - i;

                    yield return length == _valueString.Length 
                                    ? _valueString
                                    : length == 0 
                                        ? string.Empty 
                                        : _valueString.Substring(i, length);
                }

                i = splitIndex + 1;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private int GetNextSplitIndex(int startIndex)
        {
            string s = _valueString!;
            bool masked = false;

            for (int i = startIndex; i < s.Length; i++)
            {
                if (masked)
                {
                    masked = false;
                    continue;
                }

                char c = s[i];

                if (c == _splitChar)
                {
                    return i;
                }

                if (c == '\\')
                {
                    masked = true;
                }

            }//for

            return s.Length;
        }

        private bool ContainsData(int startIndex, int length)
        {
            string s = _valueString!;
            for (int i = startIndex; i < length; i++)
            {
                if (char.IsWhiteSpace(s[i]))
                {
                    continue;
                }

                return true;
            }

            return false;
        }


    }
}
