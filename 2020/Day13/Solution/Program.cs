using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            var (departureTime, busIds) = ParseInput(File.ReadAllText("input.txt"));
            var part1 = Part1(departureTime, busIds);
            var part2 = Part2(busIds);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static (int departureTime, int?[] busIds) ParseInput(string input)
        {
            var lines = input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var departureTime = int.Parse(lines[0]);
            var busIds = lines[1].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(id => id != "x" ? (int?)int.Parse(id) : null).ToArray();

            return (departureTime, busIds);
        }

        public static int Part1(int departureTime, int?[] busIds)
        {
            var buses = busIds.Where(id => id != null)
                .Select(id => id.Value)
                .Select(id => (busId: id, timeUntilDeparture: (int)CalculateTimeUntilDeparture((ulong)departureTime, id)))
                .OrderBy(bus => bus.timeUntilDeparture).ToArray();
                
            var firstBus = buses.First();

            return firstBus.busId * firstBus.timeUntilDeparture;
        }

        public static ulong CalculateTimeUntilDeparture(ulong currentTime, int busId)
        {
            return (ulong)((Math.Floor(currentTime / (double)busId) * busId) + busId) - currentTime;
        }

        public static ulong Part2(int?[] busIds)
        {
            var input = busIds.Select((id, index) => (id: (ulong?)id, index: (ulong)index))
                .Where(pair => pair.id.HasValue)
                .Select(pair => (id: pair.id.Value, index: pair.index))
                .ToArray();

            var increment = input.Last().id;
            var time = increment - input.Last().index;

            for(var i=input.Length-2; i>=0; i--)
            {
                while(true)
                {
                    if((time + input[i].index) % input[i].id == 0)
                    {
                        increment *= input[i].id;
                        break;
                    }
                    time += increment;
                }
            }

            return time;
        }
    }
}
