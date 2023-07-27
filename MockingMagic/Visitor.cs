using System.Linq.Expressions;
using System.Reflection;

public class Visitor : ExpressionVisitor
{
  private static readonly MethodInfo IsMethod = typeof(Arg).GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.Name == "Is");
  public MethodInfo Method { get; private set; }
  public IReadOnlyList<Func<object, bool>> ArgumentValidators { get; private set; }
  protected override Expression VisitMethodCall(MethodCallExpression node)
  {
    Method = node.Method;
    var argumentValidators = new List<Func<object, bool>>();
    foreach (var argument in node.Arguments)
    {
      if (argument.NodeType == ExpressionType.Constant)
      {
        argumentValidators.Add(x => x.Equals(((ConstantExpression)argument).Value));
      }
      else if (argument.NodeType == ExpressionType.Call)
      {
        VisitItExpression(argumentValidators, (MethodCallExpression)argument);
      }
    }

    ArgumentValidators = argumentValidators;
    return node;
  }

  private void VisitItExpression(List<Func<object, bool>> argumentValidators, MethodCallExpression argument)
  {
    if (argument.Method.DeclaringType != typeof(Arg))
    {
      throw new InvalidOperationException("Only Arg.Is* is supported");
    }
    else if (argument.Method.IsGenericMethod)
    {
      var genericMethodDefinition = argument.Method.GetGenericMethodDefinition();
      if (genericMethodDefinition == IsMethod)
      {
        argumentValidators.Add(VisitIsMethod(argument));
      }
      // Handle Other Methods...
    }
  }

  private Func<object, bool> VisitIsMethod(MethodCallExpression argument)
  {
    var arg = (LambdaExpression)argument.Arguments.Single(); // We know that Arg.Is has only one argument
    return BuildArgumentValidator(arg.Parameters.Single().Type, arg);
  }

  private Func<object, bool> BuildArgumentValidator(Type castToType, Expression validateExpression)
  {
    var arg = Expression.Parameter(typeof(object));
    var casted = Expression.Convert(arg, castToType);
    var body = Expression.Invoke(validateExpression, casted);
    return Expression.Lambda<Func<object, bool>>(body, arg).Compile();
  }
}