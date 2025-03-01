using DmansValidator;
using ValidationToy.Requests;
using ValidationToy.Validated;

namespace ValidationToy;

public class DemoValidator
{
    public Result<ValidatedCreateUser, IReadOnlyList<ValidationError>> Validate(CreateUser request)
    {
        throw new NotImplementedException();
    }
}