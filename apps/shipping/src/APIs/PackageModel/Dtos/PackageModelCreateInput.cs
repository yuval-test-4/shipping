namespace Shipping.APIs.Dtos;

public class PackageModelCreateInput
{
    public DateTime CreatedAt { get; set; }

    public Destination? Destination { get; set; }

    public string? Id { get; set; }

    public List<Shipment>? Shipments { get; set; }

    public string? TrackingNumber { get; set; }

    public DateTime UpdatedAt { get; set; }

    public double? Weight { get; set; }
}
