namespace Marktguru.API.Models;

public record CreateProductViewModel(
    Guid Id,
    string Name,
    bool Availability,
    decimal Price,
    string Description);