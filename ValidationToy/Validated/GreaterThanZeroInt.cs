using DmansValidator;

namespace ValidationToy.Validated;

public class GreaterThanZeroInt
{
    private GreaterThanZeroInt() { }

    public required int Value { get; set; }
    
    public static GreaterThanZeroInt Create(IValidationContext context, int value, string attributeName)
    {
        if (value <= 0)
        {
            return context.Fail<GreaterThanZeroInt>($"{attributeName} must be greater than zero.");
        }
        
        return new GreaterThanZeroInt
        {
            Value = value
        };
    }
}