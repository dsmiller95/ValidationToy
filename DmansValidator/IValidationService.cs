namespace DmansValidator;

public interface IValidationService
{
    public Result<TValidated, IReadOnlyList<ValidationError>> Validate<TValidated>(Func<IValidationContext, TValidated> validation);
}

public interface IValidationContext
{
    public T Fail<T>(string message);
    public void Fail(string message);
}