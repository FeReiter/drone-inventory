namespace DroneInventory.Api.Persistence;

public sealed class LoanItem
{
    public int Id { get; set; }
    public int LoanId { get; set; }
    public required LoanItemType Type { get; set; }
    public int Quantity { get; set; }
}

public enum LoanItemType
{
    Battery,
    Propeller
}
