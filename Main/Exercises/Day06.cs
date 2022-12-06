using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises
{
    public class Day06 : Exercise
    {
        public override string Name => "Tuning Trouble";

        public override string Description => "Detect package start in elve signals";

        public override byte Day => 6;

        private int FindIndexForMarkerLength(int markerLength)
        {
            string sequence = System.IO.File.ReadAllLines("Inputs/06.txt")[0];
            var chars = new LinkedList<char>();
            var index = 0;

            while (chars.Count < markerLength)
                chars.AddLast(sequence[index++]);

            while (index < sequence.Length)
            {
                if (chars.Distinct().Count() == markerLength)
                    return index;

                chars.RemoveFirst();
                chars.AddLast(sequence[index++]);
            }

            return -1;
        }

        public override object SolvePartOne()
        {
            return FindIndexForMarkerLength(4);   
        }

        public override object SolvePartTwo()
        {
            return FindIndexForMarkerLength(14);
        }
    }
}
