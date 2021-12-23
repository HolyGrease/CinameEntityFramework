namespace Cinema
{
  public class Movie
  {
    public Movie()
    {
      this.Name = "Movie";
      this.Year = 2021;
    }

    public int MovieId { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
  }
}