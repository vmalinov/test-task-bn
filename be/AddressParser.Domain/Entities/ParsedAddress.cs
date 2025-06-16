namespace AddressParser.Domain.Entities;

public class ParsedAddress
{
    public Guid Id { get; set; }
    public string RawInput { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string Region { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string HouseNumber { get; set; } = default!;
    public string? OfficeOrRoom { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid Version { get; set; } = default!;
}