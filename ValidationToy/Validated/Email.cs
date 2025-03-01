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
            // potential footgun: what if we return a null value from here but then we need to use that
            //  value later on, as part of validation? Maybe the validation context needs to catch null
            //  reference excpetions too, just in case?
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