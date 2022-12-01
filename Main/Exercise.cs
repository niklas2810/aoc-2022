using System;   

namespace Main
{
    internal abstract class Exercise
    {

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract byte Day { get; }

        public virtual void SetUp()
        {
            Console.WriteLine("(Nothing to do)");
        }

        public abstract Object SolvePartOne();

        public virtual String FormatPartOne(Object solution)
        {
            return solution.ToString();
        }

        public abstract Object SolvePartTwo();

        public virtual String FormatPartTwo(Object solution)
        {
            return solution.ToString();
        }

        public virtual void TearDown()
        {
            Console.WriteLine("(Nothing to do)");
        }

    }
}
