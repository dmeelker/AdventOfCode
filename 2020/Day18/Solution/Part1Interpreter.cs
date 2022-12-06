using System;
using System.Collections.Generic;
using System.Text;

namespace Solution
{
    public static class Part1Interpreter
    {
        public static long Interpret(Queue<char> input)
        {
            var operand1 = InterpretFactor(input);

            while (input.Count > 0 && input.Peek() != ')')
            {
                var op = input.Dequeue();
                var operand2 = InterpretFactor(input);

                operand1 = op switch
                {
                    '+' => operand1 + operand2,
                    '*' => operand1 * operand2,
                    _ => throw new Exception($"Unkown operator {op}")
                };
            }

            return operand1;
        }

        public static long InterpretFactor(Queue<char> input)
        {
            if (char.IsNumber(input.Peek()))
            {
                return Convert.ToInt64(input.Dequeue().ToString());
            }
            else if (input.Peek() == '(')
            {
                input.Dequeue();
                var value = Interpret(input);
                input.Dequeue();
                return value;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
