using DmansValidator;
using ValidationToy.Requests;
using ValidationToy.Validators;

namespace ValidationToy.Validated;

public class ValidatedCreateUserTodo
{
    private ValidatedCreateUserTodo() { }
    
    public required string Name { get; set; }
    public required GreaterThanZeroInt Priority { get; set; }
    
    public static ValidatedCreateUserTodo Create(IValidationContext context, CreateUserTodo request)
    {
        return new ValidatedCreateUserTodo()
        {
            Name = StringValidators.NonEmptyString(context, request.Name, "Name is missing."),
            Priority = GreaterThanZeroInt.Create(context, request.Priority, nameof(request.Priority)),
        };
    }
}