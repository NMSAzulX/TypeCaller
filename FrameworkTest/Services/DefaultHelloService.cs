namespace FrameworkTest.Services
{
    public class DefaultHelloService : IHelloServices
    {
        public override string GetHello(string name)
        {
            _typeService.Show();
            System.Console.WriteLine("Run : Contact 'Hello' and {Parameter}!");
            return "Hello " + name;
        }
    }
}
