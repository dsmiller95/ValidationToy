# Validation Toy

This repository explores ways to perform object validation as part of object construction.
The goal is to capture all the benefits of throwing exceptions when validation fails, while also providing a way to accumulate all validation errors in a single pass.


## Example


with exceptions, we might write code like this:
```C#
public static ValidatedCreateTodo Create(CreateTodo request)
{
    if (request.Priority <= 0)
        throw new ArgumentException($"priority must be greater than zero.");
    if (string.isNullOrEmpty(request.Name))
        throw new ArgumentException($"Name must be non-empty.");
    return new ValidatedCreateUserTodo()
    {
        Name = request.Name,
        Priority = request.Priority,
    };
}
```

With this library, instead we may write:

```C#
public static GreaterThanZeroInt Create(IFailValidation fail, int value, string attributeName)
{
    if (value <= 0) return fail.Fail<GreaterThanZeroInt>($"{attributeName} must be greater than zero.");
    
    return new GreaterThanZeroInt { Value = value };
}
public static ValidatedCreateUserTodo Create(IFailValidation fail, CreateUserTodo request)
{
    return new ValidatedCreateUserTodo()
    {
        Name = StringValidators.NonEmptyString(fail, request.Name, nameof(request.Name)),
        Priority = GreaterThanZeroInt.Create(fail, request.Priority, nameof(request.Priority)),
    };
}
```