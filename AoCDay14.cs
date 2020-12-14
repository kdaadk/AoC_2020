using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC_2020
{
    public static class AoCDay14
    {
        public static void SolvePart2()
        {
            var input = File.ReadAllText("input.txt");

            var memory = new Dictionary<long, long>();
            foreach (var block in input.Split("mask = ", StringSplitOptions.RemoveEmptyEntries))
            {
                var lines = block.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                var mask = lines[0];
                var changes = mask.ToCharArray()
                    .Select((x, idx) => (value: x, idx))
                    .ToDictionary(x => x.idx, x => x.value);
                var instructions = lines.Skip(1)
                    .Select(x => x.Where(c => char.IsDigit(c) || c == '=').ToArray())
                    .Select(x => new string(x).Split("="))
                    .Select(x => (idx: int.Parse(x[0]), value: long.Parse(x[1])));

                foreach (var instruction in instructions)
                    foreach (var address in GetMemoryAddresses(instruction, changes))
                        memory[address] = instruction.value;
            }
            
            Console.WriteLine(memory.Select(x => x.Value).Sum());
        }

        private static IEnumerable<long> GetMemoryAddresses((int idx, long value) instruction, Dictionary<int,char> changes)
        {
            var binary = ToBinaryString(instruction.idx);
            var sb = new StringBuilder();
            for (var i = 0; i < binary.Length; i++)
            {
                if (changes.ContainsKey(i) && changes[i] != '0') sb.Append(changes[i]);
                else sb.Append(binary[i]);
            }

            return GetAllPermutations(sb.ToString().ToArray(), 0, new List<string>())
                .Select(ToInt36);
        }

        private static List<string> GetAllPermutations(char[] source, int idx, List<string> permutations)
        {
            if (idx == source.Length - 1)
            {
                if (source[idx] == 'X')
                {
                    var copy = new char[source.Length-1];
                    Array.Copy(source, copy, source.Length - 1);
                    permutations.Add(new string(copy) + '0');
                    permutations.Add(new string(copy) + '1');
                }
                else permutations.Add(new string(source));

                return permutations;
            }


            if (source[idx] == 'X')
            {
                var copy1 = new char[source.Length];
                Array.Copy(source, copy1, source.Length);
                copy1[idx] = '1';
                GetAllPermutations(copy1, idx+1, permutations);
                
                var copy0 = new char[source.Length];
                Array.Copy(source, copy0, source.Length);
                copy0[idx] = '0';
                return GetAllPermutations(copy0, idx+1, permutations);
            }
            return GetAllPermutations(source, idx+1, permutations);
        }

        private static string ToBinaryString(long source)
        {
            var result = string.Empty;
            while (source > 0)
            {
                var remainder = (int) (source % 2);
                source /= 2;
                result = remainder + result;
            }
            return result.PadLeft(36);
        }
        
        private static long ToInt36(string source)
        {
            return source.Reverse().Select((t, idx) => t == '1' ? (long) Math.Pow(2, idx) : 0).Sum();
        }
    }
}