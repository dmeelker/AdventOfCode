namespace Day8
{
    public class Instruction
    {
        public string Operation { get; set; }
        public long Value { get; set; }
        public int ExecutionCount { get; set; } = 0;

        public Instruction(string operation, long value)
        {
            Operation = operation;
            Value = value;
        }
    }
}
