using Castle.DynamicProxy;

public class MockInterceptor : StandardInterceptor
{
  private readonly List<MockInvocation> _invocations = new();

  public IReadOnlyList<MockInvocation> Invocations => _invocations;

  public Action<IInvocation>? Perform { get; set; }

  protected override void PreProceed(IInvocation invocation)
  {
    _invocations.Add(new MockInvocation(invocation));
    base.PreProceed(invocation);
  }

  protected override void PerformProceed(IInvocation invocation) => Perform?.Invoke(invocation);
}