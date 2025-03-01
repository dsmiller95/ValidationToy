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
    
    
    [Test]
    public void ValidateCreateUserTodos_WithMultipleEmptyNames_AndNegativePriorities_Fails_WithMultipleErrors()
    {
        var createUser = GetValidCreateUser();
        createUser.Todos = 
        [
            new CreateUserTodo { Name = "", Priority = 1 },
            new CreateUserTodo { Name = "", Priority = 2 },
            new CreateUserTodo { Name = "", Priority = 55 },
            new CreateUserTodo { Name = "bob", Priority = -33 },
        ];

        var validator = GetValidator();
        var validationResult = validator.Validate(createUser);
        
        AssertFailedWithMessages(validationResult, 
            "Name is missing", "Name is missing", "Name is missing",
            "must be greater than zero"
            );
    }

    private DemoValidator GetValidator()
    {
        return new DemoValidator();
    }
}