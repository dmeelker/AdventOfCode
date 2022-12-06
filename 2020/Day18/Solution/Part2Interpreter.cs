using System;
using System.Collections.Generic;
using System.Text;

namespace Solution
{
    public static class Part2Interpreter
    {
        public static long Interpret(Queue<char> input)
        {
            return InterpretMultiplicativeExpression(input);
        }

        public static long InterpretMultiplicativeExpression(Queue<char> input)
        {
            var operand1 = InterpretAdditiveExpression(input);

            while (input.Count > 0 && input.Peek() != ')' && input.Peek() == '*')
            {
                input.Dequeue(); // Operator
                var operand2 = InterpretAdditiveExpression(input);

                operand1 *= operand2;
            }

            return operand1;
        }

        public static long InterpretAdditiveExpression(Queue<char> input)
        {
            var operand1 = InterpretFactor(input);

            while (input.Count > 0 && input.Peek() != ')' && input.Peek() == '+')
            {
                input.Dequeue(); // Operator
                var operand2 = InterpretFactor(input);

                operand1 += operand2;
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
