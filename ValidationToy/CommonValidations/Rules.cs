using DmansValidator;

namespace ValidationToy.CommonValidations;

public static class Rules
{
    public static void MustBeUnique<T>(IFailValidation fail, ICollection<T> mustBeUnique, string errorMessage)
    {
        if (mustBeUnique.Distinct().Count() != mustBeUnique.Count)
        {
            fail.Fail(errorMessage);
        }
    }
}