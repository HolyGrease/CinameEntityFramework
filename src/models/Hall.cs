namespace Cinema
{
  public class Hall
  {
    public Hall()
    {
      this.Name = "Hall";
    }

    public int HallId { get; set; }
    public string Name { get; set; }
    public int Rows { get; set; }
    public int Cols { get; set; }
  }
}