using DmansValidator;
using ValidationToy;
using ValidationToy.Requests;
using ValidationToy.Validated;
using static ValidationToyTests.TestUtilities;

namespace ValidationToyTests;

public class ValidateComplexRulesTests
{
    [Test]
    public void ValidateCreateUser_WithNonUniqueName_Fails_WithUniqueNameRequired()
    {
        var createUser = GetValidCreateUser();
        var validationContext = new ValidateCreateUserContext
        {
            DisplayNameAlreadyExists = true
        };

        var validationResult = ReturnManyValidationFailures.Validate(fail => 
            ValidatedCreateUser.Create(fail, validationContext, createUser));
        
        AssertFailedWithMessages(validationResult, 
            "Name already in use"
        );
    }
    
    [Test]
    public void ValidateCreateUser_WithRepeatedTodoPriority_Fails_WithPrioritiesMustBeUnique()
    {
        var createUser = GetValidCreateUser();
        createUser.Todos = 
        [
            new CreateUserTodo { Name = "bob", Priority = 1 },
            new CreateUserTodo { Name = "joe", Priority = 1 },
        ];
        var validationContext = ValidateCreateUserContext.CreateDefaultSuccess();

        var validationResult = ReturnManyValidationFailures.Validate(fail => 
            ValidatedCreateUser.Create(fail, validationContext, createUser));
        
        AssertFailedWithMessages(validationResult, 
            "Priorities must be unique."
        );
    }
}