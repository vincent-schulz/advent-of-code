using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day08
{
    internal class Day08
    {
        public static void Main(string[] args)
        {
            var map = GetInput();

            Console.WriteLine(CalculateImpactOfSignal(map));
            Console.WriteLine(CalculateImpactOfSignalUpdated(map));
        }

        private static char[][] GetInput()
        {
            var input = File.ReadAllLines(@"C:\GitHub\advent-of-code\year_2024\Day08\input.txt");
            return (from line in input select line.ToCharArray()).ToArray();
        }

        private static Dictionary<char, int[][]> GetAntennaPositions(char[][] map)
        {
            var antennaPositions = new Dictionary<char, List<int[]>>();

            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] != '.')
                    {
                        if (antennaPositions.ContainsKey(map[i][j]))
                        {
                            antennaPositions[map[i][j]].Add(new[] { i, j });
                        }
                        else
                        {
                            antennaPositions.Add(map[i][j], new List<int[]> { new[] { i, j } });
                        }
                    }
                }
            }

            return antennaPositions.ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value.ToArray());
        }

        private static int[] GetSignalPosition(int[] antenna1, int[] antenna2)
        {
            return new[]
            {
                antenna1[0] - (antenna2[0] - antenna1[0]),
                antenna1[1] - (antenna1[1] < antenna2[1]
                    ? antenna2[1] - antenna1[1]
                    : -1 * (antenna1[1] - antenna2[1]))
            };
        }

        private static bool IsSignalInBounds(int[] signal, int upperBound1, int upperBound2)
        {
            return signal[0] >= 0 && signal[0] <= upperBound1 && signal[1] >= 0 && signal[1] <= upperBound2;
        }
        
        private static void AddAllAntinodesOfAFrequencyToSignalMap(char[,] signalMap, int[][] positions)
        {
            for (int i = 0; i < positions.Length - 1; i++)
            {
                for (int j = i + 1; j < positions.Length; j++)
                {
                    var signal1 = GetSignalPosition(positions[i], positions[j]);
                    var signal2 = GetSignalPosition(positions[j], positions[i]);
                    
                    if (IsSignalInBounds(signal1, signalMap.GetUpperBound(0), signalMap.GetUpperBound(1)))
                    {
                        signalMap[signal1[0], signal1[1]] = '#';
                    }

                    if (IsSignalInBounds(signal2, signalMap.GetUpperBound(0), signalMap.GetUpperBound(1)))
                    {
                        signalMap[signal2[0], signal2[1]] = '#';
                    }
                }
            }
        }
        
        private static void AddAllAntinodesOfAFrequencyToSignalMapUpdated(char[,] signalMap, int[][] positions)
        {
            for (int i = 0; i < positions.Length - 1; i++)
            {
                for (int j = i + 1; j < positions.Length; j++)
                {
                    var lastSignal1 = positions[j];
                    var signal1 = positions[i];
                    var lastSignal2 = positions[i];
                    var signal2 = positions[j];

                    while (IsSignalInBounds(signal1, signalMap.GetUpperBound(0), signalMap.GetUpperBound(1)) ||
                           IsSignalInBounds(signal2, signalMap.GetUpperBound(0), signalMap.GetUpperBound(1)))
                    {
                        if (IsSignalInBounds(signal1, signalMap.GetUpperBound(0), signalMap.GetUpperBound(1)))
                        {
                            signalMap[signal1[0], signal1[1]] = '#';
                        }
                        if (IsSignalInBounds(signal2, signalMap.GetUpperBound(0), signalMap.GetUpperBound(1)))
                        {
                            signalMap[signal2[0], signal2[1]] = '#';
                        }

                        var newSignal1 = GetSignalPosition(signal1, lastSignal1);
                        lastSignal1 = signal1;
                        signal1 = newSignal1;
                        var newSignal2 = GetSignalPosition(signal2, lastSignal2);
                        lastSignal2 = signal2;
                        signal2 = newSignal2;
                    }
                }
            }
        }

        private static int CalculateImpactOfSignal(char[][] map)
        {
            var signalMap = new char[map.Length, map[0].Length];
            var antennaPositions = GetAntennaPositions(map);

            foreach (var frequencyPositions in antennaPositions)
            {
                AddAllAntinodesOfAFrequencyToSignalMap(signalMap, frequencyPositions.Value);
            }

            return (from char item in signalMap where item == '#' select item).Count();
        }

        private static int CalculateImpactOfSignalUpdated(char[][] map)
        {
            var signalMap = new char[map.Length, map[0].Length];
            var antennaPositions = GetAntennaPositions(map);

            foreach (var frequencyPositions in antennaPositions)
            {
                AddAllAntinodesOfAFrequencyToSignalMapUpdated(signalMap, frequencyPositions.Value);
            }

            return (from char item in signalMap where item == '#' select item).Count();
        }
    }
}