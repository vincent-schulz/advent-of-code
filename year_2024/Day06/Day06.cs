using System;
using System.IO;
using System.Linq;

namespace Day06
{
    internal class Day06
    {
        public static void Main(string[] args)
        {
            var map = GetInput();

            Console.WriteLine(CountPositions((from line in map select (char[])line.Clone()).ToArray()));
            Console.WriteLine(CountObstructions((from line in map select (char[])line.Clone()).ToArray()));
        }

        private static char[][] GetInput()
        {
            var input = File.ReadAllLines(@"C:\GitHub\advent-of-code\year_2024\Day06\input.txt");

            return (from line in input select line.ToArray()).ToArray();
        }

        private static int[] GetPosition(char[][] map)
        {
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == '^' || map[i][j] == '<' || map[i][j] == '>' || map[i][j] == 'v')
                    {
                        return new[] { i, j };
                    }
                }
            }

            return new[] { -1, -1 };
        }

        private static int[] GetPositionInFront(int[] position, char direction)
        {
            switch (direction)
            {
                case '^':
                    return new[] { position[0] - 1, position[1] };
                case '<':
                    return new[] { position[0], position[1] - 1 };
                case '>':
                    return new[] { position[0], position[1] + 1 };
                case 'v':
                    return new[] { position[0] + 1, position[1] };
                default:
                    return new[] { -1, -1 };
            }
        }

        private static char ChangeDirection(char direction)
        {
            switch (direction)
            {
                case '^':
                    return '>';
                case '>':
                    return 'v';
                case 'v':
                    return '<';
                case '<':
                    return '^';
                default:
                    return '^';
            }
        }

        private static char[][] DeterminePath(char[][] map)
        {
            var position = GetPosition(map);
            var direction = '^';

            while (!position.SequenceEqual(new[] { -1, -1 }))
            {
                var positionInFront = GetPositionInFront(position, direction);
                if (positionInFront[0] < 0 || positionInFront[1] < 0 ||
                    positionInFront[0] == map.Length || positionInFront[1] == map[0].Length)
                {
                    map[position[0]][position[1]] = 'X';
                    position = new[] { -1, -1 };
                }
                else
                {
                    if (map[positionInFront[0]][positionInFront[1]] == '#')
                    {
                        direction = ChangeDirection(direction);
                        map[position[0]][position[1]] = direction;
                    }
                    else
                    {
                        map[position[0]][position[1]] = 'X';
                        map[positionInFront[0]][positionInFront[1]] = direction;
                        position = positionInFront;
                    }
                }
            }

            return map;
        }

        private static bool CheckForLoop(char[][] map, int[] obstruction)
        {
            map[obstruction[0]][obstruction[1]] = 'O';
            var countObstructionInFront = 0;
            var steps = 0;

            var position = GetPosition(map);
            var direction = '^';

            while (!position.SequenceEqual(new[] { -1, -1 }))
            {
                var positionInFront = GetPositionInFront(position, direction);
                if (positionInFront[0] < 0 || positionInFront[1] < 0 ||
                    positionInFront[0] == map.Length || positionInFront[1] == map[0].Length)
                {
                    map[position[0]][position[1]] = 'X';
                    position = new[] { -1, -1 };
                }
                else
                {
                    if (map[positionInFront[0]][positionInFront[1]] == '#')
                    {
                        direction = ChangeDirection(direction);
                        map[position[0]][position[1]] = direction;
                    }
                    else if (map[positionInFront[0]][positionInFront[1]] == 'O')
                    {
                        direction = ChangeDirection(direction);
                        map[position[0]][position[1]] = direction;
                        countObstructionInFront++;
                        if (countObstructionInFront >= 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        map[position[0]][position[1]] = 'X';
                        map[positionInFront[0]][positionInFront[1]] = direction;
                        position = positionInFront;
                    }

                    steps++;
                    if (steps > 10_000)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static int CountPositions(char[][] map)
        {
            var path = DeterminePath(map);

            return path.Sum(line => line.Count(pos => pos == 'X'));
        }

        private static int CountObstructions(char[][] map)
        {
            var originalPath = DeterminePath((from line in map select (char[])line.Clone()).ToArray());
            var possibleObstructions = 0;

            for (int i = 0; i < originalPath.Length; i++)
            {
                for (int j = 0; j < originalPath[i].Length; j++)
                {
                    if (originalPath[i][j] == 'X')
                    {
                        var mapCopy = (from line in map select (char[])line.Clone()).ToArray();
                        if (CheckForLoop(mapCopy, new[] { i, j }))
                        {
                            possibleObstructions++;
                        }
                    }
                }
            }

            return possibleObstructions;
        }
    }
}