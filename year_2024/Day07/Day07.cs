using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day07
{
    internal class Day07
    {
        public static void Main(string[] args)
        {
            var testValues = GetTestValues();
            var calibrationEquations = GetCalibrationEquations();

            Console.WriteLine(GetTotalCalibrationResult(testValues, calibrationEquations, new []{'+', '*'})); 
            Console.WriteLine(GetTotalCalibrationResult(testValues, calibrationEquations, new []{'+', '*', '|'}));
        }

        private static long[] GetTestValues()
        {
            var input = File.ReadAllLines(@"C:\GitHub\advent-of-code\year_2024\Day07\input.txt");
            return (from item in input select long.Parse(item.Split(':')[0])).ToArray();
        }
        
        private static int[][] GetCalibrationEquations()
        {
            var input = File.ReadAllLines(@"C:\GitHub\advent-of-code\year_2024\Day07\input.txt");
            return (from item in input select (from num in item.Split(new[] { ": " }, StringSplitOptions.None)[1].Split(' ') select int.Parse(num)).ToArray()).ToArray();
        }

        private static char[][] GenerateOperatorCombinations(int length, char[] operators)
        {
            var combinations = new List<char[]>();

            void Generate(string current, int depth)
            {
                if (depth == length)
                {
                    combinations.Add(current.ToCharArray());
                    return;
                }

                foreach (var op in operators)
                {
                    Generate(current + op, depth + 1);
                }
            }
            
            Generate(string.Empty, 0);
            return combinations.ToArray();
        }

        private static long CalculateEquation(int[] equation, char[] operators)
        {
            long result = equation[0];
            for (int i = 0; i < operators.Length; i++)
            {
                switch (operators[i])
                {
                    case '+':
                        result += equation[i + 1];
                        break;
                    case '*':
                        result *= equation[i + 1];
                        break;
                    case '|':
                        result = long.Parse(result.ToString() + equation[i + 1].ToString());
                        break;
                }
            }
            return result;
        }

        private static bool IsEquationTrue(long testValue, int[] equation, char[] operators)
        {
            var operatorCombinations = GenerateOperatorCombinations(equation.Length - 1, operators);
            foreach (var operatorCombination in operatorCombinations)
            {
                if (CalculateEquation(equation, operatorCombination) == testValue)
                {
                    return true;
                }
            }
            
            return false;
        }
        
        private static long GetTotalCalibrationResult(long[] testValues, int[][] calibrationEquations, char[] operators)
        {
            var totalCalibrationResult = 0L;

            for (int i = 0; i < testValues.Length; i++)
            {
                if (IsEquationTrue(testValues[i], calibrationEquations[i], operators))
                {
                    totalCalibrationResult += testValues[i];
                }
            }
            
            return totalCalibrationResult;
        }
    }
}