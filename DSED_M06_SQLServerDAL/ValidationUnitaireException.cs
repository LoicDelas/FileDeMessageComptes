using System.Runtime.Serialization;

namespace DSED_M06_SQLServerDAL
{
    [Serializable]
    internal class ValidationUnitaireException : Exception
    {
        public ValidationUnitaireException()
        {
        }

        public ValidationUnitaireException(string? message) : base(message)
        {
        }

        public ValidationUnitaireException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ValidationUnitaireException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}