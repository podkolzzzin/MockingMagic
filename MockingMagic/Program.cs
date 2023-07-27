//#r "nuget: Castle.Core"

using MockingMagic;

//HarmonySample.Main();
var mock = new MockObject<IFoo2>();
mock.Setup(x => x.M(Arg.Is<string>(y => y == "test"))).Returns(42);

IFoo2 service = mock.Object;
Console.WriteLine(service.M("test"));

mock.Verify(s => s.M(Arg.Is<string>(y => y == "test")), Executed.Once);

class F {}
 
public interface IFoo2
{
  int M(string arg);
}