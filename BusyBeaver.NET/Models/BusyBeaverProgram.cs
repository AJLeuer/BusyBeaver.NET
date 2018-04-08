namespace BusyBeaver.NET.Models
{
    public class BusyBeaverProgram : Program
    {
        public BusyBeaverProgram(InstructionTable instructionTable)
        {
            this.instructionTable = instructionTable;
        }

        public virtual InstructionTable instructionTable { get; set; }
    }
}