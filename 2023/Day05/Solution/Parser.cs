using Shared;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public record MapRange(Range Destination, Range Source)
    {
        public long Offset => Destination.Start - Source.Start;
    };
    public record Mapping(string Name, MapRange[] Ranges);
    public record Input(long[] Seeds, Mapping[] Mappings);
    public record Range(long Start, long Length)
    {
        public long End => Start + Length - 1;

        public Range[] Split(long index)
        {
            return new[] {
                new Range(Start,  index - Start),
                new Range(index, Start + Length - index),
            };
        }

        public IntersectionResult? Intersect(Range other)
        {
            if (!Intersects(other))
                return null;

            var parts = new List<Range>() { this };

            if (other.Start > Start)
            {
                parts.AddRange(parts.RemoveLast().Split(other.Start));
            }

            if (other.End < End)
            {
                parts.AddRange(parts.RemoveLast().Split(other.Start + other.Length));
            }

            var overlap = parts.Where(p => p.Intersects(other)).Single();
            var remainder = parts.Except(new[] { overlap }).ToArray();

            return new IntersectionResult(overlap, remainder);
        }

        public bool Intersects(Range other) => !(Start >= other.End || End < other.Start);
        public bool Contains(long value) => value >= Start && value <= Start + Length;
        public Range Transpose(long amount) => new Range(Start + amount, Length);
    };

    public record IntersectionResult(Range Intersection, Range[] Remainder);

    public static class Parser
    {
        public static Input ParseInput(string input)
        {
            var sections = input.ToSections();
            var mappings = new List<Mapping>();

            var seeds = sections[0].StripPrefix("seeds: ").Split(' ').Select(long.Parse).ToArray();

            foreach (var section in sections.Skip(1))
            {
                var lines = section.ToLines();
                var name = lines[0];

                var ranges = lines.Skip(1).Select(line =>
                {
                    var parts = line.Split(' ').Select(long.Parse).ToArray();
                    return new MapRange(new Range(parts[0], parts[2]), new Range(parts[1], parts[2]));
                }).ToArray();

                mappings.Add(new Mapping(name, ranges));
            }

            return new Input(seeds, mappings.ToArray());
        }
    }
}
