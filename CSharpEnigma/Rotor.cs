using System;
using System.Collections.Generic;

namespace CSharpEnigma
{
    /// <summary>
    /// Representation for the enciphering rotor from the Enigma Machine
    /// </summary>
    class Rotor
    {
        /// <summary>
        /// Number of positions on a Rotor.
        /// </summary>
        public const int RingSize = 26;

        /// <summary>
        /// The available Rotors in this implementation including an error value.
        /// </summary>
        public enum Rotors { I, II, III, IV, VI, VII, VIII, BETA, GAMMA,
            BAD_ROTOR };

        /// <summary>
        /// The mappings between the right and left side of the rotors assuming no offseting
        /// between position A on the indicator ring and position A on the wiring.
        /// </summary>
        private readonly string[] RotorStrings = { "EKMFLGDQVZNTOWYHXUSPAIBRCJ",
            "AJDKSIRUXBLHWTMCQGZNPYFVOE", "BDFHJLCPRTXVZNYEIWGAKMUSQO",
            "ESOVPZJAYQUIRHXLNFTGKDCMWB", "VZBRGITYUPSDNHLXAWMJQOFECK",
            "JPGVOUMFYQBENHZRDKASXLICTW", "NZJHGRCXMYSWBOUFAIVLPEKQDT",
            "FKQHTLXOCBJSPDZRAMEWNIUYGV", "LEYJVCNIXWPBQMDRTAKZGFUHOS",
            "FSOKANUERHMBTIYCWLQPZXVGJD" };
        
        /// <summary>
        /// The internal lookup table used to encipher when going right to left.
        /// </summary>
        private Dictionary<Characters, Characters> rightToLeftMapping;
        
        /// <summary>
        /// The internal lookup table used to encipher when going left to right.
        /// </summary>
        private Dictionary<Characters, Characters> leftToRightMapping;
        
        /// <summary>
        /// Where position A on the wiring maps to on the indicator ring.
        /// Defaults to A.
        /// </summary>
        private Characters currentOffset = Characters.A;

        /// <summary>
        /// Property for easy read access of the offset between the indicator ring and the rotor wiring.
        /// </summary>
        public Characters Offset
        {
            get { return currentOffset; }
        }


        /// <summary>
        /// Which position on the indicator ring is currently visible.
        /// </summary>
        private Characters currentIndicator;

        /// <summary>
        /// Which position on the indicator ring is currently visible.
        /// </summary>
        public Characters Indicator
        {
            get { return currentIndicator; }
        }

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
                currentOffset = ringOffset;
            } else
            {
                throw new InvalidCharacterException("Sorry, the ring offset must be between Characters.A and Characters.Z.");
            }
        }


    }
}
