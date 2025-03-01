using DmansValidator;

namespace ValidationToy.Validated;

public class Email
{
    private Email() { }

    public required string Value { get; set; }

    public static Email Create(IValidationContext context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return context.Fail<Email>("Email is missing.");
        }
        
        if(value.Contains("@") == false)
        {
            return context.Fail<Email>("Email must contain an @ symbol.");
        }

        return new Email()
        {
            Value = value
        };
    }
}