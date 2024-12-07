using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day02
{
    internal class Day02
    {
        public static void Main(string[] args)
        {
            var reports = GetInput();

            Console.WriteLine(CountSafeReports(reports));
            Console.WriteLine(CountSafeReportsWithDampener(reports));
        }

        private static int[][] GetInput()
        {
            var input = File.ReadAllText(@"C:\GitHub\advent-of-code\year_2024\Day02\input.txt").Split('\n');
            var reports = new List<int[]>();

            foreach (var line in input)
            {
                var stringArray = line.Split(' ');
                reports.Add((from item in stringArray select int.Parse(item)).ToArray());
            }

            return reports.ToArray();
        }

        private static bool IsReportSafe(int[] report)
        {
            var inc = report[0] < report[report.Length - 1] ? 1 : -1;
            return Enumerable.Range(1, report.Length - 1).All(i => 
                1 <= inc * (report[i] - report[i - 1]) && 
                inc * (report[i] - report[i - 1]) <= 3);
        }

        private static int CountSafeReports(int[][] reports)
        {
            return reports.Sum(report => IsReportSafe(report) ? 1 : 0);;
        }

        private static int CountSafeReportsWithDampener(int[][] reports)
        {
            var safeReports = 0;

            foreach (var report in reports)
            {
                var isReportSafe = IsReportSafe(report);
                if (!isReportSafe)
                {
                    for (var i = 0; i < report.Length; i++)
                    {
                        var tempReport = report.Take(i).Concat(report.Skip(i + 1).Take(report.Length - i - 1)).ToArray();
                        if (IsReportSafe(tempReport))
                        {
                            isReportSafe = true;
                            break;
                        }
                    }   
                }
                safeReports += isReportSafe ? 1 : 0;
            }
            
            return safeReports;
        }
    }
}