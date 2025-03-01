using DmansValidator;
using ValidationToy.Requests;
using ValidationToy.Validated;

namespace ValidationToy;

public class CreateUserValidator
{
    private readonly IValidationService _validationService;

    public CreateUserValidator(IValidationService validationService)
    {
        _validationService = validationService;
    }
    
    public Result<ValidatedCreateUser, IReadOnlyList<ValidationError>> Validate(CreateUser request)
    {
        return _validationService.Validate(context => ValidatedCreateUser.Create(context, request));
    }
}