using DmansValidator;
using ValidationToy.Requests;
using ValidationToy.Validated;

namespace ValidationToy;

public class CreateUserValidator()
{
    public Result<ValidatedCreateUser, IReadOnlyList<ValidationError>> ValidateToResult(CreateUser request)
    {
        try
        {
            return Result<ValidatedCreateUser, IReadOnlyList<ValidationError>>.Success(
                this.ValidateWithExceptions(request));
        }
        catch (ValidationException e)
        {
            return Result<ValidatedCreateUser, IReadOnlyList<ValidationError>>.Fail(e.Errors);
        }
    }

    public ValidatedCreateUser ValidateWithExceptions(CreateUser request)
    {
        // on dispose, context throws exception containing -all- errors
        using var context = new AccumulatingValidationContext();
        return ValidatedCreateUser.Create(context, request);
    }
    
    public ValidatedCreateUser ValidateWithException(CreateUser request)
    {
        // on first error, context throws exception
        var context = new ValidationContext();
        return ValidatedCreateUser.Create(context, request);
    }
}