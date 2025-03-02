using DmansValidator;
using ValidationToy.Requests;
using ValidationToy.Validators;

namespace ValidationToy.Validated;

public class ValidatedCreateUser
{
    public required Email Email { get; set; }
    public required Password Password { get; set; }
    
    // TODO: validate that display name is unique. will need extra context data loaded in, from a DB for example.
    public required string DisplayName { get; set; }
    public required List<ValidatedCreateUserTodo> Todos { get; set; }
    
    public static ValidatedCreateUser Create(IFailValidation fail, ValidateCreateUserContext context, CreateUser request)
    {
        return new ValidatedCreateUser()
        {
            Email = Email.Create(fail, request.Email),
            Password = Password.Create(fail, request.Password),
            DisplayName = StringValidators.NonEmptyString(fail, request.DisplayName, "Display name is missing."),
            Todos = request.Todos.Select(todo => ValidatedCreateUserTodo.Create(fail, todo)).ToList()
        };
    }
}