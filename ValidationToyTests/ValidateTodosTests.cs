using ValidationToy;
using ValidationToy.Requests;
using ValidationToy.Validated;
using static ValidationToyTests.TestUtilities;

namespace ValidationToyTests;

public class ValidateTodosTests
{
    [Test]
    public void ValidateCreateUserTodos_WithNegativePriority_Fails()
    {
        var createUser = GetValidCreateUser();
        createUser.Todos[0].Priority = -1;

        var validator = GetValidator();
        var validationResult = validator.Validate(createUser);
        
        AssertFailedWithMessage(validationResult, "Priority must be greater than zero.");
    }

    [Test]
    public void ValidateCreateUserTodos_WithNullName_Fails()
    {
        var createUser = GetValidCreateUser();
        createUser.Todos[0].Name = null;

        var validator = GetValidator();
        var validationResult = validator.Validate(createUser);
        
        AssertFailedWithMessage(validationResult, "Name is missing.");
    }

    [Test]
    public void ValidateCreateUserTodos_WithEmptyName_Fails()
    {
        var createUser = GetValidCreateUser();
        createUser.Todos[0].Name = "";

        var validator = GetValidator();
        var validationResult = validator.Validate(createUser);
        
        AssertFailedWithMessage(validationResult, "Name is missing.");
    }

    private DemoValidator GetValidator()
    {
        return new DemoValidator();
    }
}