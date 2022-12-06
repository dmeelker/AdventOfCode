using System;
using System.Collections.Generic;
using System.Text;

namespace Day8
{
    public static class Operators
    {
        public const string ACC = "acc";
        public const string JMP = "jmp";
        public const string NOP = "nop";
    }

    public class Interpreter
    {
        public long Accumulator { get; private set; }
        public long Location { get; private set; }

        public void Run(Instruction[] instructions)
        {
            Accumulator = 0L;
            Location = 0L;
            ResetExecutionCounts(instructions);

            while (Location < instructions.Length)
            {
                var instruction = instructions[Location];
                InterpretInstruction(instruction);
            }
        }

        private void ResetExecutionCounts(Instruction[] instructions)
        {
            foreach (var instruction in instructions)
                instruction.ExecutionCount = 0;
        }

        public void InterpretInstruction(Instruction instruction)
        {
            instruction.ExecutionCount++;

            if (instruction.ExecutionCount > 1)
            {
                throw new LoopException($"Loop detected, Accumulator: {Accumulator}");
            }

            switch (instruction.Operation)
            {
                case Operators.ACC:
                    Accumulator += instruction.Value;
                    Location++;
                    break;

                case Operators.JMP:
                    Location += instruction.Value;
                    break;

                case Operators.NOP:
                    Location++;
                    break;

                default:
                    throw new Exception($"Unknown operator: {instruction.Operation}");
            }
        }
    }

    public class LoopException : Exception
    {
        public LoopException(string message) : base(message)
        {
            
        }
    }
}
