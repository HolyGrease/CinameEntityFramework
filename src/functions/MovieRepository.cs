namespace Cinema
{
  public class MovieRepository
  {
    CinemaContext db;
    public MovieRepository(CinemaContext db)
    {
      this.db = db;
    }

    public Movie[] getAll()
    {
      return db.Movies.ToArray();
    }

    public Movie getOne(int id)
    {
      if (!db.Movies.Any(r => r.MovieId == id))
      {
        throw new Exception("Некорректный id.");
      }

      Movie? movie = db.Movies.Find(id);
      if (movie == null)
      {
        throw new Exception("Фильм не найден.");
      }

      return movie!;
    }

    public Movie add(string name, int year)
    {
      if (name.Trim() == "")
      {
        throw new Exception("Некорректное название фильма.");
      }

      if (year < 1800)
      {
        throw new Exception("Некорректный год фильма (требуется > 1800).");
      }

      Movie movie = new Movie() { Name = name, Year = year };

      db.Movies.Add(movie);
      db.SaveChanges();

      return movie;
    }

    public Movie update(int id, string? name = null, int? year = null)
    {
      if (name != null && name.Trim() == "")
      {
        throw new Exception("Некорректное название фильма.");
      }

      if (year != null && year < 1800)
      {
        throw new Exception("Некорректный год фильма (требуется > 1800).");
      }

      Movie? movie = db.Movies.Find(id);
      if (movie == null)
      {
        throw new Exception("Фильм не найден.");
      }

      if (name != null)
      {
        movie.Name = (string)name;
      }
      if (year != null)
      {
        movie.Year = (int)year;
      }

      db.Movies.Update(movie);
      db.SaveChanges();

      return movie;
    }

    public void delete(int id)
    {
      Movie? movie = db.Movies.Find(id);
      if (movie == null)
      {
        throw new Exception("Фильм не найден.");
      }

      Session[] sessions = db.Sessions.Where(s => s.MovieId == id).ToArray();
      foreach (Session session in sessions)
      {
        db.Tickets.RemoveRange(db.Tickets.Where(r => r.SessionId == session.SessionId));
      }
      db.Sessions.RemoveRange(db.Sessions.Where(r => r.MovieId == id));
      db.Movies.Remove(movie);
      db.SaveChanges();
    }
  }
}