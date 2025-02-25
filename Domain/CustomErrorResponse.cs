namespace Domain
{
    public class CustomErrorResponse
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public CustomErrorResponse(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}
