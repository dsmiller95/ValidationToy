namespace ValidationToy.Validated;

public class ValidatedCreateUser
{
    public required Email Email { get; set; }
    public required Password Password { get; set; }
    public required string DisplayName { get; set; }
    
    public required Dictionary<string, string> CustomProperties { get; set; }
    public required List<ValidatedCreateUserTodo> Todos { get; set; }
}