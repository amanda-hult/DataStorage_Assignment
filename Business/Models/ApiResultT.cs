//using Business.Interfaces;

//namespace Business.Models;

//public abstract class ApiResultT<T> : IResult
//{
//    public bool Success { get; protected set; }
//    public int StatusCode { get; protected set; }
//    public string? Message { get; protected set; }
//    public T? Data { get; protected set; }


//    public static ApiResultT<T> Ok(T data) => new SuccessResult<T>(200, data);
//    public static ApiResultT<T> Created(T data) => new SuccessResult<T>(201, data);
//    public static ApiResultT<T> NoContent() => new SuccessResult<T>(204, default);
//    public static ApiResultT<T> NotFound(string message) => new ErrorResult<T>(404, message);
//    public static ApiResultT<T> Conflict(string message) => new ErrorResult<T>(409, message);
//    public static ApiResultT<T> Error(string message) => new ErrorResult<T>(500, message);

//}

//public class SuccessResult<T> : ApiResultT<T>
//{
//    public SuccessResult(int statusCode, T? data)
//    {
//        Success = true;
//        StatusCode = statusCode;
//        Data = data;
//    }
//}

//public class ErrorResult<T> : ApiResultT<T>
//{    public ErrorResult(int statusCode, string message)
//    {
//        Success = false;
//        StatusCode = statusCode;
//        Message = message;
//        Data = default;
//    }
//}
