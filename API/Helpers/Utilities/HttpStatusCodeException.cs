using System.Net;

namespace API.Helpers.Utilities
{
    [Serializable]
    public class HttpStatusCodeException : Exception
    {
        public List<ErrorItem> Errors { get; set; }

        public virtual HttpStatusCode HttpStatusCode { get; }

        public HttpStatusCodeException()
        {
        }

        public HttpStatusCodeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public HttpStatusCodeException(string message) : base(message)
        {
        }

        public HttpStatusCodeException(ErrorItem error)
        {
            Errors = new List<ErrorItem> { error };
        }

        public HttpStatusCodeException(List<ErrorItem> errors) : base(errors?.FirstOrDefault()?.Message)
        {
            Errors = errors;
        }
    }

    public class ClientApiError
    {
        public string Message { get; set; }

        public string InnerMessage { get; set; }

        public List<ErrorItem> Errors { get; set; }

        public string Error { get; set; }

        public DateTime? ErrorTime { get; set; }

        public string StackTrace { get; set; }
    }

    public class ErrorItem
    {
        public string Message { get; set; }

        public string Type { get; set; }
    }
}