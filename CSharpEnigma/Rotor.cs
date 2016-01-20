using System;
using System.Collections.Generic;

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
        private readonly static Alphabet[,] RotorTurnoverList = new Alphabet[,]{{Alphabet.Q, Alphabet.BadCharacter },// Rotor I
        {Alphabet.E, Alphabet.BadCharacter },// Rotor II
        {Alphabet.V, Alphabet.BadCharacter },// Rotor III
        {Alphabet.J, Alphabet.BadCharacter },// Rotor IV
        {Alphabet.Z, Alphabet.BadCharacter },// Rotor V
        {Alphabet.Z, Alphabet.M }, // Rotor VI
        {Alphabet.Z, Alphabet.M }, // Rotor VII
        {Alphabet.Z, Alphabet.M }, // Rotor VIII
        {Alphabet.BadCharacter, Alphabet.BadCharacter }, // Rotor Beta
        {Alphabet.BadCharacter, Alphabet.BadCharacter }, }; // Rotor Gamma

        private Alphabet[] indicatorTransferPositon = null;

        /// <summary>
        /// The internal lookup table used to Encipher when going right to left.
        /// </summary>
        private Dictionary<Alphabet, Alphabet> rightToLeftMapping;
        
        /// <summary>
        /// The internal lookup table used to Encipher when going left to right.
        /// </summary>
        private Dictionary<Alphabet, Alphabet> leftToRightMapping;

        /// <summary>
        /// The offset between the indicator ring and the rotor wiring.
        /// </summary>
        public Alphabet Offset { get; }

        /// <summary>
        /// Which position on the indicator ring is currently visible.
        /// </summary>
        public Alphabet Indicator { get; set; } = Alphabet.A;

        /// <summary>
        /// Quick constructor for a Rotor. Useful when you aren't changing the offset of the wiring.
        /// </summary>
        /// <param name="chosenRotor">Which Rotor from the Rotors enumeration you want.</param>
        /// <seealso cref="Rotor(Rotors, Alphabet)">
        /// This constructor has the same constraints and exceptions as the full constructor for Rotor.
        /// </seealso>
        public Rotor(Rotors chosenRotor)
            : this(chosenRotor, Alphabet.A)
        {
        }

        /// <summary>
        /// Full constructor for a Rotor.
        /// </summary>
        /// <param name="chosenRotor">Which Rotor from the Rotors enumeration you want.</param>
        /// <param name="ringOffset">Position A on the wiring map matches this position on the indicator ring.</param>
        /// <exception cref="InvalidRotorChoiceException"/>
        /// <exception cref="InvalidLetterException"/>
        public Rotor(Rotors chosenRotor, Alphabet ringOffset)
        {
            if (Enum.IsDefined(typeof(Rotors), chosenRotor) && chosenRotor != Rotors.BadRotor)
            {
                // Create the Dictionaries
                rightToLeftMapping = new Dictionary<Alphabet, Alphabet>(RingSize);
                leftToRightMapping = new Dictionary<Alphabet, Alphabet>(RingSize);

                // The Right to Left direction is represented directly by the string
                // from RotorString. So I can just directly map it.
                // This is a lot cleaner in the Java version due to Java's much superior
                // handling of Enumerations.
                for (int index = 0; index < RotorStrings[(int)chosenRotor].Length; index++)
                {
                    rightToLeftMapping.Add((Alphabet)index,
                        (Alphabet)Enum.Parse(typeof(Alphabet), RotorStrings[(int)chosenRotor].Substring(index, 1)));
                }

                // The Left to Right direction is built by inverting the Key to Value relationship
                // in the Right to Left dictionary.
                foreach (KeyValuePair<Alphabet, Alphabet> kvp in rightToLeftMapping)
                {
                    leftToRightMapping.Add(kvp.Value, kvp.Key);
                }

                indicatorTransferPositon = new Alphabet[2];
                indicatorTransferPositon[0] = RotorTurnoverList[(int)chosenRotor, 0];
                indicatorTransferPositon[1] = RotorTurnoverList[(int)chosenRotor, 1];
            } else
            {
                throw new InvalidRotorChoiceException("Sorry, The chosen rotor must be Rotors.I and Rotors.Gamma.");
            }
            if(Enum.IsDefined(typeof(Alphabet), ringOffset) && ringOffset != Alphabet.BadCharacter)
            {
                this.Offset = ringOffset;
            } else
            {
                throw new InvalidLetterException("Sorry, the ring offset must be between Alphabet.A and Alphabet.Z.");
            }
        }

        /// <summary>
        /// Steps a rotor.
        /// </summary>
        /// <returns>True or False if the rotor will also Step the next rotor.</returns>
        public bool Step()
        {
            bool returnValue = (Indicator == indicatorTransferPositon[0]) || (Indicator == indicatorTransferPositon[1]);
            Indicator = CharactersAssistant.NextCharacter(Indicator);
            return returnValue;
        }

        /// <summary>
        /// Enciphers the plaintext. This is used before the reflector.
        /// Currently not implemented.
        /// </summary>
        /// <param name="plaintext">The character to be enciphered.</param>
        /// <returns>The Ciphertext.</returns>
        /// <see cref="Encipher(Alphabet, Dictionary{Alphabet, Alphabet})"/>
        public Alphabet EncipherRightToLeft(Alphabet plaintext)
        {
            return Encipher(plaintext, rightToLeftMapping);
        }

        /// <summary>
        /// Enciphers the plaintext. This is used after the reflector.
        /// Currently not implemented.
        /// </summary>
        /// <param name="plaintext">The character to be enciphered.</param>
        /// <returns>The Ciphertext.</returns>
        /// <see cref="Encipher(Alphabet, Dictionary{Alphabet, Alphabet})"/>
        public Alphabet EncipherLeftToRight(Alphabet plaintext)
        {
            return Encipher(plaintext, leftToRightMapping);
        }

        /// <summary>
        /// Handles the internal implementation of the encipherment. Implementation is not currently complete.
        /// </summary>
        /// <remarks>
        /// The manipulation of the plaintext is required to correctly compensate for the change in relative position between the A index on the input and its current
        /// position in our representation of the rotor's wiring table. If I were to change implementations and shift the entire contents of the rotor mapings, then
        /// those manipulations would not be needed.
        /// 
        /// Implementation is not currently complete.
        /// </remarks>
        /// <param name="plaintext">The plaintext.</param>
        /// <param name="direction">The Wiring Map for the direction of encipherment.</param>
        /// <returns>The Ciphertext.</returns>
        private Alphabet Encipher(Alphabet plaintext, Dictionary<Alphabet,Alphabet> direction)
        {
            throw new NotImplementedException();
        }
    }
}
