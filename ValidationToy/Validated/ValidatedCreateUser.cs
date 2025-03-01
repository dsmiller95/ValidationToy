using DmansValidator;
using ValidationToy.Requests;
using ValidationToy.Validators;

namespace ValidationToy.Validated;

public class ValidatedCreateUser
{
    public required Email Email { get; set; }
    public required Password Password { get; set; }
    public required string DisplayName { get; set; }
    public required List<ValidatedCreateUserTodo> Todos { get; set; }
    
    public static ValidatedCreateUser Create(IValidationContext context, CreateUser request)
    {
        return new ValidatedCreateUser()
        {
            Email = Email.Create(context, request.Email),
            Password = Password.Create(context, request.Password),
            DisplayName = StringValidators.NonNullString(context, request.DisplayName, "Display name is required."),
            Todos = request.Todos.Select(todo => ValidatedCreateUserTodo.Create(context, todo)).ToList()
        };
    }
}