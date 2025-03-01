namespace DmansValidator;

internal class AccumulatingValidationContext : IValidationContext
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
}