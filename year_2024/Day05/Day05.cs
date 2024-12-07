using System;
using System.IO;
using System.Linq;

namespace Day05
{
    internal class Day05
    {
        public static void Main(string[] args)
        {
            var rules = GetRulesInput();
            var updates = GetUpdatesInput();
            
            Console.WriteLine(MiddleSumOfCorrectUpdates(rules, updates));
            Console.WriteLine(MiddleSumOfCorrectedUpdates(rules, updates));
        }

        private static int[][] GetRulesInput()
        {
            var input = File.ReadAllLines(@"C:\GitHub\advent-of-code\year_2024\Day05\rulesInput.txt");
            return (from line in input select (from i in line.Split('|') select int.Parse(i)).ToArray()).ToArray();
        }

        private static int[][] GetUpdatesInput()
        {
            var input = File.ReadAllLines(@"C:\GitHub\advent-of-code\year_2024\Day05\updatesInput.txt");
            return (from line in input select (from i in line.Split(',') select int.Parse(i)).ToArray()).ToArray();
        }

        private static bool IsUpdateCorrect(int[] update, int[][] rules)
        {
            foreach (var page in update)
            {
                var pageRulesBefore = (from rule in rules where rule[1] == page select rule[0]).ToArray();
                var pageRulesAfter = (from rule in rules where rule[0] == page select rule[1]).ToArray();
                var pagesBefore = update.Take(Array.IndexOf(update, page)).ToArray();
                var pagesAfter = update.Skip(Array.IndexOf(update, page)).ToArray();
                foreach (var pageBefore in pagesBefore)
                {
                    if (pageRulesAfter.Contains(pageBefore))
                    {
                        return false;
                    }
                }

                foreach (var pageAfter in pagesAfter)
                {
                    if (pageRulesBefore.Contains(pageAfter))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static int[] CorrectUpdate(int[] update, int[][] rules)
        {
            while (!IsUpdateCorrect(update, rules))
            {
                foreach (var page in update)
                {
                    var pageRulesBefore = (from rule in rules where rule[1] == page select rule[0]).ToArray();
                    var pageRulesAfter = (from rule in rules where rule[0] == page select rule[1]).ToArray();
                    var pagesBefore = update.Take(Array.IndexOf(update, page)).ToArray();
                    var pagesAfter = update.Skip(Array.IndexOf(update, page)).ToArray();
                    foreach (var pageBefore in pagesBefore)
                    {
                        if (pageRulesAfter.Contains(pageBefore))
                        {
                            update[Array.IndexOf(update, page)] = pageBefore;
                            update[Array.IndexOf(update, pageBefore)] = page;
                            break;
                        }
                    }

                    foreach (var pageAfter in pagesAfter)
                    {
                        update[Array.IndexOf(update, page)] = pageAfter;
                        update[Array.IndexOf(update, pageAfter)] = page;
                        break;
                    }
                }
            }
            
            return update;
        }

        private static int MiddleSumOfCorrectUpdates(int[][] rules, int[][] updates)
        {
            return updates.Where(update => IsUpdateCorrect(update, rules)).Sum(update => update[update.Length / 2]);
        }

        private static int MiddleSumOfCorrectedUpdates(int[][] rules, int[][] updates)
        {
            return updates.Where(update => !IsUpdateCorrect(update, rules)).Sum(update => CorrectUpdate(update, rules)[update.Length / 2]);
        }
        
    }
}