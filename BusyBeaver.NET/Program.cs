using System;
using BusyBeaver.NET.Models;

namespace BusyBeaver.NET
{
    internal static class MainClass
    {
        public static void Main()
        {
            var tm = new TuringMachine();

            BusyBeaverProgram busyBeaver = new FourStateBusyBeaver();

            tm.run(busyBeaver, Console.Out);

            Console.WriteLine("Program counter: " + tm.programCounter);
        }
    }
}