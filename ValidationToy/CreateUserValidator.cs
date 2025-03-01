using DmansValidator;
using ValidationToy.Requests;
using ValidationToy.Validated;

namespace ValidationToy;

public class CreateUserValidator()
{
    public Result<ValidatedCreateUser, IReadOnlyList<ValidationError>> Validate(CreateUser request)
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
        using var context = new AccumulatingThrowingValidationContext();
        return ValidatedCreateUser.Create(context, request);
    }
}