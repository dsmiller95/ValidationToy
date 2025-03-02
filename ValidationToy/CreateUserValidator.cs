using DmansValidator;
using ValidationToy.Requests;
using ValidationToy.Validated;

namespace ValidationToy;

public class CreateUserValidator(ValidateCreateUserContext context)
{
    public ValidationResult<ValidatedCreateUser> ValidateToResult(CreateUser request)
    {
        return ReturnManyValidationFailures.Validate(fail => ValidatedCreateUser.Create(fail, context, request));
    }

    public ValidatedCreateUser ValidateWithExceptions(CreateUser request)
    {
        // on dispose, context throws exception containing -all- errors
        using var fail = new ThrowManyValidationFailures();
        return ValidatedCreateUser.Create(fail, context, request);
    }
    
    public ValidatedCreateUser ValidateWithException(CreateUser request)
    {
        // on first error, context throws exception
        var fail = new ThrowValidationFailure();
        return ValidatedCreateUser.Create(fail, context, request);
    }
}