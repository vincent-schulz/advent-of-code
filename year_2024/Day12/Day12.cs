// Without Part 2

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12
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
    
    internal class Day12
    {
        private static char[][] _map = GetInput();
        
        public static void Main(string[] args)
        {
            Console.WriteLine(CalculateTotalPriceOfFencing());
        }

        private static char[][] GetInput()
        {
            var input = File.ReadAllLines(@"C:\GitHub\advent-of-code\year_2024\Day12\input.txt");
            return input.Select(line => line.ToCharArray()).ToArray();
        }

        private static bool IsPositionInBounds(int[] position)
        {
            return 0 <= position[0] && 0 <= position[1] && position[0] < _map.Length && position[1] < _map[0].Length;
        }

        private static int[][] GetAdjacentPlots(int[] plot)
        {
            return new[]
            {
                new[] { plot[0] - 1, plot[1] },
                new[] { plot[0] + 1, plot[1] },
                new[] { plot[0], plot[1] - 1 },
                new[] { plot[0], plot[1] + 1 }
            };
        }

        private static void DetermineRegionOfPlot(char[][] map, List<int[]> region)
        {
            var plant = map[region.Last()[0]][region.Last()[1]];
            var adjacentPlots = GetAdjacentPlots(region.Last());

            foreach (var adjacentPlot in adjacentPlots)
            {
                if (!IsPositionInBounds(adjacentPlot)) continue;
                if (map[adjacentPlot[0]][adjacentPlot[1]] != plant || region.Any(plot => plot.SequenceEqual(adjacentPlot))) continue;
                region.Add(new[] { adjacentPlot[0], adjacentPlot[1] });
                DetermineRegionOfPlot(map, region);
            }
        }

        private static int[][][] DetermineAllRegions()
        {
            var regions = new List<int[][]>();

            for (int x = 0; x < _map.Length; x++)
            {
                for (int y = 0; y < _map[x].Length; y++)
                {
                    var position = new[] { x, y };
                    if (regions.Any(region => region.Any(plot => plot.SequenceEqual(position)))) continue;
                    var newRegion = new List<int[]> { position };
                    DetermineRegionOfPlot(_map, newRegion);
                    regions.Add(newRegion.ToArray());
                }
            }

            return regions.ToArray();
        }

        private static int DeterminePriceForRegion(int[][] region)
        {
            var perimeter = 0;

            foreach (var plot in region)
            {
                var adjacentPlots = GetAdjacentPlots(plot);

                foreach (var adjacentPlot in adjacentPlots)
                {
                    if (!region.Any(p => p.SequenceEqual(adjacentPlot))) perimeter++;
                }
            }

            return perimeter * region.Length;
        }

        private static int CalculateTotalPriceOfFencing()
        {
            return DetermineAllRegions().Sum(DeterminePriceForRegion);
        }
    }
}