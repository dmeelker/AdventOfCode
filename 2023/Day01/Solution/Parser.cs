using System;

namespace Solution
{
    public static class Parser
    {
        public static string[] ParseInput(string input)
        {
            return input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
