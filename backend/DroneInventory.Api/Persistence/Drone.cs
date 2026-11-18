namespace DroneInventory.Api.Persistence;

public sealed class Drone
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
