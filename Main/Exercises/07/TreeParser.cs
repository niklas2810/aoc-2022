using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises._07
{
    internal class TreeParser
    {
        private readonly string[] lines;

        private int index = 0;
        private Directory current = null;
        private Directory root = null;

        public TreeParser(string[] lines)
        {
            this.lines = lines;
        }

        public Directory parse()
        {
            index = 0;
            current = new Directory();
            current.Name = "/";
            root = current;

            parseInternal();
            return root;
        }

        private void parseInternal()
        {
            if (index == lines.Length)
                return;

            while(index < lines.Length)
            {
                var line = lines[index];

                if (!line.StartsWith('$'))
                    throw new NotSupportedException();

                var cmd = line.Substring(2).Split(' ');
                var cmdName = cmd[0];

                if (cmdName.Equals("cd"))
                {
                    if (cmd[1].Equals(".."))
                        current = current.Parent;
                    else if (cmd[1].Equals("/"))
                        current = root;
                    else
                        current = current.Find(cmd[1]);
                }
                else if (cmdName.Equals("ls"))
                {
                    ++index;
                    ParseCurrentDirectory();
                    --index;
                }
                else
                {
                    throw new NotSupportedException();
                }

                ++index;
            }
        }

        private void ParseCurrentDirectory()
        {
            while (index < lines.Length && !lines[index].StartsWith('$'))
            {
                var line = lines[index++];

                if(line.StartsWith("dir"))
                {
                    var dirname = line.Substring(4);
                    current.AddSubdir(dirname);
                } 
                else
                {
                    var split = line.Split(' ');
                    var size = long.Parse(split[0]);
                    var name = split[1];

                    current.AddFile(name, size);
                }

            }
        }
    }
}
