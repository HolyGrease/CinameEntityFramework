using System;
using System.Globalization;

namespace Cinema
{
  public class UserConsole
  {
    MovieRepository movieRepository;
    SessionRepository sessionRepository;
    HallRepository hallRepository;
    TicketRepository ticketRepository;

    public UserConsole(MovieRepository movieRepository, SessionRepository sessionRepository, HallRepository hallRepository, TicketRepository ticketRepository)
    {
      this.movieRepository = movieRepository;
      this.sessionRepository = sessionRepository;
      this.hallRepository = hallRepository;
      this.ticketRepository = ticketRepository;
    }

    public void mainMenu()
    {
      Console.WriteLine("МЕНЮ КЛИЕНТА");
      Console.WriteLine("");
      Console.WriteLine("1 - Список фильмов");
      Console.WriteLine("2 - Мои билеты");
      Console.WriteLine("");

      switch (ConsoleUtils.operateMenuInput(1, 2))
      {
        case 1:
          movieList();
          break;
        case 2:
          myTicketList();
          break;
      }
    }

    public void movieList()
    {
      Console.WriteLine("СПИСОК ФИЛЬМОВ");
      Console.WriteLine("");

      Movie[] movies = movieRepository.getAll();

      if (movies.Length == 0)
      {
        Console.WriteLine("Фильмов еще нет.");
      }

      foreach (Movie movie in movies)
      {
        Console.WriteLine($"{movie.MovieId}. {movie.Name} ({movie.Year})");
      }

      Console.WriteLine("");
      Console.WriteLine("0 - Главное меню");
      if (movies.Length > 0)
      {
        Console.WriteLine("1 - Перейти к сеансам");
      }
      Console.WriteLine("");

      switch (ConsoleUtils.operateMenuInput(0, movies.Length > 0 ? 1 : 0))
      {
        case 0:
          mainMenu();
          break;
        case 1:
          Console.WriteLine("Введите id фильма для просмотра его сеансов:");
          sessionList(Convert.ToInt32(ConsoleUtils.validateNumberInput(movies[0].MovieId, movies[movies.Length - 1].MovieId)));
          break;
      }
    }

    public void sessionList(int movieId)
    {
      Movie movie = movieRepository.getOne(movieId);

      Console.WriteLine($"Сеансы фильма \"{movie.Name}\"");
      Console.WriteLine("");

      Session[] sessions = sessionRepository.getAll(movieId);

      if (sessions.Length == 0)
      {
        Console.WriteLine("Сеансов на этот фильм еще нет.");
      }

      foreach (Session session in sessions)
      {
        Hall hall = hallRepository.getOne(session.HallId);
        Console.WriteLine($"{session.SessionId}. {session.Time.ToString("dd.MM.yyyy HH:mm")}, {hall.Name}  {session.Price} грн");
      }

      Console.WriteLine("");
      Console.WriteLine("0 - Главное меню");
      Console.WriteLine("1 - Список фильмов");
      if (sessions.Length > 0)
      {
        Console.WriteLine("2 - Купить билет");
      }
      Console.WriteLine("");

      switch (ConsoleUtils.operateMenuInput(0, sessions.Length > 0 ? 2 : 1))
      {
        case 0:
          mainMenu();
          break;
        case 1:
          movieList();
          break;
        case 2:
          Console.WriteLine("Введите id сеанса для покупки билета:");
          ticketList(movieId, Convert.ToInt32(ConsoleUtils.validateNumberInput(sessions[0].SessionId, sessions[sessions.Length - 1].SessionId)));
          break;
      }
    }

    public void ticketList(int movieId, int sessionId)
    {
      Movie movie = movieRepository.getOne(movieId);
      Session session = sessionRepository.getOne(sessionId);

      Console.WriteLine($"Билеты на фильм \"{movie.Name}\" ({session.Time.ToString("dd.MM.yyyy HH:mm")})");
      Console.WriteLine("");

      Hall hall = hallRepository.getOne(session.HallId);

      int[][] chosenPlaces = ticketRepository.getAll(sessionId).Select(t => new int[2] { t.Row, t.Col }).ToArray();

      if (chosenPlaces.Length == hall.Cols * hall.Rows)
      {
        Console.WriteLine("Мест на сеанс больше не осталось.");
      }
      else
      {
        for (int i = 1; i <= hall.Rows; i++)
        {
          Console.Write("{0}\t", $"({i})");
          for (int k = 1; k <= hall.Cols; k++)
          {
            if (chosenPlaces.Any(x => x[0] == i && x[1] == k))
            {
              Console.Write("{0}\t", $"|X|");
            }
            else
            {
              Console.Write("{0}\t", $"{k}");
            }
          }
          Console.WriteLine();
        }

        Console.WriteLine("");
        Console.WriteLine("Выберите ряд и место");
        Console.WriteLine("");
        Console.WriteLine("Номер ряда:");
        int row = Convert.ToInt32(ConsoleUtils.validateNumberInput(1, hall.Rows));
        Console.WriteLine("Номер места:");
        int col = Convert.ToInt32(ConsoleUtils.validateNumberInput(1, hall.Cols));

        ticketRepository.add(sessionId, row, col);

        Console.WriteLine("");
        Console.WriteLine("Ваш билет готов!");
        Console.WriteLine("");
        Console.WriteLine($"Ряд: {row}");
        Console.WriteLine($"Место: {col}");

        Console.WriteLine("");
        Console.WriteLine("0 - Главное меню");
        Console.WriteLine("1 - Мои билеты");
        Console.WriteLine("");

        switch (ConsoleUtils.operateMenuInput(0, 1))
        {
          case 0:
            mainMenu();
            break;
          case 1:
            myTicketList();
            break;
        }
      }
    }

    public void myTicketList()
    {
      Console.WriteLine($"МОИ БИЛЕТЫ");
      Console.WriteLine("");

      Ticket[] tickets = ticketRepository.getAll();

      if (tickets.Length == 0)
      {
        Console.WriteLine("Билетов еще нет.");
      }

      foreach (Ticket ticket in tickets)
      {
        Session session = sessionRepository.getOne(ticket.SessionId);
        Movie movie = movieRepository.getOne(session.MovieId);
        Hall hall = hallRepository.getOne(session.HallId);
        Console.WriteLine($"{ticket.TicketId}. {movie.Name}, {session.Time.ToString("dd.MM.yyyy HH:mm")}, {hall.Name}  {session.Price} грн");
      }

      Console.WriteLine("");
      Console.WriteLine("0 - Главное меню");
      Console.WriteLine("");

      switch (ConsoleUtils.operateMenuInput(0, 0))
      {
        case 0:
          mainMenu();
          break;
      }
    }
  }
}