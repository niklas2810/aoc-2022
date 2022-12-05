using System;   

namespace Main
{
    public abstract class Exercise
    {

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract byte Day { get; }

        public virtual void SetUp()
        {
            Console.WriteLine("(Nothing to do)");
        }

        public abstract object SolvePartOne();

        public abstract object SolvePartTwo();

        public virtual void TearDown()
        {
            Console.WriteLine("(Nothing to do)");
        }

    }
}
