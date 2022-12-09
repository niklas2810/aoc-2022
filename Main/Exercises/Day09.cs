using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises
{
    public class Day09 : Exercise
    {
        private enum Direction
        {
            UP, DOWN, LEFT, RIGHT
        }

        public override string Name => "Rope Bridge";

        public override string Description => "Track where the head and tail of a rope move";

        public override byte Day => 9;

        private readonly List<Direction> instructions = new();

        public override void SetUp()
        {
            foreach(var line in System.IO.File.ReadAllLines("Inputs/09.txt"))
            {
                var direction = Direction.UP;
                
                switch (line[0])
                {
                    case 'D':
                        direction = Direction.DOWN;
                        break;
                    case 'L':
                        direction = Direction.LEFT;
                        break;
                    case 'R':
                        direction = Direction.RIGHT;
                        break;
                    case 'U':
                        direction = Direction.UP;
                        break;
                    default:
                        throw new NotSupportedException();
                }

                foreach(var i in Enumerable.Range(0, int.Parse(line.Substring(2))))
                    instructions.Add(direction);                   
            }

            Console.WriteLine($"Read {instructions.Count} instructions");
        }

        private bool AreNearby(Point head, Point tail)
        {
            var manhattanDistance = Math.Abs(head.Y - tail.Y) + Math.Abs(head.X - tail.X);

            if (head.X == tail.X || head.Y == tail.Y) // In same row
                return manhattanDistance <= 1;

            return manhattanDistance == 2;
        }

        private bool Follow(Point head, ref Point tail)
        {
            if (AreNearby(head, tail))
                return false;

            var horizontal = head.X - tail.X;
            var vertical = head.Y - tail.Y;

            if (horizontal != 0)
                tail.X += horizontal > 0 ? 1 : -1;

            if (vertical != 0)
                tail.Y += vertical > 0 ? 1 : -1;

            return horizontal != 0 || vertical != 0;
        }

        private void Move(ref Point head, Direction dir)
        {
            switch (dir)
            {
                case Direction.UP:
                    head.Y += 1;
                    break;
                case Direction.DOWN:
                    head.Y -= 1;
                    break;
                case Direction.LEFT:
                    head.X -= 1;
                    break;
                case Direction.RIGHT:
                    head.X += 1;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public override object SolvePartOne()
        {
            List<Point> visited = new();

            var head = new Point(0, 0);
            var tail = new Point(0, 0);

            foreach (var dir in instructions)
            {
                Move(ref head, dir);
                Follow(head, ref tail);
                if (!visited.Contains(tail))
                    visited.Add(tail);
            }

            return visited.Count;
        }

        public override object SolvePartTwo()
        {
            List<Point> visited = new();
            var rope = new Point[10];

            foreach (var i in Enumerable.Range(0, rope.Length))
                rope[i] = new Point(0, 0);

            var instructionIndex = 0;
            var moved = false;


            while(instructionIndex < instructions.Count || moved)
            {
                moved = false;

                if(instructionIndex < instructions.Count)
                {
                    var head = rope[0];
                    Move(ref head, instructions[instructionIndex++]);
                    if (head != rope[0])
                        rope[0] = head;
                    moved = true;
                }

                foreach(var i in Enumerable.Range(1, rope.Length-1))
                {
                    var p = rope[i];
                    if (Follow(rope[i - 1], ref p))
                    {
                        rope[i] = p;
                        moved = true;
                    }
                }

                if (!visited.Contains(rope[rope.Length-1]))
                   visited.Add(rope[rope.Length - 1]);
            }

            return visited.Count;
        }
    }
}
