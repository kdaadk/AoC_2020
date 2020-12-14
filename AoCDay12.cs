using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2020
{
    public class AoCDay12
    {
        private static Dictionary<string, string> directonsMap = new Dictionary<string, string>()
        {
            {"RE", "ESWN"}, {"RN", "NESW"}, {"RW", "WNES"}, {"RS", "SWNE"}, {"LE", "ENWS"}, {"LN", "NWSE"},
            {"LW", "WSEN"}, {"LS", "SENW"}
        };

        public static void SolvePart2()
        {
            var input = File.ReadAllLines("input.txt");
            var commands = input.Select(x => new {command = x[0], dist = int.Parse(x.Substring(1))});

            var (horDirection, horPower) = ('E', 10);
            var (verDirection, verPower) = ('N', 1);
            var distanceVert = 0;
            var distanceHor = 0;
            foreach (var e in commands)
            {
                if (e.command == 'F')
                {
                    distanceVert += GetDist(verDirection, e.dist * verPower, 'N', 'S');
                    distanceHor += GetDist(horDirection, e.dist * horPower, 'E', 'W');
                }

                if (e.command == 'N' || e.command == 'S' || e.command == 'E' || e.command == 'W')
                {
                    (verDirection, verPower) = GetDistPow(e.command, e.dist, (verDirection, verPower), 'N', 'S');
                    (horDirection, horPower) = GetDistPow(e.command, e.dist, (horDirection, horPower), 'E', 'W');
                }

                if (e.command == 'L' || e.command == 'R')
                    ((verDirection, verPower), (horDirection, horPower)) = GetDirection((verDirection, verPower),
                        (horDirection, horPower), e.command, e.dist);
            }

            Console.WriteLine(Math.Abs(distanceVert) + Math.Abs(distanceHor));
        }

        private static ((char, int), (char, int)) GetDirection((char dir, int power) verCurrent,
            (char dir, int power) horCurrent, char rotateDirection, int degree)
        {
            var rotateCount = degree % 360 / 90;
            var nextHorDir = directonsMap[$"{rotateDirection}{horCurrent.dir}"][rotateCount];
            var nextVerDir = directonsMap[$"{rotateDirection}{verCurrent.dir}"][rotateCount];

            var (vert, hor) = ((nextVerDir, verCurrent.power), (nextHorDir, horCurrent.power));
            return rotateCount % 2 == 0 ? (vert, hor) : (hor, vert);
        }

        private static int GetDist(char direction, int points, char dir, char oppositeDir)
        {
            if (direction == dir) return points;
            if (direction == oppositeDir) return -points;
            return 0;
        }

        private static (char, int) GetDistPow(char direction, int points, (char dir, int power) way, char dir,
            char oppositeDir)
        {
            if (direction == way.dir) return (way.dir, way.power + points);
            var pts = way.power - points;
            if (direction == dir && way.dir == oppositeDir || direction == oppositeDir && way.dir == dir)
                return pts < 0 ? (direction, Math.Abs(pts)) : (way.dir, pts);
            return way;
        }
    }
}