using Castle.DynamicProxy;

public class ProxyFactory
{
  private readonly ProxyGenerator _generator = new();

  public T GetProxy<T>(IInterceptor interceptor) where T : class => _generator.CreateInterfaceProxyWithoutTarget<T>(interceptor);
}