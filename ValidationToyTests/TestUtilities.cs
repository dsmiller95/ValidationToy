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

    public static void AssertFailedWithMessage<T>(Result<T, IReadOnlyList<ValidationError>> result, string anyMessageContains)
    {
        Assert.IsFalse(result.IsSuccess, "Should error");
        if (!result.IsSuccess)
        {
            Assert.IsTrue(result.Error.Any(error => error.Message.Contains(anyMessageContains)), $"Should have an error message containing '{anyMessageContains}'.Errors:\n{string.Join("\n", result.Error.Select(e => e.Message))}");
        }
    }
    
    
    public static void AssertFailedWithMessages<T>(Result<T, IReadOnlyList<ValidationError>> result, params string[] anyMessageContains)
    {
        Assert.IsFalse(result.IsSuccess, "Should error");
        if (!result.IsSuccess)
        {
            var errors = result.Error.Select(e => e.Message).ToList();
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
    
    public static void AssertSuccess<T, TErr>(Result<T, TErr> result, Action<T> assert)
    {
        Assert.IsTrue(result.IsSuccess, "Should succeed");
        if (result.IsSuccess)
        {
            assert(result.Value);
        }
    }
    
    public static void AssertFail<T, TErr>(Result<T, TErr> result, Action<TErr> assert)
    {
        Assert.IsFalse(result.IsSuccess, "Should error");
        if (!result.IsSuccess)
        {
            assert(result.Error);
        }
    }
    
    public static void AssertAnyFail<T, TErr>(Result<T, IReadOnlyList<TErr>> result, Func<TErr, bool> predicate)
    {
        Assert.IsFalse(result.IsSuccess, "Should error");
        if (!result.IsSuccess)
        {
            Assert.IsTrue(result.Error.Any(predicate), "Should have an error that matches predicate");
        }
    }
}