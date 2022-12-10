using Main.Exercises._10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises
{
    public class Day10 : Exercise
    {
        public override string Name => "Cathode-Ray Tube";
        public override string Description => "Interpret CPU instructions for a CRT screen";
        public override byte Day => 10;

        private CrtScreen screen;
        private readonly List<int> valuesPartOne = new();

        public override void SetUp()
        {
            var cpu = new VideoCpu(System.IO.File.ReadAllLines("Inputs/10.txt"));

            //PART ONE
            cpu.OnDuringCycle += (sender, args) =>
            {
                if ((20 + cpu.Cycle) % 40 == 0)
                {
                    valuesPartOne.Add(cpu.Cycle * cpu.RegX);
                }
            };

            //PART TWO
            screen = new CrtScreen(cpu)
            {
                ExtensiveLogging = false,
            };

            cpu.ExecuteAll();
        }

        public override object SolvePartOne()
        {
            return valuesPartOne.Sum();
        }

        public override object SolvePartTwo()
        {
            return Environment.NewLine + screen;
        }
    }
}
