using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Nozomi.Core.Exceptions;
using Nozomi.Core.Responses;

namespace Nozomi.Service.Middleware
{
    public class NozomiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public NozomiExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            var customException = exception as NozomiRequestException;
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "Unexpected error";
            var description = "Unexpected error";

            if (null != customException)
            {
                message = customException.Message;
                description = customException.Description;
                statusCode = customException.Code;
            }

            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(new NozomiRequestResponse
            {
                Message = message,
                Description = description
            }));
        }
    }
        
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class NozomiExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseNozomiExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<NozomiExceptionMiddleware>();
        }
    }
}