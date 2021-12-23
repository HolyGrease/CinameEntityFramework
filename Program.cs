using System;
using System.Linq;

namespace Cinema
{
  internal class Program
  {

    private static void Main()
    {
      using (var db = new CinemaContext())
      {
        MovieRepository movieRepository = new MovieRepository(db);
        HallRepository hallRepository = new HallRepository(db);
        SessionRepository sessionRepository = new SessionRepository(db);
        TicketRepository ticketRepository = new TicketRepository(db);

        CinemaConsole cinemaConsole = new CinemaConsole(movieRepository, sessionRepository, hallRepository, ticketRepository);

        cinemaConsole.mainMenu();
      }
    }
  }
}