namespace OrderManagement.Api.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Msg { get; set; }
        public ApiResponse(int status, string? msg = null)
        {
            StatusCode = status;
            Msg = msg ?? GetDefaultMsgForStatuCode(StatusCode);
        }


        private string? GetDefaultMsgForStatuCode(int? statusCode)
        {
            return StatusCode switch
            {
                400 => "BadRequest",
                401 => "UnAutharized",
                404 => "Resource NotFound",
                500 => "ServerError",
                _ => null,
            };
        }
    }
}
