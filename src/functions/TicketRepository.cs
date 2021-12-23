namespace Cinema
{
  public class TicketRepository
  {
    CinemaContext db;
    public TicketRepository(CinemaContext db)
    {
      this.db = db;
    }

    public Ticket[] getAll(int? sessionId = null)
    {
      if (sessionId != null)
      {
        return db.Tickets.Where(t => t.SessionId == sessionId!).ToArray();
      }
      return db.Tickets.ToArray();
    }

    public Ticket getOne(int id)
    {
      if (!db.Tickets.Any(r => r.TicketId == id))
      {
        throw new Exception("Некорректный id.");
      }

      Ticket? ticket = db.Tickets.Find(id);
      if (ticket == null)
      {
        throw new Exception("Билет не найден.");
      }

      return ticket!;
    }

    public Ticket add(int sessionId, int row, int col)
    {
      Session? session = db.Sessions.Find(sessionId);
      if (session == null)
      {
        throw new Exception("Сеанс не найден. Укажите другой id.");
      }

      Hall? hall = db.Halls.Find(session.HallId);
      if (hall == null)
      {
        throw new Exception("Зал не найден.");
      }

      if (hall.Rows < row)
      {
        throw new Exception("Некорректно указан ряд.");
      }

      if (hall.Cols < col)
      {
        throw new Exception("Некорректно указано место в ряду.");
      }

      if (db.Tickets.Any(r => r.Row == row && r.Col == col))
      {
        throw new Exception("Место уже занято! Выберите другое.");
      }

      Ticket ticket = new Ticket() { SessionId = sessionId, Session = session, PurchaseTime = DateTime.Now, Row = row, Col = col };

      db.Tickets.Add(ticket);
      db.SaveChanges();

      return ticket;
    }

    public void delete(int id)
    {
      Ticket? ticket = db.Tickets.Find(id);
      if (ticket == null)
      {
        throw new Exception("Билет не найден.");
      }

      db.Tickets.Remove(ticket);
      db.SaveChanges();
    }
  }
}