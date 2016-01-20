using System;
using System.Runtime.Serialization;

namespace CSharpEnigma
{
    /// <summary>
    /// This exception will be thrown when returning an error in Choice of Rotors is not an option.
    /// </summary>
    [Serializable]
    public class InvalidRotorChoiceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidRotorChoiceException class with the default error message.
        /// </summary>
        public InvalidRotorChoiceException() : base("The rotor you chose does not exist.") { }

        /// <summary>
        /// Initializes a new instance of the InvalidRotorChoiceException class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidRotorChoiceException(string message) : base(message)
        { }

        /// <summary>
        /// Passes message and innerException through
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InvalidRotorChoiceException(string message, Exception innerException) : base(message, innerException)
        { }

        /// <summary>
        /// Passes info and context through
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InvalidRotorChoiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

    }

    /// <summary>
    /// This exception will be thown when returning an error for a letter not being a member of the valid Alphabet is not a option.
    /// </summary>
    [Serializable]
    public class InvalidLetterException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidLetterException class with the default error message.
        /// </summary>
        public InvalidLetterException() : base("A letter was not found inside the Enimga's Alphabet processing cannot continue.") { }

        /// <summary>
        /// Initializes a new instance of the InvalidLetterException class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidLetterException(string message) : base(message)
        { }

        /// <summary>
        /// Passes message and innerException through.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InvalidLetterException(string message, Exception innerException) : base(message, innerException)
        { }

        /// <summary>
        /// Passes info and context through.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InvalidLetterException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
