using Main.Exercises._12;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Main.Exercises
{
    public class Day12 : Exercise
    {
        public override string Name => "Hill-Climbing Algorithm";

        public override string Description => "Find the shortest path to the cell tower!";

        public override byte Day => 12;


        private static List<string> lines;

        private int StartX => start.Item1;
        private int StartY => start.Item2;
        private int EndX => end.Item1;
        private int EndY => end.Item2;

        private (int, int) start;
        private (int, int) end;


        internal static int Width => lines[0].Length;
        internal static int Height => lines.Count;


        public override void SetUp()
        {
            lines = System.IO.File.ReadAllLines("Inputs/12.txt").ToList();

            var startY = lines.FindIndex(str => str.Contains('S'));
            var startX = lines[startY].IndexOf('S');
            start = (startX, startY);

            var endY = lines.FindIndex(str => str.Contains('E'));
            var endX = lines[endY].IndexOf('E');
            end = (endX, endY);
        }

        private Position CreatePosition(Position current, int dx, int dy)
        {
            int x = current.X + dx;
            int y = current.Y + dy;

            if (x < 0 || y < 0 || x > Width - 1 || y > Height - 1)
                return null;

            var c = lines[y][x];

            var pos = new Position
            {
                X = x,
                Y = y,
                Elevation = c == 'S' ? 0 : (c == 'E' ? 'z'-'a' : c - 'a')
            };

            pos.SetPrevious(current);
            pos.SetTarget(EndX, EndY);
            return pos;
        }

        private List<Position> GetNeighbors(Position current)
        {
            var x = current.X;
            var y = current.Y;

            List<Position> neighbors = new()
            {
                CreatePosition(current, -1, 0),
                CreatePosition(current, 1, 0),
                CreatePosition(current, 0, -1),
                CreatePosition(current, 0, 1)
            };

            return neighbors.Where(neigh => neigh != null && neigh.Elevation - current.Elevation <= 1).ToList();
        }

        private int AStar((int, int) startPos, (int, int) endPos)
        {
            var start = new Position
            {
                X = startPos.Item1,
                Y = startPos.Item2,
                Elevation = 0
            };
            start.SetTarget(EndX, EndY);

            var end = new Position
            {
                X = endPos.Item1,
                Y = endPos.Item2,
                Elevation = 'z' - 'a'
            };

            var active = new Hashtable();
            active.AddPos(start);
            var visited = new Hashtable();

            while (active.Count > 0)
            {
                var current = active.LowCostPos();
                if (current.X == EndX && current.Y == EndY)
                {
                    return current.Cost;
                }

                visited.AddPos(current);
                active.RemovePos(current);

                var neighbors = GetNeighbors(current);
                foreach (var neighbor in neighbors)
                {
                    if (visited.ContainsPos(neighbor))
                        continue;

                    Position existing = active.FindPos(neighbor.X, neighbor.Y);
                    if (existing != null)
                    {
                        if (existing.CostDistance > neighbor.CostDistance)
                        {
                            active.RemovePos(existing);
                            active.AddPos(neighbor);
                        }
                    }
                    else
                    {
                        active.AddPos(neighbor);
                    }
                }
            }

            return -1;
        }

        public override object SolvePartOne()
        {
            return AStar(start, end);
        }

        public override object SolvePartTwo()
        {

            int min = 0;
            int tries = 0;

            for(int y = 0; y < lines.Count; ++y)
            {
                var line = lines[y];
                for(int x = 0; x < line.Length; ++x)
                {
                    if (line[x] != 'a')
                        continue;

                    var curr = AStar((x, y), end);

                    ++tries;

                    if (curr == -1)
                        continue;

                    if (min == 0 || curr < min)
                        min = curr;
                }
            }

            return min;
        }
    }
}
