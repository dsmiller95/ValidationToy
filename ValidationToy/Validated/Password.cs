using DmansValidator;

namespace ValidationToy.Validated;

public class Password
{
    private Password () { }
    public required string Value { get; set; }
    
    public static Password Create(IValidationContext context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return context.Fail<Password>("Password is missing.");
        }
        
        if (value.Length < 8)
        {
            return context.Fail<Password>("Password must be at least 8 characters long.");
        }

        return new Password()
        {
            Value = value
        };
    }
}