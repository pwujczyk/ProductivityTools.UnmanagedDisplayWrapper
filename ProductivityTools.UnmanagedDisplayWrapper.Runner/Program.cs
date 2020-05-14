using System;

namespace ProductivityTools.UnmanagedDisplayWrapper.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var display = new Displays();
            display.LoadData();

            display.MoveExternalDisplayToLeft();
            //display.MoveExternalDisplayToRight();


            //display.MoveMainDisplayToRight();
            //display.MoveMainDisplayToLeft();
            

            //  Console.ReadLine();
        }
    }
}
