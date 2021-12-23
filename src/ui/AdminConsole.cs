using System;
using System.Globalization;

namespace Cinema
{
  public class AdminConsole
  {
    MovieRepository movieRepository;
    SessionRepository sessionRepository;
    HallRepository hallRepository;

    public AdminConsole(MovieRepository movieRepository, SessionRepository sessionRepository, HallRepository hallRepository)
    {
      this.movieRepository = movieRepository;
      this.sessionRepository = sessionRepository;
      this.hallRepository = hallRepository;
    }

    public void mainMenu()
    {
      Console.WriteLine("МЕНЮ АДМИНИСТРАТОРА");
      Console.WriteLine("");
      Console.WriteLine("1 - Список фильмов");
      Console.WriteLine("");

      switch (ConsoleUtils.operateMenuInput(1, 1))
      {
        case 1:
          movieList();
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
        Console.WriteLine("Фильмов еще нет. Добавьте новый.");
      }

      foreach (Movie movie in movies)
      {
        Console.WriteLine($"{movie.MovieId}. {movie.Name} ({movie.Year})");
      }

      Console.WriteLine("");
      Console.WriteLine("0 - Главное меню");
      Console.WriteLine("1 - Добавить фильм");
      if (movies.Length > 0)
      {
        Console.WriteLine("2 - Изменить фильм");
        Console.WriteLine("3 - Удалить фильм");
        Console.WriteLine("4 - Перейти к сеансам");
      }
      Console.WriteLine("");

      switch (ConsoleUtils.operateMenuInput(0, movies.Length > 0 ? 4 : 1))
      {
        case 0:
          mainMenu();
          break;
        case 1:
          movieAddUpdate();
          break;
        case 2:
          Console.WriteLine("Введите id фильма для изменения:");
          movieAddUpdate(Convert.ToInt32(ConsoleUtils.validateNumberInput(movies[0].MovieId, movies[movies.Length - 1].MovieId)));
          break;
        case 3:
          Console.WriteLine("Введите id фильма для удаления:");
          movieDelete(Convert.ToInt32(ConsoleUtils.validateNumberInput(movies[0].MovieId, movies[movies.Length - 1].MovieId)));
          break;
        case 4:
          Console.WriteLine("Введите id фильма для просмотра его сеансов:");
          sessionList(Convert.ToInt32(ConsoleUtils.validateNumberInput(movies[0].MovieId, movies[movies.Length - 1].MovieId)));
          break;
      }
    }

    public void movieAddUpdate(int? id = null)
    {
      Console.WriteLine($"{(id != null ? "ИЗМЕНЕНИЕ" : "ДОБАВЛЕНИЕ")} ФИЛЬМА");
      Console.WriteLine("");

      Movie? movie = id != null ? movieRepository.getOne((int)id!) : null;

      Console.WriteLine("Введите название фильма:");
      string name = Console.ReadLine();
      if (movie?.Name != null && name == "")
      {
        name = movie.Name;
      }

      Console.WriteLine("Введите год фильма:");
      int year = Convert.ToInt32(ConsoleUtils.validateNumberInput(1800, double.PositiveInfinity, movie?.Year));

      if (id != null)
      {
        movieRepository.update((int)id!, name, year);
      }
      else
      {
        movieRepository.add(name, year);
      }

      Console.WriteLine($"Фильм был {(id != null ? "изменен" : "добавлен")}:");
      Console.WriteLine("");
      Console.WriteLine($"Название: {name}");
      Console.WriteLine($"Год: {year}");

      Console.WriteLine("");
      Console.WriteLine("0 - Главное меню");
      Console.WriteLine("1 - Список фильмов");
      Console.WriteLine("");

      switch (ConsoleUtils.operateMenuInput(0, 1))
      {
        case 0:
          mainMenu();
          break;
        case 1:
          movieList();
          break;
      }
    }

