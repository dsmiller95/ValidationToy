namespace DmansValidator;

public class AccumulatingThrowingValidationContext : IValidationContext, IDisposable
{

    public List<ValidationError> Errors { get; } = new List<ValidationError>();

    public T Fail<T>(string message)
    {
        this.Fail(message);
        return default!;
    }

    public void Fail(string message)
    {
        Errors.Add(new ValidationError(message));
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }
    
    private void ReleaseUnmanagedResources()
    {
        if (Errors.Any())
        {
            throw new ValidationException(Errors);
        }
    }

    ~AccumulatingThrowingValidationContext()
    {
        ReleaseUnmanagedResources();
    }
}