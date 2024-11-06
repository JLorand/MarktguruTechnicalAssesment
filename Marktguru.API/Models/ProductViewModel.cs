namespace Marktguru.API.Models;

public record ProductViewModel(
    Guid Id,
    string Name,
    bool Availability,
    decimal Price,
    string Description,
    DateTime DateCreated);