using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AoC_2020
{
    public static class Day15
    {
        public static void SolvePart2()
        {
            var input = new[] {14, 1, 17, 0, 3, 20};
            const long goal = 30000000;

            var memory = new Dictionary<long, List<long>>();
            for (var i = 0; i < input.Length; i++)
                memory.Add(input[i], new List<long> {i + 1});

            long step = input.Length + 1;
            long lastSpokenNumber = input.Last();

            while (step < goal + 1)
            {
                lastSpokenNumber = memory[lastSpokenNumber].Count == 1
                    ? 0
                    : memory[lastSpokenNumber][^1] - memory[lastSpokenNumber][^2];
                if (!memory.ContainsKey(lastSpokenNumber)) memory.Add(lastSpokenNumber, new List<long> {step});
                else memory[lastSpokenNumber].Add(step);

                step++;
            }

            Console.WriteLine(lastSpokenNumber);
        }
    }
}