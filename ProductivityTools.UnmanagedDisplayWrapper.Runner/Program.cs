using System;

namespace ProductivityTools.UnmanagedDisplayWrapper.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var d = new Displays();
            d.ChangePosition(1, 1);
            d.LoadData();
            XXX xXX = new UnmanagedDisplayWrapper.XXX();
            xXX.GetDisplays();
            Console.ReadLine();
        }
    }
}
