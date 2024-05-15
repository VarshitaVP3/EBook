using System.Runtime.Serialization;

namespace EBook.Exceptions
{
    
    public class InValidNameException : Exception
    {
        public InValidNameException()
        {
        }

        public InValidNameException(string? message) : base(message)
        {
        }

        public InValidNameException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public override string Message
        {
            get
            {
                return "Invalid username";
            }
        }

    }
}
