using System.Reflection;
using HarmonyLib;

namespace MockingMagic;



static class HarmonySample
{
  public static void Main()
  {
    var harmony = new Harmony("com.company.project.product");

    var method = typeof(SomeUtils).GetMethod("Method", BindingFlags.Static | BindingFlags.Public);
    var prefix = typeof(HarmonyHelper).GetMethod("Prefix", BindingFlags.Static | BindingFlags.Public);
    var postfix = typeof(HarmonyHelper).GetMethod("Postfix", BindingFlags.Static | BindingFlags.Public);

    harmony.Patch(method, new HarmonyMethod(prefix), new HarmonyMethod(postfix));
    int result = SomeUtils.Method(10, 20);
    Console.WriteLine(result);
  }
}

static class SomeUtils 
{
  public static int Method(int x, int y) 
  {
    Console.WriteLine("SomeUtils.Method");
    return x + y;
  }
}

public static class HarmonyHelper 
{
  public static bool Prefix(object __result, object __state) 
  {
    Console.WriteLine(__result);
    return false;
  }

  public static int Postfix(int __result, object __state) 
  {
    Console.WriteLine(__result);
    return 1;
  }
}
