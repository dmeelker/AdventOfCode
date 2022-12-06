namespace AoC
{
    internal class Parser
    {
        public IEnumerable<Tuple<Range, Range>> ReadInput(string[] lines)
        {
            return lines.Select(ParseLine);
        }

        private Tuple<Range, Range> ParseLine(string line)
        {
            var parts = line.Split(",");
            return Tuple.Create(ParseRange(parts[0]), ParseRange(parts[1]));
        }

        private Range ParseRange(string input)
        {
            var parts = input.Split("-");
            return new Range(new Index(int.Parse(parts[0])), new Index(int.Parse(parts[1])));
        }
    }
}
