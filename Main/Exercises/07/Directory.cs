using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Main.Exercises._07
{
    internal class Directory : File
    {
        public List<File> Files { get; } = new List<File>();

        public List<Directory> Dirs { get => Files.Where(x => x is Directory).Select(x => (Directory)x).ToList(); }

        public override long Size { get => Files.Select(f => f.Size).Sum(); set => throw new NotSupportedException(); }

        public Directory Find(string name)
        {
            foreach(var file in Files)
            {
                if (file.Name.Equals(name))
                    return (Directory)file;
            }

            throw new KeyNotFoundException();
        }

        internal void AddFile(string name, long size)
        {
            var newFile = new File
            {
                Name = name,
                Parent = this,
                Size = size
            };

            Files.Add(newFile);
        }

        internal void AddSubdir(string dirname)
        {
            var newDir = new Directory
            {
                Name = dirname,
                Parent = this,
            };

            Files.Add(newDir);
        }
    }
}
