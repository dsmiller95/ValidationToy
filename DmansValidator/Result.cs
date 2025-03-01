using System.Diagnostics.CodeAnalysis;

namespace DmansValidator;

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