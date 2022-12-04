using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises
{
    public class Day04 : Exercise
    {
        public override string Name => "Camp Cleanup";

        public override string Description => "Help the elves to find redundant cleanup tasks";

        public override byte Day => 4;

        private readonly List<int[][]> tasks = new();

        public override void SetUp()
        {
            foreach (var line in System.IO.File.ReadAllLines("Inputs/04.txt"))
            {
                tasks.Add(line.Split(",").Select(part => part.Split("-").Select(x => int.Parse(x)).ToArray()).ToArray());
            }
        }

        public override long SolvePartOne()
        {
            long result = 0;

            foreach(var task in tasks)
            {
                if (task.Length != 2)
                    throw new NotSupportedException();

                var firstStart = task[0][0];
                var firstEnd = task[0][1];
                var secondStart = task[1][0];
                var secondEnd = task[1][1];

                var firstContainsSecond = firstStart <= secondStart && firstEnd >= secondEnd;
                var secondContainsFirst = secondStart <= firstStart && secondEnd >= firstEnd;

                if (firstContainsSecond || secondContainsFirst)
                    ++result;
            }

            return result;
        }

        public override long SolvePartTwo()
        {
            long result = 0;

            foreach (var task in tasks)
            {
                if (task.Length != 2)
                    throw new NotSupportedException();


                var firstStart = task[0][0];
                var firstEnd = task[0][1];
                var secondStart = task[1][0];
                var secondEnd = task[1][1];

                if (firstEnd >= secondStart && firstStart <= secondEnd)
                    ++result;
            }

            return result;
        }
    }
}
