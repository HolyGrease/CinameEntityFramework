namespace Cinema
{
  public class Session
  {
    public int SessionId { get; set; }
    public double Price { get; set; }
    public DateTime Time { get; set; }
    public int MovieId { get; set; }
    public virtual Movie Movie { get; set; }
    public int HallId { get; set; }
    public virtual Hall Hall { get; set; }
  }
}