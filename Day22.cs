using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace AoC_2020
{
    public class Day22
    {
        public static void SolvePart2()
        {
            var input = File.ReadAllText("input.txt").Split(Environment.NewLine + Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Skip(1)
                .Select(int.Parse)).ToList();

            var player1 = new Queue<int>(input[0]);
            var player2 = new Queue<int>(input[1]);
            var wonDeck = IsFirstWon(player1, player2) ? player1 : player2;
            
            var res = 0;
            for (var i = wonDeck.Count; i >= 1; i--)
                res += i * wonDeck.Dequeue();

            Console.WriteLine(res);
        }

        private static bool IsFirstWon(Queue<int> queue1, Queue<int> queue2)
        {
            var alreadyPlayed = new HashSet<string>();
            while (queue1.Count != 0 && queue2.Count != 0)
            {
                var hash = string.Join("", queue1)+","+string.Join("", queue2);
                if (alreadyPlayed.Contains(hash))
                    return true;
                alreadyPlayed.Add(hash);
                
                var top1 = queue1.Dequeue();
                var top2 = queue2.Dequeue();
                
                if (queue1.Count >= top1 && queue2.Count >= top2)
                {
                    var copy1 = new Queue<int>(queue1.Take(top1));
                    var copy2 = new Queue<int>(queue2.Take(top2));
                    if (IsFirstWon(copy1, copy2))
                        Enqueue(queue1, top1, top2);
                    else
                        Enqueue(queue2, top2, top1);
                }
                else if (top1 > top2)
                    Enqueue(queue1, top1, top2);
                else
                    Enqueue(queue2, top2, top1);
            }

            return queue1.Count != 0;
        }

        private static void Enqueue(Queue<int> queue, int first, int second)
        {
            queue.Enqueue(first);
            queue.Enqueue(second);
        }
    }
}