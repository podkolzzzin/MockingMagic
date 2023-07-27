using System.Collections.Immutable;
using System.Reflection;
using Castle.DynamicProxy;

public class MockInvocation
{
  public MockInvocation(IInvocation invocation)
  {
    Arguments = invocation.Arguments.ToImmutableArray();
    Method = invocation.Method;
  }

  public MethodInfo Method { get; }
  public ImmutableArray<object> Arguments { get; }
}