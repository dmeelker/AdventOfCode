using Priority_Queue;

namespace Shared.Dijkstra;

public class DijkstraSearcher2<T> where T : class
{
    public class Node : FastPriorityQueueNode
    {
        public T Value { get; }
        public Node? Source { get; internal set; }
        public int Cost { get; internal set; }

        public Node(T value)
        {
            Value = value;
            Cost = int.MaxValue;
        }
    }

    private readonly Func<Node, IEnumerable<T>> _optionFunction;
    private readonly Func<Node, Node, int> _costFunction;

    public DijkstraSearcher2(Func<Node, IEnumerable<T>> optionFunction, Func<Node, Node, int> costFunction)
    {
        _optionFunction = optionFunction;
        _costFunction = costFunction;
    }

    public List<T>? FindPath(T start, T end, ICollection<T> allNodes)
    {
        var internalNodes = allNodes.Select(n => new Node(n)).ToDictionary(n => n.Value, n => n);
        var startNode = internalNodes[start];
        var endNode = internalNodes[end];
        var closed = new HashSet<Node>();

        startNode.Cost = 0;
        var open = new FastPriorityQueue<Node>(allNodes.Count);

        foreach (var node in internalNodes.Values)
        {
            open.Enqueue(node, node.Cost);
        }

        while (open.Count > 0)
        {
            var node = open.Dequeue();
            if (node.Cost == int.MaxValue)
            {
                break;
            }

            var options = _optionFunction(node)
                .Select(option => internalNodes[option])
                .Where(option => !closed.Contains(option));

            foreach (var option in options)
            {
                var newCost = node.Cost + _costFunction(node, option);
                if (newCost < option.Cost)
                {
                    option.Cost = newCost;
                    option.Source = node;

                    open.UpdatePriority(option, option.Cost);
                }

                closed.Add(node);
                if (node == endNode)
                {
                    break;
                }
            }
        }

        if (endNode.Source == null)
        {
            return null; // No path
        }

        return ConstructPath(endNode);
    }

    private List<T> ConstructPath(Node endNode)
    {
        var path = new List<T>();
        var currentNode = endNode;
        while (currentNode != null)
        {
            path.Add(currentNode.Value);
            currentNode = currentNode.Source;
        }

        path.Reverse();

        return new(path);
    }
}
