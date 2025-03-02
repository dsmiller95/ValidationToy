namespace DmansValidator;

public class ThrowManyValidationFailures : IFailValidation, IDisposable
{
    public List<ValidationError> Errors { get; } = new List<ValidationError>();
    
    private bool IsDisposed { get; set; }
    
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
        GC.SuppressFinalize(this);
        if (Errors.Any())
        {
            throw new ValidationException(Errors);
        }
    }

    ~ThrowManyValidationFailures()
    {
        throw new InvalidOperationException("Validation context was not disposed. Validation context must be disposed in all cases.");
    }
    
    private void DisposeGuard()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException("Validation failure used outside of valid scope. Did it escape from a using scope?");
        }
    }
}