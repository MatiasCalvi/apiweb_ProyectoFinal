namespace Datos.Exceptions
{
    public class DatabaseQueryException : Exception
    {
        public DatabaseQueryException(string message) : base(message) { }

        public DatabaseQueryException(string message, Exception innerException) : base(message, innerException) { }
    }
}
