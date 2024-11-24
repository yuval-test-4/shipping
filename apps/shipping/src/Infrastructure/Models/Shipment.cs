using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Infrastructure.Models;

[Table("Shipments")]
public class ShipmentDbModel
{
    public DateTime? ArrivalTime { get; set; }

    [Required()]
    public DateTime CreatedAt { get; set; }

    public DateTime? DepartureTime { get; set; }

    [Key()]
    [Required()]
    public string Id { get; set; }

    public string? PackageFieldId { get; set; }

    [ForeignKey(nameof(PackageFieldId))]
    public PackageModelDbModel? PackageField { get; set; } = null;

    [Required()]
    public DateTime UpdatedAt { get; set; }
}