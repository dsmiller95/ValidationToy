using DmansValidator;
using ValidationToy.CommonValidations;
using ValidationToy.Requests;

namespace ValidationToy.Validated;

public class ValidatedCreateUserTodo
{
    private ValidatedCreateUserTodo() { }
    
    public required string Name { get; set; }
    public required GreaterThanZeroInt Priority { get; set; }
    
    public static ValidatedCreateUserTodo Create(IFailValidation fail, CreateUserTodo request)
    {
        return new ValidatedCreateUserTodo()
        {
            Name = StringValidators.NonEmptyString(fail, request.Name, "Name is missing."),
            Priority = GreaterThanZeroInt.Create(fail, request.Priority, nameof(request.Priority)),
        };
    }
}