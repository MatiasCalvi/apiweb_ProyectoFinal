namespace Datos.Exceptions
{
    public class DatabaseTransactionException : Exception
    {
        public DatabaseTransactionException(string message) : base(message) { }

        public DatabaseTransactionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
