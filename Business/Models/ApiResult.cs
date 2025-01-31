//using Business.Interfaces;

//namespace Business.Models;

//public class ApiResult : IResult
//{
//    public bool Success { get; protected set; }
//    public int StatusCode { get; protected set; }
//    public string? Message { get; protected set; }


//    public static ApiResult Ok() => new SuccessResult(200);
//    public static ApiResult NoContent() => new SuccessResult(204);
//    public static ApiResult NotFound(string message) => new ErrorResult(404, message);
//    public static ApiResult Error(string message) => new ErrorResult(500, message);
//}

//public class SuccessResult : ApiResult
//{
//    public SuccessResult(int statusCode)
//    {
//        Success = true;
//        StatusCode = statusCode;
//    }
//}

//public class ErrorResult : ApiResult
//{
//    public ErrorResult(int statusCode, string message)
//    {
//        Success = false;
//        StatusCode = statusCode;
//        Message = message;
//    }
//}
