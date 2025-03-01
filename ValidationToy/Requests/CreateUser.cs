namespace ValidationToy.Requests;

public class CreateUser
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? DisplayName { get; set; }
    public List<CreateUserTodo> Todos { get; set; } = new List<CreateUserTodo>();
}