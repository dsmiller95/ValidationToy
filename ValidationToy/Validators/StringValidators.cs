using DmansValidator;

namespace ValidationToy.Validators;

public class StringValidators
{
    public static string NonEmptyString(IValidationContext context, string? value, string errorMessage)
    {
        if (string.IsNullOrEmpty(value))
        {
            context.Fail(errorMessage);
        }

        return string.Empty;
    }
}