namespace Shared.Dijkstra
{

    public class Example
    {
        class Node
        {
            public Point Location { get; set; }
            public bool Passable { get; set; }

            public Node(Point location, bool passable)
            {
                Location = location;
                Passable = passable;
            }
        }

        public void Run()
        {
            var data = new Grid<int>(new int[,] {
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 1, 1, 0, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 0, 1, 1, 0, 1, 1, 0, 1 },
                { 1, 1, 0, 0, 0, 0, 1, 1, 0, 1 },
                { 1, 1, 0, 1, 1, 1, 1, 1, 0, 1 },
                { 1, 1, 0, 1, 1, 0, 0, 0, 0, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            });

            var searchSpace = data.Map<SearchNode<Node>>(cell => new(new Node(cell.Location, cell.Value == 1)));

            var searcher = new DijkstraSearcher<Node>(
                nodes: searchSpace.AllCells().Select(cell => cell.Value),
                start: searchSpace.Get(0, 0),
                end: searchSpace.Get(0, 9),
                optionGetter: node => searchSpace.Neighbours(node.Value.Location, true)
                    .Select(cell => cell.Value)
                    .Where(node => node.Value.Passable),
                costFunction: (from, to) =>
                to.Value.Location.Subtract(from.Value.Location).ManhattanDistance
            );

            var path = searcher.FindPath();

            VisualizePath(searchSpace, path!);
        }

        private void VisualizePath(Grid<SearchNode<Node>> grid, Path<Node> path)
        {
            for (var y = 0; y < grid.Height; y++)
            {
                for (var x = 0; x < grid.Width; x++)
                {
                    var node = grid.Get(x, y);

                    if (path.Steps.Contains(node))
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.Write(node.Value.Passable ? "." : "#");
                }
                Console.WriteLine();
            }
        }
    }
}
