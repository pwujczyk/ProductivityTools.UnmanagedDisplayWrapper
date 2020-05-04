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
            uint d = 3;
            //display.MoveExternalDisplayToLeft();
            //display.MoveExternalDisplayToRight();


            //display.MoveMainDisplayToRight();
            //display.MoveMainDisplayToLeft();
            

            //XXX xXX = new UnmanagedDisplayWrapper.XXX();
            //xXX.GetDisplays();
            //  Console.ReadLine();
        }
    }
}
