using System.Diagnostics.CodeAnalysis;

namespace DmansValidator;

public static class Result
{
    public static Result<T, TError> Success<T, TError>(T value)
    {
        return Result<T, TError>.Success(value);
    }
    
    public static Result<T, TError> Fail<T, TError>(TError error)
    {

        return Result<T, TError>.Fail(error);
    }
}

public class Result<T,TError>
{
    public T? Value { get; private init; }
    public TError? Error { get; private init; }
    
    [MemberNotNullWhen(true, "Value")]
    [MemberNotNullWhen(false, "Error")]
    public bool IsSuccess { get; private init; }
    
    public static Result<T, TError> Success(T value)
    {
        return new Result<T, TError>()
        {
            Value = value,
            IsSuccess = true
        };
    }
    
    public static Result<T, TError> Fail(TError error)
    {
        return new Result<T, TError>()
        {
            Error = error,
            IsSuccess = false
        };
    }
}