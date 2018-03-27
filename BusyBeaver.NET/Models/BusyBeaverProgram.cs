namespace BusyBeaver.NET.Models
{
    public abstract class BusyBeaverProgram : Program
    {
        public BusyBeaverProgram(InstructionTable instructionTable)
        {
            this.instructionTable = instructionTable;
        }

        public virtual InstructionTable instructionTable { get; set; }
    }
}