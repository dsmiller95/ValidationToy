using System.Runtime.Serialization;

namespace ValidationToy.Validated;

public class ValidatedCreateUserTodo
{
    public required string Name { get; set; }
    public required GreaterThanZeroInt Priority { get; set; }
}