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
        GC.SuppressFinalize(this);
        if (Errors.Any())
        {
            throw new ValidationException(Errors);
        }
    }

    ~AccumulatingThrowingValidationContext()
    {
        throw new InvalidOperationException("Validation context was not disposed. Validation context must be disposed in all cases.");
    }
}