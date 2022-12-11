using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.Exercises
{
    public class Day11 : Exercise
    {
        private const int PRIMES_SMALLER_30 = 2 * 3 * 5 * 7 * 11 * 13 * 17 * 19 * 29;

        private class Monkey
        {
            public int Name { get; set; }
            public List<long> Items { get; set; } = new List<long>();
            public Func<long, long> Operation { get; set; } = (a) => a;
            public Func<long, bool> Test { get; set; } = (a) => true;
            public int TrueTarget { get; set; }
            public int FalseTarget { get; set; }
            public long Inspections { get; set; } = 0;
        }

        public override string Name => "Monkey in the middle";
        public override string Description => "Monkeys throw stuff around, track your worries!";
        public override byte Day => 11;
        private readonly List<Monkey> monkeys = new();

        private void ReadInput()
        {
            monkeys.Clear();
            Monkey current = null;

            foreach(var line in System.IO.File.ReadAllLines("Inputs/11.txt"))
            {
                if(line.StartsWith("Monkey"))
                {
                    if (current != null)
                        monkeys.Add(current);
                    current = new();

                    current.Name = int.Parse(line.Substring(6).Replace(':', ' ').Trim());
                }
                else if(line.Contains("Starting items: "))
                {
                    var items = line.Substring(18).Split(',').Select(x => long.Parse(x));
                    current.Items.AddRange(items);
                }
                else if(line.Contains("Operation: "))
                    current.Operation = ParseOperation(line.Substring(19));
                else if(line.Contains("Test: "))
                {
                    var num = int.Parse(line.Substring(21));
                    current.Test = (a) => (a % num) == 0;
                } 
                else if(line.Contains("If true: "))
                    current.TrueTarget = int.Parse(line.Substring(29));
                else if (line.Contains("If false: "))
                    current.FalseTarget = int.Parse(line.Substring(30));
            }
            monkeys.Add(current);
        }

        private static Func<long, long> ParseOperation(string opRaw)
        {
            if (opRaw == "old * old")
                return (a) => a * a;

            if(opRaw.StartsWith("old + "))
            {
                var num = opRaw.Substring(6);
                return (a) => a + long.Parse(num);
            }

            if(opRaw.StartsWith("old * "))
            {
                var num = opRaw.Substring(6);
                return (a) => a * long.Parse(num);
            }

            throw new NotSupportedException(opRaw);
        }

        private long PlayGame(int rounds, bool divide)
        {
            ReadInput();
            var round = 1;

            while(round <= rounds)
            {
                foreach(var monkey in monkeys)
                {
                    for(int i = 0; i < monkey.Items.Count; ++i)
                    {
                        var num = monkey.Items[i];
                        num = monkey.Operation(num);
                        if (divide)
                            num /= 3;
                        else
                        {
                            // We use this as a little hack. Since our numbers can get *very* large (even too large for long),
                            // we need to trim them at some point. To not mess up our test, we use the
                            // prime factors to trim our number down a bit.
                            num %= PRIMES_SMALLER_30;
                        }


                        var target = monkey.Test(num) ? monkey.TrueTarget : monkey.FalseTarget;
                        monkeys[target].Items.Add(num);
                    }

                    monkey.Inspections += monkey.Items.Count;
                    monkey.Items.Clear(); // A monkey always throws away *all* items.
                }
                round++;
            }
            return monkeys.Select(m => m.Inspections).OrderByDescending(a => a).Take(2).Aggregate(1L, (a,b) => a*b);
        }

        public override object SolvePartOne()
        {
            return PlayGame(20, true);
        }

        public override object SolvePartTwo()
        {
            return PlayGame(10_000, false);
        }
    }
}
