using DmansValidator;
using ValidationToy.Requests;
using ValidationToy.Validators;

namespace ValidationToy.Validated;

public class GreaterThanZeroInt
{
    private GreaterThanZeroInt() { }

    public required int Value { get; set; }
    
    public static GreaterThanZeroInt Create(IValidationContext context, int value)
    {
        if (value <= 0)
        {
            return context.Fail<GreaterThanZeroInt>("Value must be greater than zero.");
        }
        
        return new GreaterThanZeroInt
        {
            Value = value
        };
    }
}