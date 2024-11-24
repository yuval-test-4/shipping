using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Infrastructure.Models;

[Table("PackageModels")]
public class PackageModelDbModel
{
    [Required()]
    public DateTime CreatedAt { get; set; }

    public string? DestinationId { get; set; }

    [ForeignKey(nameof(DestinationId))]
    public DestinationDbModel? Destination { get; set; } = null;

    [Key()]
    [Required()]
    public string Id { get; set; }

    public List<ShipmentDbModel>? Shipments { get; set; } = new List<ShipmentDbModel>();

    [StringLength(1000)]
    public string? TrackingNumber { get; set; }

    [Required()]
    public DateTime UpdatedAt { get; set; }

    [Range(-999999999, 999999999)]
    public double? Weight { get; set; }
}
