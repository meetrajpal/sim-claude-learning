namespace AspNetCore.MCP.API.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, IFileLogger logger)
{
    #region Fields
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    #endregion

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            logger.Log($"API started executing: {context.Request.Path}");
            await next(context);
            logger.Log($"API completed executing: {context.Request.Path}");
        }
        catch (Exception ex)
        {
            logger.LogError($"Unhandled exception occurred. RequestPath: {context.Request.Path}", ex);
            await HandleExceptionAsync(context, ex);
        }
    }

    #region Private Methods
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        (HttpStatusCode StatusCode, string Message, List<string> Errors) result = exception switch
        {
            ValidationException validationEx => (
                StatusCode: HttpStatusCode.BadRequest,
                Message: "Validation failed.",
                Errors: [.. validationEx.Errors.Select(e => e.ErrorMessage)]
            ),
            ArgumentNullException => (
                StatusCode: HttpStatusCode.BadRequest,
                Message: "A required value was missing.",
                Errors: [exception.Message]
            ),
            ArgumentException => (
                StatusCode: HttpStatusCode.BadRequest,
                Message: exception.Message,
                Errors: [exception.Message]
            ),
            UnauthorizedAccessException => (
                StatusCode: HttpStatusCode.Unauthorized,
                Message: "You are not authorized to perform this action.",
                Errors: [exception.Message]
            ),
            KeyNotFoundException => (
                StatusCode: HttpStatusCode.NotFound,
                Message: "The requested resource was not found.",
                Errors: [exception.Message]
            ),
            InvalidOperationException => (
                StatusCode: HttpStatusCode.BadRequest,
                Message: exception.Message,
                Errors: [exception.Message]
            ),
            NotImplementedException => (
                StatusCode: HttpStatusCode.NotImplemented,
                Message: "This feature is not yet implemented.",
                Errors: [exception.Message]
            ),
            _ => (
                StatusCode: HttpStatusCode.InternalServerError,
                Message: "An unexpected error occurred. Please try again later.",
                Errors: [exception.Message]
            )
        };

        context.Response.StatusCode = (int)result.StatusCode;

        var response = ApiResponse<object>.Failure(result.Message, result.Errors);

        var json = JsonSerializer.Serialize(response, _jsonSerializerOptions);

        await context.Response.WriteAsync(json);
    }
    #endregion
}
