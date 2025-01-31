using Business.Interfaces;

namespace Business.Models;

public abstract class ResultT<T> : IResult
{
    public bool Success { get; protected set; }
    public int StatusCode { get; protected set; }
    public string? Message { get; protected set; }
    public T? Data { get; protected set; }


    public static ResultT<T> Ok(T data) => new SuccessResult<T>(200, data);
    public static ResultT<T> Created(T data) => new SuccessResult<T>(201, data);
    public static ResultT<T> NoContent() => new SuccessResult<T>(204, default);
    public static ResultT<T> NotFound(string message) => new ErrorResult<T>(404, message);
    public static ResultT<T> Conflict(string message) => new ErrorResult<T>(409, message);
    public static ResultT<T> Error(string message) => new ErrorResult<T>(500, message);
}

public class SuccessResult<T> : ResultT<T>
{
    public SuccessResult(int statusCode, T? data)
    {
        Success = true;
        StatusCode = statusCode;
        Data = data;
    }
}

public class ErrorResult<T> : ResultT<T>
{
    public ErrorResult(int statusCode, string message)
    {
        Success = false;
        StatusCode = statusCode;
        Message = message;
        Data = default;
    }
}