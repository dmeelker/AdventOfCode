using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public class NodeDistances
    {
        private readonly Dictionary<(Node, Node), int> _distances;

        public NodeDistances(IEnumerable<Node> nodes)
        {
            _distances = GetAllDistances(nodes.ToList());
        }

        private Dictionary<(Node, Node), int> GetAllDistances(List<Node> nodes)
        {
            var result = new Dictionary<(Node, Node), int>();

            foreach (var node1 in nodes)
            {
                foreach (var node2 in nodes)
                {
                    if (node1 == node2)
                        continue;

                    if (result.ContainsKey((node2, node1)))
                        continue;

                    result.Add((node1, node2), FindShortestPath(node1, node2, new(), int.MaxValue));
                }
            }

            return result;
        }

        private int FindShortestPath(Node from, Node to, Stack<Node> path, int shortestPath)
        {
            if (from == to)
            {
                return path.Count;
            }

            var availableOptions = from.Nodes
                .Where(n => !path.Contains(n));

            path.Push(from);

            foreach (var option in availableOptions)
            {
                var foundLength = FindShortestPath(option, to, path, shortestPath);

                shortestPath = Math.Min(shortestPath, foundLength);
            }

            path.Pop();

            return shortestPath;
        }

        public int GetDistance(Node node1, Node node2)
        {
            if (_distances.TryGetValue((node1, node2), out var distance))
            {
                return distance;
            }

            return _distances[(node2, node1)];
        }
    }
}
