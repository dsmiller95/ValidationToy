namespace DmansValidator;

public class ReturnManyValidationFailures : IFailValidation, IDisposable
{
    private ReturnManyValidationFailures() { }
    
    public static ValidationResult<TValidated> Validate<TValidated>(Func<IFailValidation, TValidated> create)
    {
        using var fail = new ReturnManyValidationFailures();
        var validated = create(fail);
        return fail.Errors.Count == 0
            ? ValidationResult<TValidated>.Success(validated) 
            : ValidationResult<TValidated>.Fail(fail.Errors);
    }

    private bool IsDisposed { get; set; }
    private List<ValidationError> Errors { get; } = new();

    public T Fail<T>(string message)
    {
        DisposeGuard();
        this.Fail(message);
        return default!;
    }

    public void Fail(string message)
    {
        DisposeGuard();
        Errors.Add(new ValidationError(message));
    }

    public void Dispose()
    {
        IsDisposed = true;
    }

    private void DisposeGuard()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException("Validation failure used outside of valid scope. Did it escape from a lambda?");
        }
    }
}