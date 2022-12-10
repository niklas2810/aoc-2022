using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises._10
{
    public class VideoCpu
    {

        public event EventHandler OnBeforeCycle = (sender, args) => { };
        public event EventHandler OnDuringCycle = (sender, args) => { };
        public event EventHandler OnAfterCycle = (sender, args) => { };
        public event EventHandler OnRegisterChanged = (sender, args) => { };
        public event EventHandler OnReset = (sender, args) => { };

        public int Cycle { get; private set; } = 1;
        public int RegX { get; private set; } = 1;

        public int WaitCycles { get; private set; } = 0;
        public string WaitInstruction { get; private set; }
        public int EIP { get; private set; } // Execution instruction pointer
        public string CurrentInstruction { get { return _instructions[EIP]; } }

        private readonly List<string> _instructions = new();

        public VideoCpu(string[] instructions)
        {
            _instructions.AddRange(instructions);
        }


        public void ExecuteNext()
        {

            OnBeforeCycle(this, EventArgs.Empty);

            bool executed = false;
            if(WaitCycles <= 0)
            {
                if (EIP >= _instructions.Count)
                    return;

                Execute(CurrentInstruction);
                ++EIP;
                executed = true;
            }

            OnDuringCycle(this, EventArgs.Empty);
            


            if(!executed && WaitCycles > 0)
            {
                WaitCycles--;
                if (WaitCycles > 0)
                    return;

                Execute(WaitInstruction, true);
            }

            OnAfterCycle(this, EventArgs.Empty);
            ++Cycle;
        }

        private void Execute(string ins, bool afterScheduled = false)
        {
            var cmd = ins.Split(' ')[0];

            if (cmd.Equals("noop"))
                return;
            else if (cmd.Equals("addx"))
            {
                if(!afterScheduled)
                {
                    AddDelay(ins, 1);
                    return;
                }

                var num = ins.Substring(4);
                RegX += int.Parse(num);
                OnRegisterChanged(this, EventArgs.Empty);
            }
        }

        private void AddDelay(string ins, int executingCycles)
        {
            WaitCycles = executingCycles;
            WaitInstruction = ins;
        }

        private void Reset()
        {
            EIP = 0;
            RegX = 1;
            Cycle = 1;
            OnReset(this, EventArgs.Empty);
        }

        public void ExecuteAll()
        {
            Reset();
            while (EIP < _instructions.Count || WaitCycles > 0)
                ExecuteNext();

        }
    }
}
