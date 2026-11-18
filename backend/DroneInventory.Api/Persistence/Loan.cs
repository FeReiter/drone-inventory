namespace DroneInventory.Api.Persistence;

public sealed class Loan
{
    public int Id { get; set; }
    public int DroneId { get; set; }
    public int StudentGroupId { get; set; }
    public DateOnly LoanDate { get; set; }
    public DateOnly? ReturnDate { get; set; }
    public required LoanStatus Status { get; set; }
}

public enum LoanStatus
{
    Active,
    Returned
}
