using System.Text;
using System.Web;

namespace API.Helpers.Utilities
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var request = context.Request;

            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer);

            request.Body.Position = 0;

            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, false);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, bool htmlEncode)
        {
            var message = "";
            List<ErrorItem> errors = null;

            // set code
            int code;
            if (exception is HttpStatusCodeException statusCodeException)
            {
                code = (int)statusCodeException.HttpStatusCode;
                message = statusCodeException.Message;
                errors = statusCodeException.Errors;
            }
            else
            {
                code = exception.GetHashCode();
                if (exception.InnerException != null)
                {
                    message = exception.InnerException?.Message;
                }
                message += exception?.Message;
            }

            if (errors != null && htmlEncode)
                errors = HtmlEncode(errors);

            StringBuilder builder = new();
            builder.AppendFormat("Error = \"{0}\"{1}", "Có lỗi xảy ra!", Environment.NewLine);
            builder.AppendFormat("ErrorTime = {0}{1}", DateTime.Now, Environment.NewLine);
            builder.AppendFormat("Message = {0}{1}", htmlEncode ? HttpUtility.HtmlEncode(message) : message, Environment.NewLine);
            builder.AppendFormat("StackTrace = {0}{1}", exception.StackTrace, Environment.NewLine);
            builder.AppendFormat("Errors = {0}{1}", errors, Environment.NewLine);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;
            return context.Response.WriteAsync(builder.ToString());
        }

        private static List<ErrorItem> HtmlEncode(List<ErrorItem> errors)
        {
            return errors.Select(err =>
                new ErrorItem
                {
                    Message = HttpUtility.HtmlEncode(err.Message),
                    Type = HttpUtility.HtmlEncode(err.Type)
                }).ToList();
        }
    }
}