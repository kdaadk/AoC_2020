using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2020
{
    public static class Day13
    {
        private static Dictionary<int, List<long>> speedUp = new Dictionary<int, List<long>> {{1,new List<long>()}};

        public static void SolvePart2()
        {
            var input = File.ReadAllLines("input.txt")[1].Split(",").ToArray();

            var schedule = input.Select((x, idx) => new {bus = x, diff = idx})
                .Where(x => x.bus != "x")
                .Select(x => new {bus = int.Parse(x.bus), x.diff})
                .ToDictionary(x => x.bus, x => (dif: x.diff, time: 0L));
            var buses = input.Where(x => x != "x").Select(int.Parse).ToArray();

            long time = 0;
            long nextTime = schedule.Keys.First();

            while (!CheckSchedule(schedule, schedule.Count))
            {
                schedule = GetNextSchedule(time, buses, schedule);
                nextTime = GetNextTime(schedule, nextTime);
                time += nextTime;
            }

            Console.WriteLine(time);
        }

        private static Dictionary<int, (int dif, long time)> GetNextSchedule(long time, int[] buses, Dictionary<int, (int dif, long time)> schedule)
        {
            long prevTime;
            prevTime = time;
            foreach (var bus in buses)
            {
                if (schedule.Keys.First() == bus)
                    schedule[bus] = (schedule[bus].dif, time);
                else
                {
                    var nextBusTime = (time / bus + 1) * bus;

                    while (nextBusTime <= prevTime)
                        nextBusTime += bus;

                    schedule[bus] = (schedule[bus].dif, nextBusTime);
                    prevTime = nextBusTime;
                }
            }

            return schedule;
        }

        private static long GetNextTime(Dictionary<int, (int, long)> schedule, long currentSpeed)
        {
            var speed = speedUp.Last().Key;
            if (CheckSchedule(schedule, speed))
            {
                speedUp[speed].Add(schedule.Values.First().Item2);
                if (speedUp[speed].Count == 2)
                {
                    var nextSpeed = speedUp[speed][1] - speedUp[speed][0];
                    Console.WriteLine($"next speed #{speed}: {nextSpeed}");
                    speedUp[speed+1] = new List<long>();
                    return nextSpeed;
                }
            }

            return currentSpeed;
        }

        private static bool CheckSchedule(Dictionary<int, (int, long)> schedule, int elemCount)
        {
            var idx = 0;
            foreach (var (bus, (dif, time)) in schedule)
            {
                if (schedule.Values.First().Item2 + dif != time)
                    return false;

                if (++idx > elemCount) return true;
            }
            return true;
        }    
    }
}