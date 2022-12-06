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
            var input = Parser.ParseInput(File.ReadAllText("input.txt"));
            var part1 = Part1(input);
            var part2 = Part2(input);

            Console.WriteLine($"Part 1: {part1} Part 2: {part2}");
        }

        public static long Part1(Tile[] input)
        {
            var field = Puzzle(input);
            var gridSize = field.GetLength(0);
            return (long)field[0, 0].TileId * field[gridSize - 1, 0].TileId * field[gridSize - 1, gridSize - 1].TileId * field[0, gridSize - 1].TileId;
        }

        private static TilePermutation[,] Puzzle(Tile[] input)
        {
            var gridSize = (int)Math.Sqrt(input.Length);

            var sideCount = input.SelectMany(t => t.UniqueSides).GroupBy(side => side)
                .Select(group => (side: group.Key, count: group.Count()))
                .OrderByDescending(group => group.count)
                .ToDictionary(group => group.side, group => group.count);

            var firstCorner = input.First(t => t.IsCorner(sideCount));
            var topLeftCorner = firstCorner.GetTopLeftCornerPermutation(sideCount);

            var remainingTiles = input.Except(new[] { firstCorner }).ToList();
            var field = new TilePermutation[gridSize, gridSize];

            field[0, 0] = new TilePermutation(firstCorner.Id, topLeftCorner);

            for(int y=0; y<gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    TilePermutation previousTile;
                    string side;
                    int sideIndex;

                    if (x == 0)
                    {
                        previousTile = field[x, y - 1];
                        side = previousTile.Grid.Sides[2];
                        sideIndex = 0;
                    }
                    else
                    {
                        previousTile = field[x - 1, y];
                        side = previousTile.Grid.Sides[1];
                        sideIndex = 3;
                    }

                    var match = remainingTiles.Select(tile => (tile, grid: tile.Permutations.SingleOrDefault(p => p.Sides[sideIndex] == side)))
                        .Where(match => match.grid != null)
                        .First();

                    field[x, y] = new TilePermutation(match.tile.Id, match.grid);
                    remainingTiles.Remove(match.tile);
                }
            }

            return field;
        }

        public static int Part2(Tile[] input)
        {
            var field = Puzzle(input);

            var grid = FieldToGrid(field)
                .GeneratePermutations()
                .Select(p => (grid: p, count: CountMonsters(p)))
                .OrderByDescending(p => p.count)
                .First();

            var monsterCellCount = grid.count * GetMonsterOffsets().Count();
            var waveCount = grid.grid.AllValues.Count(value => value == true);

            return waveCount - monsterCellCount;
        }

        public static int CountMonsters(Grid grid)
        {
            var count = 0;
            for(var x=0; x<grid.Size-19; x++)
            {
                for (var y = 0; y < grid.Size-2; y++)
                {
                    if (IsMonster(grid, x, y))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public static bool IsMonster(Grid grid, int x, int y)
        {
            var p = GetMonsterPoints(x, y).Where(location => grid.Contains(location.x, location.y) && grid.Get(location.x, location.y)).ToArray();
            return GetMonsterPoints(x, y).All(location => grid.Contains(location.x, location.y) && grid.Get(location.x, location.y));
        }

        public static IEnumerable<(int x, int y)> GetMonsterPoints(int x, int y)
        {
            return GetMonsterOffsets().Select(p => (p.x + x, p.y + y));
        }

        public static IEnumerable<(int x, int y)> GetMonsterOffsets()
        {
            return new[] {
                (0, 1),
                (1, 2),
                  
                (4, 2),
                (5, 1),
                (6, 1),
                (7, 2),

                (10, 2),
                (11, 1),
                (12, 1),
                (13, 2),
                   
                (16, 2),
                (17, 1),
                (18, 1),
                (19, 1),
                   
                (18, 0),
            };
        }

        public static Grid FieldToGrid(TilePermutation[,] field)
        {
            var gridSize = field[0, 0].Grid.Size;
            var resultGridSize = field.GetLength(0) * (gridSize - 2);
            var gridData = new bool[resultGridSize, resultGridSize];

            foreach(var value in GetFieldValuesWithoutBorders(field).Select((value, index) => (value, index)))
            {
                var (x,y) = Get2dCoordinatesFromIndex(value.index, resultGridSize);
                gridData[x, y] = value.value;
            }

            return new Grid(gridData);
        }

        public static (int x, int y) Get2dCoordinatesFromIndex(int index, int gridSize)
        {
            return (
                index - index / gridSize * gridSize, 
                index / gridSize
            );
        }

        public static IEnumerable<bool> GetFieldValuesWithoutBorders(TilePermutation[,] field)
        {
            var fieldSize = field.GetLength(0);
            var gridSize = field[0, 0].Grid.Size;

            for(var fieldY=0;fieldY< fieldSize; fieldY++)
            {
                for (var y = 1; y < gridSize - 1; y++)
                {
                    for (var fieldX = 0; fieldX < fieldSize; fieldX++)
                    {
                        var grid = field[fieldX, fieldY];

                        for (var x = 1; x < gridSize - 1; x++)
                        {
                            yield return grid.Grid.Get(x, y);
                        }
                    }
                }
            }
        }
    }

    public class TilePermutation
    {
        public int TileId { get; }
        public Grid Grid { get; }

        public TilePermutation(int tileId, Grid grid)
        {
            TileId = tileId;
            Grid = grid;
        }
    }
}
