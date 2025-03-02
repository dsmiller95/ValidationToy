namespace DmansValidator;

public class ReturnManyValidationFailures : IFailValidation
{
    private ReturnManyValidationFailures() { }
    
    public static Result<TValidated, IReadOnlyList<ValidationError>> Validate<TValidated>(Func<IFailValidation, TValidated> create)
    {
        var fail = new ReturnManyValidationFailures();
        var validated = create(fail);
        return fail.Errors.Count == 0
            ? Result<TValidated, IReadOnlyList<ValidationError>>.Success(validated) 
            : Result<TValidated, IReadOnlyList<ValidationError>>.Fail(fail.Errors);
    }

    private List<ValidationError> Errors { get; } = new();

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