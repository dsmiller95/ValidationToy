namespace DmansValidator;

public interface IFailValidation
{
    public T Fail<T>(string message);
    public void Fail(string message);
}