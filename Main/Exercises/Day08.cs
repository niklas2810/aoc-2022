using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises
{
    public class Day08 : Exercise
    {
        public override string Name => "Treetop Tree House";

        public override string Description => "Compare the height of trees to check which ones are visible from the outside";

        public override byte Day => 8;

        private int[][] grid;
        private int Rows { get => grid.Length; }
        private int Cols { get => grid[0].Length; }

        public override void SetUp()
        {
            var lines = System.IO.File.ReadAllLines("Inputs/08.txt");
            grid = new int[lines.Length][];

            foreach (var i in Enumerable.Range(0, lines.Length))
            {
                var line = lines[i];
                grid[i] = new int[line.Length];
                foreach (var j in Enumerable.Range(0, line.Length))
                {
                    grid[i][j] = line[j] - '0';
                }
            }
        }

        ///<returns>First element: Reached border? => true, Hit tree? => false, Second element: Amount of trees visible</returns>
        private (bool, int) GetBlockedAfter(int row, int col, int rowChg, int colChg)
        {
            var i = 1;
            var currRow = row + i * rowChg;
            var currCol = col + i * colChg;

            while (currCol >= 0 && currRow >= 0 && currCol < Cols && currRow < Rows)
            {
                if (grid[row][col] <= grid[currRow][currCol])
                    return (false, i);

                ++i;
                currRow = row + i * rowChg;
                currCol = col + i * colChg;
            }

            return (true, i - 1);
        }

        private bool IsVisible(int row, int col)
        {
            if (row == 0 || row == Rows - 1 || col == 0 || col == Cols - 1)
                return true;

            return GetBlockedAfter(row, col, -1, 0).Item1 || GetBlockedAfter(row, col, 1, 0).Item1 || GetBlockedAfter(row, col, 0, -1).Item1 || GetBlockedAfter(row, col, 0, 1).Item1;
        }


        private int GetScenicScore(int row, int col)
        {
            if (row == 0 || row == Rows - 1 || col == 0 || col == Cols - 1)
                return 0;

            return GetBlockedAfter(row, col, -1, 0).Item2 * GetBlockedAfter(row, col, 1, 0).Item2 * GetBlockedAfter(row, col, 0, -1).Item2 * GetBlockedAfter(row, col, 0, 1).Item2;
        }

        public override object SolvePartOne()
        {
            return Enumerable.Range(0, Rows).SelectMany(row =>
            Enumerable.Range(0, Cols).Select(col =>
            IsVisible(row, col))).Where(b => b).Count();
        }

        public override object SolvePartTwo()
        {
            return Enumerable.Range(0, Rows).SelectMany(row =>
            Enumerable.Range(0, Cols).Select(col =>
            GetScenicScore(row, col))).Max();
        }
    }
}
