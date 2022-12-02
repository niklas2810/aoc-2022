using System;
using System.Linq;
using System.Reflection;

namespace Main
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022");

            var exercises = typeof(Program).GetTypeInfo().Assembly.GetTypes()
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(Exercise)))
                .Select(t => (Exercise)t.GetConstructor(new Type[0]).Invoke(null))
                .OrderBy(e => e.Day)
                .ToList();

            Console.WriteLine($"{exercises.Count} Exercises in cache");

            Utils.WriteFatLine("=========================================");

            foreach (var f in exercises)
            {
                Console.WriteLine($"[{f.Day}] {f.Name}");
            }

            Utils.WriteFatLine("=========================================");

            int hardcodedChoice = 0;
            if(args.Length > 0)
            {
                if (args[0].Equals("all"))
                {
                    Console.WriteLine("Executing ALL exercises!");
                    hardcodedChoice = -1;
                }

                if (args[0].Equals("last"))
                {
                    hardcodedChoice = exercises.OrderByDescending(ex => ex.Day).First().Day;
                    Console.WriteLine($"Executing exercise {hardcodedChoice}");
                }


                int value;
                if (int.TryParse(args[0], out value) && value > 0)
                {
                    Console.WriteLine($"Executing exercise {value} (if it exists)");
                    hardcodedChoice = value;
                }
            }

            int choice = hardcodedChoice != 0 ? hardcodedChoice : RequestDayNumber();

            foreach (Exercise f in exercises)
            {
                if (choice > 0 && f.Day != choice)
                    continue;

                Utils.WriteFatLine($"====== Day {f.Day}: {f.Name} =======");
                Console.WriteLine($"Description: {f.Description}");

                Utils.WriteFatLine("[SetUp]");
                f.SetUp();

                Utils.WriteFatLine("[SolvePartOne]");
                var p1 = f.SolvePartOne();
                Console.WriteLine("=======> P1: " + f.FormatPartOne(p1));

                Utils.WriteFatLine("[SolvePartTwo]");
                var p2 = f.SolvePartTwo();
                Console.WriteLine("=======> P2: " + f.FormatPartTwo(p2));

                Utils.WriteFatLine("[TearDown]");
                f.TearDown();

                Console.WriteLine("=========================================");
            }
            
        }

        private static int RequestDayNumber()
        {
            Utils.WriteFatLine("Please type the number in square brackets to execute the day's implementation!");
            return int.Parse(Console.ReadLine());
        }
    }
}
