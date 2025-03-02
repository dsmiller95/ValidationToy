using System.Diagnostics.CodeAnalysis;

namespace DmansValidator;

public class ValidationResult<T>
{
    public T? Value { get; private init; }
    public IReadOnlyList<ValidationError>? Errors { get; private init; }
    
    [MemberNotNullWhen(true, "Value")]
    [MemberNotNullWhen(false, "Errors")]
    public bool IsSuccess { get; private init; }
    
    public static ValidationResult<T> Success(T value)
    {
        return new ValidationResult<T>()
        {
            Value = value,
            IsSuccess = true
        };
    }
    
    public static ValidationResult<T> Fail(IEnumerable<ValidationError> errors)
    {
        return new ValidationResult<T>()
        {
            Errors = errors.ToList(),
            IsSuccess = false
        };
    }
}