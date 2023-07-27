public class Executed
{
  private readonly Func<int, bool> _validate;
  private readonly string _message;

  public Executed(Func<int, bool> validate, string message)
  {
    _validate = validate;
    _message = message;
  }

  public void Verify(int count)
  {
    if (!_validate.Invoke(count))
    {
      throw new Exception($"Executed wrong number of times. Expected: {_message} Executed: {count}");
    }
  }

  public static Executed Once => new(x => x == 1, "Once");
  public static Executed Never => new(x => x == 0, "Never");
  public static Executed AtLeast(int times) => new(x => x >= times, $"At least {times}");
  public static Executed AtMost(int times) => new(x => x <= times, $"At most {times}");
}