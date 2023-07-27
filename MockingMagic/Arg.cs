public class Arg
{
  public static T IsAny<T>() => throw new InvalidOperationException();
  public static T Is<T>(Func<T, bool> matcher) => throw new InvalidOperationException();
}