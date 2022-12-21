using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solution
{
    public abstract class Expression
    {
        public string Name { get; }

        protected Expression(string name)
        {
            Name = name;
        }

        public abstract double Evaluate();
        public abstract Expression? FindByName(string name);
        public abstract bool Contains(Expression expression);
    }

    public enum Operator
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Equals
    }

    [DebuggerDisplay("{Name} :: {Left} {Operator} {Right}")]
    public class BinaryOperator : Expression
    {
        public Expression Left { get; }
        public Expression Right { get; }
        public Operator Operator { get; set; }

        public BinaryOperator(string name, Expression left, Expression right, Operator @operator) : base(name)
        {
            Left = left;
            Right = right;
            Operator = @operator;
        }

        public override double Evaluate()
        {
            return Operator switch
            {
                Operator.Add => Left.Evaluate() + Right.Evaluate(),
                Operator.Subtract => Left.Evaluate() - Right.Evaluate(),
                Operator.Multiply => Left.Evaluate() * Right.Evaluate(),
                Operator.Divide => Left.Evaluate() / Right.Evaluate(),
                _ => throw new NotImplementedException()
            };
        }

        public override Expression? FindByName(string name)
        {
            return Left.FindByName(name) ?? Right.FindByName(name);
        }

        public override bool Contains(Expression expression)
        {
            return Left.Contains(expression) || Right.Contains(expression);
        }
    }

    [DebuggerDisplay("{Name} :: {Value}")]
    public class Literal : Expression
    {
        public double Value { get; }

        public Literal(string name, double value) : base(name)
        {
            Value = value;
        }

        public override double Evaluate()
        {
            return Value;
        }

        public override Expression? FindByName(string name)
        {
            if (Name == name)
                return this;
            else
                return null;
        }

        public override bool Contains(Expression expression)
        {
            return this == expression;
        }
    }

    public class Assignment
    {
        public string Name { get; }
        public Expression Expression { get; }

        public Assignment(string name, Expression expression)
        {
            Name = name;
            Expression = expression;
        }
    }

    public static class Parser
    {
        public static Expression ParseInput(string input)
        {
            var openAssignments = input.ToLines().Select(line => line.Split(": "))
                .ToList();
            var processedAssignments = new Dictionary<string, Expression>();

            while (openAssignments.Any())
            {
                for (var i = 0; i < openAssignments.Count; i++)
                {
                    var openAssignment = openAssignments[i];
                    var name = openAssignment[0];

                    if (openAssignment[1].IsNumber())
                    {
                        processedAssignments.Add(
                            name,
                                new Literal(name, double.Parse(openAssignment[1])));

                        openAssignments.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        var parts = openAssignment[1].Split(" ");
                        var left = parts[0];
                        var op = parts[1];
                        var right = parts[2];

                        if (processedAssignments.TryGetValue(left, out var leftAssignment) && processedAssignments.TryGetValue(right, out var rightAssignment))
                        {
                            processedAssignments.Add(name,
                                new BinaryOperator(
                                    name,
                                    leftAssignment,
                                    rightAssignment,
                                    ParseOperator(op)
                                )
                            );

                            openAssignments.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }

            return processedAssignments["root"];
        }
        private static Operator ParseOperator(string input)
        {
            return input switch
            {
                "+" => Operator.Add,
                "-" => Operator.Subtract,
                "*" => Operator.Multiply,
                "/" => Operator.Divide,
                _ => throw new Exception()
            };
        }
    }
}
