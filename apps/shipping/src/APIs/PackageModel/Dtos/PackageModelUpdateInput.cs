namespace Shipping.APIs.Dtos;

public class PackageModelUpdateInput
{
    public DateTime? CreatedAt { get; set; }

    public string? Destination { get; set; }

    public string? Id { get; set; }

    public List<string>? Shipments { get; set; }

    public string? TrackingNumber { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public double? Weight { get; set; }
}
