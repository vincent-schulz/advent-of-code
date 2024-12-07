using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Day03
{
    internal class Day03
    {
        public static void Main(string[] args)
        {
            var input = File.ReadAllText(@"C:\GitHub\advent-of-code\year_2024\Day03\input.txt");
            
            Console.WriteLine(SumOfMultiplications(input));
            Console.WriteLine(SumOfSwitchingMultiplications(input));
        }

        private static int SumOfMultiplications(string input)
        {
            var pattern = new Regex(@"mul\(([0-9]+),([0-9]+)\)");
            var sum = 0;
            foreach (Match match in pattern.Matches(input))
            {
                if (match.Groups[1].Length <= 3 && match.Groups[2].Length <= 3)
                {
                    sum += int.Parse(match.Groups[1].ToString()) * int.Parse(match.Groups[2].ToString());
                }
            }
            
            return sum;
        }

        private static int SumOfSwitchingMultiplications(string input)
        {
            var pattern = new Regex(@"mul\(([0-9]+),([0-9]+)\)|do\(\)|don't\(\)");
            var sum = 0;
            var enabled = 1;
            foreach (Match match in pattern.Matches(input))
            {
                if (match.Value.Equals("do()"))
                {
                    enabled = 1;
                } 
                else if (match.Value.Equals("don't()"))
                {
                    enabled = 0;
                }
                else
                {
                    if (match.Groups[1].Length <= 3 && match.Groups[2].Length <= 3)
                    {
                        sum += enabled * int.Parse(match.Groups[1].ToString()) * int.Parse(match.Groups[2].ToString());
                    }
                }
            }

            return sum;
        }
    }
}