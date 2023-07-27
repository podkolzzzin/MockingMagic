using System.Linq.Expressions;
using Castle.DynamicProxy;

public class SetupVoidMethodBuilder<T>
{
  private readonly MockInterceptor _interceptor;
  private Exception? _expressionToThrow;

  protected SetupVoidMethodBuilder(MockInterceptor interceptor, MockInvocationMatch invocationMatch)
  {
    _interceptor = interceptor;
    _interceptor.Perform += invocation =>
    {
      if (invocationMatch.IsMatch(new MockInvocation(invocation)))
      {
        Execute(invocation);
      }
    };
  }
  protected virtual void Execute(IInvocation invocation)
  {
    if (_expressionToThrow != null)
      throw _expressionToThrow;
  }

  public SetupVoidMethodBuilder(MockInterceptor interceptor, Expression<Action<T>> action)
    : this(interceptor, new MockInvocationMatch(action))
  {
  }

  public void Throws(Exception exception) => _expressionToThrow = exception;
}