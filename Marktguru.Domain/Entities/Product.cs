using System.ComponentModel.DataAnnotations;
using Marktguru.Domain.Common;

namespace Marktguru.Domain.Entities;

public class Product : BaseEntity
{
    public required string Name { get; set; }
    public bool Availability { get; set; }
    public decimal Price { get; set; }
    public required string Description { get; set; }
    public DateTime DateCreated { get; set; }
    public uint RowVersion { get; set; }
}