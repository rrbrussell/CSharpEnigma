using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpEnigma
{
    enum Characters {A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, BAD_CHARACTER};

    static class CharactersAssistant
    {
        static Characters nextCharacter(Characters input)
        {
            int plusOne = ((int)input + 1) % Rotor.RingSize;
            if (Enum.IsDefined(typeof(Characters), (Characters)plusOne))
            {
                return (Characters)plusOne;
            } else
            {
                return Characters.BAD_CHARACTER;
            }
        }

        static Characters previousCharacter(Characters input)
        {
            int minusOne = ((int)input - 1 + Rotor.RingSize) % Rotor.RingSize;
            if (Enum.IsDefined(typeof(Characters), (Characters)minusOne))
            {
                return (Characters)minusOne;
            } else
            {
                return Characters.BAD_CHARACTER;
            }
        }

        static Characters forwardBy(Characters first, Characters second)
        {
            int newCharacter = ((int)first + (int)second) % Rotor.RingSize;
            if (Enum.IsDefined(typeof(Characters), (Characters)newCharacter))
            {
                return (Characters)newCharacter;
            } else
            {
                return Characters.BAD_CHARACTER;
            }
        }

        static Characters backwardsBy(Characters first, Characters second)
        {
            int newCharacter = ((int)first + (int)second + Rotor.RingSize) % Rotor.RingSize;
            if( Enum.IsDefined(typeof(Characters), (Characters)newCharacter))
            {
                return (Characters)newCharacter;
            } else
            {
                return Characters.BAD_CHARACTER;
            }
        }
    }
}
