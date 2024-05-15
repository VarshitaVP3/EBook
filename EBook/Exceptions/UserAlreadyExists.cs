namespace EBook.Exceptions
{
    [Serializable]
    public class UserAlreadyExists : Exception
    {
        public UserAlreadyExists() { }
        public UserAlreadyExists(string message) : base(message) { }
        public UserAlreadyExists(string message , Exception exception) : base(message, exception) { }

        public override string Message
        {
            get
            {
                return "Username already exits";
            }
        }
    }
}
