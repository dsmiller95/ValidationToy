namespace DmansValidator;

public class FailValidation : IFailValidation
{
    public T Fail<T>(string message) => throw new ValidationException(message);
    public void Fail(string message) => throw new ValidationException(message);
}