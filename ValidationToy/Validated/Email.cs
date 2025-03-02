using DmansValidator;

namespace ValidationToy.Validated;

public class Email
{
    private Email() { }

    public required string Value { get; set; }

    public static Email Create(IFailValidation fail, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return fail.Fail<Email>("Email is missing.");
        }
        
        if(value.Contains("@") == false)
        {
            return fail.Fail<Email>("Email must contain an @ symbol.");
        }

        return new Email()
        {
            Value = value
        };
    }
}