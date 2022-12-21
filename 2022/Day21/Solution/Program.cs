using System;
using System.IO;

namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static double Part1(Expression expression)
        {
            return expression.Evaluate();
        }

        public static double Part2(Expression expression)
        {
            var humanLiteral = expression.FindByName("humn")!;

            Expression expressionToSolve;
            double expressionResult;
            var root = (BinaryOperator)expression;

            if (root.Left.Contains(humanLiteral))
            {
                expressionResult = root.Right.Evaluate();
                expressionToSolve = root.Left;
            }
            else
            {
                expressionResult = root.Left.Evaluate();
                expressionToSolve = root.Right;
            }

            while (true)
            {
                if (expressionToSolve is BinaryOperator operatorNode)
                {
                    if (operatorNode.Left.Contains(humanLiteral))
                    {
                        var rightValue = operatorNode.Right.Evaluate();
                        expressionToSolve = operatorNode.Left;
                        expressionResult = SolveForUnkownLeft(rightValue, operatorNode.Operator, expressionResult);

                        if (operatorNode.Left == humanLiteral)
                        {
                            return expressionResult;
                        }
                    }
                    else
                    {
                        var leftValue = operatorNode.Left.Evaluate();
                        expressionToSolve = operatorNode.Right;
                        expressionResult = SolveForUnkownRight(leftValue, operatorNode.Operator, expressionResult);

                        if (operatorNode.Right == humanLiteral)
                        {
                            return expressionResult;
                        }
                    }
                }
            }
        }

        private static double SolveForUnkownRight(double left, Operator op, double expressionResult)
        {
            return op switch
            {
                Operator.Add => expressionResult - left, // 10 + ? = 12
                Operator.Subtract => left - expressionResult, // 10 - ? = 8
                Operator.Multiply => expressionResult / left, // 10 * = 20
                Operator.Divide => left / expressionResult, // 10 / ? = 5
                _ => throw new Exception()
            };
        }

        private static double SolveForUnkownLeft(double right, Operator op, double expressionResult)
        {
            return op switch
            {
                Operator.Add => expressionResult - right, // ? + 5 = 10
                Operator.Subtract => expressionResult + right, // ? - 2 = 8
                Operator.Multiply => expressionResult / right, // ? * 2 = 10
                Operator.Divide => expressionResult * right, // ? / 5 = 2
                _ => throw new Exception()
            };
        }
    }
}
