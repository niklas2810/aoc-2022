using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Main.Exercises
{
    public class Day14 : Exercise
    {
        public override string Name => "Regolith Reservoir";

        public override string Description => "Track where sand is falling in!";

        public override byte Day => 14;

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

                    AddBlockedToCache(from, to);
                }
            }
        }

        private void AddBlockedToCache(int[] from, int[] to)
        {
            int dx = 0;
            int dy = 0;
            if (from[0] == to[0]) // Vertical
                dy = from[1] < to[1] ? 1 : -1;
            else if (from[1] == to[1]) // Horizontal
                dx = from[0] < to[0] ? 1 : -1;

            while (from[0] != to[0] || from[1] != to[1])
            {
                if (!cache.ContainsKey((from[0], from[1])))
                    cache.Add((from[0], from[1]), true);
                from[0] += dx;
                from[1] += dy;
            }
            if (!cache.ContainsKey((to[0], to[1])))
                cache.Add((to[0], to[1]), true);
        }

        private bool IsBlocked((int, int) pos, ref HashSet<(int, int)> sand)
        {
            if (sand.Contains(pos))
                return true;

            if (cache.ContainsKey(pos))
                return true;

            return false;
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
            var maxY = cache.Keys.Cast<(int, int)>().Select(it => it.Item2).Max();

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
            var maxY = cache.Keys.Cast<(int, int)>().Select(it => it.Item2).Max();
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
