using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Main.Exercises._10
{
    public class CrtScreen
    {
        private readonly VideoCpu _cpu;
        private StringBuilder _out = new StringBuilder();

        public int RowCount { get { return _out.Length / 40; } }
        private string CurrentRow { get { return GetRow((_out.Length - 1) / 40); } }

        public bool ExtensiveLogging { get; set; } = true;

        public CrtScreen(VideoCpu cpu) 
        {
            _cpu = cpu;

            cpu.OnBeforeCycle += (sender, args) =>
            {
                if(ExtensiveLogging && cpu.WaitCycles <= 0)
                    Console.WriteLine($"Start cycle   {cpu.Cycle}: begin executing {cpu.CurrentInstruction}");
                
            };

            cpu.OnDuringCycle += (sender, args) =>
            {
                DrawPixel();
                if(ExtensiveLogging)
                    Console.WriteLine($"Current CRT row: {CurrentRow}");
            };

            cpu.OnAfterCycle += (sender, args) =>
            {
                if(ExtensiveLogging)
                    Console.WriteLine("");
            };

            cpu.OnRegisterChanged += (sender, args) =>
            {
                if(ExtensiveLogging)
                    Console.WriteLine($"End of cycle  {cpu.Cycle}: finish executing {cpu.WaitInstruction} (Register X is now {cpu.RegX})");
                DrawXSprite();
            };

            cpu.OnReset += (sender, args) => _out.Clear();
        }

        public string GetRow(int row)
        {
            var startIndex = 40 * row;
            var length = Math.Min(40, _out.Length - startIndex);
            return _out.ToString().Substring(startIndex, length);
        }

        private void DrawXSprite()
        {
            StringBuilder bld = new StringBuilder();

            foreach (var i in Enumerable.Range(0, 40))
                bld.Append('.');


            var from = (_cpu.RegX - 1) % 40;
            var to = from + 3;
            for (int i = Math.Max(from, 0); i < Math.Min(to, 40); ++i)
                bld[i] = '#';

            if(ExtensiveLogging)
                Console.WriteLine("Sprite position: " + bld.ToString());
        }

        private void DrawPixel()
        {
            var start = Math.Min((_cpu.RegX % 40) - 1, 40);
            var end = Math.Min((_cpu.RegX % 40) + 1, 40);
            var target = (_cpu.Cycle - 1) % 40;
            if(ExtensiveLogging)
                Console.WriteLine($"During cycle  {_cpu.Cycle}: CRT draws pixel in position {target} (x: {start}-{end})");

            bool visible = start <= target && end >= target;
            _out.Append(visible ? '#' : '.');
        }

        public void DrawScreen()
        {
            foreach (var i in Enumerable.Range(0, RowCount))
                Console.WriteLine(GetRow(i));
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var i in Enumerable.Range(0, RowCount))
                builder.AppendLine(GetRow(i));

            return builder.ToString();
        }
    }
}
