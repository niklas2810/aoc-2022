using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises._12
{
    public static class HashtableExtensions
    {

        private static int GetKey(int x, int y)
        {
            return x + (y * Day12.Width);
        }

        private static int GetKey(Position pos)
        {
            return GetKey(pos.X, pos.Y);
        }

        public static void AddPos(this Hashtable hash, Position pos)
        {
            hash.Add(GetKey(pos), pos);
        }

        public static void RemovePos(this Hashtable hash, Position pos)
        {
            hash.Remove(GetKey(pos));
        }

        public static bool ContainsPos(this Hashtable hash, Position pos)
        {
            return hash.ContainsKey(GetKey(pos));
        }

        public static Position FindPos(this Hashtable hash, int x, int y)
        {
            return (Position)hash[GetKey(x, y)];
        }

        public static Position LowCostPos(this Hashtable hash)
        {
            Position min = null;

            foreach(Position pos in hash.Values)
            {
                if (min == null || min.CostDistance > pos.CostDistance)
                    min = pos;
            }

            return min;
        }
    }
}
