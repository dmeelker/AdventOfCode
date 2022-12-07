
namespace AoC;

internal static class Parser
{
    public static Directory Parse(string input)
    {
        var commandBlocks = input.Split("$", StringSplitOptions.RemoveEmptyEntries);
        commandBlocks = commandBlocks[1..]; // Skip root directory

        var stack = new Stack<Directory>();
        var root = new Directory("/");
        stack.Push(root);

        foreach (var commandBlock in commandBlocks)
        {
            var lines = commandBlock.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var commandParts = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var command = commandParts[0];

            switch (command)
            {
                case "cd":
                    {
                        var target = commandParts[1];

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
                        var outputLines = lines.Skip(1);
                        foreach (var outputLine in outputLines)
                        {
                            var outputParts = outputLine.Split(" ");

                            if (outputParts[0] == "dir")
                            {
                                stack.Peek().Directories.Add(new Directory(outputParts[1]));
                            }
                            else
                            {
                                stack.Peek().TotalFileSize += long.Parse(outputParts[0]);
                            }
                        }
                    }
                    break;
            }
        }

        return root;
    }
}
