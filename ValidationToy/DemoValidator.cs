using DmansValidator;
using ValidationToy.Requests;
using ValidationToy.Validated;

namespace ValidationToy;

public class DemoValidator
{
    public Result<ValidatedCreateUser, IReadOnlyList<ValidationError>> Validate(CreateUser request)
    {
        var validationContext = new AccumulatingValidationContext();
        var validated = ValidatedCreateUser.Create(validationContext, request);
        
        return validationContext.ToResult(validated);
    }
}