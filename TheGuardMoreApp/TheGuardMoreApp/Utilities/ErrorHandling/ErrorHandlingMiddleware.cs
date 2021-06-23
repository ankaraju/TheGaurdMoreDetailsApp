using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace TheGuardMoreApp.API.Service.Utilities
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception)
            {
                // TODO: Log exception
                throw;
            }
        }
    }
    [Serializable]
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException()
        {

        }

        public RecordNotFoundException(string message)
            : base(message)
        {

        }

        public RecordNotFoundException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }

    [Serializable]
    public class RecordConflictException : Exception
    {
        public RecordConflictException() { }

        public RecordConflictException(string message)
            : base(message)
        {

        }

        public RecordConflictException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
    [Serializable]
    public class BadRequestException : Exception
    {
        public BadRequestException() { }

        public BadRequestException(string message)
            : base(message)
        {

        }

        public BadRequestException(string message, Exception inner)
                : base(message, inner)
        {

        }
    }
}
