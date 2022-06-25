using System;
using System.Collections.Generic;

namespace TinyMemFS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<byte> list = new List<byte>();
            for (int i = 0; i < 100000; i++)
                list.Add((byte)0);
            
            File file = new File("file", list);
            TinyMemFS tinyMemFS = new TinyMemFS();
            
            Console.WriteLine(file.ToString());
        }
    }
}
