namespace Shipping.APIs.Dtos;

public class Item
{
    public DateTime CreatedAt { get; set; }

    public string? Destination { get; set; }

    public string Id { get; set; }

    public string? Name { get; set; }

    public int? Quantity { get; set; }

    public string? Shipment { get; set; }

    public DateTime UpdatedAt { get; set; }
}
