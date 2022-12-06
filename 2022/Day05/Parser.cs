namespace AoC
{
    public record PuzzleInput(string[][] Stacks, IEnumerable<MoveOperation> Operations);
    public record MoveOperation(int Quantity, int From, int To);

    internal class Parser
    {
        public PuzzleInput ReadInput(string input)
        {
            var sections = input.Split(Environment.NewLine + Environment.NewLine);

            return new(
                ParseStacks(sections[0]),
                ParseOperations(sections[1])
            );
        }

        private string[][] ParseStacks(string input)
        {
            var lines = input.Split(Environment.NewLine);
            var stackCount = (lines.Last().Length / 4) + 1;

            return Enumerable.Range(0, stackCount)
                .Select(stackIndex => ParseStack(lines, stackIndex))
                .ToArray();
        }

        private string[] ParseStack(string[] lines, int stackIndex)
        {
            var columnIndex = (stackIndex * 4) + 1;
            var maxDepth = lines.Length - 1;

            return
                Enumerable.Range(0, maxDepth)
                .Select(depth => lines[depth][columnIndex].ToString())
                .Where(value => value != " ")
                .ToArray();
        }

        private IEnumerable<MoveOperation> ParseOperations(string input)
        {
            var lines = input.Split(Environment.NewLine);

            foreach (var line in lines)
            {
                var numbers = line.Split(" ")
                    .Where(word => word.Length == 1)
                    .Select(int.Parse)
                    .ToArray();

                yield return new MoveOperation(
                    numbers[0],
                    numbers[1],
                    numbers[2]
                );
            }
        }
    }
}
