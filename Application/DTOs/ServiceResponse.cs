namespace Application.DTOs
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }

        public int StatusCode { get; set; }

        public static ServiceResponse<T> SuccessResponse(T data, int statusCode)
        {
            return new ServiceResponse<T> { Data = data, StatusCode = statusCode, Success = true };
        }

        public static ServiceResponse<T> ErrorResponse(string errorCode, string errorMessage, int statusCode)
        {
            return new ServiceResponse<T>
            {
                ErrorCode = errorCode,
                ErrorMessage = errorMessage,
                StatusCode = statusCode,
                Success = false
            };
        }

    }
}
