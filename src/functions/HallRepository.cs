namespace Cinema
{
  public class HallRepository
  {
    CinemaContext db;
    public HallRepository(CinemaContext db)
    {
      this.db = db;
    }

    public Hall[] getAll()
    {
      return db.Halls.ToArray();
    }

    public Hall getOne(int id)
    {
      if (!db.Halls.Any(r => r.HallId == id))
      {
        throw new Exception("Некорректный id.");
      }

      Hall? hall = db.Halls.Find(id);
      if (hall == null)
      {
        throw new Exception("Зал не найден.");
      }

      return hall!;
    }
  }
}