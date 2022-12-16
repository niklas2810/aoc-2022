using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises
{
    public class Day16 : Exercise
    {
        public override string Name => "Proboscidea Volcanium";

        public override string Description => "Open Valves to release as much pressure as possible";

        public override byte Day => 16;

        private Dictionary<string, int> flowRates = new();
        private Dictionary<string, string[]> destinations = new();
        private Dictionary<(string, string), int> distances = new();

        private HashSet<string> usefulValves = new();

        public override void SetUp()
        {
            foreach (var line in System.IO.File.ReadAllLines("Inputs/16.txt"))
            {
                var name = line.Split(' ')[1];
                var rate = line.Substring(line.IndexOf("rate=") + 5, line.IndexOf(';') - line.IndexOf("rate=") - 5);
                var rawDests = line.Split(line.Contains("valves") ? "valves " : "valve ")[1].Split(", ");
                flowRates.Add(name, int.Parse(rate));
                destinations.Add(name, rawDests);
            }

            CalculateDistances();

            foreach (var dest in destinations.Keys)
            {
                destinations[dest] = destinations[dest].Where(valve => flowRates[valve] > 0).ToArray();
            }

            foreach (var item in flowRates)
            {
                if (item.Value > 0)
                    usefulValves.Add(item.Key);
            }
        }

        private int CalculateDistance(string current, string destination, int currentMinute = 0)
        {
            Dictionary<string, int> visited = new();

            HashSet<string> todo = new();
            HashSet<string> next = new();

            foreach (var neighbor in destinations[current])
                todo.Add(neighbor);

            do
            {

                ++currentMinute;
                foreach (var item in todo)
                {
                    if (visited.Keys.Contains(item))
                        continue;

                    visited.Add(item, currentMinute);
                    foreach (var neighbor in destinations[item])
                        if (!next.Contains(neighbor))
                            next.Add(neighbor);


                }

                todo.Clear();
                foreach (var n in next)
                    todo.Add(n);
            } while (!visited.ContainsKey(destination));

            return visited[destination];
        }

        private void CalculateDistances()
        {
            foreach (var item in flowRates)
            {
                if (item.Value == 0 && item.Key != "AA")
                {
                    //Console.WriteLine($"Skipping {item.Key}");
                    continue;
                }

                var from = item.Key;

                foreach (var itemTo in flowRates)
                {
                    if (itemTo.Value == 0)
                        continue;

                    var to = itemTo.Key;
                    var distance = CalculateDistance(from, to);
                    //Console.WriteLine($"{from} to {to} in {distance} minute(s)");
                    distances.Add((from, to), distance);
                }

            }
        }

        private void CalculateMax(string currentLocation, int minutesLeft, int currentValue, HashSet<string> openValves, ref LinkedList<(string, int)> entries, ref int maximumResult)
        {
            if (minutesLeft < 0)
                throw new NotSupportedException();

            if (minutesLeft < 3 || openValves.Count == usefulValves.Count)
            {
                if (currentValue > maximumResult)
                {
                    /*Console.Write($"{currentValue} [NEW MAX] ");

                    foreach (var item in entries)
                    {
                        Console.Write($"({item.Item1}:{flowRates[item.Item1]}, {item.Item2})");
                        Console.Write(" -> ");
                    }

                    Console.Write("\n");*/


                    maximumResult = currentValue;
                }
                return;
            }

            foreach (var valve in usefulValves)
            {
                if (openValves.Contains(valve))
                    continue;
                var nextMinutes = minutesLeft - (distances[(currentLocation, valve)]) - 1;

                if (nextMinutes > 0)
                {
                    openValves.Add(valve);
                    entries.AddLast((valve, nextMinutes));
                    int nextValue = currentValue + ((nextMinutes) * flowRates[valve]);
                    CalculateMax(valve, nextMinutes, nextValue, openValves, ref entries, ref maximumResult);
                    openValves.Remove(valve);
                    entries.RemoveLast();
                }
                else
                {
                    CalculateMax(valve, 0, currentValue, openValves, ref entries, ref maximumResult);
                }
            }
        }

        private void CalculateMax2(string myLocation, string elLocation, int myMinutesLeft, int elMinutesLeft, int currentValue, HashSet<string> openValves, ref int maximumResult)
        {

            if ((myMinutesLeft < 3 && elMinutesLeft < 3) || openValves.Count == usefulValves.Count)
            {
                if (currentValue > maximumResult)
                {
                    /*Console.Write($"{currentValue} [NEW MAX] ");

                    foreach (var item in entries)
                    {
                        Console.Write($"({item.Item1}, {item.Item2}:{flowRates[item.Item2]}, {item.Item3})");
                        Console.Write(" -> ");
                    }

                    Console.Write("\n");*/


                    maximumResult = currentValue;
                }
                return;
            }

            foreach (var myValve in usefulValves)
            {
                if (openValves.Contains(myValve))
                    continue;


                var myNextMinutes = myMinutesLeft - (distances[(myLocation, myValve)]) - 1;
                int myNextValue = currentValue;
                var myOpen = myNextMinutes > 0;
                if (myOpen)
                {
                    openValves.Add(myValve);
                    //entries.AddLast(('H', myValve, myNextMinutes));
                    myNextValue += myNextMinutes * flowRates[myValve];
                }


                foreach (var elValve in usefulValves)
                {
                    if (openValves.Contains(elValve))
                        continue;

                    var elNextValue = myNextValue;

                    var elNextMinutes = elMinutesLeft - (distances[(elLocation, elValve)]) - 1;
                    var elOpen = elNextMinutes > 0;

                    if(elOpen)
                    {
                        openValves.Add(elValve);
                        //entries.AddLast(('E', elValve, elNextMinutes));
                        elNextValue += elNextMinutes * flowRates[elValve];
                    }

                    CalculateMax2(myOpen ? myValve : myLocation, elOpen ? elValve : elLocation, Math.Max(myNextMinutes, 0), Math.Max(elNextMinutes, 0), elNextValue, openValves, ref maximumResult);

                    if(elOpen)
                    {
                        openValves.Remove(elValve);
                        //entries.RemoveLast();
                    }
                }

                if(myOpen)
                {
                    openValves.Remove(myValve);
                    //entries.RemoveLast();
                }
            }
        }


        public override object SolvePartOne()
        {
            int max = 0;
            HashSet<string> opened = new();
            LinkedList<(string, int)> entries = new();

            CalculateMax("AA", 30, 0, opened, ref entries, ref max);


            return max;
        }

        public override object SolvePartTwo()
        {
            int max = 0;
            HashSet<string> opened = new();
            LinkedList<(char, string, int)> entries = new();

            CalculateMax2("AA", "AA", 26, 26, 0, opened, ref max);
            return max;
        }
    }
}
