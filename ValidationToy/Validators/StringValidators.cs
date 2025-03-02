using DmansValidator;

namespace ValidationToy.Validators;

public class StringValidators
{
    public static string NonEmptyString(IFailValidation fail, string? value, string errorMessage)
    {
        if (string.IsNullOrEmpty(value))
        {
            fail.Fail(errorMessage);
        }

        return string.Empty;
    }
}