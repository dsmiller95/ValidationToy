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
        AssertAnyFail(result, error => error.Message.Contains(anyMessageContains));
    }
    
    public static void AssertSuccess<T, TErr>(Result<T, TErr> result, Action<T> assert)
    {
        Assert.IsTrue(result.IsSuccess);
        if (result.IsSuccess)
        {
            assert(result.Value);
        }
    }
    
    public static void AssertFail<T, TErr>(Result<T, TErr> result, Action<TErr> assert)
    {
        Assert.IsFalse(result.IsSuccess);
        if (!result.IsSuccess)
        {
            assert(result.Error);
        }
    }
    
    public static void AssertAnyFail<T, TErr>(Result<T, IReadOnlyList<TErr>> result, Func<TErr, bool> predicate)
    {
        Assert.IsFalse(result.IsSuccess);
        if (!result.IsSuccess)
        {
            Assert.IsTrue(result.Error.Any(predicate));
        }
    }
}