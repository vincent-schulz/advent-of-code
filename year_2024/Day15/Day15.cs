using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day15
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

    internal class Day15
    {
        static int[] _position;
        static int[] _scaledPosition;

        public static void Main(string[] args)
        {
            var map = GetMapInput();
            var scaledMap = ScaleMap(map);
            var moves = GetMovesInput();
            _position = GetRobotsPosition(map);
            _scaledPosition = GetRobotsPosition(scaledMap);

            Console.WriteLine(SumOfAllBoxesGpsCoordinatesAfterMoves(map, moves));
            Console.WriteLine(SumOfAllScaledBoxesGpsCoordinatesAfterMoves(scaledMap, moves));
        }

        // Input
        private static char[][] GetMapInput()
        {
            var input = File.ReadAllLines(@"C:\GitHub\advent-of-code\year_2024\Day15\mapInput.txt");
            return input.Select(line => line.ToCharArray()).ToArray();
        }

        private static char[] GetMovesInput()
        {
            var input = File.ReadAllText(@"C:\GitHub\advent-of-code\year_2024\Day15\movementsInput.txt");
            return input.Where(c => c != '\n' && c != '\r').ToArray();
        }

        // Util
        private static int[] GetRobotsPosition(char[][] map)
        {
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[0].Length; j++)
                {
                    if (map[i][j] == '@') return new[] { i, j };
                }
            }

            return new[] { -1, -1 };
        }

        private static void PrintMap(char[][] map)
        {
            foreach (var line in map)
            {
                foreach (var c in line)
                {
                    Console.Write(c);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private static char[][] ScaleMap(char[][] map)
        {
            var scaledMap = new List<char[]>();

            foreach (var line in map)
            {
                var scaledLine = new List<char>();
                foreach (var c in line)
                {
                    switch (c)
                    {
                        case '@':
                            scaledLine.Add('@');
                            scaledLine.Add('.');
                            break;
                        case 'O':
                            scaledLine.Add('[');
                            scaledLine.Add(']');
                            break;
                        default:
                            scaledLine.Add(c);
                            scaledLine.Add(c);
                            break;
                    }
                }

                scaledMap.Add(scaledLine.ToArray());
            }

            return scaledMap.ToArray();
        }

        private static int[] GetPositionInFront(int[] position, char direction)
        {
            switch (direction)
            {
                case '^': return new[] { position[0] - 1, position[1] };
                case '<': return new[] { position[0], position[1] - 1 };
                case '>': return new[] { position[0], position[1] + 1 };
                default: return new[] { position[0] + 1, position[1] };
            }
        }

        // Part 1
        private static void NextMove(char[][] map, char move)
        {
            var positionInFront = GetPositionInFront(_position, move);
            if (map[positionInFront[0]][positionInFront[1]] == '#') return;
            if (map[positionInFront[0]][positionInFront[1]] == '.')
            {
                map[_position[0]][_position[1]] = '.';
                _position = positionInFront;
                map[_position[0]][_position[1]] = '@';
                return;
            }

            var nextPositionInFront = GetPositionInFront(positionInFront, move);
            while (map[nextPositionInFront[0]][nextPositionInFront[1]] == 'O')
            {
                nextPositionInFront = GetPositionInFront(nextPositionInFront, move);
            }

            if (map[nextPositionInFront[0]][nextPositionInFront[1]] == '#') return;
            map[nextPositionInFront[0]][nextPositionInFront[1]] = 'O';
            map[_position[0]][_position[1]] = '.';
            _position = positionInFront;
            map[_position[0]][_position[1]] = '@';
        }

        private static int SumOfAllBoxesGpsCoordinatesAfterMoves(char[][] map, char[] moves)
        {
            foreach (var move in moves)
            {
                NextMove(map, move);
            }

            var sumOfGpsCoordinates = 0;
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[0].Length; j++)
                {
                    if (map[i][j] == 'O') sumOfGpsCoordinates += 100 * i + j;
                }
            }

            return sumOfGpsCoordinates;
        }


        // Part 2
        private static List<int[]> GetPositionsInFront(char[][] map, List<int[]> positions, char move)
        {
            var positionsInFront = new List<int[]>();
            foreach (var position in positions)
            {
                var positionInFront1 = GetPositionInFront(position, move);
                if (map[positionInFront1[0]][positionInFront1[1]] == '#') return new List<int[]> { positionInFront1 };
                if (map[positionInFront1[0]][positionInFront1[1]] == '.') continue;
                var positionInFront2 = map[positionInFront1[0]][positionInFront1[1]] == '['
                    ? new[] { positionInFront1[0], positionInFront1[1] + 1 }
                    : new[] { positionInFront1[0], positionInFront1[1] - 1 };
                positionsInFront.Add(positionInFront1);
                positionsInFront.Add(positionInFront2);
            }
            if (positionsInFront.Count == 0) return positionsInFront;
            return positionsInFront.Concat(GetPositionsInFront(map, positionsInFront, move)).ToList();
        }

        private static void NextMoveScaled(char[][] map, char move)
        {
            var positionInFront = GetPositionInFront(_scaledPosition, move);
            if (map[positionInFront[0]][positionInFront[1]] == '#') return;
            if (map[positionInFront[0]][positionInFront[1]] == '.')
            {
                map[_scaledPosition[0]][_scaledPosition[1]] = '.';
                _scaledPosition = positionInFront;
                map[_scaledPosition[0]][_scaledPosition[1]] = '@';
                return;
            }

            var positionsInFront = new List<int[]>();
            if (move == '<' || move == '>')
            {
                positionsInFront.Add(positionInFront);
                var nextPositionInFront = GetPositionInFront(positionInFront, move);
                while (map[nextPositionInFront[0]][nextPositionInFront[1]] == '[' ||
                       map[nextPositionInFront[0]][nextPositionInFront[1]] == ']')
                {
                    positionsInFront.Add(nextPositionInFront);
                    nextPositionInFront = GetPositionInFront(nextPositionInFront, move);
                }
                if (map[nextPositionInFront[0]][nextPositionInFront[1]] == '#') return;
                positionsInFront.Reverse();
            }
            else
            {
                positionsInFront = GetPositionsInFront(map, new List<int[]> { _scaledPosition }, move);
                if (positionsInFront.Any(pos => map[pos[0]][pos[1]] == '#')) return;
                positionsInFront = positionsInFront.Where(pos => map[pos[0]][pos[1]] != '.').Distinct(new PositionComparer()).Reverse().ToList();
            }
            foreach (var position in positionsInFront)
            {
                var posInFront = GetPositionInFront(position, move);
                map[posInFront[0]][posInFront[1]] = map[position[0]][position[1]];
                map[position[0]][position[1]] = '.';
            }
            map[_scaledPosition[0]][_scaledPosition[1]] = '.';
            _scaledPosition = positionInFront;
            map[_scaledPosition[0]][_scaledPosition[1]] = '@';
        }

        private static int SumOfAllScaledBoxesGpsCoordinatesAfterMoves(char[][] map, char[] moves)
        {
            for (int i = 0; i < moves.Length; i++)
            {
                NextMoveScaled(map, moves[i]);
            }

            var sumOfGpsCoordinates = 0;
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[0].Length; j++)
                {
                    if (map[i][j] == '[') sumOfGpsCoordinates += 100 * i + j;
                }
            }

            return sumOfGpsCoordinates;
        }
    }
}