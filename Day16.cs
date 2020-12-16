using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace AoC_2020
{
    public static class Day16
    {
        public static void SolvePart2()
        {
            var input = File.ReadAllText("input.txt").Split(Environment.NewLine+Environment.NewLine, StringSplitOptions.None);

            var rules = input[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new
                {
                    name = x.Substring(0, x.IndexOf(':')),
                    ranges = x.Substring(x.IndexOf(':') + 2)
                        .Split(" or ")
                        .Select(y => y
                            .Split('-')
                            .Select(int.Parse).ToList()).ToList()
                })
                .Select(x => new Rule
                {
                    Name = x.name,
                    LowRange = x.ranges[0],
                    HighRange = x.ranges[1]
                }).ToList();
            var myTicket = input[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1).First().Split(',').Select(int.Parse).ToList();
            var nearbyTickets = input[2].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1).Select(x => x.Split(',').Select(int.Parse));
            
            var bannedRules = new Dictionary<int, List<string>>(); 
            foreach (var nearbyTicket in nearbyTickets)
            {
                foreach (var e in GetBannedRules(nearbyTicket, rules))
                {
                    if (!bannedRules.ContainsKey(e.Key)) bannedRules[e.Key] = e.Value;
                    else bannedRules[e.Key].AddRange(e.Value);
                }
            }

            var allowed = bannedRules.OrderBy(e => e.Key)
                .ToDictionary(banned => banned.Key, banned => rules.Select(x => x.Name)
                    .Where(rule => !banned.Value.Contains(rule))
                    .ToList())
                .OrderBy(x => x.Value.Count);
            var orderedRules = new Dictionary<int, string>();
            foreach (var (allowIdx, allowRules) in allowed)
            {
                var name = allowRules.First(x => !orderedRules.Select(y => y.Value).Contains(x));
                orderedRules.Add(allowIdx, name);
            }

            var answer = orderedRules.Where(x => x.Value.StartsWith("departure"))
                .Select(x => x.Key)
                .Aggregate<int, long>(1, (current, idx) => current * myTicket[idx]);

            Console.WriteLine(answer);
        }

        private static Dictionary<int, List<string>> GetBannedRules(IEnumerable<int> nearbyTicket, IList<Rule> rules)
        {
            var counter = 0;

            var tempCannotBe = new Dictionary<int, List<string>>();
            foreach (var number in nearbyTicket)
            {
                foreach (var rule in rules)
                {
                    if ((number < rule.LowRange[0] || number > rule.LowRange[1]) &&
                        (number < rule.HighRange[0] || number > rule.HighRange[1]))
                    {
                        if (!tempCannotBe.ContainsKey(counter)) tempCannotBe[counter] = new List<string> {rule.Name};
                        else tempCannotBe[counter].Add(rule.Name);
                    }
                }

                counter++;
            }

            return tempCannotBe.All(x => x.Value.Count < rules.Count)
                ? tempCannotBe
                : new Dictionary<int, List<string>>(0);
        }
    }

    public class Rule
    {
        public string Name { get; set; }
        public List<int> LowRange { get; set; }
        public List<int> HighRange { get; set; }
    }
}