    public void movieDelete(int id)
    {
      movieRepository.delete(id);

      Console.WriteLine("Фильм был удален!");

      Console.WriteLine("");
      Console.WriteLine("0 - Главное меню");
      Console.WriteLine("1 - Список фильмов");
      Console.WriteLine("");

      switch (ConsoleUtils.operateMenuInput(0, 1))
      {
        case 0:
          mainMenu();
          break;
        case 1:
          movieList();
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
        Console.WriteLine("Сеансов на этот фильм еще нет. Добавьте новый.");
      }

      foreach (Session session in sessions)
      {
        Hall hall = hallRepository.getOne(session.HallId);
        Console.WriteLine($"{session.SessionId}. {session.Time.ToString("dd.MM.yyyy HH:mm")}, {hall.Name}  {session.Price} грн");
      }

      Console.WriteLine("");
      Console.WriteLine("0 - Главное меню");
      Console.WriteLine("1 - Список фильмов");
      Console.WriteLine("2 - Добавить сеанс");
      if (sessions.Length > 0)
      {
        Console.WriteLine("3 - Изменить сеанс");
        Console.WriteLine("4 - Удалить сеанс");
      }
      Console.WriteLine("");

      switch (ConsoleUtils.operateMenuInput(0, sessions.Length > 0 ? 4 : 2))
      {
        case 0:
          mainMenu();
          break;
        case 1:
          movieList();
          break;
        case 2:
          sessionAddUpdate(movieId);
          break;
        case 3:
          Console.WriteLine("Введите id сеанса для изменения:");
          sessionAddUpdate(movieId, Convert.ToInt32(ConsoleUtils.validateNumberInput(sessions[0].SessionId, sessions[sessions.Length - 1].SessionId)));
          break;
        case 4:
          Console.WriteLine("Введите id сеанса для удаления:");
          sessionDelete(movieId, Convert.ToInt32(ConsoleUtils.validateNumberInput(sessions[0].SessionId, sessions[sessions.Length - 1].SessionId)));
          break;
      }
    }

    public void sessionAddUpdate(int movieId, int? id = null)
    {
      Movie movie = movieRepository.getOne(movieId);

      Console.WriteLine($"{(id != null ? "ИЗМЕНЕНИЕ" : "ДОБАВЛЕНИЕ")} СЕАНСА");
      Console.WriteLine("");

      Session? session = id != null ? sessionRepository.getOne((int)id) : null;

      int hallId = session?.HallId ?? 0;
      Hall[] halls = hallRepository.getAll();
      if (id == null)
      {
        Console.WriteLine("Введите id зала");
        Console.WriteLine("");
        Console.WriteLine("Доступные залы:");
        Console.WriteLine("");

        foreach (Hall _hall in halls)
        {
          Console.WriteLine($"{_hall.HallId}. {_hall.Name}");
        }
        Console.WriteLine("");

        Console.WriteLine("Ваш выбор:");
        hallId = Convert.ToInt32(ConsoleUtils.validateNumberInput(halls[0].HallId, halls[halls.Length - 1].HallId, session?.HallId));
      }

      Console.WriteLine("Введите стоимость показа:");
      double price = ConsoleUtils.validateNumberInput(0, Double.PositiveInfinity, session?.Price);

      Console.WriteLine("Введите дату показа (формат - \"dd.MM.yyyy HH:mm\"):");
      string consoleDate = Console.ReadLine();
      if (session?.Time != null && consoleDate.Trim() == "")
      {
        consoleDate = session.Time.ToString("dd.MM.yyyy HH:mm");
      }
      DateTime time = DateTime.ParseExact(consoleDate, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);

      if (id != null)
      {
        sessionRepository.update((int)id!, movieId, hallId, time, price);
      }
      else
      {
        sessionRepository.add(movieId, hallId, time, price);
      }

      Console.WriteLine($"Сеанс был {(id != null ? "изменен" : "добавлен")}:");
      Console.WriteLine("");
      Console.WriteLine($"Дата: {time.ToString("dd.MM.yyyy HH:mm")}");
      Console.WriteLine($"Зал: {Array.Find(halls, h => h.HallId == hallId).Name}");
      Console.WriteLine($"Цена: {price}");

      Console.WriteLine("");
      Console.WriteLine("0 - Главное меню");
      Console.WriteLine("1 - Список сеансов");
      Console.WriteLine("");

      switch (ConsoleUtils.operateMenuInput(0, 1))
      {
        case 0:
          mainMenu();
          break;
        case 1:
          sessionList(movieId);
          break;
      }
    }

    public void sessionDelete(int movieId, int id)
    {
      sessionRepository.delete(id);

      Console.WriteLine("Сеанс был удален!");

      Console.WriteLine("");
      Console.WriteLine("0 - Главное меню");
      Console.WriteLine("1 - Список сеансов");
      Console.WriteLine("");

      switch (ConsoleUtils.operateMenuInput(0, 1))
      {
        case 0:
          mainMenu();
          break;
        case 1:
          sessionList(movieId);
          break;
      }
    }
  }
}