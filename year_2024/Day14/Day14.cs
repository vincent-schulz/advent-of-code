using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day14
{
    internal class Day14
    {
        public static void Main(string[] args)
        {
            var robots = GetInput();

            Console.WriteLine(DetermineSafetyFactor(robots, 101, 103));
            Console.WriteLine(SearchForChristmasTree(robots, 101, 103));
        }

        private static Dictionary<char, int[]>[] GetInput()
        {
            var input = File.ReadAllLines(@"C:\GitHub\advent-of-code\year_2024\Day14\input.txt");

            var pattern = new Regex(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)");

            var robots = new Dictionary<char, int[]>[input.Length];
            for (var i = 0; i < input.Length; i++)
            {
                var match = pattern.Match(input[i]);

                robots[i] = new Dictionary<char, int[]>
                {
                    { 'p', new[] { int.Parse(match.Groups[1].ToString()), int.Parse(match.Groups[2].ToString()) } },
                    { 'v', new[] { int.Parse(match.Groups[3].ToString()), int.Parse(match.Groups[4].ToString()) } }
                };
            }

            return robots;
        }

        private static int[,] GetMapAfterNSeconds(Dictionary<char, int[]>[] robots, int seconds, int width, int height)
        {
            var map = new int [height, width];

            foreach (var robot in robots)
            {
                var newX = (robot['p'][0] + robot['v'][0] * seconds) % width;
                var newY = (robot['p'][1] + robot['v'][1] * seconds) % height;
                if (newX < 0) newX += width;
                if (newY < 0) newY += height;
                map[newY, newX]++;
            }

            return map;
        }

        private static int DetermineSafetyFactor(Dictionary<char, int[]>[] robots, int width, int height)
        {
            var map = GetMapAfterNSeconds(robots, 100, width, height);

            var robotsInQuadrant1 = 0;
            var robotsInQuadrant2 = 0;
            var robotsInQuadrant3 = 0;
            var robotsInQuadrant4 = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == height / 2 || j == width / 2) continue;
                    if (i < height / 2 && j < width / 2) robotsInQuadrant1 += map[i, j];
                    if (i < height / 2 && j > width / 2) robotsInQuadrant2 += map[i, j];
                    if (i > height / 2 && j < width / 2) robotsInQuadrant3 += map[i, j];
                    if (i > height / 2 && j > width / 2) robotsInQuadrant4 += map[i, j];
                }
            }

            return robotsInQuadrant1 * robotsInQuadrant2 * robotsInQuadrant3 * robotsInQuadrant4;
        }

        /**
         * Cheated by searching for the frame of the christmas tree
         */
        private static int SearchForChristmasTree(Dictionary<char, int[]>[] robots, int width, int height)
        {
            var seconds = 1;
            while (true)
            {
                var map = GetMapAfterNSeconds(robots, seconds, width, height);

                var foundTree = false;
                var x = 30;

                for (int i = 0; i < height; i++)
                {
                    var robotsInLine = 0;
                    for (int j = 0; j < width; j++)
                    {
                        if (map[i, j] == 0)
                        {
                            robotsInLine = 0;
                            continue;
                        }
                        robotsInLine++;
                        if (robotsInLine > 30)
                        {
                            foundTree = true;
                        }
                    }
                }

                if (foundTree)
                {
                    return seconds;
                }
                seconds++;
            }
        }
    }
}