namespace Cinema
{
  public class Ticket
  {
    public int TicketId { get; set; }
    public int SessionId { get; set; }
    public virtual Session Session { get; set; }
    public DateTime PurchaseTime { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }
  }
}