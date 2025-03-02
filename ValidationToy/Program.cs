﻿using DmansValidator;
using ValidationToy;
using ValidationToy.Requests;
using ValidationToy.Validated;

//var validationService = new ValidationService();
var validator = new CreateUserValidator();

var defaultSuccess = new CreateUser
{
    Email = "bob@email.com",
    Password = "password",
    DisplayName = "Bob",
    Todos = []
};
var validated = validator.ValidateToResult(defaultSuccess);
AssertErrorsMatch(validated, []);

var invalidEmail = new CreateUser
{
    Email = "invalid",
    Password = "password",
    DisplayName = "Bob",
    Todos = []
};
validated = validator.ValidateToResult(invalidEmail);
AssertErrorsMatch(validated, ["email must contain"]);

var missingMany = new CreateUser
{
    Email = null,
    Password = "",
    DisplayName = "",
    Todos = []
};
validated = validator.ValidateToResult(missingMany);
AssertErrorsMatch(validated, [
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
validated = validator.ValidateToResult(successWithTodos);
AssertErrorsMatch(validated, []);

var aggregateOfTodoErrors = new CreateUser
{
    Email = "bob@email.com",
    Password = "password",
    DisplayName = "Bob",
    Todos =
    [
        new CreateUserTodo { Name = "", Priority = 1 },
        new CreateUserTodo { Name = "laundry", Priority = -22 },
        new CreateUserTodo { Name = "", Priority = 3 },
    ],
};
validated = validator.ValidateToResult(aggregateOfTodoErrors);
AssertErrorsMatch(validated, [
    "name is missing",
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
    AccumulatingValidationContext context = new();
    context.Fail("failed");
    await Task.Delay(TimeSpan.FromSeconds(0.001));
}

void AssertErrorsMatch(Result<ValidatedCreateUser, IReadOnlyList<ValidationError>> input, string[] expectedErrors)
{
    var actualErrors = input.IsSuccess ? [] : input.Error.Select(e => e.Message).ToArray();
    if (!OutputHelpers.ErrorsMatch(expectedErrors, actualErrors))
    {
        throw new Exception($"Got errors:\n{actualErrors.LinesIndented(4)}\nbut expected errors:\n{expectedErrors.LinesIndented(4)}");
    }
}