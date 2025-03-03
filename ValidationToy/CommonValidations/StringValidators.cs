using DmansValidator;

namespace ValidationToy.CommonValidations;

public class StringValidators
{
    public static string NonEmptyString(IFailValidation fail, string? value, string attributeName)
    {
        if (string.IsNullOrEmpty(value))
        {
            fail.Fail($"{attributeName} is missing.");
            return string.Empty;
        }

        return value;
    }
}