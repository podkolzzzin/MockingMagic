using System.Linq.Expressions;
using Castle.DynamicProxy;

public class SetupFuncMethodBuilder<T, TResult> : SetupVoidMethodBuilder<T>
{
  private TResult? _result;

  public SetupFuncMethodBuilder(MockInterceptor interceptor, Expression<Func<T, TResult>> action)
    : base(interceptor, new MockInvocationMatch(action))
  {
  }

  public void Returns(TResult value)
  {
    _result = value;
  }

  protected override void Execute(IInvocation invocation)
  {
    base.Execute(invocation);
    if (_result != null)
    {
      invocation.ReturnValue = _result;
    }
  }
}