using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpEnigma
{
    /// <summary>
    /// The class that handles the conversion of the alphabet that the Enigma uses.
    /// </summary>
    public class AlphabetHandler
    {
        /// <summary>
        /// The universal value for an unrepresentable item in the Enigma's Alphabet.
        /// </summary>
        public const int BadCharacter = 26;

        /// <summary>
        /// Converts a string to an int array of numeric codes ready to be processed by the Enigma Machine.
        /// </summary>
        /// <param name="incomingText">Will be striped of a all non interpretable characters.</param>
        /// <returns>A list of integers that represent the incoming text.</returns>
        public static List<int> ConvertToAlphabet(string incomingText)
        {
            EnigmaAlphabet temp;
            List<int> returnValue = new List<int>(incomingText.Length);
            foreach (var item in incomingText)
            {
                if (Enum.TryParse<EnigmaAlphabet>(item.ToString(), out temp))
                {
                    returnValue.Add((int) temp);
                }
            }
            returnValue.TrimExcess();
            return returnValue;
        }

        /// <summary>
        /// Converts a character to a numeric code ready to be processed by the Enigma Machine.
        /// </summary>
        /// <param name="incomingText"></param>
        /// <returns>The integer representation of the incoming character or AlphabetHandler.BadCharacter</returns>
        public static int ConvertToAlphabet(char incomingText)
        {
            var tempText = AlphabetHandler.BadCharacter;
            if (TryConvertToAlphabet(incomingText, out tempText))
            {
                return tempText;
            } else
            {
                return AlphabetHandler.BadCharacter;
            }
        }

        /// <summary>
        /// Attempts to parse the incoming character and places the value into enigmaText.
        /// </summary>
        /// <param name="incomingText">The input.</param>
        /// <param name="enigmaText">The Output</param>
        /// <returns>True if the input is a valid member of the EnigmaAlphabet and false otherwise.</returns>
        public static bool TryConvertToAlphabet(char incomingText, out int enigmaText)
        {
            var tempText = EnigmaAlphabet.BadCharacter;
            if (Enum.TryParse<EnigmaAlphabet>(incomingText.ToString(), out tempText))
            {
                enigmaText = (int)tempText;
                return true;
            } else
            {
                enigmaText = AlphabetHandler.BadCharacter;
                return false;
            }
        }
    }
}
