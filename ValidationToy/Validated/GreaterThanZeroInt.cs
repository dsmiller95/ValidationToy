using DmansValidator;

namespace ValidationToy.Validated;

public class GreaterThanZeroInt
{
    private GreaterThanZeroInt() { }

    public required int Value { get; set; }
    
    public static GreaterThanZeroInt Create(IFailValidation fail, int value, string attributeName)
    {
        if (value <= 0)
            return fail.Fail<GreaterThanZeroInt>($"{attributeName} must be greater than zero.");
        
        return new GreaterThanZeroInt { Value = value };
    }
}