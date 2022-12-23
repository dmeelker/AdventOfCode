using Shared;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public class Elf
    {
        public Point Location { get; set; }

        public Elf(Point location)
        {
            Location = location;
        }
    }

    internal class ElfSimulator
    {
        private List<Elf> _elves = new();
        private List<Point> _directionsToConsider = new() {
            Point.Up,
            Point.Down,
            Point.Left,
            Point.Right
        };

        public IEnumerable<Elf> Elves => _elves;

        public ElfSimulator(IEnumerable<Elf> elves)
        {
            _elves = elves.ToList();
        }

        public int SimulateRound()
        {
            var movedElves = 0;
            var elfLocations = new HashSet<Point>(_elves.Select(e => e.Location));
            var proposedLocations = new Dictionary<Point, List<Elf>>();

            foreach (var elf in _elves)
            {
                if (!Shapes.Neighbours(elf.Location).Any(elfLocations.Contains))
                {
                    // No neighbours, do nothing
                    continue;
                }

                foreach (var direction in _directionsToConsider)
                {
                    if (!DirectionalNeighbours(elf.Location, direction).Any(elfLocations.Contains))
                    {
                        AddProposedLocation(elf, elf.Location.Add(direction), proposedLocations);
                        break;
                    }
                }
            }

            foreach (var entry in proposedLocations.Where(entry => entry.Value.Count == 1))
            {
                var elf = entry.Value.Single();
                elf.Location = entry.Key;
                movedElves++;
            }

            MoveFirstDirectionToBack();

            return movedElves;
        }

        private void MoveFirstDirectionToBack()
        {
            var firstDirection = _directionsToConsider[0];
            _directionsToConsider.RemoveAt(0);
            _directionsToConsider.Add(firstDirection);
        }

        private static void AddProposedLocation(Elf elf, Point location, Dictionary<Point, List<Elf>> locations)
        {
            if (!locations.TryGetValue(location, out var elves))
            {
                elves = new();
                locations.Add(location, elves);
            }

            elves.Add(elf);
        }

        private static IEnumerable<Point> DirectionalNeighbours(Point location, Point direction)
        {
            var side = new Point(direction.Y, direction.X);

            yield return location.Add(direction);
            yield return location.Add(direction).Add(side);
            yield return location.Add(direction).Subtract(side);
        }
    }
}
