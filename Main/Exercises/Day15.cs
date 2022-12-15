using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Main.Exercises
{
    public class Day15 : Exercise
    {

        private class ExclusionZone
        {
            public (int, int) Sensor { get; set; }
            public (int, int) Beacon { get; set; }

            public long BeaconFrequency => BeaconX * 4_000_000L + BeaconY;


            public int SensorX => Sensor.Item1;
            public int SensorY => Sensor.Item2;
            public int BeaconX => Beacon.Item1;
            public int BeaconY => Beacon.Item2;
            public int Distance => GetManhattanDistance(Sensor, Beacon);

            public int MinX => SensorX - Distance;
            public int MaxX => SensorX + Distance;
            public int MinY => SensorY - Distance;
            public int MaxY => SensorY + Distance;

            internal bool IsInRange((int, int) pos)
            {
                return GetManhattanDistance(Sensor, pos) <= Distance;
            }

            private static int GetManhattanDistance((int, int) a, (int, int) b)
            {
                return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
            }

            internal IEnumerable<(int, int)> PositionsInRange()
            {
                var dist = Distance;

                for (int dy = -dist; dy <= dist; ++dy)
                {
                    var currY = SensorY + dy;
                    if (currY < 0 || currY > 4_000_000)
                        continue;
                    for (int dx = -dist; dx <= dist; ++dx)
                    {
                        var currX = SensorX + dx;
                        if (currX < 0 || currX > 4_000_000)
                            continue;

                        if (GetManhattanDistance((currX, currY), Sensor) > Distance)
                            continue;

                        yield return (currX, currY);
                    }
                }
            }
        }

        public override string Name => "Beacon Exclusion Zone";

        public override string Description => "Find out where the distress signal comes from";

        public override byte Day => 15;


        private List<ExclusionZone> exclusionZones = new();

        public override void SetUp()
        {
            var regex = new Regex(@"-?(\d+)");

            foreach (var line in System.IO.File.ReadAllLines("Inputs/15.txt"))
            {
                var parts = regex.Matches(line);
                CalculateExclusionZone(int.Parse(parts[0].Value), int.Parse(parts[1].Value), int.Parse(parts[2].Value), int.Parse(parts[3].Value));
            }
        }


        private bool CannotContainBeacon((int, int) pos)
        {
            foreach (var zone in exclusionZones)
                if (pos == zone.Sensor || pos == zone.Beacon)
                    return false;

            foreach (var zone in exclusionZones)
                if (zone.IsInRange(pos))
                    return true;

            return false;
        }

        private (int, int) FindDoubleBeacon()
        {
            // Cache beacons for performance
            HashSet<(int, int)> bPos = new();
            foreach (var zone in exclusionZones)
            {
                bPos.Add(zone.Beacon);
            }


            var results = new ConcurrentBag<(int, int)>();

            Parallel.ForEach(exclusionZones, zone =>
            {
                var topY = zone.SensorY + zone.Distance - 1;
                var bottomY = zone.SensorY - zone.Distance - 1;

                //Console.WriteLine($"==> ({zone.SensorX}, {zone.SensorY}): top is {topY}, bottom is {bottomY}");

                foreach(var offset in Enumerable.Range(0, zone.Distance))
                {
                    var positions = new (int, int)[] { (zone.SensorX + offset, topY - offset), (zone.SensorX - offset, topY - offset), (zone.SensorX + offset, bottomY + offset), (zone.SensorX - offset, bottomY + offset) };
                    foreach ((var x, var y) in positions)
                    {
                        //Console.WriteLine($"({x}, {y}) for sensor ({zone.SensorX}, {zone.SensorY}), {i}/{zone.Distance}");


                        if (x < 0 || y < 0 || x > 4_000_000 || y > 4_000_000)
                            continue;
                        else if (bPos.Contains((x, y)))
                            continue;

                        var found = false;
                        foreach (var zone2 in exclusionZones)
                        {
                            if (!found && zone2.IsInRange((x, y)))
                                found = true;
                        }

                        if (!found)
                        {
                            if(!results.Contains((x, y)))
                                results.Add((x, y));
                            return;
                        }
                    }
                }
            });

            if (results.Count != 1)
                throw new NotSupportedException();
            return results.ElementAt(0);
        }

        private void CalculateExclusionZone(int sX, int sY, int bX, int bY, bool log = false)
        {
            var zone = new ExclusionZone
            {
                Sensor = (sX, sY),
                Beacon = (bX, bY)
            };

            exclusionZones.Add(zone);
        }

        public override object SolvePartOne()
        {

            int minX = exclusionZones.Select(zone => zone.MinX).Min();
            int maxX = exclusionZones.Select(zone => zone.MaxX).Max();
            int minY = exclusionZones.Select(zone => zone.MinY).Min();
            int maxY = exclusionZones.Select(zone => zone.MaxY).Max();

            Console.WriteLine($"X: {minX} -> {maxX}, Y: {minY} -> {maxY}");
            var relY = 2000000;

            var count = Enumerable.Range(minX, maxX - minX).AsParallel().Select(x => CannotContainBeacon((x, relY))).Where(val => val).Count();

            return count;
        }

        public override object SolvePartTwo()
        {
            var pos = FindDoubleBeacon();
            Console.WriteLine($"Found at {pos}");
            return pos.Item1 * 4_000_000L + pos.Item2;
        }
    }
}
