using System;
using System.Collections.Generic;
using BusyBeaver.NET.Models;
using NUnit.Framework;
using State = BusyBeaver.NET.Models.TuringMachine.State;

namespace BusyBeaver.NET.Test.Models
{
    [TestFixture]
    public static class TuringMachineTest
    {
        [Test]
        public static void ExecutesInstruction()
        {
            TuringMachine.Instruction instruction = new TuringMachine.Instruction('␇', TuringMachine.DirectionToMove.RIGHT,
                TuringMachine.State.C);
            
            TuringMachine turingMachine = new TuringMachine();
            uint startingPosition = turingMachine.currentHeadPosition;
            
            turingMachine.execute(instruction);
            
            Assert.AreEqual('0', turingMachine.symbolUnderHead);
            Assert.AreEqual(startingPosition + 1, turingMachine.currentHeadPosition);
            Assert.AreEqual(TuringMachine.State.C, turingMachine.state);
        }

        [Test]
        public static void RunsAndHalts()
        {
            TuringMachine.Instruction instructionForSymbol0 = new TuringMachine.Instruction('1', TuringMachine.DirectionToMove.RIGHT,
                TuringMachine.State.B);
            TuringMachine.Instruction instructionForSymbol1 = new TuringMachine.Instruction('0', TuringMachine.DirectionToMove.RIGHT,
                TuringMachine.State.HALT);
            
            InstructionTable instructionTable =
                new Dictionary<TuringMachine.State, ValueTuple<TuringMachine.Instruction, TuringMachine.Instruction>>
                {
                    {State.A, (instructionForSymbol0, instructionForSymbol1)},
                    {State.B, (instructionForSymbol1, instructionForSymbol1)}
                };
            
            TuringMachine turingMachine = new TuringMachine();

            uint startingHeadPosition = turingMachine.currentHeadPosition;
            
            turingMachine.run(new BusyBeaverProgram(instructionTable));

            Assert.AreEqual('0', turingMachine.symbolUnderHead);
            Assert.AreEqual(startingHeadPosition + 2, turingMachine.currentHeadPosition);
            Assert.AreEqual(State.HALT, turingMachine.state);
        }
    }
}