using System;
using System.Collections.Generic;
using System.Text;

namespace Main
{
    internal static class Utils
    {

        public static void WriteFatLine(string text)
        {
            Console.WriteLine($"\n{text}\n");
        }

        public static void WriteIndent(string text, int indent)
        {
            for(int i = 0; i < indent; i++)
                Console.Write(' ');
            Console.WriteLine(text);
        }

    }
}
