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
            BETA,
            /// <summary>
            /// The M4 Thin Rotor Gamma
            /// </summary>
            GAMMA,
            /// <summary>
            /// A bad rotor.
            /// </summary>
            BAD_ROTOR };

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
        private readonly static Characters[,] RotorTurnoverList = new Characters[,]{{Characters.Q, Characters.BAD_CHARACTER },// Rotor I
        {Characters.E, Characters.BAD_CHARACTER },// Rotor II
        {Characters.V, Characters.BAD_CHARACTER },// Rotor III
        {Characters.J, Characters.BAD_CHARACTER },// Rotor IV
        {Characters.Z, Characters.BAD_CHARACTER },// Rotor V
        {Characters.Z, Characters.M }, // Rotor VI
        {Characters.Z, Characters.M }, // Rotor VII
        {Characters.Z, Characters.M }, // Rotor VIII
        {Characters.BAD_CHARACTER, Characters.BAD_CHARACTER }, // Rotor Beta
        {Characters.BAD_CHARACTER, Characters.BAD_CHARACTER }, }; // Rotor Gamma

        private Characters[] indicatorTransferPositon = null;

        /// <summary>
        /// The internal lookup table used to encipher when going right to left.
        /// </summary>
        private Dictionary<Characters, Characters> rightToLeftMapping;
        
        /// <summary>
        /// The internal lookup table used to encipher when going left to right.
        /// </summary>
        private Dictionary<Characters, Characters> leftToRightMapping;

        /// <summary>
        /// The offset between the indicator ring and the rotor wiring.
        /// </summary>
        public Characters Offset { get; }

        /// <summary>
        /// Which position on the indicator ring is currently visible.
        /// </summary>
        public Characters Indicator { get; set; } = Characters.A;

        /// <summary>
        /// Quick constructor for a Rotor. Useful when you aren't changing the offset of the wiring.
        /// </summary>
        /// <param name="chosenRotor">Which Rotor from the Rotors enumeration you want.</param>
        /// <seealso cref="Rotor(Rotors, Characters)">
        /// This constructor has the same constraints and exceptions as the full constructor for Rotor.
        /// </seealso>
        public Rotor(Rotors chosenRotor)
            : this(chosenRotor, Characters.A)
        {
        }

        /// <summary>
        /// Full constructor for a Rotor.
        /// </summary>
        /// <param name="chosenRotor">Which Rotor from the Rotors enumeration you want.</param>
        /// <param name="ringOffset">Position A on the wiring map matches this position on the indicator ring.</param>
        /// <exception cref="InvalidRotorChoiceException"/>
        /// <exception cref="InvalidCharacterException"/>
        public Rotor(Rotors chosenRotor, Characters ringOffset)
        {
            if (Enum.IsDefined(typeof(Rotors), chosenRotor) && chosenRotor != Rotors.BAD_ROTOR)
            {
                // Create the Dictionaries
                rightToLeftMapping = new Dictionary<Characters, Characters>(RingSize);
                leftToRightMapping = new Dictionary<Characters, Characters>(RingSize);

                // The Right to Left direction is represented directly by the string
                // from RotorString. So I can just directly map it.
                // This is a lot cleaner in the Java version due to Java's much superior
                // handling of Enumerations.
                for (int index = 0; index < RotorStrings[(int) chosenRotor].Length; index++)
                {
                    rightToLeftMapping.Add((Characters)index,
                        (Characters) Enum.Parse(typeof(Characters), RotorStrings[(int) chosenRotor].Substring(index, 1)));
                }

                // The Left to Right direction is built by inverting the Key to Value relationship
                // in the Right to Left dictionary.
                foreach (KeyValuePair<Characters,Characters> kvp in rightToLeftMapping)
                {
                    leftToRightMapping.Add(kvp.Value, kvp.Key);
                }
            } else
            {
                throw new InvalidRotorChoiceException("Sorry, The chosen rotor must be Rotors.I and Rotors.GAMMA.");
            }
            if(Enum.IsDefined(typeof(Characters), ringOffset) && ringOffset != Characters.BAD_CHARACTER)
            {
                this.Offset = ringOffset;
            } else
            {
                throw new InvalidCharacterException("Sorry, the ring offset must be between Characters.A and Characters.Z.");
            }
        }

        /// <summary>
        /// Steps a rotor.
        /// </summary>
        /// <returns>True or False if the rotor will also step the next rotor.</returns>
        virtual public bool step()
        {
            bool returnValue = (Indicator == indicatorTransferPositon[0]) || (Indicator == indicatorTransferPositon[1]);
            Indicator = CharactersAssistant.nextCharacter(Indicator);
            return returnValue;
        }

        /// <summary>
        /// Enciphers the plaintext. This is used before the reflector.
        /// Currently not implemented.
        /// </summary>
        /// <param name="plaintext">The character to be enciphered.</param>
        /// <returns>The Ciphertext.</returns>
        public Characters encipherRightToLeft(Characters plaintext)
        {
            return encipher(plaintext, rightToLeftMapping);
        }

        /// <summary>
        /// Enciphers the plaintext. This is used after the reflector.
        /// Currently not implemented.
        /// </summary>
        /// <param name="plaintext">The character to be enciphered.</param>
        /// <returns>The Ciphertext.</returns>
        public Characters encipherLeftToRight(Characters plaintext)
        {
            return encipher(plaintext, leftToRightMapping);
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
        private Characters encipher(Characters plaintext, Dictionary<Characters,Characters> direction)
        {
            throw new NotImplementedException();
        }
    }
}
