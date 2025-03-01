namespace ValidationToy.Requests;

public record CreateUserTodo
{
    public string? Name { get; set; }
    public int Priority { get; set; }
}