namespace Shipping.APIs.Dtos;

public class ItemCreateInput
{
    public DateTime CreatedAt { get; set; }

    public Destination? Destination { get; set; }

    public string? Id { get; set; }

    public string? Name { get; set; }

    public int? Quantity { get; set; }

    public Shipment? Shipment { get; set; }

    public DateTime UpdatedAt { get; set; }
}
