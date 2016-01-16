using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpEnigma
{
    [Serializable]
    class InvalidRotorChoiceException : Exception
    {
        public InvalidRotorChoiceException(string message) : base(message) { }
    }

    [Serializable]
    class InvalidCharacterException : Exception
    {
        public InvalidCharacterException(string message) : base(message) { }
    }
}
