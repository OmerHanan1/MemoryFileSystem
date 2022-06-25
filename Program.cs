using System;
using System.Collections.Generic;

namespace TinyMemFS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] list = new byte[100000];
            for (int i = 0; i < 100000; i++)
                list[i] = (byte)i;
            
            //File file = new File("file", list);
            //TinyMemFS tinyMemFS = new TinyMemFS();
            
            //Console.WriteLine(file.ToString());
        }
    }
}
