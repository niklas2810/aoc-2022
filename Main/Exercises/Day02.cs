using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises
{
    public class Day02 : Exercise
    {
        public override string Name => "Rock Paper Scissors";

        public override string Description => "Play RPS based on instructions by an elf.";

        public override byte Day => 2;

        private List<int> others = new();
        private List<int> mine = new();
             
        public override void SetUp()
        {
            string[] lines = System.IO.File.ReadAllLines("Inputs/02.txt");

            others.Clear();
            mine.Clear();

            foreach(var line in lines)
            {
                if (line.Length != 3)
                    throw new NotSupportedException();

                string[] split = line.Split(' ');
                others.Add(ParseLetter(split[0]));
                mine.Add(ParseLetter(split[1]));
            }

            if (others.Count != mine.Count)
                throw new NotSupportedException();
        }

        private static int ParseLetter(string v)
        {
            switch(v[0])
            {
                case 'A':
                case 'X':
                    return 1; // Rock
                case 'B':
                case 'Y':
                    return 2; // Paper
                case 'C':
                case 'Z':
                    return 3; // Scissors
                default:
                    throw new NotSupportedException();
            }
        }

        public override object SolvePartOne()
        {

            int winScore = 0;
            int typeScore = 0;

           for(int i = 0; i < mine.Count; ++i)
            {
                var other = others[i];
                var mine = this.mine[i];

                var gameResult = decideResult(other, mine);

                typeScore += mine;
                winScore += gameResult;
            }

            return winScore + typeScore;
        }

        private static int decideResult(int other, int mine)
        {
            switch(mine)
            {
                case 1: // I play Rock
                    return other == 3 ? 6 : (other == 2 ? 0 : 3);
                case 2: // I play Paper
                    return other == 1 ? 6 : (other == 3 ? 0 : 3);
                case 3: // I play Scissors
                    return other == 2 ? 6 : (other == 1 ? 0 : 3);
                default:
                    throw new NotSupportedException();
            }
        }

        public override object SolvePartTwo()
        {

            int winScore = 0;
            int typeScore = 0;

            for (int i = 0; i < mine.Count; ++i)
            {
                var other = others[i];
                var mine = this.mine[i];


                var decision = getDecisionFor(other, mine);
                var points = mine == 3 ? 6 : (mine == 2 ? 3 : 0);

                //Console.WriteLine($"[{i}] {other} {mine} => {decision} ({points})");

                typeScore += decision;
                winScore += points;
            }

            return winScore + typeScore;
        }

        private static int getDecisionFor(int other, int mine)
        {
            switch(mine)
            {
                case 1: // Lose
                    return other == 3 ? 2 : (other == 2 ? 1 : 3);
                case 2: // Draw
                    return other;
                case 3: // Win
                    return other == 3 ? 1 : (other == 2 ? 3 : 2);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
