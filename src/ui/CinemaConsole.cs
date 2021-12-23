using System;

namespace Cinema
{
  public class CinemaConsole
  {
    AdminConsole adminConsole;
    UserConsole userConsole;

    public CinemaConsole(MovieRepository movieRepository, SessionRepository sessionRepository, HallRepository hallRepository, TicketRepository ticketRepository)
    {
      this.adminConsole = new AdminConsole(movieRepository, sessionRepository, hallRepository);
      this.userConsole = new UserConsole(movieRepository, sessionRepository, hallRepository, ticketRepository);
    }

    public void mainMenu()
    {
      Console.WriteLine("ГЛАВНОЕ МЕНЮ");
      Console.WriteLine("");
      Console.WriteLine("1 - Консоль Администратора");
      Console.WriteLine("2 - Консоль Клиента");
      Console.WriteLine("");

      switch (ConsoleUtils.operateMenuInput(1, 3))
      {
        case 1:
          try
          {
            adminConsole.mainMenu();
          }
          catch (Exception e)
          {
            errorMenu(e);
          }
          break;
        case 2:
          try
          {
            userConsole.mainMenu();
          }
          catch (Exception e)
          {
            errorMenu(e);
          }
          break;
      }
    }

    public void errorMenu(Exception e)
    {
      Console.WriteLine($"Ошибка: {e.Message}");
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