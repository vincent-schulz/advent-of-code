using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;


namespace Day01
{
    internal class Day01
    {
        private static void Main(string[] args)
        {
            var input = GetInput();

            Console.WriteLine(MeasureTotalDistance(new List<List<int>>() { input[0].ToList(), input[1].ToList() }));
            Console.WriteLine(MeasureSimilarityScore(input));
        }

        private static int[][] GetInput()
        {
            var input = File.ReadAllText(@"C:\GitHub\advent-of-code\year_2024\Day01\input.txt").Split('\n');
            var left = new List<int>();
            var right = new List<int>();
            foreach (var line in input)
            {
                var lineSplit = line.Split(new[] { "   " }, StringSplitOptions.None);
                left.Add(int.Parse(lineSplit[0]));
                right.Add(int.Parse(lineSplit[1]));
            }

            return new[] { left.ToArray(), right.ToArray() };
        }

        private static int MeasureTotalDistance(List<List<int>> input)
        {
            var totalDistance = 0;
            while (input[0].Count > 0)
            {
                var minLeft = input[0].Min();
                var minRight = input[1].Min();
                totalDistance += Math.Abs(minLeft - minRight);
                input[0].Remove(minLeft);
                input[1].Remove(minRight);
            }

            return totalDistance;
        }

        private static int MeasureSimilarityScore(int[][] input)
        {
            var similarityScore = 0;
            foreach (var id in input[0])
            {
                similarityScore += id * input[1].Count(item => item == id);
            }

            return similarityScore;

            // in linq-expression
            return input[0].Sum(id => id * input[1].Count(item => item == id));
        }
    }
}