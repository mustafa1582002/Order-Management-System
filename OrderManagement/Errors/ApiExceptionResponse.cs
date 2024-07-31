namespace OrderManagement.Api.Errors
{
    public class ApiExceptionResponse : ApiResponse
    {
        public ApiExceptionResponse(int StatusCode, string? Message = null, string? details = null) : base(StatusCode, Message)
        {
            Details = details;
        }

        public string? Details { get; set; }

    }
}
