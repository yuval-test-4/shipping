namespace Shipping.APIs.Dtos;

public class ShipmentCreateInput
{
    public DateTime? ArrivalTime { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DepartureTime { get; set; }

    public string? Id { get; set; }

    public PackageModel? PackageField { get; set; }

    public DateTime UpdatedAt { get; set; }
}
