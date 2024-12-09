using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day09
{
    internal class Day09
    {
        public static void Main(string[] args)
        {
            var diskMap = GetInput();

            Console.WriteLine(CalculateCompactedChecksum(diskMap));
            Console.WriteLine(CalculateCompactedChecksumUpdated(diskMap)); 
        }

        private static int[] GetInput()
        {
            var input = File.ReadAllText(@"C:\GitHub\advent-of-code\year_2024\Day09\input.txt");
            return (from c in input.ToCharArray() select int.Parse(c.ToString())).ToArray();
        }

        private static string[] ConvertDiskMapToBlockArray(int[] diskMap)
        {
            var id = 0;
            var freeSpace = false;
            var blocks = new List<string>();
            
            foreach (var file in diskMap)
            {
                if (freeSpace && Array.IndexOf(diskMap, file) < diskMap.Length - 1)
                {
                    for (int i = 0; i < file; i++)
                    {
                        blocks.Add(".");
                    }
                }
                else
                {
                    for (int i = 0; i < file; i++)
                    {
                        blocks.Add(id.ToString());
                    }
                    id++;
                }
                freeSpace = !freeSpace;
            }
            
            return blocks.ToArray();
        }
        
        private static void RearrangeBlocks(string[] blocks)
        {
            for (int i = blocks.Length - 1; i > 0; i--)
            {
                if (blocks[i] == "." || i < Array.IndexOf(blocks, ".")) continue;
                var firstFreeSpace = Array.IndexOf(blocks, ".");
                blocks[firstFreeSpace] = blocks[i];
                blocks[i] = ".";
            }
        }

        private static void RearrangeFiles(string[] blocks)
        {
            for (int i = blocks.Length - 1; i > 0; i--)
            {
                // Console.WriteLine(i);
                if (blocks[i] == "." || i < Array.IndexOf(blocks, ".")) continue;
                var id = blocks[i];
                var fileSpace = blocks.Count(block => block == id);
                var firstFreeSpace = Array.IndexOf(blocks, ".");
                
                while (true)
                {
                    if (firstFreeSpace > i - 1)
                    {
                        i -= fileSpace - 1;
                        break;
                    }
                    if (blocks.Skip(firstFreeSpace).Take(fileSpace).Count(block => block == ".") == fileSpace)
                    {
                        for (int j = 0; j < fileSpace; j++)
                        {
                            blocks[i - j] = ".";
                            blocks[firstFreeSpace + j] = id;
                        }
                        break;
                    }
                    firstFreeSpace = Array.IndexOf(blocks, ".", firstFreeSpace + 2);
                }

            }   
        }

        private static long CalculateChecksum(string[] blocks)
        {
            var checksum = 0L;
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i] != ".")
                {
                    checksum += i * int.Parse(blocks[i]);
                }
            }
            return checksum;
        }

        private static long CalculateCompactedChecksum(int[] diskMap)
        {
            var blocks = ConvertDiskMapToBlockArray(diskMap);
            RearrangeBlocks(blocks);

            return CalculateChecksum(blocks);
        }

        private static long CalculateCompactedChecksumUpdated(int[] diskMap)
        {
            var blocks = ConvertDiskMapToBlockArray(diskMap);
            RearrangeFiles(blocks);
            
            return CalculateChecksum(blocks);
        }
    }
}