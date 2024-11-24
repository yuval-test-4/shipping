namespace Shipping.APIs.Dtos;

public class Destination
{
    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Id { get; set; }

    public List<string>? Packages { get; set; }

    public DateTime UpdatedAt { get; set; }
}
