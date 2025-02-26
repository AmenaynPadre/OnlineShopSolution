namespace OnlineShop.Api.Common
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public static Result SuccessResult(string message = "Operation succeeded")
        {
            return new Result { Success = true, Message = message, StatusCode = StatusCodes.Status200OK };
        }

        public static Result FailureResult(string message, int statusCode = StatusCodes.Status400BadRequest)
        {
            return new Result { Success = false, Message = message, StatusCode = statusCode };
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        public static new Result<T> SuccessResult(T data, string message = "Operation succeeded")
        {
            return new Result<T> { Success = true, Data = data, Message = message, StatusCode = StatusCodes.Status200OK };
        }

        public static new Result<T> FailureResult(string message, int statusCode = StatusCodes.Status400BadRequest)
        {
            return new Result<T> { Success = false, Message = message, StatusCode = statusCode };
        }
    }

}
