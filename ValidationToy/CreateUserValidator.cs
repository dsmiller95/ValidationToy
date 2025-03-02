using DmansValidator;
using ValidationToy.Requests;
using ValidationToy.Validated;

namespace ValidationToy;

public class CreateUserValidator()
{
    public Result<ValidatedCreateUser, IReadOnlyList<ValidationError>> ValidateToResult(CreateUser request)
    {
        return ReturnManyValidationFailures.Validate(fail => ValidatedCreateUser.Create(fail, request));
    }

    public ValidatedCreateUser ValidateWithExceptions(CreateUser request)
    {
        // on dispose, context throws exception containing -all- errors
        using var context = new ThrowManyValidationFailures();
        return ValidatedCreateUser.Create(context, request);
    }
    
    public ValidatedCreateUser ValidateWithException(CreateUser request)
    {
        // on first error, context throws exception
        var context = new ThrowValidationFailure();
        return ValidatedCreateUser.Create(context, request);
    }
}