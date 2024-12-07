using System;
using System.IO;
using System.Linq;

namespace Day04
{
    internal class Day04
    {
        public static void Main(string[] args)
        {
            var puzzle = GetInput();
            
            Console.WriteLine(CountXmas(puzzle));
            Console.WriteLine(CountXMas(puzzle));
        }

        private static char[][] GetInput()
        {
            var input = File.ReadAllLines(@"C:\GitHub\advent-of-code\year_2024\Day04\input.txt");
            return (from line in input select line.ToCharArray()).ToArray();
        }

        private static int CountXmas(char[][] puzzle)
        {
            var xmas = new[] { 'X', 'M', 'A', 'S' };
            var count = 0;

            for (int row = 0; row < puzzle.Length; row++)
            {
                for (int col = 0; col < puzzle[row].Length; col++)
                {
                    if (3 <= col)
                    {
                        count += puzzle[row][col - 3] == xmas[0] && puzzle[row][col - 2] == xmas[1] && puzzle[row][col - 1] == xmas[2] && puzzle[row][col] == xmas[3] ? 1 : 0;
                        count += puzzle[row][col - 3] == xmas[3] && puzzle[row][col - 2] == xmas[2] && puzzle[row][col - 1] == xmas[1] && puzzle[row][col] == xmas[0] ? 1 : 0;
                    }
                    if (row <= puzzle.Length - 4)
                    {
                        count += puzzle[row][col] == xmas[0] && puzzle[row + 1][col] == xmas[1] && puzzle[row + 2][col] == xmas[2] && puzzle[row + 3][col] == xmas[3] ? 1 : 0;
                        count += puzzle[row + 3][col] == xmas[0] && puzzle[row + 2][col] == xmas[1] && puzzle[row + 1][col] == xmas[2] && puzzle[row][col] == xmas[3] ? 1 : 0;
                    }
                    if (3 <= col && row <= puzzle.Length - 4)
                    {
                        count += puzzle[row][col - 3] == xmas[0] && puzzle[row + 1][col - 2] == xmas[1] && puzzle[row + 2][col - 1] == xmas[2] && puzzle[row + 3][col] == xmas[3] ? 1 : 0;
                        count += puzzle[row + 3][col - 3] == xmas[0] && puzzle[row + 2][col - 2] == xmas[1] && puzzle[row + 1][col - 1] == xmas[2] && puzzle[row][col] == xmas[3] ? 1 : 0;
                        count += puzzle[row][col - 3] == xmas[3] && puzzle[row + 1][col - 2] == xmas[2] && puzzle[row + 2][col - 1] == xmas[1] && puzzle[row + 3][col] == xmas[0] ? 1 : 0;
                        count += puzzle[row + 3][col - 3] == xmas[3] && puzzle[row + 2][col - 2] == xmas[2] && puzzle[row + 1][col - 1] == xmas[1] && puzzle[row][col] == xmas[0] ? 1 : 0;
                    }
                }
            }

            return count;
        }

        private static int CountXMas(char[][] puzzle)
        {
            var xmas = new[] { 'X', 'M', 'A', 'S' };
            var count = 0;

            for (int row = 0; row < puzzle.Length; row++)
            {
                for (int col = 0; col < puzzle[row].Length; col++)
                {
                    if (2 <= col && row <= puzzle.Length - 3)
                    {
                        if (puzzle[row + 1][col - 1] == xmas[2])
                        {
                            count += puzzle[row][col - 2] == xmas[1] && puzzle[row][col] == xmas[1] && puzzle[row + 2][col - 2] == xmas[3] && puzzle[row + 2][col] == xmas[3] ? 1 : 0;
                            count += puzzle[row][col - 2] == xmas[3] && puzzle[row][col] == xmas[3] && puzzle[row + 2][col - 2] == xmas[1] && puzzle[row + 2][col] == xmas[1] ? 1 : 0;
                            count += puzzle[row][col - 2] == xmas[3] && puzzle[row][col] == xmas[1] && puzzle[row + 2][col - 2] == xmas[3] && puzzle[row + 2][col] == xmas[1] ? 1 : 0;
                            count += puzzle[row][col - 2] == xmas[1] && puzzle[row][col] == xmas[3] && puzzle[row + 2][col - 2] == xmas[1] && puzzle[row + 2][col] == xmas[3] ? 1 : 0;
                        }
                    }
                }
            }

            return count;
        }
    }
}