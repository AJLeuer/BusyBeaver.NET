﻿using System;
using System.Collections.Generic;

namespace BusyBeaver.NET.Models
{
    
    using State = TuringMachine.State; 
    using Instruction = TuringMachine.Instruction;
    using Direction = TuringMachine.DirectionToMove; 

    /**
     * An example Busy Beaver program with two-states and a complete instruction table
     */
    class TwoStateBusyBeaver : BusyBeaverProgram
    {
        private static readonly InstructionTable DefaultInstructionTable;

        static TwoStateBusyBeaver()
        {            
            var defaultInstructionTableState = 
                new Dictionary<State, (Instruction, Instruction)>
                {
                    { State.A, (new Instruction('1', Direction.RIGHT, State.B), new Instruction('1', Direction.LEFT,  State.B)) },
                    { State.B, (new Instruction('1', Direction.LEFT,  State.A), new Instruction('1', Direction.RIGHT, State.HALT)) }
                };

            DefaultInstructionTable = defaultInstructionTableState;
        }

        public TwoStateBusyBeaver() :
            base(DefaultInstructionTable)
        {
            
        }
    
    }
}