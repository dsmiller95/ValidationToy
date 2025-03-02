using DmansValidator;
using ValidationToy;
using ValidationToy.Requests;
using ValidationToy.Validated;

var defaultSuccess = new CreateUser
{
    Email = "bob@email.com",
    Password = "password",
    DisplayName = "Bob",
    Todos = []
};
var defaultSuccessContext = ValidateCreateUserContext.CreateDefaultSuccess();
var validateResult = ReturnManyValidationFailures.Validate(fail => 
    ValidatedCreateUser.Create(fail, defaultSuccessContext, defaultSuccess));
AssertTrue(validateResult.IsSuccess);

var invalidEmail = new CreateUser
{
    Email = "invalid",
    Password = "password",
    DisplayName = "Bob",
    Todos = []
};
validateResult = ReturnManyValidationFailures.Validate(fail => 
    ValidatedCreateUser.Create(fail, defaultSuccessContext, invalidEmail));
AssertFalse(validateResult.IsSuccess);
AssertErrorsMatch(validateResult, ["email must contain"]);

var missingMany = new CreateUser
{
    Email = null,
    Password = "",
    DisplayName = "",
    Todos = []
};
validateResult = ReturnManyValidationFailures.Validate(fail => 
    ValidatedCreateUser.Create(fail, defaultSuccessContext, missingMany));
AssertFalse(validateResult.IsSuccess);
AssertErrorsMatch(validateResult, [
    "email is missing",
    "password is missing", 
    "display name is missing"
]);

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
validateResult = ReturnManyValidationFailures.Validate(fail => 
    ValidatedCreateUser.Create(fail, defaultSuccessContext, successWithTodos));
AssertTrue(validateResult.IsSuccess);

var aggregateOfTodoErrors = new CreateUser
{
    Email = "bob@email.com",
    Password = "password",
    DisplayName = "Bob",
    Todos =
    [
        new CreateUserTodo { Name = "", Priority = -22 },
        new CreateUserTodo { Name = "laundry", Priority = -22 },
        new CreateUserTodo { Name = "", Priority = 3 },
    ],
};
validateResult = ReturnManyValidationFailures.Validate(fail => 
    ValidatedCreateUser.Create(fail, defaultSuccessContext, aggregateOfTodoErrors));
AssertFalse(validateResult.IsSuccess);
AssertErrorsMatch(validateResult, [
    "priority must be unique",
    "name is missing",
    "priority must be greater than zero",
    "priority must be greater than zero",
    "name is missing"
]);



Console.WriteLine("trying to throw from inside a finalizer");
for(int i = 0; i < 10_000; i++)
{
    Console.Write(".");
    if(i % 100 == 99)
    {
        Console.WriteLine();
        GC.Collect();
    }
    await CreateAndDontDisposeThing();
}

async Task CreateAndDontDisposeThing()
{
    ThrowManyValidationFailures context = new();
    context.Fail("failed");
    await Task.Delay(TimeSpan.FromSeconds(0.001));
}

void AssertErrorsMatch(ValidationResult<ValidatedCreateUser> input, string[] expectedErrors)
{
    var actualErrors = input.IsSuccess ? [] : input.Errors.Select(e => e.Message).ToArray();
    if (!OutputHelpers.ErrorsMatch(expectedErrors, actualErrors))
    {
        throw new Exception($"Got errors:\n{actualErrors.LinesIndented(4)}\nbut expected errors:\n{expectedErrors.LinesIndented(4)}");
    }
}

void AssertTrue(bool condition)
{
    if (!condition)
    {
        throw new Exception($"Expected true, but got false");
    }
}
void AssertFalse(bool condition)
{
    if (condition)
    {
        throw new Exception($"Expected false, but got true");
    }
}