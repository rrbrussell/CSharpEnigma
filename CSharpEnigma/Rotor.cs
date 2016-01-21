using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpEnigma
{
    /// <summary>
    /// Representation for the enciphering rotor from the Enigma Machine
    /// </summary>
    public class Rotor
    {
        /// <summary>
        /// Number of positions on a Rotor.
        /// </summary>
        public const int RingSize = 26;

        /// <summary>
        /// The available rotors for the library.
        /// </summary>
        public enum Rotors {
            /// <summary>
            /// The M3 Rotor I
            /// </summary>
            I,
            /// <summary>
            /// The M3 Rotor II
            /// </summary>
            II,
            /// <summary>
            /// The M3 Rotor III
            /// </summary>
            III,
            /// <summary>
            /// The M3 Rotor IV
            /// </summary>
            IV,
            /// <summary>
            /// The M3 Rotor V
            /// </summary>
            V,
            /// <summary>
            /// The M3 and M4 Rotor VI
            /// </summary>
            VI,
            /// <summary>
            /// The M3 and M4 Rotor VII
            /// </summary>
            VII,
            /// <summary>
            /// The M3 and M4 Rotor VII
            /// </summary>
            VIII,
            /// <summary>
            /// The M4 Thin Rotor Beta
            /// </summary>
            Beta,
            /// <summary>
            /// The M4 Thin Rotor Gamma
            /// </summary>
            Gamma,
            /// <summary>
            /// A bad rotor.
            /// </summary>
            BadRotor };

        /// <summary>
        /// The mappings between the right and left side of the rotors assuming no offseting
        /// between position A on the indicator ring and position A on the wiring.
        /// </summary>
        private readonly static string[] RotorStrings = { "EKMFLGDQVZNTOWYHXUSPAIBRCJ",
            "AJDKSIRUXBLHWTMCQGZNPYFVOE", "BDFHJLCPRTXVZNYEIWGAKMUSQO",
            "ESOVPZJAYQUIRHXLNFTGKDCMWB", "VZBRGITYUPSDNHLXAWMJQOFECK",
            "JPGVOUMFYQBENHZRDKASXLICTW", "NZJHGRCXMYSWBOUFAIVLPEKQDT",
            "FKQHTLXOCBJSPDZRAMEWNIUYGV", "LEYJVCNIXWPBQMDRTAKZGFUHOS",
            "FSOKANUERHMBTIYCWLQPZXVGJD" };
        /// <summary>
        /// Lookup Table for the stepping transfer points for the rotors.
        /// </summary>
        private readonly static int[,] RotorTurnoverList = new int[,]{{(int)EnigmaAlphabet.Q, (int)EnigmaAlphabet.BadCharacter },// Rotor I
        {(int)EnigmaAlphabet.E, (int)EnigmaAlphabet.BadCharacter },// Rotor II
        {(int)EnigmaAlphabet.V, (int)EnigmaAlphabet.BadCharacter },// Rotor III
        {(int)EnigmaAlphabet.J, (int)EnigmaAlphabet.BadCharacter },// Rotor IV
        {(int)EnigmaAlphabet.Z, (int)EnigmaAlphabet.BadCharacter },// Rotor V
        {(int)EnigmaAlphabet.Z, (int)EnigmaAlphabet.M }, // Rotor VI
        {(int)EnigmaAlphabet.Z, (int)EnigmaAlphabet.M }, // Rotor VII
        {(int)EnigmaAlphabet.Z, (int)EnigmaAlphabet.M }, // Rotor VIII
        {(int)EnigmaAlphabet.BadCharacter, (int)EnigmaAlphabet.BadCharacter }, // Rotor Beta
        {(int)EnigmaAlphabet.BadCharacter, (int)EnigmaAlphabet.BadCharacter }, }; // Rotor Gamma

        private int[] indicatorTransferPositon = null;

        /// <summary>
        /// The internal lookup table used to Encipher when going right to left.
        /// </summary>
        private LinkedList<int> RightToLeftMapping;

        /// <summary>
        /// The internal lookup table used to Encipher when going left to right.
        /// </summary>
        private LinkedList<int> LeftToRightMapping;

        /// <summary>
        /// The offset between the indicator ring and the rotor wiring.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// Which position on the indicator ring is currently visible.<br/>
        /// All math is done % Rotor.RingSize and negative values are ingnored.
        /// </summary>
        public int Indicator
        {
            get
            {
                return _Indicator;
            }

            set
            {
                if (value >= 0)
                {
                    var tvalue = value;
                    if (tvalue >= Rotor.RingSize)
                    {
                        tvalue = tvalue % Rotor.RingSize;
                    }
                    if (tvalue < _Indicator)
                    {
                        Shift((value + Rotor.RingSize) - _Indicator);
                    }
                    else
                    {
                        Shift(value - _Indicator);
                    }
                    _Indicator = tvalue;
                } else
                {
                    // do nothing
                }
            }
        }

        private int _Indicator = 0;

        /// <summary>
        /// Quick constructor for a Rotor. Useful when you aren't changing the offset of the wiring.
        /// </summary>
        /// <param name="chosenRotor">Which Rotor from the Rotors enumeration you want.</param>
        /// <seealso cref="Rotor(Rotors, int)">
        /// This constructor has the same constraints and exceptions as the full constructor for Rotor.
        /// </seealso>
        public Rotor(Rotors chosenRotor)
            : this(chosenRotor, 0)
        {
        }

        /// <summary>
        /// Full constructor for a Rotor.
        /// </summary>
        /// <param name="chosenRotor">Which Rotor from the Rotors enumeration you want.</param>
        /// <param name="ringOffset">Position A on the wiring map matches this position on the indicator ring.</param>
        /// <exception cref="InvalidRotorChoiceException"/>
        /// <exception cref="InvalidLetterException"/>
        public Rotor(Rotors chosenRotor, int ringOffset)
        {
            RightToLeftMapping = new LinkedList<int>();
            LeftToRightMapping = new LinkedList<int>();

            // Bail early and hard if conditions aren't perfect.
            if (!Enum.IsDefined(typeof(Rotors), chosenRotor) || chosenRotor == Rotors.BadRotor)
            {
                throw new InvalidRotorChoiceException("Sorry, The chosen rotor must be Rotors.I and Rotors.Gamma.");
            }
            if (ringOffset < 0 || ringOffset > 25)
            {
                throw new InvalidLetterException("Sorry, the ring offset must be between 0 and 25.");
            }

            // Create the Dictionaries
            SortedDictionary<EnigmaAlphabet, EnigmaAlphabet> rightToLeftMappingDictionary = new SortedDictionary<EnigmaAlphabet, EnigmaAlphabet>();
            SortedDictionary<EnigmaAlphabet, EnigmaAlphabet> leftToRightMappingDictionary = new SortedDictionary<EnigmaAlphabet, EnigmaAlphabet>();

            // The Right to Left direction is represented directly by the string
            // from RotorStrings. So I can just directly map it.
            // This is a lot cleaner in the Java version due to Java's much superior
            // handling of Enumerations.
            for (int index = 0; index < RotorStrings[(int)chosenRotor].Length; index++)
            {
                rightToLeftMappingDictionary.Add((EnigmaAlphabet)index,
                    (EnigmaAlphabet)Enum.Parse(typeof(EnigmaAlphabet), RotorStrings[(int)chosenRotor].Substring(index, 1)));
            }

            // The Left to Right direction is built by inverting the Key to Value relationship
            // in the Right to Left dictionary.
            foreach (KeyValuePair<EnigmaAlphabet, EnigmaAlphabet> kvp in rightToLeftMappingDictionary)
            {
                leftToRightMappingDictionary.Add(kvp.Value, kvp.Key);
            }

            // now that I have created a sorted key to value mapping convert those mappings into lists.
            RightToLeftMapping = new LinkedList<int>(rightToLeftMappingDictionary.Values.Cast<int>());
            LeftToRightMapping = new LinkedList<int>(leftToRightMappingDictionary.Values.Cast<int>());

            indicatorTransferPositon = new int[2];
            indicatorTransferPositon[0] = RotorTurnoverList[(int)chosenRotor, 0];
            indicatorTransferPositon[1] = RotorTurnoverList[(int)chosenRotor, 1];

            Offset = ringOffset;
            // Shift the rotor's wiring forward until the correct offset between the indicator and the rotor's wiring is achieved
            if (ringOffset != 0 )
            {
                Shift(ringOffset);
            }
            _Indicator = 0;
        }

        private void Shift(int howManyTimes)
        {
            if(howManyTimes > 0)
            {
                for (int i = howManyTimes; i > 0; i--)
                {
                    var temp = RightToLeftMapping.First;
                    RightToLeftMapping.AddLast(temp);
                    temp = LeftToRightMapping.First;
                    LeftToRightMapping.AddLast(temp);
                }
            }
        }

        /// <summary>
        /// Steps a rotor.
        /// </summary>
        /// <returns>True or False if the rotor will also Step the next rotor.</returns>
        public bool Step()
        {
            bool returnValue = (Indicator == indicatorTransferPositon[0]) || (Indicator == indicatorTransferPositon[1]);
            Indicator += 1;
            return returnValue;
        }

        /// <summary>
        /// Enciphers the plaintext. This is used before the reflector.
        /// </summary>
        /// <param name="plaintext">The character to be enciphered.</param>
        /// <returns>The Ciphertext.</returns>
        /// <see cref="Encipher(int, LinkedList{int})"/>
        public int EncipherRightToLeft(int plaintext)
        {
            return Encipher(plaintext, RightToLeftMapping);
        }

        /// <summary>
        /// Enciphers the plaintext. This is used after the reflector.
        /// Currently not implemented.
        /// </summary>
        /// <param name="plaintext">The character to be enciphered.</param>
        /// <returns>The Ciphertext.</returns>
        /// <see cref="Encipher(int, LinkedList{int})"/>
        public int EncipherLeftToRight(int plaintext)
        {
            return Encipher(plaintext, LeftToRightMapping);
        }

        /// <summary>
        /// Handles the internal implementation of the encipherment.
        /// </summary>
        /// <remarks>
        /// The manipulation of the plaintext is required to correctly compensate for the change in relative position between the A index on the input and its current
        /// position in our representation of the rotor's wiring table. If I were to change implementations and shift the entire contents of the rotor mapings, then
        /// those manipulations would not be needed.
        /// </remarks>
        /// <param name="plaintext">The plaintext.</param>
        /// <param name="direction">The Wiring Map for the direction of encipherment.</param>
        /// <returns>The Ciphertext.</returns>
        private int Encipher(int plaintext, LinkedList<int> direction)
        {
            return direction.ElementAt(plaintext);
            throw new NotImplementedException();
        }
    }
}
