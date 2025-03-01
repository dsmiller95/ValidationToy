namespace DmansValidator;

public class ValidationException : Exception
{
    public ValidationException(string message) : this([new ValidationError(message)])
    {
    }
    
    public ValidationException(IReadOnlyCollection<ValidationError> errors) : base("Validation failed: " + string.Join(", ", errors))
    {
        Errors = errors.ToList();
    }

    public List<ValidationError> Errors { get; set; }
}