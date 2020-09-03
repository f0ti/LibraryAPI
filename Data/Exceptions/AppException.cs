using System;

namespace LibraryAPI.Data.Exceptions
{
    // Custom exception class for throwing application specific exceptions   
    public class AppException : Exception
    {
        public AppException(string message) : base(message)
        {
        }
    }
}