using System;
using System.Collections.Generic;

namespace BusyBeaver.NET.Models
{
    
    using State = TuringMachine.State; 
    using Instruction = TuringMachine.Instruction;
    using Direction = TuringMachine.DirectionToMove; 
    
    public class FourStateBusyBeaver : BusyBeaverProgram
    {
        private static readonly InstructionTable defaultInstructionTable;

        static FourStateBusyBeaver()
        {            
            var defaultInstructionTableState = 
                new Dictionary<State, ValueTuple<Instruction, Instruction>>
                {
                    { State.A, (new Instruction('1', Direction.RIGHT, State.B),    new Instruction('1', Direction.LEFT,  State.B)) },
                    { State.B, (new Instruction('1', Direction.LEFT,  State.A),    new Instruction('0', Direction.LEFT,  State.C)) },
                    { State.C, (new Instruction('1', Direction.RIGHT, State.HALT), new Instruction('1', Direction.LEFT,  State.D)) },
                    { State.D, (new Instruction('1', Direction.RIGHT, State.D),    new Instruction('0', Direction.RIGHT, State.A)) }
                };
                        
            defaultInstructionTable = defaultInstructionTableState;
        }
        
        public FourStateBusyBeaver() : 
            base(defaultInstructionTable)
        {
        }
    }
}