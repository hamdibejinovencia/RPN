namespace Rpn.Api.Data.Exceptions;

using System;
using System.Runtime.Serialization;


    /// <summary>
    /// Exception indicate that an input of the user is incorrect.
    /// </summary>
    public class UserInputException : Exception
    {
        public UserInputException()
        {
        }

        public UserInputException(string message) : base(message)
        {
        }

        public UserInputException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserInputException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class ConflictException : Exception
    {
        public ConflictException()
        {
        }

        public ConflictException(string message) : base(message)
        {
        }

        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

