using System;
using System.IO;
using System.Linq;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = @"nop +0
                acc +1
                jmp +4
                acc +3
                jmp -3
                acc -99
                acc +1
                jmp -4
                acc +6";

            input = File.ReadAllText("input.txt");

            var instructions = ParseScript(input);

            foreach (var jump in instructions.Where(instruction => instruction.Operation == Operators.JMP))
            {
                var interpreter = new Interpreter();
                jump.Operation = Operators.NOP;

                try
                {
                    interpreter.Run(instructions);
                    Console.WriteLine($"Found it! {interpreter.Accumulator}");
                    break;
                }
                catch (LoopException)
                {
                    Console.WriteLine($"Loop detected");
                }
                finally
                {
                    jump.Operation = Operators.JMP;
                }
            }

            Console.WriteLine("Program ended");
        }

        static Instruction[] ParseScript(string input)
        {
            return input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Select(instruction => new Instruction(instruction[0], long.Parse(instruction[1])))
                .ToArray();
        }
    }
}
