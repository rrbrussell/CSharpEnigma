using System;

namespace CSharpEnigma
{
    #pragma warning disable 1591
    /// <summary>
    /// The possible alphabet for the Engima both plaintext and ciphertext.
    /// </summary>
    public enum Alphabet { A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
        /// <summary>
        /// Returned when the input or result of computation is not a valid member of the alphabet.
        /// </summary>
        BadCharacter };
    #pragma warning disable 1591

    public static class CharactersAssistant
    {
        public static Alphabet NextCharacter(Alphabet input)
        {
            int plusOne = ((int)input + 1) % Rotor.RingSize;
            if (Enum.IsDefined(typeof(Alphabet), (Alphabet)plusOne))
            {
                return (Alphabet)plusOne;
            } else
            {
                return Alphabet.BadCharacter;
            }
        }

        public static Alphabet PreviousCharacter(Alphabet input)
        {
            int minusOne = ((int)input - 1 + Rotor.RingSize) % Rotor.RingSize;
            if (Enum.IsDefined(typeof(Alphabet), (Alphabet)minusOne))
            {
                return (Alphabet)minusOne;
            } else
            {
                return Alphabet.BadCharacter;
            }
        }
        
        /// <summary>
        /// Goes forward in the alphabet from First by Second's place in the alphabet.
        /// </summary>
        /// <remarks>
        /// This is used almost entirely for handling the offsets of the lookup keys by the current rotation of the indicator
        /// and the rotor wiring offset aka Ringstellung.
        /// </remarks>
        /// <param name="first">Where you are now.</param>
        /// <param name="second">How many Alphabet to go forward.</param>
        /// <returns></returns>
        public static Alphabet ForwardsBy(Alphabet first, Alphabet second)
        {
            int newCharacter = ((int)first + (int)second) % Rotor.RingSize;
            if (Enum.IsDefined(typeof(Alphabet), (Alphabet)newCharacter))
            {
                return (Alphabet)newCharacter;
            } else
            {
                return Alphabet.BadCharacter;
            }
        }

        public static Alphabet BackwardsBy(Alphabet first, Alphabet second)
        {
            int newCharacter = ((int)first + (int)second + Rotor.RingSize) % Rotor.RingSize;
            if( Enum.IsDefined(typeof(Alphabet), (Alphabet)newCharacter))
            {
                return (Alphabet)newCharacter;
            } else
            {
                return Alphabet.BadCharacter;
            }
        }
    }
}
