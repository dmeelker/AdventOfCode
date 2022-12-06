using Solution;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void Part1()
        {
            var result = Parser.ParseInput(File.ReadAllText("input.txt"));
            Assert.Equal(111, Program.Part1(result));
        }

        [Fact]
        public void Part2()
        {
            var result = Parser.ParseInput(File.ReadAllText("input.txt"));
            Assert.Equal(343, Program.Part2(result));
        }

        [Fact]
        public void TestCase1()
        {
            var result = Parser.ParseInput(@"42: 9 14 | 10 1
9: 14 27 | 1 26
10: 23 14 | 28 1
1: ""a""
11: 42 31
5: 1 14 | 15 1
19: 14 1 | 14 14
12: 24 14 | 19 1
16: 15 1 | 14 14
31: 14 17 | 1 13
6: 14 14 | 1 14
2: 1 24 | 14 4
0: 8 11
13: 14 3 | 1 12
15: 1 | 14
17: 14 2 | 1 7
23: 25 1 | 22 14
28: 16 1
4: 1 1
20: 14 14 | 1 15
3: 5 14 | 16 1
27: 1 6 | 14 18
14: ""b""
21: 14 1 | 1 14
25: 1 1 | 1 14
22: 14 14
8: 42
26: 14 22 | 1 20
18: 15 15
7: 14 5 | 1 21
24: 14 1

abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa
bbabbbbaabaabba
babbbbaabbbbbabbbbbbaabaaabaaa
aaabbbbbbaaaabaababaabababbabaaabbababababaaa
bbbbbbbaaaabbbbaaabbabaaa
bbbababbbbaaaaaaaabbababaaababaabab
ababaaaaaabaaab
ababaaaaabbbaba
baabbaaaabbaaaababbaababb
abbbbabbbbaaaababbbbbbaaaababb
aaaaabbaabaaaaababaa
aaaabbaaaabbaaa
aaaabbaabbaaaaaaabbbabbbaaabbaabaaa
babaaabbbaaabaababbaabababaaab
aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba");

            Assert.Equal(3, Program.Part1(result));
        }

        [Theory]
        [InlineData("abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa")]
        [InlineData("bbabbbbaabaabba")]
        [InlineData("babbbbaabbbbbabbbbbbaabaaabaaa")]
        [InlineData("aaabbbbbbaaaabaababaabababbabaaabbababababaaa")]
        [InlineData("bbbbbbbaaaabbbbaaabbabaaa")]
        [InlineData("bbbababbbbaaaaaaaabbababaaababaabab")]
        [InlineData("ababaaaaaabaaab")]
        [InlineData("ababaaaaabbbaba")]
        [InlineData("baabbaaaabbaaaababbaababb")]
        [InlineData("abbbbabbbbaaaababbbbbbaaaababb")]
        [InlineData("aaaaabbaabaaaaababaa")]
        [InlineData("aaaabbaaaabbaaa")]
        [InlineData("aaaabbaabbaaaaaaabbbabbbaaabbaabaaa")]
        [InlineData("babaaabbbaaabaababbaabababaaab")]
        [InlineData("aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba")]
        public void TestCase2(string input)
        {
            var result = Parser.ParseInput(@"42: 9 14 | 10 1
9: 14 27 | 1 26
10: 23 14 | 28 1
1: ""a""
11: 42 31
5: 1 14 | 15 1
19: 14 1 | 14 14
12: 24 14 | 19 1
16: 15 1 | 14 14
31: 14 17 | 1 13
6: 14 14 | 1 14
2: 1 24 | 14 4
0: 8 11
13: 14 3 | 1 12
15: 1 | 14
17: 14 2 | 1 7
23: 25 1 | 22 14
28: 16 1
4: 1 1
20: 14 14 | 1 15
3: 5 14 | 16 1
27: 1 6 | 14 18
14: ""b""
21: 14 1 | 1 14
25: 1 1 | 1 14
22: 14 14
8: 42
26: 14 22 | 1 20
18: 15 15
7: 14 5 | 1 21
24: 14 1

");
            Program.AddLoops(result);
            
            Assert.True(Program.MatchesRule(input, result.Rules[0], result.Rules));
        }

        [Theory]
        [InlineData("ab", true)]
        [InlineData("abab", true)]
        [InlineData("ababab", true)]
        [InlineData("a", false)]
        public void Loop(string input, bool expectedMatch)
        {
            var result = Parser.ParseInput(@"
1: ""a""
2: ""b""
3: 1 2
0: 3 | 3 0

");

            Assert.Equal(expectedMatch, Program.MatchesRule(input, result.Rules[0], result.Rules));
        }

        [Theory]
        [InlineData("ab", true)]
        [InlineData("ababc", true)]
        [InlineData("abababcc", true)]
        [InlineData("a", false)]
        public void Loop2(string input, bool expectedMatch)
        {
            var result = Parser.ParseInput(@"
1: ""a""
2: ""b""
3: ""c""
4: 1 2
0: 4 | 4 0 3

");

            Assert.Equal(expectedMatch, Program.MatchesRule(input, result.Rules[0], result.Rules));
        }

        [Theory]
        [InlineData("cabc", true)]
        [InlineData("cababcc", true)]
        [InlineData("cabababccc", true)]
        [InlineData("a", false)]
        public void Loop3(string input, bool expectedMatch)
        {
            var result = Parser.ParseInput(@"
1: ""a""
2: ""b""
3: ""c""
4: 1 2
5: 4 | 4 5 3
0: 3 5 3

");

            Assert.Equal(expectedMatch, Program.MatchesRule(input, result.Rules[0], result.Rules));
        }


    }
}
