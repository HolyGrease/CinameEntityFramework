using Microsoft.EntityFrameworkCore;

namespace Cinema
{
  public class CinemaContext : DbContext
  {
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Hall> Halls { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    public string DbPath { get; }

    public CinemaContext()
    {
      var folder = Environment.SpecialFolder.LocalApplicationData;
      var path = Environment.GetFolderPath(folder);
      DbPath = System.IO.Path.Join(path, "cinema.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Hall>().HasData(
        new Hall { HallId = 1, Name = "Зал А", Rows = 5, Cols = 4 },
        new Hall { HallId = 2, Name = "Зал Б", Rows = 20, Cols = 20 }
        );
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
  }
}