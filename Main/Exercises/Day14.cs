using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises
{
    public class Day14 : Exercise
    {

        private class RockFormation
        {
            public (int, int) From { get; set; }
            public (int, int) To { get; set; }

            public int FX => From.Item1;
            public int FY => From.Item2;

            public int TX => To.Item1;
            public int TY => To.Item2;

            public bool Blocks(int x, int y)
            {
                if (FX == TX)
                    return BlocksVertical(x, y);
                else if (FY == TY)
                    return BlocksHorizontal(x, y);

                throw new NotSupportedException();
            }

            private bool BlocksVertical(int x, int y)
            {
                if (x != FX)
                    return false;
                if (y < Math.Min(FY, TY))
                    return false;
                if (y > Math.Max(FY, TY))
                    return false;

                return true;
            }

            private bool BlocksHorizontal(int x, int y)
            {
                if (y != FY)
                    return false;
                if (x < Math.Min(FX, TX))
                    return false;
                if (x > Math.Max(FX, TX))
                    return false;

                return true;
            }
        }

        public override string Name => "Regolith Reservoir";

        public override string Description => "Track where sand is falling in!";

        public override byte Day => 14;

        private List<RockFormation> formations = new();
        private Hashtable cache = new();

        public override void SetUp()
        {
            foreach(var line in System.IO.File.ReadAllLines("Inputs/14.txt"))
            {
                var parts = line.Split(" -> ").Select(x => x.Split(',').Select(s => int.Parse(s)).ToArray()).ToArray();
                for(int i = 1; i < parts.Length; i++)
                {
                    var from = parts[i - 1];
                    var to = parts[i];
                    RockFormation rock = new RockFormation
                    {
                        From = (from[0], from[1]),
                        To = (to[0], to[1])
                    };

                    formations.Add(rock);
                }
            }
        }

        private bool IsBlocked((int, int) pos, ref HashSet<(int, int)> sand)
        {
            if (sand.Contains(pos))
                return true;

            if (!cache.ContainsKey(pos))
                cache.Add(pos, formations.Where(f => f.Blocks(pos.Item1, pos.Item2)).Any());

            return (bool)cache[pos];
        }

        private (int, int) SimulateFall((int, int) start, int yDelimeter, ref HashSet<(int, int)> sandPoints, bool partTwo = false)
        {
            int x = start.Item1;
            int y = start.Item2;

            while(partTwo || y < yDelimeter)
            {


                if (partTwo && y >= yDelimeter + 1)
                    return (x, y);
                if (!IsBlocked((x, y + 1), ref sandPoints)) // down one step
                    ++y;
                else if(!IsBlocked((x-1, y + 1), ref sandPoints)) //one step down and to the left
                {
                    --x;
                    ++y;
                }
                else if (!IsBlocked((x + 1, y + 1), ref sandPoints)) //one step down and to the right
                {
                    ++x;
                    ++y;
                }
                else
                {
                    return (x, y);
                }
            }


            return (-1, -1);
        }

        public override object SolvePartOne()
        {
            var maxY = formations.Select(x => Math.Max(x.FY, x.TY)).Max();

            HashSet<(int, int)> sandPoints = new();

            while(true)
            {
                var newSandpoint = SimulateFall((500, 0), maxY, ref sandPoints);
                if (newSandpoint.Item1 < 0)
                    break;
                sandPoints.Add(newSandpoint);
            }

            return sandPoints.Count;
        }

        public override object SolvePartTwo()
        {
            var maxY = formations.Select(x => Math.Max(x.FY, x.TY)).Max();
            HashSet<(int, int)> sandPoints = new();

            while (true)
            {
                var newSandpoint = SimulateFall((500, 0), maxY, ref sandPoints, true);
                if (newSandpoint == (500, 0))
                    break;
                sandPoints.Add(newSandpoint);
            }

            return sandPoints.Count+1;
        }
    }
}
