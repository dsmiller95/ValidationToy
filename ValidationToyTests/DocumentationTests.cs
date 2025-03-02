using DmansValidator;
using ValidationToy;
using ValidationToy.Requests;
using ValidationToy.Validated;

namespace ValidationToyTests;

public class DocumentationTests
{
    [Test]
    public void ReturnDefaultSuccess()
    {
        var defaultSuccess = new CreateUser
        {
            Email = "bob@email.com",
            Password = "password",
            DisplayName = "Bob",
            Todos = []
        };
        var defaultSuccessContext = ValidateCreateUserContext.CreateDefaultSuccess();
        
        ValidationResult<ValidatedCreateUser> validateResult = ReturnManyValidationFailures.Validate(fail => 
            ValidatedCreateUser.Create(fail, defaultSuccessContext, defaultSuccess));
        
        Assert.That(validateResult.IsSuccess, Is.True);
    }
    
    [Test]
    public void ReturnInvalidEmail()
    {
        var invalidEmail = new CreateUser
        {
            Email = "invalid",
            Password = "password",
            DisplayName = "Bob",
            Todos = []
        };
        var defaultSuccessContext = ValidateCreateUserContext.CreateDefaultSuccess();
        
        ValidationResult<ValidatedCreateUser> validateResult = ReturnManyValidationFailures.Validate(fail => 
            ValidatedCreateUser.Create(fail, defaultSuccessContext, invalidEmail));
        
        Assert.That(validateResult.IsSuccess, Is.False);
        AssertFailedWithMessages(validateResult, [
            "Email must contain an @ symbol."
        ]);
    }
    
    [Test]
    public void ReturnMissingMany()
    {
        var missingMany = new CreateUser
        {
            Email = null,
            Password = "",
            DisplayName = "",
            Todos = []
        };
        var defaultSuccessContext = ValidateCreateUserContext.CreateDefaultSuccess();
        
        // collects all failures emitted during the lambda execution
        ValidationResult<ValidatedCreateUser> validateResult = ReturnManyValidationFailures.Validate(fail => 
            ValidatedCreateUser.Create(fail, defaultSuccessContext, missingMany));
        
        Assert.That(validateResult.IsSuccess, Is.False);
        AssertFailedWithMessages(validateResult, [
            "Email is missing.",
            "Password is missing.", 
            "Display name is missing."
        ]);
    }
    
    [Test]
    public void ThrowSingleFailure()
    {
        var missingMany = new CreateUser
        {
            Email = null,
            Password = "",
            DisplayName = "",
            Todos = []
        };
        var defaultSuccessContext = ValidateCreateUserContext.CreateDefaultSuccess();

        ValidationException thrown = Assert.Throws<ValidationException>(() =>
        {
            var fail = new ThrowValidationFailure();
            // throws inside the validation, at first failure.
            _ = ValidatedCreateUser.Create(fail, defaultSuccessContext, missingMany);
        });
        
        AssertFailedWithMessages(thrown, [
            "Email is missing.",
        ]);
    }
    
    [Test]
    public void ThrowMultipleFailures()
    {
        var missingMany = new CreateUser
        {
            Email = null,
            Password = "",
            DisplayName = "",
            Todos = []
        };
        var defaultSuccessContext = ValidateCreateUserContext.CreateDefaultSuccess();

        ValidationException thrown = Assert.Throws<ValidationException>(() =>
        {
            using (var fail = new ThrowManyValidationFailures())
            {
                _ = ValidatedCreateUser.Create(fail, defaultSuccessContext, missingMany);
            } // throws here, when disposed
        });
        
        AssertFailedWithMessages(thrown, [
            "Email is missing.",
            "Password is missing.", 
            "Display name is missing."
        ]);
    }

