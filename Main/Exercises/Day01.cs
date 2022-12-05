using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.Days
{
    public class Day01 : Exercise
    {
        public override string Name => "Calorie Counting";

        public override string Description => "Take note of the calories each elf carries.";

        public override byte Day => 1;

        private List<int> calories = new List<int>();

        public override void SetUp()
        {
            string[] lines = System.IO.File.ReadAllLines("Inputs/01.txt");
            CountCalories(lines);
        }

        private void CountCalories(string[] lines)
        {
            calories.Clear();
            int currValue = 0;

            foreach (var line in lines)
            {
                if (line.Trim().Length == 0)
                {
                    calories.Add(currValue);
                    currValue = 0;
                    continue;
                }

                var value = int.Parse(line);
                currValue += value;
            }

            if (currValue > 0)
                calories.Add(currValue);
        }

        public override object SolvePartOne()
        {
            (var elf, var maxValue) = calories.Select((value, i) => (i + 1, value)).OrderByDescending(v => v.value).First();
            return maxValue;
        }

        public override object SolvePartTwo()
        {
            var topThree = calories.OrderByDescending(v => v).Take(3).Sum();
            return topThree;
        }
    }
}
