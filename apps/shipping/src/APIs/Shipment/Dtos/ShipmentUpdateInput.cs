namespace Shipping.APIs.Dtos;

public class ShipmentUpdateInput
{
    public DateTime? ArrivalTime { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? DepartureTime { get; set; }

    public string? Id { get; set; }

    public string? PackageField { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
