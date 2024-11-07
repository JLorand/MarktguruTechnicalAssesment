namespace Marktguru.API.Models;

public record UpdateProductViewModel(
    Guid Id,
    string Name,
    bool Availability,
    decimal Price,
    string Description);