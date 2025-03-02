namespace ValidationToy;

public class ValidateCreateUserContext
{
    public static ValidateCreateUserContext CreateDefaultSuccess() => new()
    {
        DisplayNameAlreadyExists = false
    };
    
    public required bool DisplayNameAlreadyExists { get; init; }
}