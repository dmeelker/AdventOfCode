using Shared;
using System;
using System.Linq;

namespace Solution
{
    public class Block
    {
        public Point Location { get; set; }
        public Point[] Cells { get; set; }
        public int Width { get; }
        public int Height { get; }

        public Block(Point location, Point[] cells)
        {
            Location = location;
            Cells = cells;
            Width = cells.Max(c => c.X) + 1;
            Height = cells.Max(c => c.Y) + 1;
        }
    }

    public class Simulator
    {
        private Point[][] _blockShapes = new[] {
            new Point[] { new(0, 0), new(1,0), new(2,0), new(3,0) }, // Hline
            new Point[] { new(1, 0), new(1,1), new(1,2), new(0,1), new(2,1) }, // Cross
            new Point[] { new(2, 0), new(2,1), new(2,2), new(1,2), new(0,2) }, // L
            new Point[] { new(0, 0), new(0,1), new(0,2), new(0,3) }, // Vline
            new Point[] { new(0, 0), new(1,0), new(0,1), new(1,1) }, // Square
        };

        private Grid<bool> _field = new Grid<bool>(7, 50000, false);
        private Block _block;
        private int _nextShape = 0;
        private int _step = 0;
        private long _landedRockCount = 0;
        private readonly string _movements;

        public Simulator(string movements)
        {
            _block = NewBlock();
            _movements = movements;
        }

        public long Simulate(long maxLandedRocks, int part)
        {
            var skippedHeight = 0L;
            var skippedRocks = 0L;

            if (part == 2)
            {
                Console.WriteLine("Priming");
                for (var i = 0; i < 30000; i++)
                    SimulateStep();
            }

            Console.WriteLine("Finding repeating part");
            var repeatHash = part == 2 ? HashTopSection() : "";
            var blocksDroppedAtStartOfRepeat = _landedRockCount;
            var heightAtStartOfRepeat = GetTowerHeight();

            while (true)
            {
                if (SimulateStep())
                {
                    if (part == 2)
                    {
                        var hash = HashTopSection();

                        if (hash == repeatHash && skippedHeight == 0)
                        {
                            Console.WriteLine("Repeating part found");
                            var repeatBlocks = _landedRockCount - blocksDroppedAtStartOfRepeat;
                            var repeatHeight = GetTowerHeight() - heightAtStartOfRepeat;

                            var repeats = ((1_000_000_000_000L - _landedRockCount) / repeatBlocks) - 1L;
                            skippedRocks = repeats * repeatBlocks;
                            skippedHeight = repeatHeight * repeats;
                        }
                    }

                    if (_landedRockCount + skippedRocks == maxLandedRocks)
                    {
                        return (_field.Height + skippedHeight - FindHighestOccupiedRow());
                    }
                }
            }
        }

        private string HashTopSection()
        {
            var repeatingSection = _field.Copy(new(0, FindHighestOccupiedRow(), _field.Width, 10));
            return CreateKey(repeatingSection);
        }

        private string CreateKey(Grid<bool> input)
        {
            return string.Concat(input.AllCells().Select(c => c.Value ? '#' : '.'));
        }

        private bool SimulateStep()
        {
            TryMoveBlock(_block, GetJetDirection());
            _step++;

            if (!TryMoveBlock(_block, Point.Down))
            {
                _landedRockCount++;
                PlaceBlockInField(_block);
                _block = NewBlock();
                return true;
            }

            return false;
        }

        private Point GetJetDirection()
        {
            return _movements[_step % _movements.Length] switch
            {
                '<' => Point.Left,
                '>' => Point.Right,
                _ => throw new Exception()
            };
        }

        public Block NewBlock()
        {
            var cells = _blockShapes[_nextShape];
            var height = cells.Max(c => c.Y) + 1;
            _nextShape = (_nextShape + 1) % 5;

            return new Block(
                new Point(2, FindHighestOccupiedRow() - 3 - height),
                cells
            );
        }

        public bool TryMoveBlock(Block block, Point direction)
        {
            var newLocation = block.Location.Add(direction);

            if (newLocation.Y + block.Height > _field.Height || BlockCollides(block, newLocation))
            {
                return false;
            }
            else
            {
                block.Location = block.Location.Add(direction);
                return true;
            }
        }

        public long GetTowerHeight()
        {
            return _field.Height - FindHighestOccupiedRow();
        }

        public int FindHighestOccupiedRow()
        {
            for (var y = _field.Height - 1; y >= 0; y--)
            {
                if (_field.Line(y).All(cell => !cell.Value))
                {
                    return y + 1;
                }
            }

            return _field.Height;
        }

        public bool BlockCollides(Block block, Point location)
        {
            if (location.X < 0 || location.X + block.Width > _field.Width)
                return true;

            return block.Cells.Any(cell => _field.Get(location.Add(cell)));
        }

        public void PlaceBlockInField(Block block)
        {
            foreach (var cell in block.Cells)
            {
                _field.Set(block.Location.Add(cell), true);
            }
        }
    }
}
