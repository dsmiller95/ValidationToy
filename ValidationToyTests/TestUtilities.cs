using DmansValidator;
using ValidationToy.Requests;

namespace ValidationToyTests;

public class TestUtilities
{
    public static CreateUser GetValidCreateUser()
    {
        return new CreateUser
        {
            Email = "email@email.com",
            Password = "abc123!@#dkfjdkf",
            DisplayName = "Bob",
            Todos =
            [
                new CreateUserTodo { Name = "Dishes", Priority = 1, },
                new CreateUserTodo { Name = "Laundry", Priority = 4, },
            ]
        };
    }

    public static void AssertFailedWithMessage<T>(ValidationResult<T> validationResult, string anyMessageContains)
    {
        Assert.IsFalse(validationResult.IsSuccess, "Should error");
        if (!validationResult.IsSuccess)
        {
            Assert.IsTrue(validationResult.Errors.Any(error => error.Message.Contains(anyMessageContains)), $"Should have an error message containing '{anyMessageContains}'.Errors:\n{string.Join("\n", validationResult.Errors.Select(e => e.Message))}");
        }
    }
    
    public static void AssertFailedWithMessages<T>(ValidationResult<T> validationResult, params string[] anyMessageContains)
    {
        Assert.IsFalse(validationResult.IsSuccess, "Should error");
        if (!validationResult.IsSuccess)
        {
            var errors = validationResult.Errors.Select(e => e.Message).ToList();
            var mustContain = anyMessageContains.ToList();
            foreach (string error in errors)
            {
                var matching = mustContain.FirstOrDefault(e => error.Contains(e));
                if (matching != null)
                {
                    mustContain.Remove(matching);
                }
            }

            if (mustContain.Any())
            {
                Assert.Fail($"Should have an error message containing all:\n{string.Join("\n", anyMessageContains)}'\nBut got errors:\n{string.Join("\n", errors)}");
            }
        }
    }
    
    public static void AssertSuccess<T>(ValidationResult<T> validationResult, Action<T> assert)
    {
        Assert.IsTrue(validationResult.IsSuccess, "Should succeed");
        if (validationResult.IsSuccess)
        {
            assert(validationResult.Value);
        }
    }
}