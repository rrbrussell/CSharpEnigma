using System;

namespace CSharpEnigma
{
    
    ///<summary>
    ///The valid Alphabet for the Enigma machines.
    ///</summary>
    enum EnigmaAlphabet
    {
        /// <summary>
        /// The first letter of the EnigmaAlphabet
        /// </summary>
        A,
        /// <summary>
        /// The second letter of the EngimaAlphabet
        /// </summary>
        B,
        /// <summary>
        /// The third letter of the EngimaAlphabet
        /// </summary>
        C,
        /// <summary>
        /// The fourth letter of the EngimaAlphabet
        /// </summary>
        D,
        /// <summary>
        /// The fifth letter of the EngimaAlphabet
        /// </summary>
        E,
        /// <summary>
        /// The sixth letter of the EngimaAlphabet
        /// </summary>
        F,
        /// <summary>
        /// The seventh letter of the EngimaAlphabet
        /// </summary>
        G,
        /// <summary>
        /// The eighth letter of the EngimaAlphabet
        /// </summary>
        H,
        /// <summary>
        /// The ninth letter of the EngimaAlphabet
        /// </summary>
        I,
        /// <summary>
        /// The tenth letter of the EngimaAlphabet
        /// </summary>
        J,
        /// <summary>
        /// The eleventh letter of the EngimaAlphabet
        /// </summary>
        K,
        /// <summary>
        /// The twelveth letter of the EngimaAlphabet
        /// </summary>
        L,
        /// <summary>
        /// The thirteenth letter of the EngimaAlphabet
        /// </summary>
        M,
        /// <summary>
        /// The fourteenth letter of the EngimaAlphabet
        /// </summary>
        N,
        /// <summary>
        /// The fifteenth letter of the EngimaAlphabet
        /// </summary>
        O,
        /// <summary>
        /// The sixteenth letter of the EngimaAlphabet
        /// </summary>
        P,
        /// <summary>
        /// The seventeenth letter of the EngimaAlphabet
        /// </summary>
        Q,
        /// <summary>
        /// The eighteenth letter of the EngimaAlphabet
        /// </summary>
        R,
        /// <summary>
        /// The nineteenth letter of the EngimaAlphabet
        /// </summary>
        S,
        /// <summary>
        /// The twentieth letter of the EngimaAlphabet
        /// </summary>
        T,
        /// <summary>
        /// The twenty-first letter of the EngimaAlphabet
        /// </summary>
        U,
        /// <summary>
        /// The twenty-second letter of the EngimaAlphabet
        /// </summary>
        V,
        /// <summary>
        /// The twenty-third letter of the EngimaAlphabet
        /// </summary>
        W,
        /// <summary>
        /// The twenty-fourth letter of the EngimaAlphabet
        /// </summary>
        X,
        /// <summary>
        /// The twenty-fifth letter of the EngimaAlphabet
        /// </summary>
        Y,
        /// <summary>
        /// The twenty-sixth letter of the EngimaAlphabet
        /// </summary>
        Z,
        /// <summary>
        /// Returned when the input or result of computation is not a valid member of the alphabet.
        /// </summary>
        BadCharacter
    };

    /* the following will be kept in the source code because I may or may not need it. If I finish implementing the m3 and haven't used it
    then I will delete it.
    */
    /* enum Alphabet { A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
         /// <summary>
         /// Returned when the input or result of computation is not a valid member of the alphabet.
         /// </summary>
         BadCharacter };*/
    /*
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
    }*/
}
