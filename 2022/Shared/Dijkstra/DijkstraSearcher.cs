namespace Shared.Dijkstra
{
    public record Path<T>(List<SearchNode<T>> Steps) where T : class;

    public class SearchNode<T> : IEquatable<SearchNode<T>> where T : class
    {
        public T Value { get; set; }
        public int Cost { get; set; } = int.MaxValue;
        public SearchNode<T>? Source { get; set; }

        public SearchNode(T value)
        {
            Value = value;
        }

        public bool Equals(SearchNode<T>? other)
        {
            if (other == null) return false;
            return Value!.Equals(other.Value);
        }
    }

    public class DijkstraSearcher<T> where T : class
    {
        private Queue<SearchNode<T>> _open = new();
        private HashSet<SearchNode<T>> _closed = new();
        private readonly SearchNode<T> _start;
        private readonly SearchNode<T> _end;
        private readonly Func<SearchNode<T>, IEnumerable<SearchNode<T>>> _optionGetter;
        private readonly Func<SearchNode<T>, SearchNode<T>, int> _costFunction;

        public DijkstraSearcher(IEnumerable<SearchNode<T>> nodes, SearchNode<T> start, SearchNode<T> end, Func<SearchNode<T>, IEnumerable<SearchNode<T>>> optionGetter, Func<SearchNode<T>, SearchNode<T>, int> costFunction)
        {
            _open = new(nodes.ToList());
            _start = start;
            _end = end;
            _optionGetter = optionGetter;
            _costFunction = costFunction;

            InitializeNodes();
        }

        private void InitializeNodes()
        {
            foreach (var node in _open)
            {
                node.Cost = int.MaxValue;
            }

            _start.Cost = 0;

            SortOpen();
        }

        private void SortOpen()
        {
            var newList = _open.Where(n => n.Cost != int.MaxValue)
                       .OrderBy(n => n.Cost)
                       .Concat(_open.Where(n => n.Cost == int.MaxValue))
                       .ToList();

            _open = new(newList);
        }

        public Path<T>? FindPath()
        {
            while (_open.TryDequeue(out var currentNode))
            {
                if (currentNode.Cost == int.MaxValue)
                    break;

                var options = _optionGetter(currentNode)
                    .Where(node => !_closed.Contains(node));

                var nodeUpdated = false;
                foreach (var option in options)
                {
                    var newCost = currentNode.Cost + _costFunction(currentNode, option);
                    if (newCost < option.Cost)
                    {
                        option.Cost = newCost;
                        option.Source = currentNode;
                        nodeUpdated = true;
                    }
                }

                _closed.Add(currentNode);
                if (currentNode == _end)
                {
                    break;
                }

                if (nodeUpdated)
                    SortOpen();
            }

            if (_end.Source == null)
            {
                return null; // No Path
            }

            return ConstructPath(_end);
        }

        private Path<T> ConstructPath(SearchNode<T> endNode)
        {
            var path = new List<SearchNode<T>>();
            SearchNode<T>? currentNode = endNode;
            while (currentNode != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.Source;
            }

            path.Reverse();

            return new(path);
        }
    }
}
