using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Infrastructure.Models;

[Table("Items")]
public class ItemDbModel
{
    [Required()]
    public DateTime CreatedAt { get; set; }

    public string? DestinationId { get; set; }

    [ForeignKey(nameof(DestinationId))]
    public DestinationDbModel? Destination { get; set; } = null;

    [Key()]
    [Required()]
    public string Id { get; set; }

    [StringLength(1000)]
    public string? Name { get; set; }

    [Range(-999999999, 999999999)]
    public int? Quantity { get; set; }

    public string? ShipmentId { get; set; }

    [ForeignKey(nameof(ShipmentId))]
    public ShipmentDbModel? Shipment { get; set; } = null;

    [Required()]
    public DateTime UpdatedAt { get; set; }
}
