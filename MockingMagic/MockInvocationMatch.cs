using System.Linq.Expressions;
using System.Reflection;

public class MockInvocationMatch
{
  private readonly MethodInfo _method;
  private readonly IReadOnlyList<Func<object, bool>> _argumentValidators;

  public MockInvocationMatch(Expression expression)
  {
    var visitor = new Visitor();
    visitor.Visit(expression);
    (_method, _argumentValidators) = (visitor.Method, visitor.ArgumentValidators);
  }
  public bool IsMatch(MockInvocation invocation) => invocation.Method == _method && invocation.Arguments.Zip(_argumentValidators, (x, y) => y(x)).All(x => x);
}