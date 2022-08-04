using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SkillTracker.API
{
    public class GlobalErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;
        public GlobalErrorHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            var exceptionType = exception.GetType();
            string message;
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = "Unauthorized Access";
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                message = "A server error occurred";
                status = HttpStatusCode.NotImplemented;
            }
            else
            {
                message = exception.Message;
                status = HttpStatusCode.InternalServerError;
            }

            HttpResponse response = context.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";
            var err = "Unhandled Exception - " + message;
            LogContext.PushProperty("Exception", exception.StackTrace);
            Log.Error(err);

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = response.StatusCode,
                Message = err
            }.ToString());

        }
    }
}
