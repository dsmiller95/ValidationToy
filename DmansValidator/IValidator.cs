namespace DmansValidator;

public interface IValidator
{
}

public interface IValidationContext
{
    public T Fail<T>(string message);
    public void Fail(string message);
}

public class AccumulatingValidationContext : IValidationContext
{
    private List<ValidationError> _errors = new List<ValidationError>();
    
    public Result<T, IReadOnlyList<ValidationError>> ToResult<T>(T result)
    {
        if (_errors.Count == 0)
        {
            return Result<T, IReadOnlyList<ValidationError>>.Success(result);
        }
        
        var errors = _errors.ToList();
        return Result.Fail<T, IReadOnlyList<ValidationError>>(errors);

    }
    
    public T Fail<T>(string message)
    {
        this.Fail(message);
        return default!;
    }

    public void Fail(string message)
    {
        _errors.Add(new ValidationError(message));
    }
}