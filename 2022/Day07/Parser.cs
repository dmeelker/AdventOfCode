
namespace AoC;

internal static class Parser
{
    public static Directory Parse(string[] input)
    {
        input = input[1..]; // Skip root entry
        var stack = new Stack<Directory>();
        var root = new Directory("/");
        stack.Push(root);

        for (var i = 0; i < input.Length; i++)
        {
            if (input[i].StartsWith("$"))
            {
                var parts = input[i].Split(" ");
                var command = parts[1];

                switch (command)
                {
                    case "cd":
                        {
                            var target = parts[2];

                            if (target == "..")
                            {
                                stack.Pop();
                            }
                            else if (target == "/")
                            {
                                stack.Clear();
                                stack.Push(root);
                            }
                            else
                            {
                                var subDirectory = stack.Peek().Directories.Single(d => d.Name == target);
                                stack.Push(subDirectory);
                            }
                            break;
                        }
                    case "ls":
                        {
                            for (var y = i + 1; y < input.Length; y++)
                            {
                                if (input[y].StartsWith("$"))
                                {
                                    break;
                                }
                                var outputParts = input[y].Split(" ");

                                if (outputParts[0] == "dir")
                                {
                                    stack.Peek().Directories.Add(new Directory(outputParts[1]));
                                }
                                else
                                {
                                    stack.Peek().Files.Add(outputParts[1], long.Parse(outputParts[0]));
                                }
                                i++;
                            }
                        }
                        break;
                }
            }
        }
        return root;
    }
}
