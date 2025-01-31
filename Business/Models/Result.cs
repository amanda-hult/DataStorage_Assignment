using Business.Interfaces;

namespace Business.Models;

public class Result : IResult
{
    public bool Success { get; protected set; }
    public int StatusCode { get; protected set; }
    public string? Message { get; protected set; }

    public static Result Ok() => new SuccessResult(200);
    public static Result NoContent() => new SuccessResult(204);
    public static Result NotFound(string message) => new ErrorResult(404, message);
    public static Result Error(string message) => new ErrorResult(500, message);
}


public class SuccessResult : Result
{
    public SuccessResult(int statusCode)
    {
        Success = true;
        StatusCode = statusCode;
    }
}

public class ErrorResult : Result
{
    public ErrorResult(int statusCode, string message)
    {
        Success = false;
        StatusCode = statusCode;
        Message = message;
    }
}