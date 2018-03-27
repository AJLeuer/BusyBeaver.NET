﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BusyBeaver.NET.Models {
    
    using Instruction = TuringMachine.Instruction;
    
    public class TuringMachine 
    {
        
        /**
         * The tape represents the Turing Machine's memory. It is theoretically unbounded in both directions (though obviously in the real world we're limited
         * by the amount of physical memory we have available). The tape is divided into squares or cells, each of which can contain a single character. The
         * read/write head of the Turing Machine moves along the tape one cell at a time.
         */
        public List<char> tape;

        public enum DirectionToMove : short
        {
            RIGHT = 1,
            LEFT = -1
        }
        
        /**
         * Represents the possible states of the Turing Machine
         */
        public enum State {
            A = 'A',
            B = 'B',
            C = 'C',
            D = 'D',
            E = 'E',
            F = 'F',
            /**
             * Signifies that the Turing Machine's state should remain unchanged
             */
            UNCHANGED = '␀',
            /**
             * Signifies that the Turing Machine should halt
             */
            HALT = '␄'
        }

        public State state { get; set; }

        public struct Instruction {
            
            public Instruction(char symbolToPrint, DirectionToMove directionToMove, State newState)
            {
                this.symbolToPrint = symbolToPrint;
                this.directionToMove = directionToMove;
                this.newState = newState;
            }

            public readonly char symbolToPrint;
            
            public readonly DirectionToMove directionToMove;
            
            public readonly State newState;
            
        }
        
        public ulong programCounter { get; set; }
        
        uint currentHeadPosition;
        
        uint lowestTapePositionWritten;
        
        uint highestTapePositionWritten;
        
        public TuringMachine() {
            reset();
        }
        
        public void reset() {
            state = State.A; //Always starts in state 'A'
            programCounter = 0;
            currentHeadPosition = 0;
            lowestTapePositionWritten = 0;
            highestTapePositionWritten = 0;
            tape = Enumerable.Repeat('0', 4096).ToList(); //Tape starts out with each each symbol zeroed out...

            initializePositionMarkers(); 
        }
        
        public void run(Program program, TextWriter output) {
            while (state != State.HALT) {
                
                if (output != null)
                {
                    output.Write("Machine state: " + state + ' ');
                    writeTapeToOutput(output);
                }
                
                var currentInstruction = program.instructionTable.getInstructionsForStateAndSymbol(state, getSymbolUnderHead());

                execute(currentInstruction);

                programCounter++;
            }
        }
        
        /**
         * Writes the symbol from every cell on the tape that has been modified so far (ignores all the untouched squares), starting from
         * the lowest-indexed cell on the left to the highest index on the right
         *
         * @param outputStream The output stream to write
         * @param padding How far outside the highest and lowest values written on the tape to read from. By default this is two.
         */
        private void writeTapeToOutput(TextWriter output, uint padding = 2)
        {
            output.Write("Program output: " );
            
            for (int symbolIndex = calculateFirstPrintedSymbolIndex(padding); symbolIndex <= (highestTapePositionWritten + padding); symbolIndex++) {
        
                StringBuilder outputBuilder = new StringBuilder();

                char symbol = tape[symbolIndex];
        
                if (symbolIndex == currentHeadPosition) {
//                    outputBuilder.Append('*' + symbol.ToString() + '*');
                    outputBuilder.Append(Util.FormatWithUnderscore(symbol));
                }
                else {
                    outputBuilder.Append(symbol);
                }
        
                output.Write(outputBuilder.ToString() + ' ');
            }
            output.Write(Environment.NewLine);
        }

        private int calculateFirstPrintedSymbolIndex(uint padding)
        {
            int symbolIndex = (int)((int)lowestTapePositionWritten - padding);
            
            if (symbolIndex < 0)
            {
                symbolIndex = 0;
            }

            return symbolIndex;
        }

        private void initializePositionMarkers() {
            currentHeadPosition = (uint)(tape.Count / 2); // ... and the turing machine will always start reading at the zero in the center of the tape
            lowestTapePositionWritten = currentHeadPosition;
            highestTapePositionWritten = currentHeadPosition;
        }
        
        /**
         * Executes the given instruction. One execution cycle can do up to three things (writing a new symbol, moving the head,
         * and setting the new state), but only moving the head is guarunteed to occur. If the symbolToPrint equals the null character (0x00),
         * symbolToPrint is treated as a NOP and the symbol under the head is left unchanged. If the newState is equal to UNCHANGED (or its underlying
         * representation, ␀), newState is treated as a NOP and the machine state is left unchanged
         */
        private void execute(Instruction instruction)
        {
            if (instruction.symbolToPrint != 0x00) {
                writeSymbolToTape(instruction.symbolToPrint);
            }
    
            moveHead(instruction.directionToMove);
    
            if (instruction.newState != State.UNCHANGED) {
                state = instruction.newState;
            }
        }

        /**
        * @return The current symbol
        */
        private char getSymbolUnderHead() { return tape[(int)currentHeadPosition] ; }

        private void writeSymbolToTape(char symbol) {
            tape[(int)currentHeadPosition] = symbol;
    
            if (currentHeadPosition < lowestTapePositionWritten) {
                lowestTapePositionWritten = currentHeadPosition;
            }
            else if (currentHeadPosition > highestTapePositionWritten) {
                highestTapePositionWritten = currentHeadPosition;
            }
        }
        
        /**
         * Moves head to the left or right. Turing machines always move one position on the tape at a time.
         */
        private void moveHead(DirectionToMove direction) {
            if (direction == DirectionToMove.RIGHT) {
                currentHeadPosition++;
            }
            else /* if (direction == DirectionToMove.LEFT) */ {
                currentHeadPosition--;
            }
        }

    }
    

    
    /**
     * The instructions table contains map entries for each possible state ('A', 'B', 'C', etc.). Each map entry in turn contains two instructions, one to
     * read when the symbol at the current tape position is '0' (found at index zero of the instructions), and one for when the current symbol is
     * '1'.
     */
    public struct InstructionTable 
    {
        
        Dictionary<TuringMachine.State, ValueTuple<Instruction, Instruction>>  instructions;

        public static implicit operator InstructionTable(
            Dictionary<TuringMachine.State, ValueTuple<Instruction, Instruction>> instructions)
        {
            return new InstructionTable(instructions);
        }

        public InstructionTable(Dictionary<TuringMachine.State, ValueTuple<Instruction, Instruction>> instructions)
        {
            this.instructions = instructions;
            
        }
    
        public TuringMachine.Instruction getInstructionsForStateAndSymbol(TuringMachine.State state, char symbol)
        {
            uint symbolAsIndex = (uint)(symbol - '0');
            ITuple instructionsForState = getInstructionsForState(state);
            
            return (Instruction) instructionsForState[(int)symbolAsIndex];
        }


        private ValueTuple<Instruction, Instruction> getInstructionsForState(TuringMachine.State state)
        {
            instructions.TryGetValue(state, out var instructionsForState);
            return instructionsForState;
        }
    
}

    public interface Program
    {
        InstructionTable instructionTable { get; set; }
    }

}
