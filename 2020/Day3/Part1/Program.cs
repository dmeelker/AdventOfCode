using System;
using System.IO;
using System.Linq;

namespace Part1
{
    class Program
    {
        static void Main(string[] args)
        {
            var world = World.FromFile("input.txt");
            var slopes = new[] {
                new Vector(1, 1),
                new Vector(3, 1),
                new Vector(5, 1),
                new Vector(7, 1),
                new Vector(1, 2)
            };

            var result = slopes.Select(vector => (long) CountHitTrees(world, vector)).Aggregate((x, y) => x * y);



            Console.WriteLine($"Trees hit: {result}");
        }

        private static int CountHitTrees(World world, Vector vector)
        {
            var hits = 0;
            var x = 0;
            var y = 0;

            while(y < world.Height)
            {
                if(world.Get(x, y))
                {
                    hits++;
                }

                x += vector.X;
                y += vector.Y;
            }

            return hits;
        }
    }

    class Vector
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class World
    {
        public int Width { get; }
        public int Height { get; }
        public bool[,] Data { get;  }

        public World(int width, int height)
        {
            Width = width;
            Height = height;
            Data = new bool[width, height];
        }

        public void Set(int x, int y, bool occupied)
        {
            Data[x, y] = occupied;
        }

        public bool Get(int x, int y)
        {
            x = x % Width;
            y = y % Height;

            return Data[x, y];
        }

        public static World FromFile(string path)
        {
            var lines = File.ReadAllLines(path);

            var world = new World(lines[0].Length, lines.Length);

            var y = 0;

            foreach(var line in lines)
            {
                var x = 0;

                foreach(var character in line)
                {
                    if(character == '#')
                    {
                        world.Set(x, y, true);
                    }
                    x++;
                }
                y++;
            }

            return world;
        }
    }
}