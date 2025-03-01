namespace DmansValidator;

public class ValidationService : IValidationService
{
    public Result<TValidated, IReadOnlyList<ValidationError>> Validate<TValidated>(
        Func<IValidationContext, TValidated> validation)
    {
        var context = new AccumulatingValidationContext();
        var validated = validation(context);
        if (context.Errors.Count == 0)
        {
            return Result<TValidated, IReadOnlyList<ValidationError>>.Success(validated);
        }
        
        var errors = context.Errors.ToList();
        return Result<TValidated, IReadOnlyList<ValidationError>>.Fail(errors);
    }
}