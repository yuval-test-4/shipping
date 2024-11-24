using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Infrastructure.Models;

[Table("Destinations")]
public class DestinationDbModel
{
    [StringLength(1000)]
    public string? Address { get; set; }

    [StringLength(1000)]
    public string? City { get; set; }

    [StringLength(1000)]
    public string? Country { get; set; }

    [Required()]
    public DateTime CreatedAt { get; set; }

    [Key()]
    [Required()]
    public string Id { get; set; }

    public List<ItemDbModel>? Items { get; set; } = new List<ItemDbModel>();

    [Required()]
    public DateTime UpdatedAt { get; set; }
}
