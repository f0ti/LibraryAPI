// aka not found or not found values

namespace LibraryAPI.Data.Exceptions
{
    public class EntityInvalidationException : AppException
    {
        public EntityInvalidationException(string message) : base(message)
        {
        }
    }
}