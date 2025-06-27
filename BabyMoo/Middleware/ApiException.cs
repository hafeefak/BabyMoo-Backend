namespace BabyMoo.Middleware
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }

        public ApiException(string message, int statusCode = StatusCodes.Status500InternalServerError)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }

    public class NotFoundException : ApiException
    {
        public NotFoundException(string message)
            : base(message, StatusCodes.Status404NotFound) { }
    }

    public class BadRequestException : ApiException
    {
        public BadRequestException(string message)
            : base(message, StatusCodes.Status400BadRequest) { }
    }

    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message)
            : base(message, StatusCodes.Status401Unauthorized) { }
    }

    public class ForbiddenException : ApiException
    {
        public ForbiddenException(string message)
            : base(message, StatusCodes.Status403Forbidden) { }
    }
    public class ConflictException : ApiException
    {
        public ConflictException(string message)
            : base(message, StatusCodes.Status409Conflict) { }

    }
}
