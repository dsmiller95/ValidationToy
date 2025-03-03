# Validation Toy

This repository explores ways to perform object validation as part of object construction.
The goal is to capture all the benefits of throwing exceptions when validation fails, while also providing a way to accumulate all validation errors in a single pass.


## Example

Relying on exceptions alone, we can write validation code like this:
```C#
public class GreaterThanZeroInt {
    public int Value { get; private set; }
    
    public static GreaterThanZeroInt Create(int value) {
        if (value <= 0)
            throw new ArgumentException("Value must be greater than zero.");
        
        return new GreaterThanZeroInt { Value = value };
    }
}
public static string NonEmptyString(string? value)
{
    if (string.IsNullOrEmpty(value))
        throw new ArgumentException("Value must be non-empty.");

    return value;
}
public static ValidatedCreateTodo ValidateCreateTodoExceptions(CreateTodo request)
{
    if (request.Name == "John")
        throw new ArgumentException($"Cannot be named John.");
    return new ValidatedCreateUserTodo()
    {
        Name = NonEmptyString(request.Name),
        Priority = GreaterThanZeroInt.Create(request.Priority),
    };
}
```

The `ValidateCreateTodoExceptions` function guarantees that the object is valid by stopping execution at first failure with an exception.
This code is great because it is easy to read and understand, it composes very well, and validation occurs through simple function calls.
Nothing fancy here.

However, it has a major drawback. It only reports the first failure.
If the name is `null` and the priority is `-1` we will only learn about the name, but we would like to know about the priority as well.
This is what this library attempts to solve while allowing for the same simple and composable validation code.

The previous code would convert into this code:

```C#
public class GreaterThanZeroInt {
    public int Value { get; private set; }
    
    public static GreaterThanZeroInt Create(IFailValidation fail, int value) {
        if (value <= 0)
            return fail.Fail<GreaterThanZeroInt>($"Value must be greater than zero.");
        
        return new GreaterThanZeroInt { Value = value };
    }
}
public static string NonEmptyString(IFailValidation fail, string? value)
{
    if (string.IsNullOrEmpty(value)) {
        fail.Fail($"Value must be non-empty.");
        return string.Empty;
    }

    return value;
}
public static ValidatedCreateTodo ValidateCreateTodo(IFailValidation fail, CreateTodo request)
{
    if (request.Name == "John")
        fail.Fail($"Cannot be named John.");
    return new ValidatedCreateUserTodo()
    {
        Name = NonEmptyString(request.Name),
        Priority = GreaterThanZeroInt.Create(request.Priority),
    };
}
```

For the most part this code reads the same as the previous code. The important difference is that our functions now need to handle
returning a default value when validation fails, and we can't rely on exceptions to stop our functions partway through.

### Failure modes

The `IFailValidation` object allows the caller of a validator to decide how to receive validation failures.
For example, only the first failure, or all failures? Throwing an exception, or returning a Result type?

All of these are possible with different IFailValidation implementations.

#### Only first failure

If we truly only care about the first failure, we can trivially implement the `IFailValidation` interface to do this for us.
This is functionally equivalent to writing regular exception-throwing validation code.

```C# 
public class ThrowValidationFailure : IFailValidation
{
    public T Fail<T>(string message) => throw new ValidationException(message);
    public void Fail(string message) => throw new ValidationException(message);
}

var fail = new ThrowValidationFailure();
ValidatedCreateTodo validatedRequest = ValidateCreateTodo(fail, request);
Execute(validatedRequest);
```


#### All failures

To accumulate all failures, our failure mode implementation needs to collect up all the failures and provide a way to 
safely fail the validation process once all failures have been collected.
In this implementation I use the Disposable pattern to ensure that the composite failure exception is thrown after the 
validation function completes.

```C#
// edited for brevity. See the full implementation in the codebase.
public class ThrowManyValidationFailures : IFailValidation, IDisposable {
    public List<ValidationError> Errors { get; } = new List<ValidationError>();
    
    public T Fail<T>(string message) {
        this.Fail(message);
        return default!;
    }

    public void Fail(string message) => 
        Errors.Add(new ValidationError(message));

    public void Dispose() {
        if (Errors.Any()) throw new ValidationException(Errors);
    }
}

ValidatedCreateTodo validatedRequest;
using (var fail = new ThrowManyValidationFailures()) {
    validatedRequest = ValidateCreateTodo(fail, request);
} // throws here, when disposed

Execute(validatedRequest);
```

#### All Failures in Result Type

Very similar to the previous example, but in this case we must use a lambda because we want to modify the return type to a `ValidationResult<T>`.
This implementation should be more performant than wrapping ThrowManyValidationFailures in a try-catch because it allows us to completely
avoid exceptions!

```C#
// edited for brevity. See the full implementation in the codebase.
public class ReturnManyValidationFailures : IFailValidation
{   
    public static ValidationResult<TValidated> Validate<TValidated>(Func<IFailValidation, TValidated> create)
    {
        var fail = new ReturnManyValidationFailures();
        var validated = create(fail);
        return fail.Errors.Count == 0
            ? ValidationResult<TValidated>.Success(validated) 
            : ValidationResult<TValidated>.Fail(fail.Errors);
    }

    private List<ValidationError> Errors { get; } = new();

    public T Fail<T>(string message)
    {
        this.Fail(message);
        return default!;
    }

    public void Fail(string message) =>
        Errors.Add(new ValidationError(message));
}

ValidationResult<ValidatedCreateTodo> result = ReturnManyValidationFailures
    .Validate(fail => ValidateCreateTodo(fail, request));

if(result.IsFailure) {
    return BadRequest(result.Errors);  
}

Execute(result.Success);
```


## Conclusion

This library provides a way to accumulate all validation errors in a single pass, while still allowing for simple and composable validation code.
To dig in deeper, I recommend you start at [DocumentationTests.cs](ValidationToyTests/DocumentationTests.cs).
These are laid out to demonstrate the benefits and footguns that show up when using this library. 