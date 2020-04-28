using System;

namespace ProductivityTools.UnmanagedDisplayWrapper.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var d = new Display();
            d.LoadData();
            Console.ReadLine();
        }
    }
}
