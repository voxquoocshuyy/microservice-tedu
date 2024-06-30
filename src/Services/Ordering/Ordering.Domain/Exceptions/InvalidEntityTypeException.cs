namespace Ordering.Domain.Exceptions;

public class InvalidEntityTypeException : ApplicationException
{
    public InvalidEntityTypeException(string name, object type)
        : base($"Entity \"{name}\" not supported type: {type}.")
    {
    }
}