namespace UserApi.Exceptions
{
    public class UserApiException : Exception
    {
        public int StatusCode { get; }

        public UserApiException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
