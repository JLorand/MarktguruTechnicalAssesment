namespace Marktguru.Application.Common.Exceptions;

public class NotFoundException(Guid id) : Exception($"The requested resource with ID={id} was not found.")
{
}