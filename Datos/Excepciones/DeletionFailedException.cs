namespace Datos.Exceptions
{
    public class DeletionFailedException : Exception
    {
        public DeletionFailedException() { }
        public DeletionFailedException(string message) : base(message) { }
        public DeletionFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
