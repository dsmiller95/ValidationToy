namespace ValidationToy.Requests;

public record CreateUser
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? DisplayName { get; set; }
    public List<CreateUserTodo> Todos { get; set; } = new List<CreateUserTodo>();

    public override string ToString()
    {
        return $"CreateUser {{ Email = {Email}, Password = {Password}, DisplayName = {DisplayName}, Todos =\n{Todos.Select(x => x.ToString()).LinesIndented(4)}\n}}";
    }
}