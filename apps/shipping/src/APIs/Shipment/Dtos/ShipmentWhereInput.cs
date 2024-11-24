namespace Shipping.APIs.Dtos;

public class ShipmentWhereInput
{
    public DateTime? ArrivalTime { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? DepartureTime { get; set; }

    public string? Id { get; set; }

    public List<string>? Items { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
