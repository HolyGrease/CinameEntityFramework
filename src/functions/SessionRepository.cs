namespace Cinema
{
  public class SessionRepository
  {
    CinemaContext db;
    public SessionRepository(CinemaContext db)
    {
      this.db = db;
    }

    public Session[] getAll(int movieId)
    {
      return db.Sessions.Where(s => s.MovieId == movieId).ToArray();
    }

    public Session getOne(int id)
    {
      if (!db.Sessions.Any(r => r.SessionId == id))
      {
        throw new Exception("Некорректный id.");
      }

      Session? session = db.Sessions.Find(id);
      if (session == null)
      {
        throw new Exception("Сеанс не найден.");
      }

      return session!;
    }

    public Session add(int movieId, int hallId, DateTime time, double price)
    {
      Movie? movie = db.Movies.Find(movieId);
      if (movie == null)
      {
        throw new Exception("Фильм не найден. Укажите другой id.");
      }

      Hall? hall = db.Halls.Find(hallId);
      if (hall == null)
      {
        throw new Exception("Зал не найден. Укажите другой id.");
      }

      if (time.Year < movie.Year)
      {
        throw new Exception("Некорректно указана дата сеанса.");
      }

      if (price < 0)
      {
        throw new Exception("Некорректно указана цена сеанса");
      }

      Session session = new Session() { MovieId = movieId, Movie = movie, Hall = hall, HallId = hallId, Time = time, Price = price };

      db.Sessions.Add(session);
      db.SaveChanges();

      return session;
    }

    public Session update(int id, int? movieId = null, int? hallId = null, DateTime? time = null, double? price = null)
    {
      Movie? movie = null;
      if (movieId != null)
      {
        movie = db.Movies.Find(movieId);
        if (movie == null)
        {
          throw new Exception("Фильм не найден. Укажите другой id.");
        }
      }

      Hall? hall = null;
      if (hallId != null)
      {
        hall = db.Halls.Find(hallId);
        if (hall == null)
        {
          throw new Exception("Зал не найден. Укажите другой id.");
        }
      }

      if (time != null && ((DateTime)time).Year < movie!.Year)
      {
        throw new Exception("Некорректно указана дата сеанса.");
      }

      if (price != null && price < 0)
      {
        throw new Exception("Некорректно указана цена сеанса");
      }

      Session? session = db.Sessions.Find(id);
      if (session == null)
      {
        throw new Exception("Сеанс не найден.");
      }

      if (movieId != null)
      {
        session.MovieId = (int)movieId;
        session.Movie = movie!;
      }
      if (hallId != null)
      {
        session.HallId = (int)hallId;
        session.Hall = hall!;
      }
      if (time != null)
      {
        session.Time = (DateTime)time;
      }
      if (price != null)
      {
        session.Price = (int)price;
      }

      db.Sessions.Update(session);
      db.SaveChanges();

      return session;
    }

    public void delete(int id)
    {
      Session? session = db.Sessions.Find(id);
      if (session == null)
      {
        throw new Exception("Сеанс не найден.");
      }

      db.Tickets.RemoveRange(db.Tickets.Where(r => r.SessionId == id));
      db.Sessions.Remove(session);
      db.SaveChanges();
    }
  }
}