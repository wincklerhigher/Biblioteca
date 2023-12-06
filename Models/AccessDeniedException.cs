using System;

namespace Biblioteca.Models
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException(string message) : base(message)
        {
        }
    }
}