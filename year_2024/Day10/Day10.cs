using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    internal class PositionComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] x, int[] y)
        {
            return x[0] == y[0] && x[1] == y[1];
        }

        public int GetHashCode(int[] pos)
        {
            return pos[0].GetHashCode() ^ pos[1].GetHashCode();
        }
    }
    
    internal class Day10
    {
        public static void Main(string[] args)
        {
            var map = GetInput();

            Console.WriteLine(SumOfTrailheadScores(map));
            Console.WriteLine(SumOfTrailheadRatings(map));
        }

        private static int[][] GetInput()
        {
            var input = File.ReadAllLines(@"C:\GitHub\advent-of-code\year_2024\Day10\input.txt");
            return input.Select(line => line.Select(x => int.Parse(x.ToString())).ToArray()).ToArray();
        }

        private static int[][] FindTrailheadPositions(int[][] map)
        {
            var trailheadPositions = new List<int[]>();
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] != 0) continue;
                    trailheadPositions.Add(new[] { i, j });
                }
            }

            return trailheadPositions.ToArray();
        }

        private static bool IsPositionInBounds(int[] position, int upperBound1, int upperBound2)
        {
            return 0 <= position[0] && 0 <= position[1] && position[0] < upperBound1 && position[1] < upperBound2;
        }

        private static List<int[]> DetermineSummits(int[][] map, int[] thPos, int height)
        {
            if (height == 9) return new List<int[]>(new []{thPos});
            var adjacentPositions = new[]
            {
                new[] { thPos[0] - 1, thPos[1] },
                new[] { thPos[0], thPos[1] + 1 },
                new[] { thPos[0], thPos[1] - 1 },
                new[] { thPos[0] + 1, thPos[1] }
            };

            var summits = new List<int[]>();
            foreach (var adjacentPosition in adjacentPositions)
            {
                if (!IsPositionInBounds(adjacentPosition, map.Length, map[0].Length)) continue;
                if (map[adjacentPosition[0]][adjacentPosition[1]] != height + 1) continue;
                summits.AddRange(DetermineSummits(map, adjacentPosition, height + 1));
            }
            return summits;
        }

        private static int SumOfTrailheadScores(int[][] map)
        {
            var thPos = FindTrailheadPositions(map);

            return thPos.Sum(thP => DetermineSummits(map, thP, 0).Distinct(new PositionComparer()).Count());
        }
        
        private static int SumOfTrailheadRatings(int[][] map)
        {
            var thPos = FindTrailheadPositions(map);

            return thPos.Sum(thP => DetermineSummits(map, thP, 0).Count());
        }
    }
}