    [Test]
    public void ReturnSuccessWithTodos()
    {
        var successWithTodos = new CreateUser
        {
            Email = "bob@email.com",
            Password = "password",
            DisplayName = "Bob",
            Todos =
            [
                new CreateUserTodo { Name = "dishes", Priority = 1 },
                new CreateUserTodo { Name = "laundry", Priority = 2 },
                new CreateUserTodo { Name = "vacuum", Priority = 3 },
            ],
        };
        var defaultSuccessContext = ValidateCreateUserContext.CreateDefaultSuccess();
        
        ValidationResult<ValidatedCreateUser> validateResult = ReturnManyValidationFailures.Validate(fail =>
            ValidatedCreateUser.Create(fail, defaultSuccessContext, successWithTodos));
        
        Assert.That(validateResult.IsSuccess, Is.True);
    }

    [Test]
    public void ReturnAggregateOfTodoErrors()
    {
        var aggregateOfTodoErrors = new CreateUser
        {
            Email = "bob@email.com",
            Password = "password",
            DisplayName = "Bob",
            Todos =
            [
                new CreateUserTodo { Name = "", Priority = 3 },
                new CreateUserTodo { Name = "laundry", Priority = -22 },
                new CreateUserTodo { Name = "", Priority = 3 },
            ],
        };
        var defaultSuccessContext = ValidateCreateUserContext.CreateDefaultSuccess();
        
        ValidationResult<ValidatedCreateUser> validateResult = ReturnManyValidationFailures.Validate(fail => 
            ValidatedCreateUser.Create(fail, defaultSuccessContext, aggregateOfTodoErrors));
        
        Assert.That(validateResult.IsSuccess, Is.False);
        AssertFailedWithMessages(validateResult, [
            "Priorities must be unique.",
            "Name is missing.",
            "Priority must be greater than zero.",
            "Name is missing."
        ]);
    }
    
    [Test]
    public void AttemptToEscapeReturnManyContext()
    {
        IFailValidation? escapedFail = null;
        _ = ReturnManyValidationFailures.Validate(fail =>
        {
            escapedFail = fail;
            return 0;
        });
        
        Assert.That(escapedFail, Is.Not.Null);
        // A ReturnManyValidationFailures must not escape its scope. Escape allows invalid objects to be constructed
        //  because the validation failures will not be captured by the wrapping lambda, as it has already returned.
        Assert.Throws<ObjectDisposedException>(() =>
        { 
            escapedFail.Fail("Failed outside of failure context");
        });
    }
    
    [Test]
    public void AttemptToEscapeThrowManyContext()
    {
        IFailValidation? escapedFail = null;
        using (var fail = new ThrowManyValidationFailures())
        {
            escapedFail = fail;
        }
        
        Assert.That(escapedFail, Is.Not.Null);
        // a ThrowManyValidationFailures must not escape its scope. Escape allows invalid objects to be constructed
        //  without the corresponding validation exception being thrown when the validated object is fully constructed.
        Assert.Throws<ObjectDisposedException>(() =>
        {
            escapedFail.Fail("Failed outside of failure context");
        });
    }
    
    [Test]
    public void AttemptToEscapeThrowContext()
    {
        IFailValidation? escapedFail = null;
        {
            var fail = new ThrowValidationFailure();
            escapedFail = fail;
        }
        
        Assert.That(escapedFail, Is.Not.Null);
        
        // a ThrowValidationFailure is safe to escape. It holds no state, and does not allow invalid objects to be constructed.
        Assert.Throws<ValidationException>(() =>
        {
            escapedFail.Fail("Failed outside of failure context");
        });
    }
    
    void AssertFailedWithMessages(ValidationResult<ValidatedCreateUser> input, string[] expectedErrors)
    {
        var actualErrors = input.IsSuccess ? [] : input.Errors.Select(e => e.Message).ToArray();
        Assert.That(actualErrors, Is.EquivalentTo(expectedErrors));
    }
    
    /// <summary>
    /// Asserts that the expected errors list maps 1-to-1 with the actual errors list, in order.
    /// </summary>
    void AssertFailedWithMessages(ValidationException exception, string[] expectedErrors)
    {
        var actualErrors = exception.Errors.Select(x => x.Message).ToArray();
        Assert.That(actualErrors, Is.EquivalentTo(expectedErrors));
    }
}