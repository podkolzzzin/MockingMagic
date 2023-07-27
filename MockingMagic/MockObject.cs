using System.Linq.Expressions;

public class MockObject<T> where T : class
{
  private readonly Lazy<T> _lazy;
  private readonly MockInterceptor _interceptor = new();

  public MockObject()
  {
    _lazy = new Lazy<T>(CreateInstance);
  }

  public SetupVoidMethodBuilder<T> Setup(Expression<Action<T>> action) => new(_interceptor, action);
  public SetupFuncMethodBuilder<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> action) => new(_interceptor, action);

  public void Verify(Expression<Action<T>> action, Executed times)
  {
    var invocationMatch = new MockInvocationMatch(action);
    times.Verify(_interceptor.Invocations.Count(x => invocationMatch.IsMatch(x)));
  }

  public T Object => _lazy.Value;

  private T CreateInstance() => FactoryProvider.Factory.GetProxy<T>(_interceptor);
}