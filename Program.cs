using System.IO;

namespace AoC_2020
{
    internal static class Program
    {
        
        private static void Main()
        {
            var input = File.ReadAllLines("input.txt");
            AoCDay13.SolvePart2(input);
        }
    }
}