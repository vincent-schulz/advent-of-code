using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day13
{
    internal class Day13
    {
        public static void Main(string[] args)
        {
            var clawMachines = GetInput();
            
            Console.WriteLine(FewestTokensForPossiblePrizes(clawMachines));
            Console.WriteLine(FewestTokensForPossiblePrizesCorrected(clawMachines));
        }

        private static Dictionary<char, double[]>[] GetInput()
        {
            var input = File.ReadAllText(@"C:\GitHub\advent-of-code\year_2024\Day13\input.txt")
                .Split(new[] { "\r\n\r\n" }, StringSplitOptions.None);

            var machines = new Dictionary<char, double[]>[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                var buttonA = new Regex(@"Button A: X\+([0-9]+), Y\+([0-9]+)").Match(input[i]);
                var buttonB = new Regex(@"Button B: X\+([0-9]+), Y\+([0-9]+)").Match(input[i]);
                var prize = new Regex(@"Prize: X=([0-9]+), Y=([0-9]+)").Match(input[i]);

                machines[i] = new Dictionary<char, double[]>
                {
                    {
                        'A', new[] { double.Parse(buttonA.Groups[1].ToString()), double.Parse(buttonA.Groups[2].ToString()) }
                    },
                    {
                        'B', new[] { double.Parse(buttonB.Groups[1].ToString()), double.Parse(buttonB.Groups[2].ToString()) }
                    },
                    {
                        'P', new[] { double.Parse(prize.Groups[1].ToString()), double.Parse(prize.Groups[2].ToString()) }
                    }
                };
            }

            return machines;
        }

        private static int FewestTokensForPossiblePrizes(Dictionary<char, double[]>[] clawMachines)
        {
            var tokens = 0;

            foreach (var cM in clawMachines)
            {
                var a = (cM['P'][0] * cM['B'][1] - cM['P'][1] * cM['B'][0]) /
                                (cM['A'][0] * cM['B'][1] - cM['A'][1] * cM['B'][0]);
                var b = (cM['P'][0] - cM['A'][0] * a) / cM['B'][0];
                
                if (a % 1 != 0 || b % 1 != 0) continue;
                tokens += 3 * (int) a + (int) b;
            }

            return tokens;
        }

        private static long FewestTokensForPossiblePrizesCorrected(Dictionary<char, double[]>[] clawMachines)
        {
           var tokens = 0L;

           foreach (var cM in clawMachines)
           {
               cM['P'][0] += 10_000_000_000_000;
               cM['P'][1] += 10_000_000_000_000;
               
               var a = (cM['P'][0] * cM['B'][1] - cM['P'][1] * cM['B'][0]) /
                                (cM['A'][0] * cM['B'][1] - cM['A'][1] * cM['B'][0]);
               var b = (cM['P'][0] - cM['A'][0] * a) / cM['B'][0];
                
               if (a % 1 != 0 || b % 1 != 0) continue;
               tokens += 3 * (long) a + (long) b;
           }

           return tokens;
        }
    }
}