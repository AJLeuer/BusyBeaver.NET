using System;
using System.CodeDom.Compiler;
using BusyBeaver.NET.Models;
using Mono.CSharp;

namespace BusyBeaver.NET
{
    internal static class MainClass
    {
        public static void RunBusyBeaverProgram()
        {
            var tm = new TuringMachine();

            BusyBeaverProgram busyBeaver = new FourStateBusyBeaver();

            tm.run(busyBeaver, Console.Out);

            Console.WriteLine("Program counter: " + tm.programCounter);
        }
        
        public static void Main() {
            RunBusyBeaverProgram();
        }
        
    }
}