using System;
using System.Collections.Generic;

namespace TinyMemFS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TinyMemFS tinyMemFS = new TinyMemFS();


            byte[] list = new byte[10];
            for (int i = 0; i < 10; i++)
                list[i] = (byte)i;

            for (int i = 0; i < 20; i++)
            {
                var file = new FileSystemFile($"file #{i}", DateTime.Now, list);
                tinyMemFS.AddFileToFS(file);
            }
           

            // First scenario - simetric encryption & decryption
            //Console.WriteLine(tinyMemFS.ToString());
            //tinyMemFS.encrypt("firstEncryption");
            //Console.WriteLine(tinyMemFS.ToString());
            //tinyMemFS.encrypt("secondEncryption");
            //Console.WriteLine(tinyMemFS.ToString());
            //tinyMemFS.decrypt("secondEncryption");
            //Console.WriteLine(tinyMemFS.ToString());
            //tinyMemFS.decrypt("firstEncryption");
            //Console.WriteLine(tinyMemFS.ToString());

            // Second scenario - Isimetric encryption & decription
            Console.WriteLine(tinyMemFS.ToString());
            tinyMemFS.encrypt("password");
            Console.WriteLine(tinyMemFS.ToString());
            tinyMemFS.decrypt("wrong");
            Console.WriteLine(tinyMemFS.ToString());
        }
    }
}
