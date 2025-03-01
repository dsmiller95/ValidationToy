using System.Diagnostics.CodeAnalysis;

namespace DmansValidator;

public class Result<T,TError>
{
    public T? Value { get; set; }
    public TError? Error { get; set; }
    
    [MemberNotNullWhen(true, "Value")]
    [MemberNotNullWhen(false, "Error")]
    public bool IsSuccess { get; set; }
}