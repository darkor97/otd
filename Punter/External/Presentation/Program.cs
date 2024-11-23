namespace Punter.Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, Punter!");
            while (true)
            {
                var rand = new Random();
                Console.WriteLine("Some numb" + rand.NextDouble());
                Thread.Sleep(rand.Next(5, 10) * 1000);
            }
        }
    }
}
