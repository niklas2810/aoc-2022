using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises._12
{
    public class Position
    {

        public int X { get; set; }
        public int Y { get; set; }
        public int Elevation { get; set; }

        // Minimum amount of steps to reach this tile.
        public int Cost { get; private set; }

        // Estimated distance to target (manhattan distance)
        public int Distance { get; private set; }

        public int CostDistance => Cost + Distance;

        public Position Previous { get; private set; } = null;

        public void SetTarget(int targetX, int targetY)
        {
            Distance = DistanceTo(targetX, targetY);
        }

        public void SetPrevious(Position prev)
        {
            Cost = prev.Cost + 1;
            Previous = prev;
        }

        internal int DistanceTo(int x, int y)
        {
            return Math.Abs(X - x) + Math.Abs(Y - y);
        }

    }
}
