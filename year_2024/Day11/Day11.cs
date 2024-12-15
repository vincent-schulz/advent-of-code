using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day11
{
    internal class Day11
    {
        public static void Main(string[] args)
        {
            var input = GetInput();

            Console.WriteLine(CountStonesAfterNbBLinks(input, 25));
            Console.WriteLine(CountStonesAfterNbBLinks(input, 75));
        }

        private static Dictionary<long, long> GetInput()
        {
            var input = File.ReadAllText(@"C:\GitHub\advent-of-code\year_2024\Day11\input.txt");
            return input.Split(' ').ToDictionary(long.Parse, x => 1L);
        }

        private static void AddStonesToNewLine(Dictionary<long, long> newLine, long key, long value)
        {
            if (newLine.ContainsKey(key))
            {
                newLine[key] += value;
            }
            else
            {
                newLine.Add(key, value);
            }
        }

        private static long CountStonesAfterNbBLinks(Dictionary<long, long> line, int blinks)
        {
            for (int i = 0; i < blinks; i++)
            {
                var newLine = new Dictionary<long, long>();
                foreach (var stone in line)
                {
                    if (stone.Key == 0)
                    {
                        AddStonesToNewLine(newLine, 1, stone.Value);
                    }
                    else if (stone.Key.ToString().Length % 2 == 0)
                    {
                        var stoneString = stone.Key.ToString();
                        var newStone1 = long.Parse(stoneString.Substring(0, stoneString.Length / 2));
                        var newStone2 = long.Parse(stoneString.Substring(stoneString.Length / 2));
                        AddStonesToNewLine(newLine, newStone1, stone.Value);
                        AddStonesToNewLine(newLine, newStone2, stone.Value);
                    }
                    else
                    {
                        AddStonesToNewLine(newLine, stone.Key * 2024, stone.Value);
                    }
                }
                line = newLine;
            }

            return line.Aggregate(0L, (current, stone) => current + stone.Value);
        }
    }
}