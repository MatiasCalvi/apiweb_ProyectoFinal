namespace Datos.Exceptions
{
    public class PasswordVerificationException : Exception
    {
        public PasswordVerificationException() { }

        public PasswordVerificationException(string message) : base(message) { }

        public PasswordVerificationException(string message, Exception innerException) : base(message, innerException) { }
    }
}

