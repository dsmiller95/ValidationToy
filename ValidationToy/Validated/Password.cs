using DmansValidator;

namespace ValidationToy.Validated;

public class Password
{
    private Password () { }
    public required string Value { get; set; }
    
    public static Password Create(IFailValidation fail, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return fail.Fail<Password>("Password is missing.");
        }
        
        if (value.Length < 8)
        {
            return fail.Fail<Password>("Password must be at least 8 characters long.");
        }

        return new Password()
        {
            Value = value
        };
    }
}