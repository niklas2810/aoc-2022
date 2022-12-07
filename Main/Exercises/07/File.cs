using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises._07
{
    internal class File
    {
        public string Name { get; set; }

        public virtual long Size { get; set; }

        public int IndentationLevel
        {
            get
            {
                File curr = Parent;
                int depth = 0;

                while(curr != null)
                {
                    depth++;
                    curr = curr.Parent;
                }


                return depth;
            }
        }

        public Directory Parent { get; set; }
    }
}
