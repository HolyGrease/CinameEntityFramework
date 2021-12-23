namespace Cinema
{
  public class ConsoleUtils
  {
    static public double validateNumberInput(double? min = null, double? max = null, double? def = null)
    {
      double? input = null;

      do
      {
        try
        {
          string line = Console.ReadLine();

          if (line.Trim() == "" && def != null)
          {
            return def ?? 0;
          }

          double num = double.Parse(line);
          input = num;

          if (input == null || (min != null && input < min) || (max != null && input > max))
          {
            throw new Exception();
          }

          Console.WriteLine("");

          return num;
        }
        catch
        {
          Console.WriteLine("Неверно указан номер. Попробуйте еще раз.");
          continue;
        }
      } while (true);
    }

    static public int operateMenuInput(int min, int max)
    {
      Console.WriteLine("Выберите пункт меню:");
      return Convert.ToInt32(validateNumberInput(min, max));
    }
  }
}