namespace UserApi.Exceptions
{
    public class UserApiException : Exception
    {
        public int StatusCode { get; }

        public UserApiException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
        public UserApiException(string message) : this(message, 500) { }
        public static UserApiException BadRequest(string message) =>
            new UserApiException(message, 400);

        public static UserApiException NotFound(string message) =>
            new UserApiException(message, 404);

        public static UserApiException Unauthorized(string message) =>
            new UserApiException(message, 401);

        public static UserApiException InternalServerError(string message) =>
            new UserApiException(message, 500);
    }
}
