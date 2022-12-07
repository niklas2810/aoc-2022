using Main.Exercises._07;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises
{
    public class Day07 : Exercise
    {
        public override string Name => "No space left on device";

        public override string Description => "Build a file tree based on console output";

        public override byte Day => 7;

        private Directory root;

        public override void SetUp()
        {
            root = new TreeParser(System.IO.File.ReadAllLines("Inputs/07.txt")).parse();
        }


        private List<Directory> CollectDirs(Func<Directory, bool> matcher)
        {
            List<Directory> dirs = new List<Directory>();
            CollectDirsInternal(ref dirs, matcher, root);
            return dirs;
        }

        private void CollectDirsInternal(ref List<Directory> dirs, Func<Directory, bool> matcher, Directory current)
        {
            if (matcher(current))
                dirs.Add(current);

            foreach (var dir in current.Dirs)
                CollectDirsInternal(ref dirs, matcher, dir);
        }

        public override object SolvePartOne()
        {
            var directories = CollectDirs(dir => dir.Size < 100_000);
            return directories.Select(x => x.Size).Sum();
        }

        public override object SolvePartTwo()
        {
            var totalSize = 70000000;
            var requiredFree = 30000000;
            var currentFree = totalSize-root.Size;

            var requiredToDelete = Math.Max(0, requiredFree - currentFree);

            var directories = CollectDirs(dir => dir.Size > requiredToDelete);
            return directories.OrderBy(dir => dir.Size).First().Size;
        }
    }
}
