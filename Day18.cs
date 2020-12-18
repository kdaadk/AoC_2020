using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AoC_2020
{
    public class Day18
    {
        public static void SolvePart2()
        {
            var input = File.ReadAllLines("input.txt");
            
            var result = input.Select(line => Compute(line.Replace(" ", ""))).Sum();
            Console.WriteLine(result);
        }

        private static long Compute(string line)
        {
            if (CanReduceParenthesis(line))
                line = line.Substring(1, line.Length - 2);
            return HasParenthesis(line)
                ? ComputeWithParenthesis(line)
                : ComputeWithoutParenthesis(line);
        }

        private static bool CanReduceParenthesis(string source)
        {
            if (!HasParenthesis(source)) return false;
            var currentDepth = 0;
            var open = -1;
            var close = -1;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == '(')
                {
                    if (currentDepth == 0)
                        open = i;
                    currentDepth++;
                }

                if (source[i] == ')')
                {
                    currentDepth--;
                    if (currentDepth == 0)
                        close = i;
                }
            }

            return open == 0 && close == source.Length - 1;
        }

        private static long ComputeWithParenthesis(string source)
        {
            var signIdx = GetSignIdx(source);
            
            var left = source.Substring(0, signIdx);
            var leftSum = Compute(left);
            
            var right = source.Substring(signIdx + 1);
            var rightSum = Compute(right);

            if (source[signIdx] == '+') return leftSum + rightSum;
            return leftSum * rightSum;
        }

        private static int GetSignIdx(string source)
        {
            var depth = 0;
            var signsIdx = new List<(int, char)>();
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == '(') depth++;
                if (source[i] == ')') depth--;
                if (depth == 0 && (source[i] == '+' || source[i] == '*')) signsIdx.Add((i, source[i]));
            }

            return signsIdx.Any(x => x.Item2 == '*')
                ? signsIdx.First(x => x.Item2 == '*').Item1
                : signsIdx[0].Item1;
        }

        private static long ComputeWithoutParenthesis(string source)
        {
            var lastIdx = 0;
            var multipliers = new List<long>();
            
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == '*' || i == source.Length - 1)
                {
                    multipliers.Add(Sum(source.Substring(lastIdx, i - lastIdx)));
                    lastIdx = i + 1;
                }
                if (i == source.Length - 1)
                    multipliers.Add(Sum(source.Substring(lastIdx)));
            }

            return multipliers.Aggregate<long, long>(1, (x, y) => x * y);
        }
        
        private static bool HasParenthesis(string source) => source.Any(x => x == '(');

        private static long Sum(string source) => source.Split('+').Select(int.Parse).Sum();
    }
}