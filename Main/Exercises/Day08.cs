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

        public override void SetUp()
        {
            var lines = System.IO.File.ReadAllLines("Inputs/08.txt");

            grid = new int[lines.Length][];

            for(int i=0; i < lines.Length; ++i)
            {
                var line = lines[i];

                grid[i] = new int[line.Length];
                if (grid[i].Length != grid[0].Length)
                    throw new NotSupportedException();

                for(int j=0; j < line.Length; ++j)
                {
                    grid[i][j] = line[j] - '0';
                }
            }

            Console.WriteLine($"Grid: [{grid.Length}x{grid[0].Length}]");
        }

        private int GetHorizontalBlockedAfter(int row, int col, int direction)
        {
            var height = grid[row][col];
            var curr = col+ direction;

            while(curr >= 0 && curr < grid[0].Length)
            {
                if(grid[row][curr] >= height)
                    return Math.Abs(curr-col);

                curr += direction;
            }

            return -1;
        }

        private int GetVerticalBlockedAfter(int row, int col, int direction)
        {
            var height = grid[row][col];
            var curr = row + direction;

            while (curr >= 0 && curr < grid.Length)
            {
                if (grid[curr][col] >= height)
                    return Math.Abs(curr-row);

                curr += direction;
            }

            return -1;
        }

        private bool IsVisibleFrom(int row, int col, int hori, int vert)
        {
            if (hori != 0 && vert != 0)
                throw new NotSupportedException();

            if (hori != 0)
                return GetHorizontalBlockedAfter(row, col, hori) == -1;
            else
                return GetVerticalBlockedAfter(row, col, vert) == -1;
        }

        private bool IsVisible(int row, int col)
        {
            if (row == 0 || row == grid.Length - 1 || col == 0 || col == grid[0].Length - 1)
                return true;

            return IsVisibleFrom(row, col, -1, 0) || IsVisibleFrom(row, col, 1, 0) || IsVisibleFrom(row, col, 0, -1) || IsVisibleFrom(row, col, 0, 1);
        }


        private int GetScenicScore(int row, int col)
        {
            var nordBlocked = GetVerticalBlockedAfter(row, col, -1);
            var nordScore = nordBlocked == -1 ? row : nordBlocked;

            var southBlocked = GetVerticalBlockedAfter(row, col, 1);
            var southScore = southBlocked == -1 ? grid.Length-row - 1 : southBlocked;

            var westBlocked = GetHorizontalBlockedAfter(row, col, -1);
            var westScore = westBlocked == -1 ? col : westBlocked;

            var eastBlocked = GetHorizontalBlockedAfter(row, col, 1);
            var eastScore = eastBlocked == -1 ? grid[0].Length - col - 1 : eastBlocked;

            return nordScore * southScore * westScore  * eastScore ;
        }


        public override object SolvePartOne()
        {
            var rows = grid.Length;
            var cols = grid[0].Length;
            var visible = 0;

            for(int row = 0; row < rows; ++row)
            {
                for(int col = 0; col < cols; ++col)
                {
                    if (IsVisible(row, col))
                        ++visible;
                }
            }

            return visible;
        }

        public override object SolvePartTwo()
        {
            var rows = grid.Length;
            var cols = grid[0].Length;
            var maxScore = 0;

            for (int row = 0; row < rows; ++row)
            {
                for (int col = 0; col < cols; ++col)
                {
                    var score = GetScenicScore(row, col);

                    if(score > maxScore)
                        maxScore = score;
                }
            }

            return maxScore;
        }
    }
}
